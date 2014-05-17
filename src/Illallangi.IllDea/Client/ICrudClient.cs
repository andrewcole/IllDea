namespace Illallangi.IllDea.Client
{
    using System;
    using System.Collections.Generic;

    public interface ICrudClient<T>
    {
        T Create(Guid companyId, T txn, string log = null);

        IEnumerable<T> Retrieve(Guid companyId);

        T Update(Guid companyId, T document, string log = null);

        void Delete(Guid companyId, T txn, string log = null);
    }
}
