namespace Illallangi.IllDea.PowerShell.Employee
{
    using System;
    using System.Linq;
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    [Cmdlet(VerbsCommon.Remove, Nouns.Employee, SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    public sealed class RemoveEmployeeCmdlet : GetEmployeeCmdlet
    {
        protected override void ProcessRecord()
        {
            foreach (var employee in this.Client.Employee.Retrieve(this.CompanyId).Where(this.IsMatch))
            {
                this.Client.Employee.Delete(
                    this.CompanyId,
                    employee,
                    this.ToString());
            }
        }

        protected override bool IsMatch(IEmployee employee)
        {
            return base.IsMatch(employee) &&
                   this.ShouldProcess(employee.ToString(), VerbsCommon.Remove);
        }

        public override string ToString()
        {
            return string.Format(
                @"Remove-Employee -Id ""{0}""",
                this.Id);
        }
    }
}