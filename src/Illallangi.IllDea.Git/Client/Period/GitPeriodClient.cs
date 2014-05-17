namespace Illallangi.IllDea.Client.Period
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using AutoMapper;

    using Illallangi.IllDea.Model;

    public sealed class GitPeriodClient : BaseClient, ICrudClient<IPeriod>
    {
        #region Fields

        private readonly GitDeaClient currentClient;

        #endregion

        #region Constructor

        public GitPeriodClient(GitDeaClient client)
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

        public IPeriod Create(Guid companyId, IPeriod period, string log = null)
        {
            return this.CreatePeriod(companyId, Mapper.DynamicMap<GitPeriod>(period), log);
        }

        public IEnumerable<IPeriod> Retrieve(Guid companyId)
        {
            return this.RetrievePeriod(companyId).OrderBy(p => p.Start);
        }

        public IPeriod Update(Guid companyId, IPeriod document, string log = null)
        {
            return this.UpdatePeriod(
                this.RetrievePeriod(companyId: companyId, id: document.Id).Single(),
                document.Start,
                document.End,
                log);
        }

        public void Delete(Guid companyId, IPeriod period, string log = null)
        {
            this.DeletePeriod(
                this.RetrievePeriod(companyId: companyId, id: period.Id).Single(),
                log);
        }

        private GitPeriod CreatePeriod(Guid companyId, GitPeriod period, string log = null)
        {
            var index = this.Client.Retrieve(companyId: companyId).Single();
            
            using (var atomic = index.Atomic(log ?? "Adding period {0}-{1}", period.Start, period.End))
            {
                index.Periods.Add(period.Id);
                atomic.Save(period);
            }

            return period;
        }

        private IEnumerable<GitPeriod> RetrievePeriod(Guid companyId, Guid? id = null, DateTime? start = null, DateTime? end = null, DateTime? date = null)
        {
            var index = this.Client.Retrieve(companyId: companyId).Single();

            foreach (var periodId in index.Periods)
            {
                GitPeriod period;
                try
                {
                    period = index.Load<GitPeriod>(periodId);
                }
                catch (Exception e)
                {
                    this.OnError(e);
                    continue;
                }

                if ((null == id || period.Id.Equals(id)) &&
                    (null == start || period.Start.Equals(start)) &&
                    (null == end || period.End.Equals(end)) &&
                    (null == date || (period.Start < date && period.End > date)))
                {
                    yield return period;
                }
            }
        }

        private GitPeriod UpdatePeriod(GitPeriod period, DateTime start, DateTime end, string log)
        {
            period.Start = start;
            period.End = end;

            using (var atomic = this.Client.Retrieve(id: period.Index).Single().Atomic(log ?? "Updating period"))
            {
                return atomic.Save(period);
            }
        }

        private void DeletePeriod(GitPeriod period, string log)
        {
            var index = this.Client.Retrieve(id: period.Index).Single();
            index.Periods.Remove(period.Id);

            using (var atomic = index.Atomic(log ?? "Removing Period"))
            {
                atomic.Delete(period);
            }
        }

        #endregion
    }
}