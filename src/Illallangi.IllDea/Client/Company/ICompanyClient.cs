namespace Illallangi.IllDea.Client.Company
{
    using System;
    using System.Collections.Generic;

    using Illallangi.IllDea.Logging;
    using Illallangi.IllDea.Model;

    public interface ICompanyClient : ILogging
    {
        ICompany Create(ICompany company, ISettings settings, string log = null);

        IEnumerable<ICompany> Retrieve();

        ICompany Update(Guid companyId, ICompany company, string log = null);
    }
}
