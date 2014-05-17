namespace Illallangi.IllDea.PowerShell.Document
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Management.Automation;
    using System.Net;

    using Illallangi.IllDea.Model;

    [Cmdlet(VerbsCommon.Open, Nouns.Document)]
    public sealed class OpenDocumentCmdlet : DeaCmdlet
    {
        private string currentTitle;

        private WildcardPattern currentTitleWildcardPattern;
        
        [SupportsWildcards]
        [Parameter(Position = 1)]
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
            var path = string.Empty;
            while (string.IsNullOrWhiteSpace(path) || File.Exists(path))
            {
                path = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(Guid.NewGuid().ToString(), @"pdf"));
            }

            var document = this.Client.Document.Retrieve(this.CompanyId).Single(this.IsMatch);
            using (var wc = new WebClient())
            {
                wc.DownloadFile(document.Uri, path);
            }

            Process.Start(path);
        }

        private bool IsMatch(IDocument document)
        {
            return this.TitleWildcardPattern.IsMatch(document.Title);
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
    }
}
