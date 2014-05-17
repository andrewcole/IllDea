namespace Illallangi.IllDea.PowerShell.Document
{
    using System.Linq;
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    [Cmdlet(VerbsCommon.Get, Nouns.Document)]
    public sealed class GetDocumentCmdlet : DeaCmdlet
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
            this.WriteObject(this.Client.Document.Retrieve(this.CompanyId).Where(this.IsMatch), true);
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
