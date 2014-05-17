namespace Illallangi.IllDea.PowerShell.Txn
{
    using System;
    using System.Linq;
    using System.Management.Automation;

    [Cmdlet(VerbsCommon.Remove, Nouns.Txn)]
    public sealed class RemoveTxnCmdlet : DeaCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public Guid Id { get; set; }

        protected override void ProcessRecord()
        {
            this.Client.Txn.Delete(
                this.CompanyId,
                this.Client.Txn.Retrieve(this.CompanyId).Single(t => t.Id.Equals(this.Id)),
                this.ToString());
        }

        public override string ToString()
        {
            return string.Format(
                @"Remove-Txn -Id ""{0}""",
                this.Id);
        }
    }
}