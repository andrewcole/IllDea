using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Illallangi.IllDea.Model;

namespace Illallangi.IllDea.Client.Txn
{
    public static class TxnClientExtensions
    {
        public static IEnumerable<TxnWithBalance> RetrieveForPeriodAndAccount(this ICrudClient<ITxn> txnClient, Guid companyId, Guid periodId, Guid accountId)
        {
            decimal balance = 0;

            foreach (var txn in txnClient.Retrieve(companyId).Where(txn => txn.Items.Any(i => i.Account.Equals(accountId))))
            {
                balance = balance + txn.Items.Where(i => i.Account.Equals(accountId)).Sum(i => i.Amount);

                if (txn.Period.Equals(periodId))
                {
                    yield return Mapper.DynamicMap<TxnWithBalance>(txn).SetBalance(balance);
                }
            }
        }

        private static TxnWithBalance SetBalance(this TxnWithBalance txn, decimal balance)
        {
            txn.Balance = balance;
            return txn;
        }
    }
}
