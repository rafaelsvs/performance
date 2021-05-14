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
        public readonly long[] NetPremiums;

        public ProposalResult(ReadOnlyProposal[] proposals)
        {
            this.Proposals = Pad(proposals);
            this.NetPremiums = Pad(proposals.Select(p => decimal.ToOACurrency(p.NetPremium)).ToArray())
                .ToArray();
        }

        private T[] Pad<T>(T[] proposals)
        {
            var result = new T[(int)Math.Ceiling(proposals.Length / (float)Vector256<long>.Count) * Vector256<long>.Count];
            proposals.CopyTo(result, 0);
            return result;
        }
    }
}
