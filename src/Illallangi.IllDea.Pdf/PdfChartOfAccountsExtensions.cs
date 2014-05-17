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
        private static FontSelection staticFont;

        private static FontSelection Font
        {
            get
            {
                return PdfChartOfAccountsExtensions.staticFont ?? (PdfChartOfAccountsExtensions.staticFont = new FontSelection());
            }
        }
        
        public static void CreateChartOfAccounts(this IDeaClient client, Guid companyId, Stream stream)
        {
            using (var document = new Document(new Rectangle(PageSize.A4), 72, 72, 72, 72))
            using (var writer = PdfWriter.GetInstance(document, stream))
            {
                document.Open();
                
                client.CreateChartOfAccounts(companyId, document);
                
                document.Close();
                writer.Close();
            }
        }

        internal static void CreateChartOfAccounts(this IDeaClient client, Guid companyId, Document document)
        {
            var company = client.Company.Retrieve().Single(c => c.Id.Equals(companyId));

            var table = new PdfPTable(4) { TotalWidth = 50 };

            table.SetWidths(new[] { 4, 21, 4, 21 });

            table.AddCell(new PdfPCell(new Phrase(company.Name.ToUpper(), PdfChartOfAccountsExtensions.Font.CompanyHeader)) { Colspan = 4, HorizontalAlignment = 1, Border = Rectangle.NO_BORDER });
            table.AddCell(new PdfPCell(new Phrase(@"Chart of Accounts", PdfChartOfAccountsExtensions.Font.DocumentHeader)) { Colspan = 4, HorizontalAlignment = 1, Border = Rectangle.NO_BORDER });

            table.AddCell(new PdfPCell { MinimumHeight = 13f, Colspan = 4, Border = Rectangle.NO_BORDER });
            table.AddCell(new PdfPCell { MinimumHeight = 13f, Colspan = 4, Border = Rectangle.NO_BORDER });

            table.AddCell(new PdfPCell(new Phrase("A/C No.", PdfChartOfAccountsExtensions.Font.Body)) { BackgroundColor = BaseColor.LIGHT_GRAY });
            table.AddCell(new PdfPCell(new Phrase("Description", PdfChartOfAccountsExtensions.Font.Body)) { BackgroundColor = BaseColor.LIGHT_GRAY });
            table.AddCell(new PdfPCell(new Phrase("A/C No.", PdfChartOfAccountsExtensions.Font.Body)) { BackgroundColor = BaseColor.LIGHT_GRAY });
            table.AddCell(new PdfPCell(new Phrase("Description", PdfChartOfAccountsExtensions.Font.Body)) { BackgroundColor = BaseColor.LIGHT_GRAY });

            table.AddCell(new PdfPCell { MinimumHeight = 13f });
            table.AddCell(new PdfPCell { MinimumHeight = 13f });
            table.AddCell(new PdfPCell { MinimumHeight = 13f });
            table.AddCell(new PdfPCell { MinimumHeight = 13f });

            table.AddCell(new PdfPCell { MinimumHeight = 13f });
            table.AddCell(new PdfPCell(new Phrase("Assets", PdfChartOfAccountsExtensions.Font.AccountTypeHeader)));
            table.AddCell(new PdfPCell { MinimumHeight = 13f });
            table.AddCell(new PdfPCell(new Phrase("Income", PdfChartOfAccountsExtensions.Font.AccountTypeHeader)));

            var assets = client.Account.Retrieve(companyId).Where(a => a.Type.Equals(AccountType.Asset)).ToList();
            var income = client.Account.Retrieve(companyId).Where(a => a.Type.Equals(AccountType.Income)).ToList();

            for (var i = 0; i < (assets.Count > income.Count ? assets.Count : income.Count); i++)
            {
                if (i >= assets.Count)
                {
                    table.AddCell(new PdfPCell { MinimumHeight = 13f });
                    table.AddCell(new PdfPCell { MinimumHeight = 13f });
                }
                else
                {
                    table.AddCell(new PdfPCell(new Phrase(assets[i].Number, PdfChartOfAccountsExtensions.Font.Body)));
                    table.AddCell(new PdfPCell(new Phrase(assets[i].Name, PdfChartOfAccountsExtensions.Font.Body)));
                }

                if (i >= income.Count)
                {
                    table.AddCell(new PdfPCell { MinimumHeight = 13f });
                    table.AddCell(new PdfPCell { MinimumHeight = 13f });
                }
                else
                {
                    table.AddCell(new PdfPCell(new Phrase(income[i].Number, PdfChartOfAccountsExtensions.Font.Body)));
                    table.AddCell(new PdfPCell(new Phrase(income[i].Name, PdfChartOfAccountsExtensions.Font.Body)));
                }
            }

            table.AddCell(new PdfPCell { MinimumHeight = 13f });
            table.AddCell(new PdfPCell { MinimumHeight = 13f });
            table.AddCell(new PdfPCell { MinimumHeight = 13f });
            table.AddCell(new PdfPCell { MinimumHeight = 13f });

            table.AddCell(new PdfPCell { MinimumHeight = 13f });
            table.AddCell(new PdfPCell(new Phrase("Liabilities", PdfChartOfAccountsExtensions.Font.AccountTypeHeader)));
            table.AddCell(new PdfPCell { MinimumHeight = 13f });
            table.AddCell(new PdfPCell(new Phrase("Expenses", PdfChartOfAccountsExtensions.Font.AccountTypeHeader)));

            var liabilities = client.Account.Retrieve(companyId).Where(a => a.Type.Equals(AccountType.Liability)).ToList();
            var expenses = client.Account.Retrieve(companyId).Where(a => a.Type.Equals(AccountType.Expense)).ToList();

            for (var i = 0; i < (liabilities.Count > expenses.Count ? liabilities.Count : expenses.Count); i++)
            {
                if (i < liabilities.Count)
                {
                    table.AddCell(new PdfPCell(new Phrase(liabilities[i].Number, PdfChartOfAccountsExtensions.Font.Body)));
                    table.AddCell(new PdfPCell(new Phrase(liabilities[i].Name, PdfChartOfAccountsExtensions.Font.Body)));
                }
                else
                {
                    table.AddCell(new PdfPCell { MinimumHeight = 13f });
                    table.AddCell(new PdfPCell { MinimumHeight = 13f });
                }

                if (i < expenses.Count)
                {
                    table.AddCell(new PdfPCell(new Phrase(expenses[i].Number, PdfChartOfAccountsExtensions.Font.Body)));
                    table.AddCell(new PdfPCell(new Phrase(expenses[i].Name, PdfChartOfAccountsExtensions.Font.Body)));
                }
                else
                {
                    table.AddCell(new PdfPCell { MinimumHeight = 13f });
                    table.AddCell(new PdfPCell { MinimumHeight = 13f });
                }
            }

            table.AddCell(new PdfPCell { MinimumHeight = 13f });
            table.AddCell(new PdfPCell { MinimumHeight = 13f });
            table.AddCell(new PdfPCell { MinimumHeight = 13f });
            table.AddCell(new PdfPCell { MinimumHeight = 13f });

            table.AddCell(new PdfPCell { MinimumHeight = 13f });
            table.AddCell(new PdfPCell(new Phrase("Equity", PdfChartOfAccountsExtensions.Font.AccountTypeHeader)));
            table.AddCell(new PdfPCell { MinimumHeight = 13f });
            table.AddCell(new PdfPCell { MinimumHeight = 13f });

            foreach (var equity in client.Account.Retrieve(companyId).Where(a => a.Type.Equals(AccountType.Equity)))
            {
                table.AddCell(new PdfPCell(new Phrase(equity.Number, PdfChartOfAccountsExtensions.Font.Body)));
                table.AddCell(new PdfPCell(new Phrase(equity.Name, PdfChartOfAccountsExtensions.Font.Body)));
                table.AddCell(new PdfPCell { MinimumHeight = 13f });
                table.AddCell(new PdfPCell { MinimumHeight = 13f });
            }
            
            table.AddCell(new PdfPCell { MinimumHeight = 13f });
            table.AddCell(new PdfPCell { MinimumHeight = 13f });
            table.AddCell(new PdfPCell { MinimumHeight = 13f });
            table.AddCell(new PdfPCell { MinimumHeight = 13f });

            table.AddCell(new PdfPCell { MinimumHeight = 13f, Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER });
            table.AddCell(new PdfPCell { MinimumHeight = 13f, Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER });
            table.AddCell(new PdfPCell { MinimumHeight = 13f, Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER });
            table.AddCell(new PdfPCell { MinimumHeight = 13f, Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER });

            document.NewPage();
            document.Add(table);
        }
    }
}
