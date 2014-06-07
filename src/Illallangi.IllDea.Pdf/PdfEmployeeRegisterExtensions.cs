using System.Collections.Generic;

namespace Illallangi.IllDea.Pdf
{
    using System;
    using System.IO;
    using System.Linq;

    using Illallangi.IllDea.Client;

    using iTextSharp.text;
    using iTextSharp.text.pdf;

    public static class PdfEmployeeRegisterExtensions
    {
        public static void CreateEmployeeRegister(this IDeaClient client, Guid companyId, Stream stream)
        {
            using (var document = new Document(new Rectangle(PageSize.A4), 72, 72, 72, 72))
            using (var writer = PdfWriter.GetInstance(document, stream))
            {
                document.Open();

                client.CreateEmployeeRegister(companyId, document);

                document.Close();
                writer.Close();
            }
        }

        internal static void CreateEmployeeRegister(this IDeaClient client, Guid companyId, Document document)
        {
            var table = new PdfPTable(3) { WidthPercentage = 100 };

            table.SetWidths(new[] { 361, 85, 210 });

            table
                .AddPageHeaderCell(@"Employee Register").Go()
                .AddColumnHeaderCell().Go()
                .AddColumnHeaderCell(@"").Inverted().Go()
                .AddColumnHeaderCell(@"").Go();

            foreach (var employee in client.Employee.Retrieve(companyId))
            {
                table
                    .AddHeaderCell(employee.Name).Go()
                    .AddBodyCell().Inverted().Go()
                    .AddBodyCell().Go();

                table
                    .AddBodyCell(employee.Address).Go()
                    .AddBodyCell().Inverted().Go()
                    .AddBodyCell().Go();

                table
                    .AddBodyCell(string.Format(@"{0}, {1} {2}", employee.City, employee.State, employee.PostCode)).Go()
                    .AddBodyCell().Inverted().Go()
                    .AddBodyCell().Go();

                table
                    .AddBodyCell().Go()
                    .AddBodyCell().Inverted().Go()
                    .AddBodyCell().Go();

                foreach (var account in new Dictionary<string, Guid>
                    {
                        { "Salary Expense Account", employee.SalaryExpenseAccount },
                        { "Superannuation Expense Account", employee.SuperannuationExpenseAccount },
                        { "Employee Liability Account", employee.EmployeeLiabilityAccount},
                        { "Income Tax Liability Account", employee.IncomeTaxLiabilityAccount},
                        { "Superannuation Liability Account", employee.SuperannuationLiabilityAccount},
                    })
                {
                    var accObj = client.Account.Retrieve(companyId).Single(a => a.Id.Equals(account.Value));

                    table
                        .AddBodyCell(account.Key).WithTabStops().Go()
                        .AddBodyCell(accObj.Number).Inverted().CenterAligned().Go()
                        .AddBodyCell(accObj.Name).Go();    
                }

                
            }

            document.NewPage();
            document.Add(table);
        }
    }
}
