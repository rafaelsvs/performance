using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics;
using System.Text;

namespace CSharpBenchmark.Linq
{
    public class ProposalResult
    {
        public readonly ReadOnlyProposal[] Proposals;
        public readonly int[] InsuranceIds;
        public readonly long[] NetPremiums;

        public ProposalResult(ReadOnlyProposal[] proposals)
        {
            this.Proposals = Pad(proposals, Vector256<int>.Count);
            this.NetPremiums = Pad(proposals.Select(p => decimal.ToOACurrency(p.NetPremium)).ToArray(), Vector256<int>.Count)
                .ToArray();
            this.InsuranceIds = Pad(proposals.Select(p => p.InsuranceId).ToArray(), Vector256<int>.Count)
                .ToArray();
        }

        private T[] Pad<T>(T[] proposals, int vectorCount)
        {
            var result = new T[(int)Math.Ceiling(proposals.Length / (float)vectorCount) * vectorCount];
            proposals.CopyTo(result, 0);
            return result;
        }
    }
}