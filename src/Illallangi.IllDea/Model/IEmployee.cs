namespace Illallangi.IllDea.Model
{
    using System;

    public interface IEmployee : IBaseModel
    {
        string Name { get; set; }
        string Address { get; set; }
        string City { get; set; }
        string State { get; set; }
        string PostCode { get; set; }
        Guid SalaryExpenseAccount { get; set; }
        Guid EmployeeLiabilityAccount { get; set; }
        Guid IncomeTaxLiabilityAccount { get; set; }
        Guid SuperannuationExpenseAccount { get; set; }
        Guid SuperannuationLiabilityAccount { get; set; }
    }
}