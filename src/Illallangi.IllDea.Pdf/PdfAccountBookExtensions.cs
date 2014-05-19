using System.Collections.Generic;
using Illallangi.IllDea.Client.Txn;
using Illallangi.IllDea.Model;

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
            var company = client.Company.Retrieve().Single(c => c.Id.Equals(companyId));
            var account = client.Account.Retrieve(companyId).Single(a => a.Id.Equals(accountId));
            var table = new PdfPTable(7) { WidthPercentage = 100 };

            table.SetWidths(new[] { 30, 21, 265, 45, 85, 85, 85 });

            table.AddCell(new PdfPCell(new Phrase(company.Name.ToUpper(), PdfAccountBookExtensions.Font.CompanyHeader)) { Colspan = 7, HorizontalAlignment = 1, Border = Rectangle.NO_BORDER });
            table.AddCell(new PdfPCell(new Phrase(string.Format("{0} A/C {1}: {2}", account.Type, account.Number, account.Name), PdfAccountBookExtensions.Font.DocumentHeader)) { Colspan = 7, HorizontalAlignment = 1, Border = Rectangle.NO_BORDER });

            table.AddCell(
                new PdfPCell(
                    new Phrase(client.Period.Retrieve(companyId).Single(p => p.Id.Equals(periodId)).ToTitle(), PdfAccountBookExtensions.Font.Body))
                    {
                        Colspan = 7,
                        HorizontalAlignment = 1,
                        Border = Rectangle.NO_BORDER,
                    });

            table.AddCell(new PdfPCell { MinimumHeight = 13f, Colspan = 7, Border = Rectangle.NO_BORDER });

            table.AddCell(new PdfPCell(new Phrase("Date", PdfAccountBookExtensions.Font.Body)) { BackgroundColor = BaseColor.LIGHT_GRAY, Colspan = 2 });
            table.AddCell(new PdfPCell(new Phrase("Explanation", PdfAccountBookExtensions.Font.Body)) { BackgroundColor = BaseColor.LIGHT_GRAY });
            table.AddCell(new PdfPCell(new Phrase("Post Ref", PdfAccountBookExtensions.Font.Body)) { BackgroundColor = BaseColor.LIGHT_GRAY });
            table.AddCell(new PdfPCell(new Phrase("Debit", PdfAccountBookExtensions.Font.Body)) { BackgroundColor = BaseColor.LIGHT_GRAY });
            table.AddCell(new PdfPCell(new Phrase("Credit", PdfAccountBookExtensions.Font.Body)) { BackgroundColor = BaseColor.LIGHT_GRAY });
            table.AddCell(new PdfPCell(new Phrase("Balance", PdfAccountBookExtensions.Font.Body)) { BackgroundColor = BaseColor.LIGHT_GRAY });

            var year = string.Empty;
            var month = string.Empty;
            var day = string.Empty;

            foreach (var txn in client.Txn.RetrieveWithBalances(companyId, periodId, accountId))
            {
                var item = txn.Items.Single(i => i.Account.Equals(accountId));

                if (year != txn.Date.Year.ToString())
                {
                    year = txn.Date.Year.ToString();
                    table.AddCell(
                        new PdfPCell(new Phrase(year, PdfAccountBookExtensions.Font.Body)) { Colspan = 2, HorizontalAlignment = 1 });
                    table.AddCell(new PdfPCell { MinimumHeight = 13f });
                    table.AddCell(new PdfPCell { MinimumHeight = 13f });
                    table.AddCell(new PdfPCell { MinimumHeight = 13f });
                    table.AddCell(new PdfPCell { MinimumHeight = 13f });
                    table.AddCell(new PdfPCell { MinimumHeight = 13f });
                }

                if (month != txn.Date.ToString("MMM"))
                {
                    month = txn.Date.ToString("MMM");
                    table.AddCell(new PdfPCell(new Phrase(month, PdfAccountBookExtensions.Font.Body)));
                }
                else
                {
                    table.AddCell(new PdfPCell { MinimumHeight = 13f });
                }

                if (day != txn.Date.Day.ToString())
                {
                    day = txn.Date.Day.ToString();
                    table.AddCell(
                        new PdfPCell(new Phrase(day, PdfAccountBookExtensions.Font.Body)) { HorizontalAlignment = 2 });
                }
                else
                {
                    table.AddCell(new PdfPCell { MinimumHeight = 13f });
                }

                var explanation = string.Join(
                    "/",
                    txn.Items
                        .Where(i => i.IsDebit != item.IsDebit)
                        .Select(i => client.Account.Retrieve(companyId).Single(a => a.Id.Equals(i.Account)).Name));

                table.AddCell(new PdfPCell(new Phrase(explanation, PdfAccountBookExtensions.Font.Body)));
                table.AddCell(new PdfPCell(new Phrase(client.Account.Retrieve(companyId).Single(a => a.Id.Equals(item.Account)).Number, PdfAccountBookExtensions.Font.Body)) { HorizontalAlignment = 1 });

                if (item.IsDebit)
                {
                    table.AddCell(new PdfPCell(new Phrase((0 - item.Amount).ToString(@"C"), PdfAccountBookExtensions.Font.Body)) { HorizontalAlignment = 2 });
                    table.AddCell(new PdfPCell { MinimumHeight = 13f });
                }
                else
                {
                    table.AddCell(new PdfPCell { MinimumHeight = 13f });
                    table.AddCell(new PdfPCell(new Phrase(item.Amount.ToString(@"C"), PdfAccountBookExtensions.Font.Body)) { HorizontalAlignment = 2 });
                }

                table.AddCell(new PdfPCell(new Phrase(item.BalanceAfter.ToString(@"C"), PdfAccountBookExtensions.Font.Body)) { HorizontalAlignment = 2 });

                table.AddCell(new PdfPCell { MinimumHeight = 13f });
                table.AddCell(new PdfPCell { MinimumHeight = 13f });

                var line = string.Empty;

                foreach (var word in txn.Description.Split())
                {
                    if (line.Length + word.Length + 1 > 55)
                    {
                        table.AddCell(new PdfPCell(new Phrase(line.Trim(), PdfAccountBookExtensions.Font.BodyItalic)));
                        table.AddCell(new PdfPCell { MinimumHeight = 13f });
                        table.AddCell(new PdfPCell { MinimumHeight = 13f });
                        table.AddCell(new PdfPCell { MinimumHeight = 13f });
                        table.AddCell(new PdfPCell { MinimumHeight = 13f });
                        table.AddCell(new PdfPCell { MinimumHeight = 13f });
                        table.AddCell(new PdfPCell { MinimumHeight = 13f });
                        line = string.Empty;
                    }
                    line = string.Concat(line, @" ", word);
                }

                table.AddCell(new PdfPCell(new Phrase(line.Trim(), PdfAccountBookExtensions.Font.BodyItalic)));

                table.AddCell(new PdfPCell { MinimumHeight = 13f });
                table.AddCell(new PdfPCell { MinimumHeight = 13f });
                table.AddCell(new PdfPCell { MinimumHeight = 13f });
                table.AddCell(new PdfPCell { MinimumHeight = 13f });
            }

            table.AddCell(new PdfPCell { MinimumHeight = 13f, Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER });
            table.AddCell(new PdfPCell { MinimumHeight = 13f, Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER });
            table.AddCell(new PdfPCell { MinimumHeight = 13f, Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER });
            table.AddCell(new PdfPCell { MinimumHeight = 13f, Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER });
            table.AddCell(new PdfPCell { MinimumHeight = 13f, Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER });
            table.AddCell(new PdfPCell { MinimumHeight = 13f, Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER });
            table.AddCell(new PdfPCell { MinimumHeight = 13f, Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER });

            document.NewPage();
            document.Add(table);
        }
    }
}
