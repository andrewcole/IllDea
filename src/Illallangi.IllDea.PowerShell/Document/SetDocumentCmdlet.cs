namespace Illallangi.IllDea.PowerShell.Document
{
    using System;
    using System.IO;
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    [Cmdlet(VerbsCommon.Set, Nouns.Document)]
    public sealed class SetDocumentCmdlet : DeaCmdlet, IDocument
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public Guid Id { get; set; }

        public Guid Period { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public DateTime Date { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public string Title { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public bool Compilation { get; set; }

        protected override void ProcessRecord()
        {
            this.WriteObject(this.Client.Document.Update(this.CompanyId, this, this.ToString()));
        }

        public override string ToString()
        {
            return string.Format(
                @"Set-Document -Id {0} -Name ""{1}"" -Date ""{2}"" -Compilation ${3}",
                this.Id,
                this.Title,
                this.Date,
                this.Compilation);
        }

        Uri IDocument.Uri { get; set; }
        
        //Stream IDocument.Stream
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
    }
}