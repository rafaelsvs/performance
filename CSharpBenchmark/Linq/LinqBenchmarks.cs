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
    [BenchmarkCategory(Categories.Linq)]
    public class LinqBenchmarks
    {
        #region Where 1

        [Benchmark]
        public List<Proposal> Where1LinqQueryX()
        {
            return (from proposal in ProposalBuilder.GetInsurances()
                    where proposal.InsuranceId == 1
                     select proposal).ToList();
        }

        [Benchmark]
        public List<Proposal> Where1LinqLambdaX()
        {
            return ProposalBuilder
                .GetInsurances()
                .Where(p => p.InsuranceId == 1)
                .ToList();
        }

        [Benchmark]
        public List<Proposal> Where1LinqForeachX()
        {
            List<Proposal> proposals = ProposalBuilder.GetInsurances();
            List<Proposal> result = new List<Proposal>();

            foreach (Proposal p in proposals)
            {
                if (p.InsuranceId == 1)
                {
                    result.Add(p);
                }
            }
            return result;
        }

        [Benchmark]
        public List<Proposal> Where1LinqForX()
        {
            List<Proposal> proposals = ProposalBuilder.GetInsurances();
            List<Proposal> result = new List<Proposal>();
            for (int i = 0; i < proposals.Count; i++)
            {
                Proposal p = proposals[i];
                if (p.InsuranceId == 1)
                    result.Add(p);
            }
            return result;
        }

        [Benchmark]
        public List<Proposal> Where1LinqUnrolledForX()
        {
            List<Proposal> proposals = ProposalBuilder.GetInsurances();
            List<Proposal> result = new List<Proposal>();
            int i = 0;
            for (; i <= proposals.Count - 4; i += 4)
            {
                Proposal p0 = proposals[i];
                Proposal p1 = proposals[i + 1];
                Proposal p2 = proposals[i + 2];
                Proposal p3 = proposals[i + 3];
                if (p0.InsuranceId == 1)
                    result.Add(p0);
                if (p1.InsuranceId == 1)
                    result.Add(p1);
                if (p2.InsuranceId == 1)
                    result.Add(p2);
                if (p3.InsuranceId == 1)
                    result.Add(p3);
            }
            for (; i < proposals.Count; i++)
            {
                Proposal p = proposals[i];
                if (p.InsuranceId == 1)
                    result.Add(p);
            }

            return result;
        }

        [Benchmark]
        public List<ReadOnlyProposal> Where1LinqForeachSortedX()
        {
            List<ReadOnlyProposal> proposals = ProposalBuilder.GetSortedInsurances();
            List<ReadOnlyProposal> result = new List<ReadOnlyProposal>();

            foreach (ReadOnlyProposal p in proposals)
            {
                if (p.InsuranceId == 1)
                    result.Add(p);
                else if (p.InsuranceId > 1)
                    break;
            }
            return result;
        }

        [Benchmark]
        public ImmutableArray<ReadOnlyProposal> Where1LinqKeyedX()
        {
            Dictionary<int, ImmutableArray<ReadOnlyProposal>> proposals = ProposalBuilder.GetKeyedSortedInsurances();
            return proposals[1];
        }

        [Benchmark]
        public ImmutableArray<ReadOnlyProposal> Where1LinqPositionalX()
        {
            ImmutableArray<ReadOnlyProposal>[] proposals = ProposalBuilder.GetPositionalSortedInsurances();
            
            return proposals[1];
        }


        #endregion

        #region Where 2

        [Benchmark]
        public List<Proposal> Where2LinqQueryX()
        {
            return (from proposal in ProposalBuilder.GetInsurances()
                    where proposal.InsuranceId == 1 && proposal.NetPremium > 20000M
                    select proposal)
                    .ToList();
        }

        [Benchmark]
        public List<Proposal> Where2LinqLambdaX()
        {
            return ProposalBuilder.GetInsurances()
                .Where(p => p.InsuranceId == 1 && p.NetPremium > 20000M)
                .ToList();
        }

        [Benchmark]
        public List<Proposal> Where2LinqForeachX()
        {
            List<Proposal> result = new List<Proposal>();

            foreach (Proposal p in ProposalBuilder.GetInsurances())
            {
                if (p.InsuranceId == 1 && p.NetPremium > 20000M)
                    result.Add(p);
            }
            return result;
        }

        [Benchmark]
        public List<Proposal> Where2LinqForX()
        {
            List<Proposal> proposals = ProposalBuilder.GetInsurances();
            List<Proposal> result = new List<Proposal>();

            for (int i = 0; i < proposals.Count; i++)
            {
                Proposal p = proposals[i];
                if (p.InsuranceId == 1 && p.NetPremium > 20000M)
                    result.Add(p);
            }
            return result;
        }

        [Benchmark]
        public List<ReadOnlyProposal> Where2LinqForeachSortedX()
        {
            List<ReadOnlyProposal> proposals = ProposalBuilder.GetSortedInsurances();
            List<ReadOnlyProposal> result = new List<ReadOnlyProposal>();

            foreach (ReadOnlyProposal p in proposals)
            {
                if (p.InsuranceId == 1)
                {
                    if (p.NetPremium > 20000M)
                        result.Add(p);
                }
                else if (p.InsuranceId > 1)
                    break;
            }
            return result;
        }
        
        [Benchmark]
        public List<ReadOnlyProposal> Where2LinqKeyedSortedX()
        {
            Dictionary<int, ImmutableArray<ReadOnlyProposal>> proposals = ProposalBuilder.GetKeyedSortedInsurances();
            List<ReadOnlyProposal> result = new List<ReadOnlyProposal>();

            foreach (ReadOnlyProposal p in proposals[1])
            {
                if (p.NetPremium > 20000M)
                    result.Add(p);
                else
                    break;
            }

            return result;
        }

        [Benchmark]
        public List<ReadOnlyProposal> Where2LinqPositionalSortedX()
        {
            ImmutableArray<ReadOnlyProposal>[] proposals = ProposalBuilder.GetPositionalSortedInsurances();
            List<ReadOnlyProposal> result = new List<ReadOnlyProposal>();

            foreach (ReadOnlyProposal p in proposals[1])
            {
                if (p.NetPremium > 20000M)
                    result.Add(p);
                else
                    break;
            }

            return result;
        }
        [Benchmark]
        public unsafe ReadOnlyProposal[] Where2LinqPositionalSortedVectorizedX()
        {
            ProposalResult[] proposals = ProposalBuilder.GetPositionalSortedVectorizedInsurances();
            ProposalResult p = proposals[1];

            var minPremium = Vector256.Create(decimal.ToOACurrency(20000M));
            fixed (long* npp = p.NetPremiums)
            {
                long* offset = npp;
                long* endBlock = npp + p.NetPremiums.Length - Vector256<long>.Count + 1;
                for (; offset <= endBlock; offset += Vector256<long>.Count)
                {
                    int mask = (int)Lzcnt.LeadingZeroCount(
                        (uint)Avx2.MoveMask(
                            Vector256.AsByte(
                                Avx2.CompareGreaterThan(
                                    Avx2.LoadVector256(offset),
                                    minPremium
                                 )
                            )
                        )
                    ) >> 3;
                    if (mask != 0)
                    {
                        int length = (int)(offset - npp) + Vector256<long>.Count - mask;
                        return p.Proposals
                            .AsSpan(0, length)
                            .ToArray();
                    }
                }
            }
            return p.Proposals;
        }
        #endregion
    }
}
