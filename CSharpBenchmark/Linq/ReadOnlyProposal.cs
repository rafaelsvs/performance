namespace CSharpBenchmark.Linq
{
    public class ReadOnlyProposal
    {
        public readonly int ProposalNumber;
        public readonly int InsuranceId;
        public readonly string InsuranceName;
        public readonly int Coverage;
        public readonly decimal NetPremium;

        public ReadOnlyProposal(Proposal proposal)
        {
            this.ProposalNumber = proposal.ProposalNumber;
            this.InsuranceId = proposal.InsuranceId;
            this.InsuranceName = proposal.InsuranceName;
            this.Coverage = proposal.Coverage;
            this.NetPremium = proposal.NetPremium;
        }
    }
}
