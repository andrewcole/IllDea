namespace Illallangi.IllDea.PowerShell.Document
{
    using System;
    using System.Linq;
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    [Cmdlet(VerbsCommon.Remove, Nouns.Document)]
    public sealed class RemoveDocumentCmdlet : DeaCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public Guid Id { get; set; }

        protected override void ProcessRecord()
        {
            this.Client.Document.Delete(
                this.CompanyId,
                this.Client.Document.Retrieve(this.CompanyId).Single(a => a.Id.Equals(this.Id)),
                this.ToString());
        }

        public override string ToString()
        {
            return string.Format(
                @"Remove-Document -Id ""{0}""",
                this.Id);
        }
    }
}