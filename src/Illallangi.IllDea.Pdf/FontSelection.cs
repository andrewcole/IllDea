namespace Illallangi.IllDea.Pdf
{
    using iTextSharp.text;

    public sealed class FontSelection
    {
        private Font currentBody;

        private Font currentBodyItalic;

        private Font currentAccountTypeHeader;

        private Font currentCompanyHeader;

        private Font currentDocumentHeader;

        public Font Body
        {
            get
            {
                return this.currentBody
                       ?? (this.currentBody = FontFactory.GetFont("Arial", 10, BaseColor.BLACK));
            }
        }

        public Font BodyItalic
        {
            get
            {
                return this.currentBodyItalic
                       ?? (this.currentBodyItalic = FontFactory.GetFont("Arial", 10, Font.ITALIC, BaseColor.BLACK));
            }
        }

        public Font AccountTypeHeader
        {
            get
            {
                return this.currentAccountTypeHeader
                       ?? (this.currentAccountTypeHeader = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK));
            }
        }

        public Font CompanyHeader
        {
            get
            {
                return this.currentCompanyHeader
                       ?? (this.currentCompanyHeader = FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.BLACK));
            }
        }

        public Font DocumentHeader
        {
            get
            {
                return this.currentDocumentHeader
                       ?? (this.currentDocumentHeader = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK));
            }
        }

    }
}