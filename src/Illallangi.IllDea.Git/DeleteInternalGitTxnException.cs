using System;
using Illallangi.IllDea.Model;

namespace Illallangi.IllDea
{
    public class DeleteInternalGitTxnException : Exception
    {
        public DeleteInternalGitTxnException(GitTxn gitTxn)
            : base(string.Format(@"Attempted to delete transaction {0} with Internal flag set", gitTxn.Id))
        {
        }
    }
}