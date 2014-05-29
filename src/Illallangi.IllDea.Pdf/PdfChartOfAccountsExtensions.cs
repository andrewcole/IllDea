namespace Illallangi.IllDea.Pdf
{
    using System;
    using System.IO;
    using System.Linq;

    using Illallangi.IllDea.Client;

    using iTextSharp.text;
    using iTextSharp.text.pdf;

    public static class PdfChartOfAccountsExtensions
    {
        public static void CreateChartOfAccounts(this IDeaClient client, Guid companyId, Guid periodId, Stream stream)
        {
            using (var document = new Document(new Rectangle(PageSize.A4), 72, 72, 72, 72))
            using (var writer = PdfWriter.GetInstance(document, stream))
            {
                document.Open();
                
                client.CreateChartOfAccounts(companyId, periodId, document);
                
                document.Close();
                writer.Close();
            }
        }

        internal static void CreateChartOfAccounts(this IDeaClient client, Guid companyId, Guid periodId, Document document)
        {
            var table = new PdfPTable(2) { WidthPercentage = 100 };

            table.SetWidths(new[] { 21, 4 });

            table
                .AddPageHeaderCell(@"Chart of Accounts").Go()
                .AddColumnHeaderCell().Go()
                .AddColumnHeaderCell(@"Account No.").Inverted().Go();
            
            foreach (var accountType in Enum.GetValues(typeof(AccountType)).Cast<AccountType>())
            {
                table
                    .AddBodyCell().Go()
                    .AddBodyCell().Inverted().Go();
                
                table
                    .AddHeaderCell(accountType.ToString()).Go()
                    .AddHeaderCell().Inverted().Go();

                foreach (var account in client.Account.Retrieve(companyId).Where(a => a.Type.Equals(accountType)))
                {
                    table.AddBodyCell(account.Name).WithTabStops().Go()
                         .AddBodyCell(account.Number).Inverted().CenterAligned().Go();
                }
            }

            document.NewPage();
            document.Add(table);
        }
    }
}
