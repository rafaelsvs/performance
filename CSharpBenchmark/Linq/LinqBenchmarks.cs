using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpBenchmark.Linq
{
    [MemoryDiagnoser]
    [BenchmarkCategory(Categories.Linq)]
    public class LinqBenchmarks
    {
        public const int IterationsWhere1 = 2000000;
        public const int IterationsWhere2 = 2000000;
        public const int IterationsCount1 = 2000000;
        public const int IterationsOrder2 = 2000000;

        #region Where 1

        [Benchmark]
        public bool Where1LinqQueryX()
        {
            List<Proposal> proposals = ProposalBuilder.GetInsurances();
            List<Proposal> result = null;
            int count = 0;

            for (int i = 0; i < IterationsWhere1; i++)
            {
                result =
                    (from proposal in proposals
                     where proposal.InsuranceId == 1
                     select proposal).ToList();

                foreach (var r in result)
                {
                    count++;
                }
            }

            return count == 20 * IterationsWhere1;
        }

        [Benchmark]
        public bool Where1LinqLambdaX()
        {
            List<Proposal> proposals = ProposalBuilder.GetInsurances();
            List<Proposal> result = null;
            int count = 0;

            for (int i = 0; i < IterationsWhere1; i++)
            {
                result = proposals.Where(p => p.InsuranceId == 1).ToList();

                foreach (var r in result)
                {
                    count++;
                }
            }

            return count == 20 * IterationsWhere1;
        }

        [Benchmark]
        public bool Where1LinqForX()
        {
            List<Proposal> proposals = ProposalBuilder.GetInsurances();
            List<Proposal> result = null;
            int count = 0;

            for (int i = 0; i < IterationsWhere1; i++)
            {
                result = new List<Proposal>();

                foreach (Proposal p in proposals)
                {
                    if (p.InsuranceId == 1)
                    {
                        result.Add(p);
                    }
                }

                foreach (var r in result)
                {
                    count++;
                }
            }

            return count == 20 * IterationsWhere1;
        }

        #endregion

        #region Where 2

        [Benchmark]
        public bool Where2LinqQueryX()
        {
            List<Proposal> proposals = ProposalBuilder.GetInsurances();
            List<Proposal> result = null;
            int count = 0;

            for (int i = 0; i < IterationsWhere2; i++)
            {
                result =
                    (from proposal in proposals
                     where proposal.InsuranceId == 1 && proposal.NetPremium > 20000M
                     select proposal).ToList();

                foreach (var r in result)
                {
                    count++;
                }
            }

            return count == 18 * IterationsWhere2;
        }

        [Benchmark]
        public bool Where2LinqLambdaX()
        {
            List<Proposal> proposals = ProposalBuilder.GetInsurances();
            List<Proposal> result = null;
            int count = 0;

            for (int i = 0; i < IterationsWhere2; i++)
            {
                result = proposals.Where(p => p.InsuranceId == 1 && p.NetPremium > 20000M).ToList();

                foreach (var r in result)
                {
                    count++;
                }
            }

            return count == 18 * IterationsWhere2;
        }

        [Benchmark]
        public bool Where2LinqForX()
        {
            List<Proposal> proposals = ProposalBuilder.GetInsurances();
            List<Proposal> result = null;
            int count = 0;

            for (int i = 0; i < IterationsWhere2; i++)
            {
                result = new List<Proposal>();

                foreach (Proposal p in proposals)
                {
                    if (p.InsuranceId == 1 && p.NetPremium > 20000M)
                    {
                        result.Add(p);
                    }
                }

                foreach (var r in result)
                {
                    count++;
                }
            }

            return count == 18 * IterationsWhere2;
        }

        #endregion
    }
}
