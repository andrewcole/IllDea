namespace Illallangi.IllDea.Pdf
{
    using System;
    using System.IO;
    using System.Linq;

    using Illallangi.IllDea.Client;

    using iTextSharp.text;
    using iTextSharp.text.pdf;

    public static class PdfPeriodCoverPageExtensions
    {
        private static FontSelection staticFont;

        private static FontSelection Font
        {
            get
            {
                return PdfPeriodCoverPageExtensions.staticFont ?? (PdfPeriodCoverPageExtensions.staticFont = new FontSelection());
            }
        }

        public static void CreatePeriodCoverPage(this IDeaClient client, Guid companyId, Guid periodId, Stream stream)
        {
            using (var document = new Document(new Rectangle(PageSize.A4), 72, 72, 72, 72))
            using (var writer = PdfWriter.GetInstance(document, stream))
            {
                document.Open();

                client.CreatePeriodCoverPage(companyId, periodId, document);

                document.Close();
                writer.Close();
            }
        }

        internal static void CreatePeriodCoverPage(this IDeaClient client, Guid companyId, Guid periodId, Document document)
        {
            var company = client.Company.Retrieve().Single(c => c.Id.Equals(companyId));

            var table = new PdfPTable(1) { WidthPercentage = 100 };

            table.SetWidths(new[] { 616 });

            table
                .AddPageHeaderCell(company.Name).Go()
                .AddColumnHeaderCell(client.Period.Retrieve(companyId).Single(p => p.Id.Equals(periodId)).ToTitle()).Go();

            document.NewPage();
            document.Add(table);
        }
    }
}
