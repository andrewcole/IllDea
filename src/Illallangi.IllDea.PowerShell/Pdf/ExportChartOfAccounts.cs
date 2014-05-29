namespace Illallangi.IllDea.PowerShell.Pdf
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Management.Automation;

    using Illallangi.IllDea.Pdf;

    [Cmdlet(VerbsData.Export, Nouns.Pdf.ChartOfAccounts)]
    public sealed class ExportChartOfAccounts : DeaCmdlet
    {
        [Parameter(Position = 1)]
        public string Path { get; set; }

        [Parameter]
        public SwitchParameter Open { get; set; }

        [Parameter(Mandatory = true)]
        public Guid PeriodId { get; set; }

        protected override void BeginProcessing()
        {
            var fileName = this.Path ?? ExportChartOfAccounts.GetPath();

            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                this.Client.CreateChartOfAccounts(this.CompanyId, this.PeriodId, fs);
            }

            if ((null == this.Path) || this.Open.ToBool())
            {
                Process.Start(fileName);
            }
        }

        public override string ToString()
        {
            return string.Format(@"Export-PdfChartOfAccounts");
        }

        private static string GetPath()
        {
            var path = string.Empty;
            while (string.IsNullOrWhiteSpace(path) || File.Exists(path))
            {
                path = System.IO.Path.Combine(
                            System.IO.Path.GetTempPath(),
                            System.IO.Path.ChangeExtension(Guid.NewGuid().ToString(), @"pdf"));
            }

            return path;
        }
    }
}