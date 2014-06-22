using System;
using System.Collections.Generic;

using Illallangi.IllDea.Model;

namespace Illallangi.IllDea.Client.Account
{
    public interface IAccountClient : ICrudClient<IAccount>
    {
        IEnumerable<IAccount> RetrieveWithBalances(Guid companyId, Guid? periodId = null, AccountType? accountType = null);
    }
}