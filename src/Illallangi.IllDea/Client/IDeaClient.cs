using Illallangi.IllDea.Client.Txn;

namespace Illallangi.IllDea.Client
{
    using Illallangi.IllDea.Client.Account;
    using Illallangi.IllDea.Client.Company;
    using Illallangi.IllDea.Client.Settings;
    using Illallangi.IllDea.Logging;
    using Illallangi.IllDea.Model;

    public interface IDeaClient : ILogging
    {
        ICompanyClient Company { get; }
        ISettingsClient Settings { get; }

        IAccountClient Account { get; }
        ICrudClient<IDocument> Document { get; } 
        ICrudClient<IPeriod> Period { get; }
        ICrudClient<IEmployee> Employee { get; } 
        ITxnClient Txn { get; }

        ICrudClient<IPayroll> Payroll { get; } 
    }
}