namespace Illallangi.IllDea.PowerShell.Pdf
{
    using System.IO;
    using System.Management.Automation;

    using Illallangi.IllDea.Pdf;

    [Cmdlet(VerbsData.Export, Nouns.Pdf.ChartOfAccounts)]
    public sealed class ExportChartOfAccounts : DeaCmdlet
    {
        [Parameter(Mandatory = true, Position = 1)]
        public string Path { get; set; }

        protected override void BeginProcessing()
        {
            using (var fs = new FileStream(this.Path, FileMode.Create, FileAccess.Write))
            {
                this.Client.CreateChartOfAccounts(this.CompanyId, fs);
            }
        }

        public override string ToString()
        {
            return string.Format(@"Export-PdfChartOfAccounts");
        }
    }
}