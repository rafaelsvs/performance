using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace CSharpBenchmark.Linq
{
    [MemoryDiagnoser]
    [DisassemblyDiagnoser(printSource: true)]
    [BenchmarkCategory(Categories.LinqWhere1)]

    public class LinqBenchmarksWhere1
    {
        private const int SearchedInsuranceId = 1;
        private const decimal SearchedNetPremium = 20000M;

        [Benchmark(Baseline = true)]
        public List<Proposal> LinqQuery()
        {
            return (from proposal in ProposalBuilder.GetInsurances()
                    where proposal.InsuranceId == SearchedInsuranceId
                    select proposal).ToList();
        }


        [Benchmark]
        public List<Proposal> Lambda()
        {
            return ProposalBuilder
                .GetInsurances()
                .Where(p => p.InsuranceId == SearchedInsuranceId)
                .ToList();
        }


        [Benchmark]
        public List<Proposal> Foreach()
        {
            List<Proposal> proposals = ProposalBuilder.GetInsurances();
            List<Proposal> result = new List<Proposal>();

            foreach (Proposal p in proposals)
            {
                if (p.InsuranceId == SearchedInsuranceId)
                {
                    result.Add(p);
                }
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
                if (p.InsuranceId == SearchedInsuranceId)
                    result.Add(p);
            }
            return result;
        }


        [Benchmark]
        public List<Proposal> UnrolledFor()
        {
            List<Proposal> proposals = ProposalBuilder.GetInsurances();
            List<Proposal> result = new List<Proposal>(64); // cheating
            int i = 0;
            for (; i <= proposals.Count - 4; i += 4)
            {
                Proposal p0 = proposals[i];
                Proposal p1 = proposals[i + 1];
                Proposal p2 = proposals[i + 2];
                Proposal p3 = proposals[i + 3];
                if (p0.InsuranceId == SearchedInsuranceId)
                    result.Add(p0);
                if (p1.InsuranceId == SearchedInsuranceId)
                    result.Add(p1);
                if (p2.InsuranceId == SearchedInsuranceId)
                    result.Add(p2);
                if (p3.InsuranceId == SearchedInsuranceId)
                    result.Add(p3);
            }
            for (; i < proposals.Count; i++)
            {
                Proposal p = proposals[i];
                if (p.InsuranceId == SearchedInsuranceId)
                    result.Add(p);
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
                while (mask == 0 && i < length)
                {
                    mask = (int)Lzcnt.LeadingZeroCount(
                        (uint)Avx2.MoveMask(
                            Vector256.AsByte(
                                Avx2.CompareEqual(
                                    Avx2.LoadVector256(iip + i),
                                    insuranceId
                                 )
                            )
                        )
                    ) >> 2;
                    i += Vector256<int>.Count;
                }
                return p.Proposals.AsSpan(initial, i - mask - initial).ToArray();
            }
        }

        [Benchmark]


        public List<ReadOnlyProposal> ForeachSorted()
        {
            List<ReadOnlyProposal> proposals = ProposalBuilder.GetSortedInsurances();
            List<ReadOnlyProposal> result = new List<ReadOnlyProposal>(64); // cheating

            foreach (ReadOnlyProposal p in proposals)
            {
                if (p.InsuranceId == SearchedInsuranceId)
                    result.Add(p);
                else if (p.InsuranceId > SearchedInsuranceId)
                    break;
            }
            return result;
        }

        // removed, not testing loop speed
        //[Benchmark]
        public ImmutableArray<ReadOnlyProposal> Keyed()
        {
            Dictionary<int, ImmutableArray<ReadOnlyProposal>> proposals = ProposalBuilder.GetKeyedSortedInsurances();
            return proposals[1];
        }

        //[Benchmark]
        public ImmutableArray<ReadOnlyProposal> Positional()
        {
            ImmutableArray<ReadOnlyProposal>[] proposals = ProposalBuilder.GetPositionalSortedInsurances();

            return proposals[1];
        }

    }
}
