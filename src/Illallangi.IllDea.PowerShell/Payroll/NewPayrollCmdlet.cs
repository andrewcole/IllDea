using System;
using System.Linq;
using System.Management.Automation;

using Illallangi.IllDea.Model;

namespace Illallangi.IllDea.PowerShell.Payroll
{
    [Cmdlet(VerbsCommon.New, Nouns.Payroll)]
    public sealed class NewPayrollCmdlet : IdDeaCmdlet, IPayroll
    {
        #region Fields

        private const string FilterParameterSet = @"Filter";
        private const string IdParameterSet = @"Id";

        private Guid? currentSuperTxn;
        private Guid? currentPayTxn;
        private Guid? currentEmployee;

        private string currentEmployeeName;
        private WildcardPattern currentEmployeeNameWildcardPattern;


        #endregion

        [Parameter(Mandatory = true, ParameterSetName = NewPayrollCmdlet.IdParameterSet)]
        [Parameter(Mandatory = true, ParameterSetName = NewPayrollCmdlet.FilterParameterSet)]
        public DateTime Start { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = NewPayrollCmdlet.IdParameterSet)]
        [Parameter(Mandatory = true, ParameterSetName = NewPayrollCmdlet.FilterParameterSet)]
        public DateTime End { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = NewPayrollCmdlet.IdParameterSet)]
        [Parameter(Mandatory = true, ParameterSetName = NewPayrollCmdlet.FilterParameterSet)]
        public decimal GrossPay { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = NewPayrollCmdlet.IdParameterSet)]
        [Parameter(Mandatory = true, ParameterSetName = NewPayrollCmdlet.FilterParameterSet)]
        public decimal Tax { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = NewPayrollCmdlet.IdParameterSet)]
        [Parameter(Mandatory = true, ParameterSetName = NewPayrollCmdlet.FilterParameterSet)]
        public decimal Super { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = NewPayrollCmdlet.IdParameterSet, ValueFromPipelineByPropertyName = true)]
        public Guid? EmployeeId { get; set; }

        [SupportsWildcards]
        [Parameter(Position = 1, ParameterSetName = NewPayrollCmdlet.FilterParameterSet)]
        public string EmployeeName
        {
            get
            {
                return this.currentEmployeeName;
            }
            set
            {
                this.currentEmployeeNameWildcardPattern = null;
                this.currentEmployeeName = value;
            }
        }

        public Guid Employee
        {
            get
            {
                if (!this.currentEmployee.HasValue)
                {
                    this.currentEmployee =
                        this.Client.Employee.Retrieve(this.CompanyId).Single(this.IsEmployeeMatch).Id;
                }

                return this.currentEmployee.Value;
            }
            set
            {
                this.currentEmployee = value;
            }
        }

        private bool IsEmployeeMatch(IEmployee employee)
        {
            switch (this.ParameterSetName)
            {
                case NewPayrollCmdlet.IdParameterSet:
                    return this.EmployeeId.HasValue &&
                           this.EmployeeId.Value.Equals(employee.Id);

                case NewPayrollCmdlet.FilterParameterSet:
                    return this.EmployeeNameWildcardPattern.IsMatch(employee.Name);

                default:
                    throw new NotImplementedException(this.ParameterSetName);
            }
        }

        private WildcardPattern EmployeeNameWildcardPattern
        {
            get
            {
                return this.currentEmployeeNameWildcardPattern
                       ?? (this.currentEmployeeNameWildcardPattern =
                           new WildcardPattern(this.EmployeeName ?? @"*", WildcardOptions.Compiled | WildcardOptions.IgnoreCase));
            }
        }

        public Guid PayTxn
        {
            get
            {
                return (this.currentPayTxn.HasValue ? this.currentPayTxn : (this.currentPayTxn = Guid.NewGuid())).Value;
            }
            set
            {
                this.currentPayTxn = value;
            }
        }

        public Guid SuperTxn
        {
            get
            {
                return (this.currentSuperTxn.HasValue ? this.currentSuperTxn : (this.currentSuperTxn = Guid.NewGuid())).Value;
            }
            set
            {
                this.currentSuperTxn = value;
            }
        }

        protected override void ProcessRecord()
        {
            this.WriteObject(this.Client.Payroll.Create(this.CompanyId, this, this.ToString()));
        }

        public override string ToString()
        {
            return string.Format(
                @"New-Payroll -FuckKnows");
        }
    }
}