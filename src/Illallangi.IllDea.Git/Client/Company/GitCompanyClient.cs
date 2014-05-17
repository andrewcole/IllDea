namespace Illallangi.IllDea.Client.Company
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using AutoMapper;

    using Illallangi.IllDea.Model;

    public sealed class GitCompanyClient : BaseClient, ICompanyClient
    {
        #region Fields

        private readonly GitDeaClient currentClient;

        #endregion

        #region Constructor

        public GitCompanyClient(GitDeaClient client)
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

        public ICompany Create(ICompany company, ISettings companySettings, string log = null)
        {
            return this.CreateCompany(Mapper.DynamicMap<GitCompany>(company), companySettings, log);
        }

        public IEnumerable<ICompany> Retrieve()
        {
            return this.RetrieveCompany();
        }

        public ICompany Update(Guid companyId, ICompany company, string log = null)
        {
            return this.UpdateCompany(
                this.RetrieveCompany(id: companyId).Single(),
                company.Name,
                log);
        }

        private GitCompany CreateCompany(GitCompany company, ISettings companySettings, string log = null)
        {
            if (this.Retrieve().Any(c => c.Name.Equals(company.Name)))
            {
                throw new DataException(string.Format(@"Company with Name of ""{0}"" already exists", company.Name));
            }

            if (this.Retrieve().Any(c => c.Id.Equals(company.Id)))
            {
                throw new DataException(string.Format(@"Company with Id of ""{0}"" already exists", company.Id));
            }

            var index = this.Client.Create(companySettings);

            using (var atomic = index.Atomic(log ?? "Initial creation of {0}", company.Name))
            {
                index.Company = company.Id;
                atomic.Save(company);
            }

            return company;
        }

        private IEnumerable<GitCompany> RetrieveCompany(Guid? id = null, string name = null)
        {
            foreach (var index in this.Client.Retrieve())
            {
                GitCompany company;
                try
                {
                    company = index.Load<GitCompany>(index.Company);
                }
                catch (Exception e)
                {
                    this.OnError(e);
                    continue;
                }

                if ((null == id || company.Id.Equals(id)) && (null == name || company.Name.Equals(name)))
                {
                    yield return company;
                }
            }
        }

        private GitCompany UpdateCompany(GitCompany company, string name, string log = null)
        {
            company.Name = name ?? company.Name;

            using (var atomic = this.Client.Retrieve(id: company.Index).Single().Atomic(log ?? "Updating company"))
            {
                return atomic.Save(company);
            }
        }

        #endregion
    }
}