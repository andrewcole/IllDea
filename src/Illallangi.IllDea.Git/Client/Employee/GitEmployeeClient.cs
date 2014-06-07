namespace Illallangi.IllDea.Client.Employee
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using AutoMapper;

    using Illallangi.IllDea.Model;

    public sealed class GitEmployeeClient : BaseClient, ICrudClient<IEmployee>
    {
        #region Fields

        private readonly GitDeaClient currentClient;

        #endregion

        #region Constructor

        public GitEmployeeClient(GitDeaClient client)
        {
            this.currentClient = client;
        }

        #endregion

        #region Properties

        private GitDeaClient Client
        {
            get
            {
                return this.currentClient;
            }
        }

        #endregion

        #region Methods

        public IEmployee Create(Guid companyId, IEmployee employee, string log = null)
        {
            return this.CreateEmployee(companyId, Mapper.DynamicMap<GitEmployee>(employee), log);
        }

        public IEnumerable<IEmployee> Retrieve(Guid companyId)
        {
            return this.RetrieveEmployee(companyId).OrderBy(a => a.Name);
        }

        public IEmployee Update(Guid companyId, IEmployee employee, string log = null)
        {
            return this.UpdateEmployee(
                this.RetrieveEmployee(companyId: companyId, id: employee.Id).Single(),
                employee.Name,
                employee.Address,
                employee.City,
                employee.State,
                employee.PostCode,
                employee.SalaryExpenseAccount,
                employee.EmployeeLiabilityAccount,
                employee.IncomeTaxLiabilityAccount,
                employee.SuperannuationExpenseAccount,
                employee.SuperannuationLiabilityAccount,
                log);
        }

        public void Delete(Guid companyId, IEmployee employee, string log = null)
        {
            this.DeleteEmployee(
                this.RetrieveEmployee(companyId: companyId, id: employee.Id).Single(),
                log);
        }

        private GitEmployee CreateEmployee(Guid companyId, GitEmployee employee, string log = null)
        {
            if (this.Retrieve(companyId).Any(a => a.Name.Equals(employee.Name)))
            {
                throw new DataException(string.Format(@"Employee with Name of ""{0}"" already exists", employee.Name));
            }

            if (this.Retrieve(companyId).Any(a => a.Id.Equals(employee.Id)))
            {
                throw new DataException(string.Format(@"Employee with Id of ""{0}"" already exists", employee.Id));
            }

            var index = this.Client.Retrieve(companyId: companyId).Single();
            
            using (var atomic = index.Atomic(log ?? "Adding Employee {0}", employee.Name))
            {
                index.Employees.Add(employee.Id);
                atomic.Save(employee);
            }

            return employee;
        }

        private IEnumerable<GitEmployee> RetrieveEmployee(Guid companyId, Guid? id = null)
        {
            var index = this.Client.Retrieve(companyId: companyId).Single();

            foreach (var employeeId in index.Employees)
            {
                GitEmployee employee;
                try
                {
                    employee = index.Load<GitEmployee>(employeeId);
                }
                catch (Exception e)
                {
                    this.OnError(e);
                    continue;
                }

                if ((null == id || employee.Id.Equals(id)))
                {
                    yield return employee;
                }
            }
        }

        
        private GitEmployee UpdateEmployee(GitEmployee employee, string name, string address, string city, string state, string postCode, Guid? salaryExpenseAccount, Guid? employeeLiabilityAccount, Guid? incomeTaxLiabilityAccount, Guid? superannuationExpenseAccount, Guid? superannuationLiabilityAccount, string log)
        {
            employee.Name = name ?? employee.Name;
            employee.Address = address ?? employee.Address;
            employee.City = city ?? employee.City;
            employee.State = state ?? employee.State;
            employee.PostCode = postCode ?? employee.PostCode;
            employee.SalaryExpenseAccount = salaryExpenseAccount.HasValue ? salaryExpenseAccount.Value : employee.SalaryExpenseAccount;
            employee.EmployeeLiabilityAccount = employeeLiabilityAccount.HasValue ? employeeLiabilityAccount.Value : employee.EmployeeLiabilityAccount;
            employee.IncomeTaxLiabilityAccount = incomeTaxLiabilityAccount.HasValue ? incomeTaxLiabilityAccount.Value : employee.IncomeTaxLiabilityAccount;
            employee.SuperannuationExpenseAccount = superannuationExpenseAccount.HasValue ? superannuationExpenseAccount.Value : employee.SuperannuationExpenseAccount;
            employee.SuperannuationLiabilityAccount = superannuationLiabilityAccount.HasValue ? superannuationLiabilityAccount.Value : employee.SuperannuationLiabilityAccount;
            
            using (var atomic = this.Client.Retrieve(id: employee.Index).Single().Atomic(log ?? "Updating Employee"))
            {
                return atomic.Save(employee);
            }
        }

        private void DeleteEmployee(GitEmployee employee, string log)
        {
            var index = this.Client.Retrieve(id: employee.Index).Single();
            index.Employees.Remove(employee.Id);

            using (var atomic = index.Atomic(log ?? "Removing Employee"))
            {
                atomic.Delete(employee);
            }
        }

        #endregion
    }
}