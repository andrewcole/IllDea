using System;
using Illallangi.IllDea.Model;

namespace Illallangi.IllDea
{
    public class UpdateInternalTxnException : Exception
    {
        public UpdateInternalTxnException(GitTxn gitTxn)
                    : base(string.Format(@"Attempted to update transaction {0} with Internal flag set", gitTxn.Id))
        {
        }
    }
}