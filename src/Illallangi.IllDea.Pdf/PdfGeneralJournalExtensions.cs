namespace Illallangi.IllDea.Pdf
{
    using System;
    using System.IO;
    using System.Linq;

    using Illallangi.IllDea.Client;

    using iTextSharp.text;
    using iTextSharp.text.pdf;

    public static class PdfGeneralJournalExtensions
    {
        private static FontSelection staticFont;

        private static FontSelection Font
        {
            get
            {
                return PdfGeneralJournalExtensions.staticFont ?? (PdfGeneralJournalExtensions.staticFont = new FontSelection());
            }
        }

        public static void CreateGeneralJournal(this IDeaClient client, Guid companyId, Guid periodId, Stream stream)
        {
            using (var document = new Document(new Rectangle(PageSize.A4), 72, 72, 72, 72))
            using (var writer = PdfWriter.GetInstance(document, stream))
            {
                document.Open();

                client.CreateGeneralJournal(companyId, periodId, document);

                document.Close();
                writer.Close();
            }
        }

        internal static void CreateGeneralJournal(this IDeaClient client, Guid companyId, Guid periodId, Document document)
        {
            var table = new PdfPTable(6) { WidthPercentage = 100 };

            table.SetWidths(new[] { 32, 21, 348, 45, 85, 85 });

            table
                .AddPageHeaderCell(@"General Journal").Go()
                .AddColumnHeaderCell().Go()
                .AddColumnHeaderCell().Go()
                .AddColumnHeaderCell().Go()
                .AddColumnHeaderCell(@"Post Ref").Inverted().Go()
                .AddColumnHeaderCell(@"Dr").Go()
                .AddColumnHeaderCell(@"Cr").Inverted().Go();

            var year = string.Empty;
            var month = string.Empty;

            foreach (var txn in client.Txn.Retrieve(companyId).Where(txn => txn.Period.Equals(periodId)))
            {
                if (year != txn.Date.Year.ToString())
                {
                    year = txn.Date.Year.ToString();
                    table
                        .AddBodyCell(year).WithColspan(2).CenterAligned().Go()
                        .AddBodyCell().Go()
                        .AddBodyCell().Inverted().Go()
                        .AddBodyCell().Go()
                        .AddBodyCell().Inverted().Go();
                }

                var date = false;

                foreach (var item in txn.Items.Where(i => i.Amount < 0).OrderBy(i => i.Amount))
                {
                    if (!date)
                    {
                        if (month != txn.Date.ToString("MMM"))
                        {
                            table.AddBodyCell(month = txn.Date.ToString("MMM")).CenterAligned().Go();
                        }
                        else
                        {
                            table.AddBodyCell().Go();
                        }

                        table.AddBodyCell(txn.Date.Day.ToString()).CenterAligned().Go();
                        date = true;
                    }
                    else
                    {
                        table
                            .AddBodyCell().Go()
                            .AddBodyCell().Go();
                    }

                    table
                        .AddBodyCell(client.Account.Retrieve(companyId).Single(a => a.Id.Equals(item.Account)).Name).Go()
                        .AddBodyCell(client.Account.Retrieve(companyId).Single(a => a.Id.Equals(item.Account)).Number).CenterAligned().Inverted().Go()
                        .AddBodyCell((0 - item.Amount).ToString(@"C")).RightAligned().Go()
                        .AddBodyCell().Inverted().Go();
                }

                foreach (var item in txn.Items.Where(i => i.Amount > 0).OrderByDescending(i => i.Amount))
                {
                    table
                        .AddBodyCell().Go()
                        .AddBodyCell().Go()
                        .AddBodyCell(string.Concat("    ", client.Account.Retrieve(companyId).Single(a => a.Id.Equals(item.Account)).Name)).Go()
                        .AddBodyCell(client.Account.Retrieve(companyId).Single(a => a.Id.Equals(item.Account)).Number).CenterAligned().Inverted().Go()
                        .AddBodyCell().Go()
                        .AddBodyCell(item.Amount.ToString(@"C")).Inverted().RightAligned().Go();
                }

                table
                    .AddBodyCell().Go()
                    .AddBodyCell().Go()
                    .AddItalicisedBodyCell(txn.Description.Trim()).Go()
                    .AddBodyCell().Inverted().Go()
                    .AddBodyCell().Go()
                    .AddBodyCell().Inverted().Go();

                table
                    .AddBodyCell().Go()
                    .AddBodyCell().Go()
                    .AddBodyCell().Go()
                    .AddBodyCell().Inverted().Go()
                    .AddBodyCell().Go()
                    .AddBodyCell().Inverted().Go();
            }

            document.NewPage();
            document.Add(table);
        }
    }
}
