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
            var company = client.Company.Retrieve().Single(c => c.Id.Equals(companyId));

            var table = new PdfPTable(6) { WidthPercentage = 100 };

            table.SetWidths(new[] { 30, 21, 350, 45, 85, 85 });

            table.AddCell(new PdfPCell(new Phrase(company.Name.ToUpper(), PdfGeneralJournalExtensions.Font.CompanyHeader)) { Colspan = 6, HorizontalAlignment = 1, Border = Rectangle.NO_BORDER });
            table.AddCell(new PdfPCell(new Phrase(@"General Journal", PdfGeneralJournalExtensions.Font.DocumentHeader)) { Colspan = 6, HorizontalAlignment = 1, Border = Rectangle.NO_BORDER });

            table.AddCell(
                new PdfPCell(
                    new Phrase(client.Period.Retrieve(companyId).Single(p => p.Id.Equals(periodId)).ToTitle(), PdfGeneralJournalExtensions.Font.Body))
                    {
                        Colspan = 6, 
                        HorizontalAlignment = 1, 
                        Border = Rectangle.NO_BORDER,
                    });

            table.AddCell(new PdfPCell { MinimumHeight = 13f, Colspan = 6, Border = Rectangle.NO_BORDER });

            table.AddCell(new PdfPCell(new Phrase("Date", PdfGeneralJournalExtensions.Font.Body)) { BackgroundColor = BaseColor.LIGHT_GRAY, Colspan = 2 });
            table.AddCell(new PdfPCell(new Phrase("Description", PdfGeneralJournalExtensions.Font.Body)) { BackgroundColor = BaseColor.LIGHT_GRAY });
            table.AddCell(new PdfPCell(new Phrase("Post Ref", PdfGeneralJournalExtensions.Font.Body)) { BackgroundColor = BaseColor.LIGHT_GRAY });
            table.AddCell(new PdfPCell(new Phrase("Debit", PdfGeneralJournalExtensions.Font.Body)) { BackgroundColor = BaseColor.LIGHT_GRAY });
            table.AddCell(new PdfPCell(new Phrase("Credit", PdfGeneralJournalExtensions.Font.Body)) { BackgroundColor = BaseColor.LIGHT_GRAY });

            var year = string.Empty;
            var month = string.Empty;

            foreach (var txn in client.Txn.Retrieve(companyId).Where(txn => txn.Period.Equals(periodId)))
            {
                if (year != txn.Date.Year.ToString())
                {
                    year = txn.Date.Year.ToString();
                    table.AddCell(
                        new PdfPCell(new Phrase(year, PdfGeneralJournalExtensions.Font.Body)) { Colspan = 2, HorizontalAlignment = 1 });
                    table.AddCell(new PdfPCell { MinimumHeight = 13f });
                    table.AddCell(new PdfPCell { MinimumHeight = 13f });
                    table.AddCell(new PdfPCell { MinimumHeight = 13f });
                    table.AddCell(new PdfPCell { MinimumHeight = 13f });
                }

                var date = false;

                foreach (var item in txn.Items.Where(i => i.Amount < 0))
                {
                    if (!date)
                    {
                        if (month != txn.Date.ToString("MMM"))
                        {
                            month = txn.Date.ToString("MMM");
                            table.AddCell(new PdfPCell(new Phrase(month, PdfGeneralJournalExtensions.Font.Body)));
                        }
                        else
                        {
                            table.AddCell(new PdfPCell { MinimumHeight = 13f });
                        }

                        table.AddCell(new PdfPCell(new Phrase(txn.Date.Day.ToString(), PdfGeneralJournalExtensions.Font.Body)) { HorizontalAlignment = 2 });
                        date = true;
                    }
                    else
                    {
                        table.AddCell(new PdfPCell { MinimumHeight = 13f });
                        table.AddCell(new PdfPCell { MinimumHeight = 13f });
                    }

                    table.AddCell(new PdfPCell(new Phrase(client.Account.Retrieve(companyId).Single(a => a.Id.Equals(item.Account)).Name, PdfGeneralJournalExtensions.Font.Body)));
                    table.AddCell(new PdfPCell(new Phrase(client.Account.Retrieve(companyId).Single(a => a.Id.Equals(item.Account)).Number, PdfGeneralJournalExtensions.Font.Body)) { HorizontalAlignment = 1 });

                    table.AddCell(new PdfPCell(new Phrase((0 - item.Amount).ToString(@"C"), PdfGeneralJournalExtensions.Font.Body)) { HorizontalAlignment = 2 });
                    table.AddCell(new PdfPCell { MinimumHeight = 13f });
                }

                foreach (var item in txn.Items.Where(i => i.Amount > 0))
                {
                    table.AddCell(new PdfPCell { MinimumHeight = 13f });
                    table.AddCell(new PdfPCell { MinimumHeight = 13f });

                    table.AddCell(new PdfPCell(new Phrase(string.Concat("  ", client.Account.Retrieve(companyId).Single(a => a.Id.Equals(item.Account)).Name), PdfGeneralJournalExtensions.Font.Body)));
                    table.AddCell(new PdfPCell(new Phrase(client.Account.Retrieve(companyId).Single(a => a.Id.Equals(item.Account)).Number, PdfGeneralJournalExtensions.Font.Body)) { HorizontalAlignment = 1 });

                    table.AddCell(new PdfPCell { MinimumHeight = 13f });
                    table.AddCell(new PdfPCell(new Phrase(item.Amount.ToString(@"C"), PdfGeneralJournalExtensions.Font.Body)) { HorizontalAlignment = 2 });
                }

                table.AddCell(new PdfPCell { MinimumHeight = 13f });
                table.AddCell(new PdfPCell { MinimumHeight = 13f });

                var line = string.Empty;

                foreach (var word in txn.Description.Split())
                {
                    if (line.Length + word.Length + 1 > 55)
                    {
                        table.AddCell(new PdfPCell(new Phrase(line.Trim(), PdfGeneralJournalExtensions.Font.BodyItalic)));
                        table.AddCell(new PdfPCell { MinimumHeight = 13f });
                        table.AddCell(new PdfPCell { MinimumHeight = 13f });
                        table.AddCell(new PdfPCell { MinimumHeight = 13f });
                        table.AddCell(new PdfPCell { MinimumHeight = 13f });
                        table.AddCell(new PdfPCell { MinimumHeight = 13f });
                        line = string.Empty;
                    }
                    line = string.Concat(line, @" ", word);
                }

                table.AddCell(new PdfPCell(new Phrase(line.Trim(), PdfGeneralJournalExtensions.Font.BodyItalic)));

                table.AddCell(new PdfPCell { MinimumHeight = 13f });
                table.AddCell(new PdfPCell { MinimumHeight = 13f });
                table.AddCell(new PdfPCell { MinimumHeight = 13f });



                table.AddCell(new PdfPCell { MinimumHeight = 13f });
                table.AddCell(new PdfPCell { MinimumHeight = 13f });
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

            document.NewPage();
            document.Add(table);
        }
    }
}
