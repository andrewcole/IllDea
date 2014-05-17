namespace Illallangi.IllDea.PowerShell.Pdf
{
    using System;
    using System.IO;
    using System.Management.Automation;

    using Illallangi.IllDea.Pdf;

    [Cmdlet(VerbsData.Export, Nouns.Pdf.GeneralJournal)]
    public sealed class ExportGeneralJournal : DeaCmdlet
    {
        [Parameter(Mandatory = true, Position = 1)]
        public string Path { get; set; }

        [Parameter(Mandatory = true)]
        public Guid PeriodId { get; set; }

        protected override void BeginProcessing()
        {
            using (var fs = new FileStream(this.Path, FileMode.Create, FileAccess.Write))
            {
                this.Client.CreateGeneralJournal(this.CompanyId, this.PeriodId, fs);
            }
        }

        public override string ToString()
        {
            return string.Format(@"Export-GeneralJournal");
        }
    }
}