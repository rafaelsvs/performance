using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace CSharpBenchmark.Linq
{
    public static class ProposalBuilder
    {
        private static Lazy<List<Proposal>> proposals = new Lazy<List<Proposal>>(CreateProposal);
        private static Lazy<List<ReadOnlyProposal>> sortedProposals = new Lazy<List<ReadOnlyProposal>>(CreateSortedProposal);
        private static Lazy<Dictionary<int, ImmutableArray<ReadOnlyProposal>>> keyedSortedProposals = new Lazy<Dictionary<int, ImmutableArray<ReadOnlyProposal>>>(CreateKeyedSortedProposal);
        private static Lazy<ImmutableArray<ReadOnlyProposal>[]> positionalSortedProposals = new Lazy<ImmutableArray<ReadOnlyProposal>[]>(CreatePositionalSortedProposal);
        private static Lazy<ProposalResult[]> positionalSortedVectorizedProposals = new Lazy<ProposalResult[]>(CreatePositionalSortedVectorizedProposal);
        private static Lazy<ProposalResult> sortedVectorizedProposals = new Lazy<ProposalResult>(CreateSortedVectorizedProposal);

        private static List<Proposal> CreateProposal()
        {
            return Enumerable
                .Range(0, 100)
                .Select(i => new
                {
                    Sequence = i + 1,
                    Remainder = i % 5 + 1,
                    Quotient = i / 5 + 1
                })
                .Select(n => new Proposal
                {
                    ProposalNumber = n.Sequence,
                    InsuranceName = string.Concat("Insurance_", n.Remainder), // ugly but faster
                    InsuranceId = n.Remainder,
                    Coverage = n.Quotient,
                    NetPremium = 10000M * n.Remainder + 1000M * (n.Quotient - 1) + 10000M * Math.Max(n.Quotient - 2, 0)// not equal but close enougth
                }).ToList();
            //return new List<Proposal>()
            //{
            //    new Proposal{ ProposalNumber = 1, InsuranceName = "Insurance_1", InsuranceId = 1, Coverage = 1, NetPremium = 10000M },
            //    new Proposal{ ProposalNumber = 2, InsuranceName = "Insurance_2", InsuranceId = 2, Coverage = 1, NetPremium = 20000M },
            //    new Proposal{ ProposalNumber = 3, InsuranceName = "Insurance_3", InsuranceId = 3, Coverage = 1, NetPremium = 30000M },
            //    new Proposal{ ProposalNumber = 4, InsuranceName = "Insurance_4", InsuranceId = 4, Coverage = 1, NetPremium = 40000M },
            //    new Proposal{ ProposalNumber = 5, InsuranceName = "Insurance_5", InsuranceId = 5, Coverage = 1, NetPremium = 50000M },
            //    new Proposal{ ProposalNumber = 6, InsuranceName = "Insurance_1", InsuranceId = 1, Coverage = 2, NetPremium = 11000M },
            //    new Proposal{ ProposalNumber = 7, InsuranceName = "Insurance_2", InsuranceId = 2, Coverage = 2, NetPremium = 21000M },
            //    new Proposal{ ProposalNumber = 8, InsuranceName = "Insurance_3", InsuranceId = 3, Coverage = 2, NetPremium = 31000M },
            //    new Proposal{ ProposalNumber = 9, InsuranceName = "Insurance_4", InsuranceId = 4, Coverage = 2, NetPremium = 41000M },
            //    new Proposal{ ProposalNumber = 10, InsuranceName = "Insurance_5", InsuranceId = 5, Coverage = 2, NetPremium = 51000M },
            //    new Proposal{ ProposalNumber = 11, InsuranceName = "Insurance_1", InsuranceId = 1, Coverage = 3, NetPremium = 22000M },
            //    new Proposal{ ProposalNumber = 12, InsuranceName = "Insurance_2", InsuranceId = 2, Coverage = 3, NetPremium = 32000M },
            //    new Proposal{ ProposalNumber = 13, InsuranceName = "Insurance_3", InsuranceId = 3, Coverage = 3, NetPremium = 42000M },
            //    new Proposal{ ProposalNumber = 14, InsuranceName = "Insurance_4", InsuranceId = 4, Coverage = 3, NetPremium = 52000M },
            //    new Proposal{ ProposalNumber = 15, InsuranceName = "Insurance_5", InsuranceId = 5, Coverage = 3, NetPremium = 62000M },
            //    new Proposal{ ProposalNumber = 16, InsuranceName = "Insurance_1", InsuranceId = 1, Coverage = 4, NetPremium = 33000M },
            //    new Proposal{ ProposalNumber = 17, InsuranceName = "Insurance_2", InsuranceId = 2, Coverage = 4, NetPremium = 43000M },
            //    new Proposal{ ProposalNumber = 18, InsuranceName = "Insurance_3", InsuranceId = 3, Coverage = 4, NetPremium = 53000M },
            //    new Proposal{ ProposalNumber = 19, InsuranceName = "Insurance_4", InsuranceId = 4, Coverage = 4, NetPremium = 63000M },
            //    new Proposal{ ProposalNumber = 20, InsuranceName = "Insurance_5", InsuranceId = 5, Coverage = 4, NetPremium = 73000M },
            //    new Proposal{ ProposalNumber = 21, InsuranceName = "Insurance_1", InsuranceId = 1, Coverage = 5, NetPremium = 44000M },
            //    new Proposal{ ProposalNumber = 22, InsuranceName = "Insurance_2", InsuranceId = 2, Coverage = 5, NetPremium = 54000M },
            //    new Proposal{ ProposalNumber = 23, InsuranceName = "Insurance_3", InsuranceId = 3, Coverage = 5, NetPremium = 64000M },
            //    new Proposal{ ProposalNumber = 24, InsuranceName = "Insurance_4", InsuranceId = 4, Coverage = 5, NetPremium = 74000M },
            //    new Proposal{ ProposalNumber = 25, InsuranceName = "Insurance_5", InsuranceId = 5, Coverage = 5, NetPremium = 84000M },
            //    new Proposal{ ProposalNumber = 26, InsuranceName = "Insurance_1", InsuranceId = 1, Coverage = 6, NetPremium = 55000M },
            //    new Proposal{ ProposalNumber = 27, InsuranceName = "Insurance_2", InsuranceId = 2, Coverage = 6, NetPremium = 65000M },
            //    new Proposal{ ProposalNumber = 28, InsuranceName = "Insurance_3", InsuranceId = 3, Coverage = 6, NetPremium = 75000M },
            //    new Proposal{ ProposalNumber = 29, InsuranceName = "Insurance_4", InsuranceId = 4, Coverage = 6, NetPremium = 85000M },
            //    new Proposal{ ProposalNumber = 30, InsuranceName = "Insurance_5", InsuranceId = 5, Coverage = 6, NetPremium = 95000M },
            //    new Proposal{ ProposalNumber = 31, InsuranceName = "Insurance_1", InsuranceId = 1, Coverage = 7, NetPremium = 66000M },
            //    new Proposal{ ProposalNumber = 32, InsuranceName = "Insurance_2", InsuranceId = 2, Coverage = 7, NetPremium = 76000M },
            //    new Proposal{ ProposalNumber = 33, InsuranceName = "Insurance_3", InsuranceId = 3, Coverage = 7, NetPremium = 86000M },
            //    new Proposal{ ProposalNumber = 34, InsuranceName = "Insurance_4", InsuranceId = 4, Coverage = 7, NetPremium = 96000M },
            //    new Proposal{ ProposalNumber = 35, InsuranceName = "Insurance_5", InsuranceId = 5, Coverage = 7, NetPremium = 106000M },
            //    new Proposal{ ProposalNumber = 36, InsuranceName = "Insurance_1", InsuranceId = 1, Coverage = 8, NetPremium = 77000M },
            //    new Proposal{ ProposalNumber = 37, InsuranceName = "Insurance_2", InsuranceId = 2, Coverage = 8, NetPremium = 87000M },
            //    new Proposal{ ProposalNumber = 38, InsuranceName = "Insurance_3", InsuranceId = 3, Coverage = 8, NetPremium = 97000M },
            //    new Proposal{ ProposalNumber = 39, InsuranceName = "Insurance_4", InsuranceId = 4, Coverage = 8, NetPremium = 107000M },
            //    new Proposal{ ProposalNumber = 40, InsuranceName = "Insurance_5", InsuranceId = 5, Coverage = 8, NetPremium = 117000M },
            //    new Proposal{ ProposalNumber = 41, InsuranceName = "Insurance_1", InsuranceId = 1, Coverage = 9, NetPremium = 88000M },
            //    new Proposal{ ProposalNumber = 42, InsuranceName = "Insurance_2", InsuranceId = 2, Coverage = 9, NetPremium = 98000M },
            //    new Proposal{ ProposalNumber = 43, InsuranceName = "Insurance_3", InsuranceId = 3, Coverage = 9, NetPremium = 108000M },
            //    new Proposal{ ProposalNumber = 44, InsuranceName = "Insurance_4", InsuranceId = 4, Coverage = 9, NetPremium = 118000M },
            //    new Proposal{ ProposalNumber = 45, InsuranceName = "Insurance_5", InsuranceId = 5, Coverage = 9, NetPremium = 128000M },
            //    new Proposal{ ProposalNumber = 46, InsuranceName = "Insurance_1", InsuranceId = 1, Coverage = 10, NetPremium = 99000M },
            //    new Proposal{ ProposalNumber = 47, InsuranceName = "Insurance_2", InsuranceId = 2, Coverage = 10, NetPremium = 109000M },
            //    new Proposal{ ProposalNumber = 48, InsuranceName = "Insurance_3", InsuranceId = 3, Coverage = 10, NetPremium = 119000M },
            //    new Proposal{ ProposalNumber = 49, InsuranceName = "Insurance_4", InsuranceId = 4, Coverage = 10, NetPremium = 129000M },
            //    new Proposal{ ProposalNumber = 50, InsuranceName = "Insurance_5", InsuranceId = 5, Coverage = 10, NetPremium = 139000M },
            //    new Proposal{ ProposalNumber = 51, InsuranceName = "Insurance_1", InsuranceId = 1, Coverage = 11, NetPremium = 110000M },
            //    new Proposal{ ProposalNumber = 52, InsuranceName = "Insurance_2", InsuranceId = 2, Coverage = 11, NetPremium = 120000M },
            //    new Proposal{ ProposalNumber = 53, InsuranceName = "Insurance_3", InsuranceId = 3, Coverage = 11, NetPremium = 130000M },
            //    new Proposal{ ProposalNumber = 54, InsuranceName = "Insurance_4", InsuranceId = 4, Coverage = 11, NetPremium = 140000M },
            //    new Proposal{ ProposalNumber = 55, InsuranceName = "Insurance_5", InsuranceId = 5, Coverage = 11, NetPremium = 150000M },
            //    new Proposal{ ProposalNumber = 56, InsuranceName = "Insurance_1", InsuranceId = 1, Coverage = 12, NetPremium = 121000M },
            //    new Proposal{ ProposalNumber = 57, InsuranceName = "Insurance_2", InsuranceId = 2, Coverage = 12, NetPremium = 131000M },
            //    new Proposal{ ProposalNumber = 58, InsuranceName = "Insurance_3", InsuranceId = 3, Coverage = 12, NetPremium = 141000M },
            //    new Proposal{ ProposalNumber = 59, InsuranceName = "Insurance_4", InsuranceId = 4, Coverage = 12, NetPremium = 151000M },
            //    new Proposal{ ProposalNumber = 60, InsuranceName = "Insurance_5", InsuranceId = 5, Coverage = 12, NetPremium = 161000M },
            //    new Proposal{ ProposalNumber = 61, InsuranceName = "Insurance_1", InsuranceId = 1, Coverage = 13, NetPremium = 132000M },
            //    new Proposal{ ProposalNumber = 62, InsuranceName = "Insurance_2", InsuranceId = 2, Coverage = 13, NetPremium = 142000M },
            //    new Proposal{ ProposalNumber = 63, InsuranceName = "Insurance_3", InsuranceId = 3, Coverage = 13, NetPremium = 152000M },
            //    new Proposal{ ProposalNumber = 64, InsuranceName = "Insurance_4", InsuranceId = 4, Coverage = 13, NetPremium = 162000M },
            //    new Proposal{ ProposalNumber = 65, InsuranceName = "Insurance_5", InsuranceId = 5, Coverage = 13, NetPremium = 172000M },
            //    new Proposal{ ProposalNumber = 66, InsuranceName = "Insurance_1", InsuranceId = 1, Coverage = 14, NetPremium = 143000M },
            //    new Proposal{ ProposalNumber = 67, InsuranceName = "Insurance_2", InsuranceId = 2, Coverage = 14, NetPremium = 153000M },
            //    new Proposal{ ProposalNumber = 68, InsuranceName = "Insurance_3", InsuranceId = 3, Coverage = 14, NetPremium = 163000M },
            //    new Proposal{ ProposalNumber = 69, InsuranceName = "Insurance_4", InsuranceId = 4, Coverage = 14, NetPremium = 173000M },
            //    new Proposal{ ProposalNumber = 70, InsuranceName = "Insurance_5", InsuranceId = 5, Coverage = 14, NetPremium = 183000M },
            //    new Proposal{ ProposalNumber = 71, InsuranceName = "Insurance_1", InsuranceId = 1, Coverage = 15, NetPremium = 154000M },
            //    new Proposal{ ProposalNumber = 72, InsuranceName = "Insurance_2", InsuranceId = 2, Coverage = 15, NetPremium = 164000M },
            //    new Proposal{ ProposalNumber = 73, InsuranceName = "Insurance_3", InsuranceId = 3, Coverage = 15, NetPremium = 174000M },
            //    new Proposal{ ProposalNumber = 74, InsuranceName = "Insurance_4", InsuranceId = 4, Coverage = 15, NetPremium = 184000M },
            //    new Proposal{ ProposalNumber = 75, InsuranceName = "Insurance_5", InsuranceId = 5, Coverage = 15, NetPremium = 194000M },
            //    new Proposal{ ProposalNumber = 76, InsuranceName = "Insurance_1", InsuranceId = 1, Coverage = 16, NetPremium = 165000M },
            //    new Proposal{ ProposalNumber = 77, InsuranceName = "Insurance_2", InsuranceId = 2, Coverage = 16, NetPremium = 175000M },
            //    new Proposal{ ProposalNumber = 78, InsuranceName = "Insurance_3", InsuranceId = 3, Coverage = 16, NetPremium = 185000M },
            //    new Proposal{ ProposalNumber = 79, InsuranceName = "Insurance_4", InsuranceId = 4, Coverage = 16, NetPremium = 195000M },
            //    new Proposal{ ProposalNumber = 80, InsuranceName = "Insurance_5", InsuranceId = 5, Coverage = 16, NetPremium = 205000M },
            //    new Proposal{ ProposalNumber = 81, InsuranceName = "Insurance_1", InsuranceId = 1, Coverage = 17, NetPremium = 176000M },
            //    new Proposal{ ProposalNumber = 82, InsuranceName = "Insurance_2", InsuranceId = 2, Coverage = 17, NetPremium = 186000M },
            //    new Proposal{ ProposalNumber = 83, InsuranceName = "Insurance_3", InsuranceId = 3, Coverage = 17, NetPremium = 196000M },
            //    new Proposal{ ProposalNumber = 84, InsuranceName = "Insurance_4", InsuranceId = 4, Coverage = 17, NetPremium = 206000M },
            //    new Proposal{ ProposalNumber = 85, InsuranceName = "Insurance_5", InsuranceId = 5, Coverage = 17, NetPremium = 216000M },
            //    new Proposal{ ProposalNumber = 86, InsuranceName = "Insurance_1", InsuranceId = 1, Coverage = 18, NetPremium = 187000M },
            //    new Proposal{ ProposalNumber = 87, InsuranceName = "Insurance_2", InsuranceId = 2, Coverage = 18, NetPremium = 197000M },
            //    new Proposal{ ProposalNumber = 88, InsuranceName = "Insurance_3", InsuranceId = 3, Coverage = 18, NetPremium = 207000M },
            //    new Proposal{ ProposalNumber = 89, InsuranceName = "Insurance_4", InsuranceId = 4, Coverage = 18, NetPremium = 217000M },
            //    new Proposal{ ProposalNumber = 90, InsuranceName = "Insurance_5", InsuranceId = 5, Coverage = 18, NetPremium = 227000M },
            //    new Proposal{ ProposalNumber = 91, InsuranceName = "Insurance_1", InsuranceId = 1, Coverage = 19, NetPremium = 198000M },
            //    new Proposal{ ProposalNumber = 92, InsuranceName = "Insurance_2", InsuranceId = 2, Coverage = 19, NetPremium = 208000M },
            //    new Proposal{ ProposalNumber = 93, InsuranceName = "Insurance_3", InsuranceId = 3, Coverage = 19, NetPremium = 218000M },
            //    new Proposal{ ProposalNumber = 94, InsuranceName = "Insurance_4", InsuranceId = 4, Coverage = 19, NetPremium = 228000M },
            //    new Proposal{ ProposalNumber = 95, InsuranceName = "Insurance_5", InsuranceId = 5, Coverage = 19, NetPremium = 238000M },
            //    new Proposal{ ProposalNumber = 96, InsuranceName = "Insurance_1", InsuranceId = 1, Coverage = 20, NetPremium = 209000M },
            //    new Proposal{ ProposalNumber = 97, InsuranceName = "Insurance_2", InsuranceId = 2, Coverage = 20, NetPremium = 219000M },
            //    new Proposal{ ProposalNumber = 98, InsuranceName = "Insurance_3", InsuranceId = 3, Coverage = 20, NetPremium = 229000M },
            //    new Proposal{ ProposalNumber = 99, InsuranceName = "Insurance_4", InsuranceId = 4, Coverage = 20, NetPremium = 239000M },
            //    new Proposal{ ProposalNumber = 100, InsuranceName = "Insurance_5", InsuranceId = 5, Coverage = 20, NetPremium = 249000M }
            //};
        }
        private static List<ReadOnlyProposal> CreateSortedProposal()
        {
            return CreateProposal()
                .Select(p => new ReadOnlyProposal(p))
                .OrderBy(p => p.InsuranceId)
                .ToList();
        }
        private static Dictionary<int, ImmutableArray<ReadOnlyProposal>> CreateKeyedSortedProposal()
        {
            return CreateProposal()
                .Select(p => new ReadOnlyProposal(p))
                .GroupBy(p => p.InsuranceId)
                .ToDictionary(
                    p => p.Key,
                    p => ImmutableArray.CreateRange(p.OrderByDescending(p => p.NetPremium).ToList()));
        }
        private static ImmutableArray<ReadOnlyProposal>[] CreatePositionalSortedProposal()
        {
            var grouped = CreateProposal()
                .Select(p => new ReadOnlyProposal(p))
                .GroupBy(p => p.InsuranceId)
                .ToList();
            ;
            ImmutableArray<ReadOnlyProposal>[] result = new ImmutableArray<ReadOnlyProposal>[grouped.Max(p => p.Key) + 1];
            foreach (var p in grouped)
                result[p.Key] = ImmutableArray.CreateRange(p.OrderByDescending(ps => ps.NetPremium).ToList());

            return result;
        }
        private static ProposalResult[] CreatePositionalSortedVectorizedProposal()
        {
            var grouped = CreateProposal()
                .Select(p => new ReadOnlyProposal(p))
                .GroupBy(p => p.InsuranceId)
                .ToList();
            
            ProposalResult[] result = new ProposalResult[grouped.Max(p => p.Key) + 1];
            foreach (var p in grouped)
                result[p.Key] = new ProposalResult(p.OrderByDescending(ps => ps.NetPremium).ToArray());

            return result;
        }

        private static ProposalResult CreateSortedVectorizedProposal()
        {
            var readonlyProposals = CreateProposal()
                .Select(p => new ReadOnlyProposal(p))
                .OrderBy(p => p.InsuranceId)
                .ThenByDescending(p => p.NetPremium)
                .ToArray();

            return new ProposalResult(readonlyProposals);
        }

        public static List<Proposal> GetInsurances() => proposals.Value;
        public static List<ReadOnlyProposal> GetSortedInsurances() => sortedProposals.Value;
        public static Dictionary<int, ImmutableArray<ReadOnlyProposal>> GetKeyedSortedInsurances() => keyedSortedProposals.Value;
        public static ImmutableArray<ReadOnlyProposal>[] GetPositionalSortedInsurances() => positionalSortedProposals.Value;
        public static ProposalResult[] GetPositionalSortedVectorizedInsurances() => positionalSortedVectorizedProposals.Value;
        public static ProposalResult GetSortedVectorizedInsurances() => sortedVectorizedProposals.Value;
    }
}