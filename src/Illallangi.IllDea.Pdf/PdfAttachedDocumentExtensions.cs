namespace Illallangi.IllDea.Pdf
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;

    using Illallangi.IllDea.Client;

    using iTextSharp.text;
    using iTextSharp.text.pdf;

    public static class PdfAttachedDocumentExtensions
    {
        private static FontSelection staticFont;

        private static FontSelection Font
        {
            get
            {
                return PdfAttachedDocumentExtensions.staticFont ?? (PdfAttachedDocumentExtensions.staticFont = new FontSelection());
            }
        }

        public static void CreateAttachedDocument(this IDeaClient client, Guid companyId, Guid periodId, Guid documentId, Stream stream)
        {
            using (var document = new Document(new Rectangle(PageSize.A4), 72, 72, 72, 72))
            using (var writer = PdfWriter.GetInstance(document, stream))
            {
                document.Open();

                client.CreateAttachedDocument(companyId, periodId, documentId, document, writer);

                document.Close();
                writer.Close();
            }
        }

        internal static void CreateAttachedDocument(this IDeaClient client, Guid companyId, Guid periodId, Guid documentId, Document document, PdfWriter writer)
        {
            var company = client.Company.Retrieve().Single(c => c.Id.Equals(companyId));
            var attachment = client.Document.Retrieve(companyId).Single(d => d.Id.Equals(documentId));

            var table = new PdfPTable(6) { WidthPercentage = 100 };

            table.SetWidths(new[] { 30, 21, 350, 45, 85, 85 });

            table.AddCell(new PdfPCell(new Phrase(company.Name.ToUpper(), PdfAttachedDocumentExtensions.Font.CompanyHeader)) { Colspan = 6, HorizontalAlignment = 1, Border = Rectangle.NO_BORDER });
            table.AddCell(new PdfPCell(new Phrase(string.Format(@"{0} - {1}", attachment.Date.ToString("yyyy-MM-dd"), attachment.Title), PdfAttachedDocumentExtensions.Font.DocumentHeader)) { Colspan = 6, HorizontalAlignment = 1, Border = Rectangle.NO_BORDER });
            
            document.NewPage();
            document.Add(table);

            using (var reader = new PdfReader(attachment.Uri))
            {
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    var page = writer.GetImportedPage(reader, i);
                    var pageSize = reader.GetPageSizeWithRotation(i);

                    document.SetPageSize(pageSize);
                    document.NewPage();

                    switch (pageSize.Rotation)
                    {
                        case 90:
                        case 270:
                            writer.DirectContent.AddTemplate(page, 0, -1f, 1f, 0, 0, pageSize.Height);
                            break;

                        case 0:
                            writer.DirectContent.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                            break;

                        case 180:
                            writer.DirectContent.AddTemplate(page, -1f, 0, 0, -1f, pageSize.Width, pageSize.Height);
                            break;

                        default:
                            throw new NotImplementedException(string.Format("PDF Rotation of {0} invalid", pageSize.Rotation));
                    }
                }

                writer.FreeReader(reader);
            }
        }
    }
}
