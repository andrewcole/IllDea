using LibGit2Sharp;

namespace Illallangi.IllDea.Client.Payroll
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using AutoMapper;

    using Illallangi.IllDea.Model;

    public sealed class GitPayrollClient : BaseClient, ICrudClient<IPayroll>
    {
        #region Fields

        private readonly GitDeaClient currentClient;

        #endregion

        #region Constructor

        public GitPayrollClient(GitDeaClient client)
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

        public IPayroll Create(Guid companyId, IPayroll payroll, string log = null)
        {
            using (
                var atomic = this.Client.Retrieve(companyId: companyId)
                    .Single()
                    .Atomic(log ?? "Adding Payroll {0}", payroll.Id))
            {

                return this.CreatePayroll(companyId, Mapper.DynamicMap<GitPayroll>(payroll), atomic);
            }
        }

        public IEnumerable<IPayroll> Retrieve(Guid companyId)
        {
            return this.RetrievePayroll(companyId).OrderBy(a => a.Start);
        }

        public IPayroll Update(Guid companyId, IPayroll payroll, string log = null)
        {
            return this.UpdatePayroll(
                this.RetrievePayroll(companyId: companyId, id: payroll.Id).Single(),
                payroll.Start,
                payroll.End,
                payroll.Employee,
                payroll.PayTxn,
                payroll.SuperTxn,
                payroll.GrossPay,
                payroll.Tax,
                payroll.Super,
                log);
        }

        public void Delete(Guid companyId, IPayroll payroll, string log = null)
        {
            this.DeletePayroll(
                this.RetrievePayroll(companyId: companyId, id: payroll.Id).Single(),
                log);
        }

        private GitPayroll CreatePayroll(Guid companyId, GitPayroll payroll, Atomic atomic)
        {
            if (this.Retrieve(companyId).Any(a => a.Id.Equals(payroll.Id)))
            {
                throw new DataException(string.Format(@"Payroll with Id of ""{0}"" already exists", payroll.Id));
            }

            payroll.PayTxn = this.Client.GitTxn.CreateTxn(companyId, payroll.GetPayTxn(this.Client, companyId), atomic).Id;
            payroll.SuperTxn = this.Client.GitTxn.CreateTxn(companyId, payroll.GetSuperTxn(this.Client, companyId), atomic).Id;

            atomic.Index.Payrolls.Add(payroll.Id);
            atomic.Save(payroll);

            return payroll;
        }

        private IEnumerable<GitPayroll> RetrievePayroll(Guid companyId, Guid? id = null)
        {
            var index = this.Client.Retrieve(companyId: companyId).Single();

            foreach (var payrollId in index.Payrolls)
            {
                GitPayroll payroll;
                try
                {
                    payroll = index.Load<GitPayroll>(payrollId);
                }
                catch (Exception e)
                {
                    this.OnError(e);
                    continue;
                }

                if ((null == id || payroll.Id.Equals(id)))
                {
                    yield return payroll;
                }
            }
        }

        private IPayroll UpdatePayroll(GitPayroll payroll, DateTime? start, DateTime? end, Guid? employee, Guid? payTxn, Guid? superTxn, decimal? grossPay, decimal? tax, decimal? super, string log)
        {
            payroll.Start = start.HasValue ? start.Value : payroll.Start;
            payroll.End = end.HasValue ? end.Value : payroll.End;
            payroll.Employee = employee.HasValue ? employee.Value : payroll.Employee;
            payroll.PayTxn = payTxn.HasValue ? payTxn.Value : payroll.PayTxn;
            payroll.SuperTxn = superTxn.HasValue ? superTxn.Value : payroll.SuperTxn;
            payroll.GrossPay = grossPay ?? payroll.GrossPay;
            payroll.Tax = tax ?? payroll.Tax;
            payroll.Super = super ?? payroll.Super;

            using (var atomic = this.Client.Retrieve(id: payroll.Index).Single().Atomic(log ?? "Updating Payroll"))
            {
                return atomic.Save(payroll);
            }
        }

        private void DeletePayroll(GitPayroll payroll, string log)
        {
            var index = this.Client.Retrieve(id: payroll.Index).Single();
            index.Payrolls.Remove(payroll.Id);

            using (var atomic = index.Atomic(log ?? "Removing Payroll"))
            {
                atomic.Delete(payroll);
            }
        }

        #endregion
    }
}