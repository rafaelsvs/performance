using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace CSharpBenchmark.Linq
{
    [MemoryDiagnoser]
    [DisassemblyDiagnoser(printSource: true)]
    [BenchmarkCategory(Categories.LinqWhere2)]
    public class LinqBenchmarksWhere2
    {
        private const int SearchedInsuranceId = 1;
        private const decimal SearchedNetPremium = 20000M;

        [Benchmark(Baseline = true)]
        public List<Proposal> LinqQuery()
        {
            return (from proposal in ProposalBuilder.GetInsurances()
                    where proposal.InsuranceId == SearchedInsuranceId && proposal.NetPremium > SearchedNetPremium
                    select proposal)
                    .ToList();
        }


        [Benchmark]
        public List<Proposal> Lambda()
        {
            return ProposalBuilder.GetInsurances()
                .Where(p => p.InsuranceId == SearchedInsuranceId && p.NetPremium > SearchedNetPremium)
                .ToList();
        }


        [Benchmark]
        public List<Proposal> Foreach()
        {
            List<Proposal> result = new List<Proposal>();

            foreach (Proposal p in ProposalBuilder.GetInsurances())
            {
                if (p.InsuranceId == SearchedInsuranceId && p.NetPremium > SearchedNetPremium)
                    result.Add(p);
            }
            return result;
        }


        [Benchmark]
        public List<Proposal> For()
        {
            List<Proposal> proposals = ProposalBuilder.GetInsurances();
            List<Proposal> result = new List<Proposal>(64); // cheating

            for (int i = 0; i < proposals.Count; i++)
            {
                Proposal p = proposals[i];
                if (p.InsuranceId == SearchedInsuranceId && p.NetPremium > SearchedNetPremium)
                    result.Add(p);
            }
            return result;
        }


        [Benchmark]
        public List<ReadOnlyProposal> ForSorted()
        {
            List<ReadOnlyProposal> proposals = ProposalBuilder.GetSortedInsurances();
            List<ReadOnlyProposal> result = new List<ReadOnlyProposal>(64); // cheating

            for (int i = 0; i < proposals.Count; i++)
            {
                ReadOnlyProposal p = proposals[i];
                if (p.InsuranceId == SearchedInsuranceId)
                {
                    if (p.NetPremium > SearchedNetPremium)
                        result.Add(p);
                }
                else if (p.InsuranceId > SearchedInsuranceId)
                    break;
            }
            return result;
        }

        // removed, not testing loop speed
        //[Benchmark]
        public List<ReadOnlyProposal> KeyedSorted()
        {
            Dictionary<int, ImmutableArray<ReadOnlyProposal>> proposalsByKey = ProposalBuilder.GetKeyedSortedInsurances();
            List<ReadOnlyProposal> result = new List<ReadOnlyProposal>();

            var proposals = proposalsByKey[SearchedInsuranceId];
            for (int i = 0; i < proposals.Length; i++)
            {
                ReadOnlyProposal p = proposals[i];
                if (p.NetPremium > SearchedNetPremium)
                    result.Add(p);
                else
                    break;
            }

            return result;
        }

        //[Benchmark]
        public List<ReadOnlyProposal> PositionalSorted()
        {
            ImmutableArray<ReadOnlyProposal>[] proposalsByPosition = ProposalBuilder.GetPositionalSortedInsurances();
            List<ReadOnlyProposal> result = new List<ReadOnlyProposal>();

            var proposals = proposalsByPosition[SearchedInsuranceId];
            for (int i = 0; i < proposals.Length; i++)
            {
                ReadOnlyProposal p = proposals[i];
                if (p.NetPremium > SearchedNetPremium)
                    result.Add(p);
                else
                    break;
            }

            return result;
        }


        [Benchmark]
        public unsafe ReadOnlyProposal[] ForVectorized()
        {
            ProposalResult p = ProposalBuilder.GetSortedVectorizedInsurances();

            var insuranceId = Vector256.Create(SearchedInsuranceId);
            fixed (int* iip = p.InsuranceIds)
            {
                int i = 0;
                int length = p.InsuranceIds.Length - Vector256<int>.Count + 1;
                int mask = 8;
                while (mask == 8 && i < length)
                {
                    mask = (int)Lzcnt.LeadingZeroCount(
                        (uint)Avx2.MoveMask(
                            Vector256.AsByte(
                                Avx2.CompareEqual(
                                    Avx2.LoadVector256(iip + i),
                                    insuranceId
                                )
                            ).Reverse()
                        )
                    ) >> 2;
                    i += Vector256<int>.Count;
                }
                i -= Vector256<int>.Count;

                int initial = i + mask;
                if (initial == p.InsuranceIds.Length)
                    return Array.Empty<ReadOnlyProposal>();
                
                mask = 0;
                fixed (long* npp = p.NetPremiums)
                {
                    var minPremium = Vector256.Create(decimal.ToOACurrency(SearchedNetPremium));
                    while (mask == 0 && i < length)
                    {
                        mask = (int)Lzcnt.LeadingZeroCount(
                            (uint)Avx2.MoveMask(
                                Vector256.AsByte(
                                    Avx2.And(
                                        Avx2.CompareEqual(
                                            Avx2.LoadVector256(iip + i),
                                            insuranceId
                                        ),
                                        Avx2.CompareGreaterThan(
                                            Avx2.LoadVector256(npp + i),
                                            minPremium
                                        ).JoinMask(
                                            Avx2.CompareGreaterThan(
                                                Avx2.LoadVector256(npp + i + Vector256<long>.Count),
                                                minPremium
                                            )
                                        )
                                    )
                                )
                            )
                        ) >> 2;
                        i += Vector256<int>.Count;
                    }
                }
                return p.Proposals.AsSpan(initial, i - mask - initial).ToArray();
            }
        }

        // removed, not testing loop speed
        //[Benchmark]
        public unsafe ReadOnlyProposal[] PositionalSortedVectorized()
        {
            ProposalResult[] proposals = ProposalBuilder.GetPositionalSortedVectorizedInsurances();
            ProposalResult p = proposals[SearchedInsuranceId];

            var minPremium = Vector256.Create(decimal.ToOACurrency(SearchedNetPremium));
            fixed (long* npp = p.NetPremiums)
            {
                int i = 0;
                int initial = 0;
                for (; i < p.NetPremiums.Length - Vector256<long>.Count + 1; i += Vector256<long>.Count)
                {
                    int mask = (int)Lzcnt.LeadingZeroCount(
                        (uint)Avx2.MoveMask(
                            Vector256.AsByte(
                                Avx2.CompareGreaterThan(
                                    Avx2.LoadVector256(npp + i),
                                    minPremium
                                )
                            ).Reverse()
                        )
                    ) >> 3;

                    if (mask != 8)
                    {
                        initial = i + mask;
                        break;
                    }
                }
                for (; i < p.NetPremiums.Length - Vector256<long>.Count + 1; i += Vector256<long>.Count)
                {
                    int mask = (int)Lzcnt.LeadingZeroCount(
                        (uint)Avx2.MoveMask(
                            Vector256.AsByte(
                                Avx2.CompareGreaterThan(
                                    Avx2.LoadVector256(npp + i),
                                    minPremium
                                )
                            )
                        )
                    ) >> 3;
                    if (mask != 0)
                    {
                        int length = i + Vector256<long>.Count - mask - initial;
                        return p.Proposals.AsSpan(initial, length).ToArray();
                    }
                }
                return p.Proposals.AsSpan(initial).ToArray();
            }
        }
    }
}
