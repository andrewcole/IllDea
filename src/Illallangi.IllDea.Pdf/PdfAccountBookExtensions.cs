namespace Illallangi.IllDea.Pdf
{
    using System;
    using System.IO;
    using System.Linq;

    using Illallangi.IllDea.Client;

    using iTextSharp.text;
    using iTextSharp.text.pdf;

    public static class PdfAccountBookExtensions
    {
        private static FontSelection staticFont;

        private static FontSelection Font
        {
            get
            {
                return PdfAccountBookExtensions.staticFont ?? (PdfAccountBookExtensions.staticFont = new FontSelection());
            }
        }

        public static void CreateAccountBook(this IDeaClient client, Guid companyId, Guid periodId, Guid accountId, Stream stream)
        {
            using (var document = new Document(new Rectangle(PageSize.A4), 72, 72, 72, 72))
            using (var writer = PdfWriter.GetInstance(document, stream))
            {
                document.Open();

                client.CreateAccountBook(companyId, periodId, accountId, document);

                document.Close();
                writer.Close();
            }
        }

        internal static void CreateAccountBook(this IDeaClient client, Guid companyId, Guid periodId, Guid accountId, Document document)
        {
            var account = client.Account.Retrieve(companyId).Single(a => a.Id.Equals(accountId));
            var period = client.Period.Retrieve(companyId).Single(p => p.Id.Equals(periodId));

            var table = new PdfPTable(7) { WidthPercentage = 100 };

            table.SetWidths(new[] { 32, 21, 263, 45, 85, 85, 85 });

            table
                .AddPageHeaderCell(string.Format("{0} A/C {1}: {2}", account.Type, account.Number, account.Name)).Go();

            table
                .AddColumnHeaderCell().WithColspan(2).Go()
                .AddColumnHeaderCell().Go()
                .AddColumnHeaderCell("Post Ref").Inverted().Go()
                .AddColumnHeaderCell("Debit").Go()
                .AddColumnHeaderCell("Credit").Inverted().Go()
                .AddColumnHeaderCell("Balance").Go();


            var txns = client.Txn.RetrieveWithBalances(companyId, periodId, accountId).ToList();

            var year = period.Start.Year.ToString();
            var month = period.Start.ToString("MMM");
            var day = period.Start.Day.ToString();

            table
                .AddBodyCell(year).WithColspan(2).CenterAligned().Go()
                .AddBodyCell().Go()
                .AddBodyCell().Inverted().Go()
                .AddBodyCell().Go()
                .AddBodyCell().Inverted().Go()
                .AddBodyCell().Go();

            table
                .AddBodyCell(month).CenterAligned().Go()
                .AddBodyCell(day).CenterAligned().Go()
                .AddBodyCell(@"Opening Balance").Go()
                .AddBodyCell().Inverted().Go()
                .AddBodyCell().Go()
                .AddBodyCell().Inverted().Go()
                .AddBodyCell(txns.First().Items.Single(i => i.Account.Equals(accountId)).BalanceBefore.ToString(@"C")).RightAligned().Go();
            
            table
                .AddBodyCell().Go()
                .AddBodyCell().Go()
                .AddBodyCell().Go()
                .AddBodyCell().Inverted().Go()
                .AddBodyCell().Go()
                .AddBodyCell().Inverted().Go()
                .AddBodyCell().Go();

            foreach (var txn in txns)
            {
                if (year != txn.Date.Year.ToString())
                {
                    year = txn.Date.Year.ToString();
                    table
                        .AddBodyCell(year).WithColspan(2).CenterAligned().Go()
                        .AddBodyCell().Go()
                        .AddBodyCell().Inverted().Go()
                        .AddBodyCell().Go()
                        .AddBodyCell().Inverted().Go()
                        .AddBodyCell().Go();
                }

                var item = txn.Items.Single(i => i.Account.Equals(accountId));

                if (month != txn.Date.ToString("MMM"))
                {
                    table.AddBodyCell(month = txn.Date.ToString("MMM")).CenterAligned().Go();
                }
                else
                {
                    table.AddBodyCell().Go();
                }

                if (day != txn.Date.Day.ToString())
                {
                    day = txn.Date.Day.ToString();
                    table
                        .AddBodyCell(day).CenterAligned().Go();
                }
                else
                {
                    table
                        .AddBodyCell().Go();
                }

                var explanation = string.Join(
                    "/",
                    txn.Items
                        .Where(i => i.IsDebit != item.IsDebit)
                        .OrderByDescending(i => Math.Abs(i.Amount))
                        .Select(i => client.Account.Retrieve(companyId).Single(a => a.Id.Equals(i.Account)).Name));

                table
                    .AddBodyCell(explanation).Go()
                    .AddBodyCell(client.Account.Retrieve(companyId).Single(a => a.Id.Equals(item.Account)).Number).CenterAligned().Inverted().Go();


                if (item.IsDebit)
                {
                    table
                        .AddBodyCell((0 - item.Amount).ToString(@"C")).RightAligned().Go()
                        .AddBodyCell().Inverted().Go();
                }
                else
                {
                    table
                        .AddBodyCell().Go()
                        .AddBodyCell((0 + item.Amount).ToString(@"C")).RightAligned().Inverted().Go();
                }

                table
                    .AddBodyCell(item.BalanceAfter.ToString(@"C")).RightAligned().Go();

                table
                    .AddBodyCell().Go()
                    .AddBodyCell().Go()
                    .AddItalicisedBodyCell(txn.Description.Trim()).Go()
                    .AddBodyCell().Inverted().Go()
                    .AddBodyCell().Go()
                    .AddBodyCell().Inverted().Go()
                    .AddBodyCell().Go();

                table
                    .AddBodyCell().Go()
                    .AddBodyCell().Go()
                    .AddBodyCell().Go()
                    .AddBodyCell().Inverted().Go()
                    .AddBodyCell().Go()
                    .AddBodyCell().Inverted().Go()
                    .AddBodyCell().Go();
            }

            if (year != period.End.Year.ToString())
            {
                year = period.End.Year.ToString();
                table
                    .AddBodyCell(year).WithColspan(2).CenterAligned().Go()
                    .AddBodyCell().Go()
                    .AddBodyCell().Inverted().Go()
                    .AddBodyCell().Go()
                    .AddBodyCell().Inverted().Go()
                    .AddBodyCell().Go();
            }

            if (month != period.End.ToString("MMM"))
            {
                table.AddBodyCell(month = period.End.ToString("MMM")).CenterAligned().Go();
            }
            else
            {
                table.AddBodyCell().Go();
            }

            if (day != period.End.Day.ToString())
            {
                day = period.End.Day.ToString();
                table
                    .AddBodyCell(day).CenterAligned().Go();
            }
            else
            {
                table
                    .AddBodyCell().Go();
            }

            table
                .AddBodyCell(@"Closing Balance").Go()
                .AddBodyCell().Inverted().Go()
                .AddBodyCell().Go()
                .AddBodyCell().Inverted().Go()
                .AddBodyCell(txns.Last().Items.Single(i => i.Account.Equals(accountId)).BalanceAfter.ToString(@"C")).RightAligned().Go();
            
            document.NewPage();
            document.Add(table);
        }
    }
}
