using System;
using System.Linq;
using Illallangi.IllDea.Client;
using Newtonsoft.Json;

namespace Illallangi.IllDea.Model
{
    public class GitPayroll : BaseModel, IPayroll
    {
        [JsonProperty("start")]
        public DateTime Start { get; set; }

        [JsonProperty("end")]
        public DateTime End { get; set; }

        [JsonProperty("employee")]
        public Guid Employee { get; set; }

        [JsonProperty("paytxn")]
        public Guid PayTxn { get; set; }

        [JsonProperty("supertxn")]
        public Guid SuperTxn { get; set; }

        [JsonProperty("grosspay")]
        public decimal GrossPay { get; set; }

        [JsonProperty("tax")]
        public decimal Tax { get; set; }

        [JsonProperty("super")]
        public decimal Super { get; set; }

        internal GitTxn GetPayTxn(IDeaClient client, Guid companyId)
        {
            var employee = client.Employee.Retrieve(companyId).Single(e => e.Id.Equals(this.Employee));
            var payTxn = new GitTxn()
            {
                Id = this.PayTxn,
                Date = this.End,
                Description = string.Format(
                    "Payroll for {0} from {1} to {2}",
                    employee.Name,
                    this.Start,
                    this.End),
                Internal = true,
            };

            payTxn.Items.Add(new TxnItem { Account = employee.EmployeeLiabilityAccount, Amount = this.GrossPay - this.Tax });
            payTxn.Items.Add(new TxnItem { Account = employee.IncomeTaxLiabilityAccount, Amount = this.Tax });
            payTxn.Items.Add(new TxnItem { Account = employee.SalaryExpenseAccount, Amount = 0 - this.GrossPay });

            return payTxn;

        }

        internal GitTxn GetSuperTxn(IDeaClient client, Guid companyId)
        {
            var employee = client.Employee.Retrieve(companyId).Single(e => e.Id.Equals(this.Employee));
            var payTxn = new GitTxn()
            {
                Id = this.SuperTxn,
                Date = this.End,
                Description = string.Format(
                    "Super for {0} from {1} to {2}",
                    employee.Name,
                    this.Start,
                    this.End),
                Internal = true,
            };

            payTxn.Items.Add(new TxnItem { Account = employee.SuperannuationLiabilityAccount, Amount = this.Super });
            payTxn.Items.Add(new TxnItem { Account = employee.SuperannuationExpenseAccount, Amount = 0 - this.Super });

            return payTxn;

        }
    }
}