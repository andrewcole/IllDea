using Illallangi.IllDea.Client.Txn;

namespace Illallangi.IllDea.Client
{
    using Illallangi.IllDea.Client.Company;
    using Illallangi.IllDea.Client.Settings;
    using Illallangi.IllDea.Logging;
    using Illallangi.IllDea.Model;

    public interface IDeaClient : ILogging
    {
        ICompanyClient Company { get; }
        ISettingsClient Settings { get; }

        ICrudClient<IAccount> Account { get; }
        ICrudClient<IDocument> Document { get; } 
        ICrudClient<IPeriod> Period { get; }
        ITxnClient Txn { get; }
    }
}