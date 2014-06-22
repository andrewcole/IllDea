namespace Illallangi.IllDea.Client.Account
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using AutoMapper;

    using Illallangi.IllDea.Client.Txn;
    using Illallangi.IllDea.Model;

    public sealed class GitAccountClient : BaseClient, IAccountClient
    {
        #region Fields

        private readonly GitDeaClient currentClient;

        #endregion

        #region Constructor

        public GitAccountClient(GitDeaClient client)
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

        public IAccount Create(Guid companyId, IAccount account, string log = null)
        {
            return this.CreateAccount(companyId, Mapper.DynamicMap<GitAccount>(account), log);
        }

        public IEnumerable<IAccount> Retrieve(Guid companyId)
        {
            return this.RetrieveAccount(companyId).OrderBy(a => a.Number);
        }

        public IAccount Update(Guid companyId, IAccount document, string log = null)
        {
            return this.UpdateAccount(
                this.RetrieveAccount(companyId: companyId, id: document.Id).Single(),
                document.Name,
                document.Type,
                document.Number,
                log);
        }

        public void Delete(Guid companyId, IAccount account, string log = null)
        {
            this.DeleteAccount(
                this.RetrieveAccount(companyId: companyId, id: account.Id).Single(),
                log);
        }

        public IEnumerable<IAccount> RetrieveWithBalances(Guid companyId, Guid? periodId = null, AccountType? accountType = null)
        {
            var period = periodId.HasValue
                             ? this.Client.Period.Retrieve(companyId).Single(p => p.Id.Equals(periodId.Value))
                             : null;

            foreach (var account in this.Retrieve(companyId).Where(a => !accountType.HasValue || a.Type.Equals(accountType.Value)))
            {
                var txns =
                    this.Client.Txn.Retrieve(companyId)
                        .Where(t => t.Items.Any(i => i.Account.Equals(account.Id)))
                        .ToList();

                account.Opening = txns
                                    .Where(t => null != period && t.Date < period.Start)
                                    .Select(t => t.Items.Single(i => i.Account.Equals(account.Id)).Amount)
                                    .Sum();

                account.Closing = txns
                                    .Where(t => null == period || t.Date <= period.End)
                                    .Select(t => t.Items.Single(i => i.Account.Equals(account.Id)).Amount)
                                    .Sum();

                yield return account;
            }
        }

        private GitAccount CreateAccount(Guid companyId, GitAccount account, string log = null)
        {
            if (this.Retrieve(companyId).Any(a => a.Name.Equals(account.Name)))
            {
                throw new DataException(string.Format(@"Account with Name of ""{0}"" already exists", account.Name));
            }

            if (this.Retrieve(companyId).Any(a => a.Id.Equals(account.Id)))
            {
                throw new DataException(string.Format(@"Account with Id of ""{0}"" already exists", account.Id));
            }

            if (this.Retrieve(companyId).Any(a => a.Number.Equals(account.Number)))
            {
                throw new DataException(string.Format(@"Account with Number of ""{0}"" already exists", account.Number));
            }

            var index = this.Client.Retrieve(companyId: companyId).Single();
            
            using (var atomic = index.Atomic(log ?? "Adding account {0}", account.Name))
            {
                index.Accounts.Add(account.Id);
                atomic.Save(account);
            }

            return account;
        }

        private IEnumerable<GitAccount> RetrieveAccount(Guid companyId, Guid? id = null, string name = null, AccountType? type = null, string number = null)
        {
            var index = this.Client.Retrieve(companyId: companyId).Single();

            foreach (var accountId in index.Accounts)
            {
                GitAccount account;
                try
                {
                    account = index.Load<GitAccount>(accountId);
                }
                catch (Exception e)
                {
                    this.OnError(e);
                    continue;
                }

                if ((null == id || account.Id.Equals(id)) &&
                    (null == name || account.Name.Equals(name)) &&
                    (null == type || account.Type.Equals(type)) &&
                    (null == number || account.Number.Equals(number)))
                {
                    yield return account;
                }
            }
        }

        private GitAccount UpdateAccount(GitAccount account, string name, AccountType? type, string number, string log)
        {
            account.Name = name ?? account.Name;
            account.Type = type ?? account.Type;
            account.Number = number ?? account.Number;

            using (var atomic = this.Client.Retrieve(id: account.Index).Single().Atomic(log ?? "Updating account"))
            {
                return atomic.Save(account);
            }
        }

        private void DeleteAccount(GitAccount account, string log)
        {
            var index = this.Client.Retrieve(id: account.Index).Single();
            index.Accounts.Remove(account.Id);

            using (var atomic = index.Atomic(log ?? "Removing Account"))
            {
                atomic.Delete(account);
            }
        }

        #endregion
    }
}