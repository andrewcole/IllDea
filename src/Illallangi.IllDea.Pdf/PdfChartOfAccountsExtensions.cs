﻿namespace Illallangi.IllDea.Pdf
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
            var table = new PdfPTable(4) { WidthPercentage = 100 };

            table.SetWidths(new[] { 401, 85, 85, 85 });

            table
                .AddPageHeaderCell(@"Chart of Accounts").Go()
                .AddColumnHeaderCell().Go()
                .AddColumnHeaderCell(@"Account No.").Inverted().Go()
                .AddColumnHeaderCell(@"Opening").Go()
                .AddColumnHeaderCell(@"Closing").Inverted().Go();
            
            foreach (var accountType in Enum.GetValues(typeof(AccountType)).Cast<AccountType>())
            {
                decimal open = 0;
                decimal close = 0;

                table
                    .AddBodyCell().Go()
                    .AddBodyCell().Inverted().Go()
                    .AddBodyCell().Go()
                    .AddBodyCell().Inverted().Go();
                
                table
                    .AddHeaderCell(accountType.ToString()).Go()
                    .AddHeaderCell().Inverted().Go()
                    .AddBodyCell().Go()
                    .AddBodyCell().Inverted().Go();

                foreach (var account in client.Account.RetrieveWithBalances(companyId, periodId, accountType))
                {
                    table
                        .AddBodyCell(account.Name).WithTabStops().Go()
                        .AddBodyCell(account.Number).Inverted().CenterAligned().Go()
                        .AddBodyCell(account.Opening.ToString(@"C")).RightAligned().Go()
                        .AddBodyCell(account.Closing.ToString(@"C")).Inverted().RightAligned().Go();

                    open += account.Opening;
                    close += account.Closing;
                }

                table
                    .AddBodyCell("Total").WithTabStops().Go()
                    .AddBodyCell().Inverted().CenterAligned().Go()
                    .AddBodyCell(open.ToString(@"C")).RightAligned().Go()
                    .AddBodyCell(close.ToString(@"C")).Inverted().RightAligned().Go();
            }

            document.NewPage();
            document.Add(table);
        }
    }
}
