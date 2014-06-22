using System;
using System.Collections.Generic;
using Illallangi.IllDea.Model;

namespace Illallangi.IllDea.Client.Txn
{
    public interface ITxnClient : ICrudClient<ITxn>
    {
        IEnumerable<ITxn> RetrieveWithBalances(Guid companyId, Guid? periodId = null, Guid? accountId = null);
    }
}