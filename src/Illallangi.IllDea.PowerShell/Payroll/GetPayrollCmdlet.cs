using System;
using System.Linq;
using System.Management.Automation;

using Illallangi.IllDea.Model;

namespace Illallangi.IllDea.PowerShell.Payroll
{
    [Cmdlet(VerbsCommon.Get, Nouns.Payroll)]
    public class GetPayrollCmdlet : DeaCmdlet
    {
        private const string FilterParameterSet = @"Filter";
        private const string IdParameterSet = @"Id";

        [Parameter(Mandatory = true, ParameterSetName = GetPayrollCmdlet.IdParameterSet, ValueFromPipelineByPropertyName = true)]
        public Guid? Id { get; set; }

        [Parameter(Position = 1, ParameterSetName = GetPayrollCmdlet.FilterParameterSet)]
        public DateTime? Date { get; set; }

        [Parameter(ParameterSetName = GetPayrollCmdlet.FilterParameterSet)]
        public DateTime? Start { get; set; }

        [Parameter(ParameterSetName = GetPayrollCmdlet.FilterParameterSet)]
        public DateTime? End { get; set; }

        protected override void ProcessRecord()
        {
            this.WriteObject(this.Client.Payroll.Retrieve(this.CompanyId).Where(this.IsMatch), true);
        }

        protected virtual bool IsMatch(IPayroll payroll)
        {
            switch (this.ParameterSetName)
            {
                case GetPayrollCmdlet.IdParameterSet:
                    return this.Id.HasValue &&
                           this.Id.Value.Equals(payroll.Id);

                case GetPayrollCmdlet.FilterParameterSet:
                    return (null == this.Start || payroll.Start.Equals(this.Start)) &&
                           (null == this.End || payroll.End.Equals(this.End)) &&
                           (null == this.Date || (payroll.Start < this.Date && payroll.End > this.Date));
                default:
                    throw new NotImplementedException(this.ParameterSetName);
            }
        }
    }
}
