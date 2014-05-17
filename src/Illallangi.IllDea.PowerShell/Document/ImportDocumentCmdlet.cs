namespace Illallangi.IllDea.PowerShell.Document
{
    using System;
    using System.IO;
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    [Cmdlet(VerbsData.Import, Nouns.Document)]
    public sealed class ImportDocumentCmdlet : IdDeaCmdlet, IDocument
    {
        private DateTime? currentDate;

        public Guid Period { get; set; }

        [Parameter(Mandatory = false)]
        public DateTime Date
        {
            get
            {
                return (this.currentDate.HasValue ? this.currentDate : (this.currentDate = DateTime.Now)).Value;
            }
            set
            {
                this.currentDate = value;
            }
        }

        [Parameter(Mandatory = true)]
        public string Title { get; set; }

        [Parameter]
        public SwitchParameter Compilation { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        public string Path
        {
            get
            {
                return this.Uri.LocalPath;
            }
            set
            {
                var path = System.IO.Path.GetFullPath(value);
                if (!File.Exists(path))
                {
                    throw new Exception("Path does not exist");
                }
                this.Uri = new Uri(path);
            }
        }

        protected override void ProcessRecord()
        {
            this.WriteObject(this.Client.Document.Create(this.CompanyId, this, this.ToString()));
        }

        public override string ToString()
        {
            return string.Format(
                @"Import-Document -Name ""{0}"" -Date ""{1}""{2}",
                this.Title,
                this.Date,
                this.Compilation.ToBool() ? " -Compilation" : string.Empty);
        }

        bool IDocument.Compilation
        {
            get
            {
                return this.Compilation.ToBool();
            }
            set
            {
                this.Compilation = new SwitchParameter(value);
            }
        }

        public Uri Uri { get; set; }
        
        //Stream IDocument.Stream
        //{
        //    get
        //    {
        //        return new FileStream(this.Path, FileMode.Open, FileAccess.Read);
        //    }
        //}
    }
}