namespace Illallangi.IllDea.Pdf
{
    using System;
    using System.IO;
    using System.Linq;

    using Illallangi.IllDea.Client;

    using iTextSharp.text;
    using iTextSharp.text.pdf;

    public static class PdfBookExtensions
    {
        public static void CreateBook(this IDeaClient client, Guid companyId, Stream stream)
        {
            using (var document = new Document(new Rectangle(PageSize.A4), 72, 72, 72, 72))
            using (var writer = PdfWriter.GetInstance(document, stream))
            {
                document.Open();

                client.CreateEmployeeRegister(companyId, document);

                foreach (var periodId in client.Period.Retrieve(companyId).Select(p => p.Id))
                {
                    client.CreatePeriodCoverPage(companyId, periodId, document);

                    client.CreateChartOfAccounts(companyId, periodId, document);

                    client.CreateGeneralJournal(companyId, periodId, document);

                    foreach (var accountId in client.Account.Retrieve(companyId).Where(a => client.Txn.Retrieve(companyId).Any(t => t.Period.Equals(periodId) && t.Items.Any(i => i.Account.Equals(a.Id)))).Select(a => a.Id))
                    {
                        client.CreateAccountBook(companyId, periodId, accountId, document);
                    }

                    foreach (var documentId in client.Document.Retrieve(companyId).Where(d => d.Period.Equals(periodId)).Select(d => d.Id))
                    {
                        client.CreateAttachedDocument(companyId, periodId, documentId, document, writer);
                    }
                }

                document.Close();
                writer.Close();
            }
        }
    }
}