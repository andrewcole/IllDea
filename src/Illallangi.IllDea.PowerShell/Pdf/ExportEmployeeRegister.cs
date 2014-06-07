namespace Illallangi.IllDea.PowerShell.Pdf
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Management.Automation;

    using Illallangi.IllDea.Pdf;

    [Cmdlet(VerbsData.Export, Nouns.Pdf.EmployeeRegister)]
    public sealed class ExportEmployeeRegister : DeaCmdlet
    {
        [Parameter(Position = 1)]
        public string Path { get; set; }

        [Parameter]
        public SwitchParameter Open { get; set; }

        protected override void BeginProcessing()
        {
            var fileName = this.Path ?? ExportEmployeeRegister.GetPath();

            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                this.Client.CreateEmployeeRegister(this.CompanyId, fs);
            }

            if ((null == this.Path) || this.Open.ToBool())
            {
                Process.Start(fileName);
            }
        }

        public override string ToString()
        {
            return string.Format(@"Export-PDFEmployeeRegister");
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