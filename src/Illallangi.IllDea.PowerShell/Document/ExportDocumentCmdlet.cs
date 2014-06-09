namespace Illallangi.IllDea.PowerShell.Document
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Management.Automation;
    using System.Net;

    using Illallangi.IllDea.Model;

    [Cmdlet(VerbsData.Export, Nouns.Document)]
    public sealed class ExportDocumentCmdlet : DeaCmdlet
    {
        #region Fields

        private const string FilterParameterSet = @"Filter";
        private const string IdParameterSet = @"Id";

        private string currentTitle;

        private WildcardPattern currentTitleWildcardPattern;

        #endregion

        [Parameter]
        public string Path { get; set; }

        [Parameter]
        public SwitchParameter Open { get; set; }


        [Parameter(Mandatory = true, ParameterSetName = ExportDocumentCmdlet.IdParameterSet, ValueFromPipelineByPropertyName = true)]
        public Guid? Id { get; set; }

        [SupportsWildcards]
        [Parameter(Position = 1, ParameterSetName = ExportDocumentCmdlet.FilterParameterSet)]
        public string Title
        {
            get
            {
                return this.currentTitle;
            }
            set
            {
                this.currentTitleWildcardPattern = null;
                this.currentTitle = value;
            }
        }

        protected override void ProcessRecord()
        {
            foreach (var document in this.Client.Document.Retrieve(this.CompanyId).Where(this.IsMatch))
            {
                var fileName = this.Path ?? ExportDocumentCmdlet.GetPath();

                using (var wc = new WebClient())
                {
                    wc.DownloadFile(document.Uri, fileName);
                }

                if ((null == this.Path) || this.Open.ToBool())
                {
                    Process.Start(fileName);
                }
            }
        }

        private bool IsMatch(IDocument document)
        {
            switch (this.ParameterSetName)
            {
                case ExportDocumentCmdlet.IdParameterSet:
                    return this.Id.HasValue &&
                           this.Id.Value.Equals(document.Id);

                case ExportDocumentCmdlet.FilterParameterSet:
                    return this.TitleWildcardPattern.IsMatch(document.Title);

                default:
                    throw new NotImplementedException(this.ParameterSetName);
            }
        }

        private WildcardPattern TitleWildcardPattern
        {
            get
            {
                return this.currentTitleWildcardPattern
                       ?? (this.currentTitleWildcardPattern =
                           new WildcardPattern(this.Title ?? @"*", WildcardOptions.Compiled | WildcardOptions.IgnoreCase));
            }
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
