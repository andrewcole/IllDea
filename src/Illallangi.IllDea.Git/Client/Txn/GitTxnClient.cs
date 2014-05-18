using System.Data;

namespace Illallangi.IllDea.Client.Txn
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using Illallangi.IllDea.Model;

    public sealed class GitTxnClient : BaseClient, ICrudClient<ITxn>
    {
        #region Fields

        private readonly GitDeaClient currentClient;

        #endregion

        #region Constructor

        public GitTxnClient(GitDeaClient client)
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

        public ITxn Create(Guid companyId, ITxn txn, string log = null)
        {
            return this.CreateTxn(companyId, Mapper.DynamicMap<GitTxn>(txn), log);
        }

        public IEnumerable<ITxn> Retrieve(Guid companyId)
        {
            return this.RetrieveTxn(companyId).OrderBy(t => t.Date);
        }

        public ITxn Update(Guid companyId, ITxn document, string log = null)
        {
            return this.UpdateTxn(
                companyId,
                this.RetrieveTxn(companyId: companyId, id: document.Id).Single(),
                document.Date,
                document.Description,
                document.Items,
                log);
        }

        public void Delete(Guid companyId, ITxn txn, string log = null)
        {
            this.DeleteTxn(
                this.RetrieveTxn(companyId: companyId, id: txn.Id).Single(),
                log);
        }

        private GitTxn CreateTxn(Guid companyId, GitTxn txn, string log = null)
        {
            var index = this.Client.Retrieve(companyId: companyId).Single();
            var period = this.Client.Period.Retrieve(companyId).Single(p => (p.Start <= txn.Date) && (p.End >= txn.Date));

            txn.Period = period.Id;

            foreach (var item in txn.Items.Where(i => 0 == i.Amount))
            {
                txn.Items.Remove(item);
            }

            if (0 != txn.Items.Sum(i => i.Amount))
            {
                throw new DataException(
                    string.Format(
                        "Sum of items in transaction is {0} (expected 0)",
                        txn.Items.Sum(i => i.Amount)));
            }


            using (var atomic = index.Atomic(log ?? "Adding Txn {0}:- {1}", txn.Date.ToString("yyyy-MM-dd"), txn.Description))
            {
                index.Txns.Add(txn.Id);
                atomic.Save(txn);
            }

            return txn;
        }

        private IEnumerable<GitTxn> RetrieveTxn(Guid companyId, Guid? id = null, Guid? accountId = null)
        {
            var index = this.Client.Retrieve(companyId: companyId).Single();

            foreach (var txnId in index.Txns)
            {
                GitTxn txn;
                try
                {
                    txn = index.Load<GitTxn>(txnId);
                }
                catch (Exception e)
                {
                    this.OnError(e);
                    continue;
                }

                if ((null == id || txn.Id.Equals(id)) &&
                    (null == accountId || txn.Items.Any(acc => acc.Account.Equals(accountId))))
                {
                    yield return txn;
                }
            }
        }

        private GitTxn UpdateTxn(Guid companyId, GitTxn txn, DateTime? date, string description, IEnumerable<TxnItem> items, string log)
        {
            txn.Date = date ?? txn.Date;
            txn.Description = description ?? txn.Description;
            if (null != items)
            {
                txn.Items.Clear();
                foreach (var item in items.Where(i => 0 != i.Amount))
                {
                    txn.Items.Add(item);
                }
            }

            var period = this.Client.Period.Retrieve(companyId).Single(p => (p.Start <= txn.Date) && (p.End >= txn.Date));

            txn.Period = period.Id;

            if (0 != txn.Items.Sum(i => i.Amount))
            {
                throw new DataException(
                    string.Format(
                        "Sum of items in transaction is {0} (expected 0)",
                        txn.Items.Sum(i => i.Amount)));
            }

            using (var atomic = this.Client.Retrieve(id: txn.Index).Single().Atomic(log ?? "Updating Txn"))
            {
                return atomic.Save(txn);
            }
        }

        private void DeleteTxn(GitTxn txn, string log)
        {
            var index = this.Client.Retrieve(id: txn.Index).Single();
            index.Txns.Remove(txn.Id);

            using (var atomic = index.Atomic(log ?? "Removing Txn"))
            {
                atomic.Delete(txn);
            }
        }

        #endregion
    }
}