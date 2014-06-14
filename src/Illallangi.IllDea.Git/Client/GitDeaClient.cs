using Illallangi.IllDea.Client.Employee;
using Illallangi.IllDea.Client.Payroll;

namespace Illallangi.IllDea.Client
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Illallangi.IllDea.Client.Account;
    using Illallangi.IllDea.Client.Company;
    using Illallangi.IllDea.Client.Document;
    using Illallangi.IllDea.Client.Period;
    using Illallangi.IllDea.Client.Settings;
    using Illallangi.IllDea.Client.Txn;
    using Illallangi.IllDea.Logging;
    using Illallangi.IllDea.Model;

    public sealed class GitDeaClient : BaseClient, IDeaClient
    {
        #region Fields

        private const string DataFolderPath = @"%localappdata%\Illallangi Enterprises\Double Entry Accounting";

        private string currentDataFolder;

        private ICompanyClient currentCompany;

        private ISettingsClient currentSettings;
        
        private ICrudClient<IAccount> currentAccount;

        private ICrudClient<IDocument> currentDocument;

        private ICrudClient<IPeriod> currentPeriod;

        private ICrudClient<IEmployee> currentEmployee;
 
        private GitTxnClient currentTxn;

        private ICrudClient<IPayroll> currentPayroll; 
        #endregion

        #region Properties

        private string DataFolder
        {
            get
            {
                return this.currentDataFolder ?? (this.currentDataFolder = GitDeaClient.GetDataFolder());
            }
        }

        public ICompanyClient Company
        {
            get
            {
                return this.currentCompany ?? (this.currentCompany = this.GetCompanyClient());
            }
        }

        public ISettingsClient Settings
        {
            get
            {
                return this.currentSettings ?? (this.currentSettings = this.GetSettingsClient());
            }
        }

        public ICrudClient<IAccount> Account
        {
            get
            {
                return this.currentAccount ?? (this.currentAccount = this.GetAccountClient());
            }
        }

        public ICrudClient<IDocument> Document
        {
            get
            {
                return this.currentDocument?? (this.currentDocument = this.GetDocumentClient());
            }
        }

        public ICrudClient<IPeriod> Period
        {
            get
            {
                return this.currentPeriod ?? (this.currentPeriod = this.GetPeriodClient());
            }
        }

        public ITxnClient Txn
        {
            get { return this.GitTxn; }
        }

        public GitTxnClient GitTxn
        {
            get
            {
                return this.currentTxn ?? (this.currentTxn = this.GetTxnClient());
            }
        }

        public ICrudClient<IPayroll> Payroll
        {
            get
            {
                return this.currentPayroll ?? (this.currentPayroll = this.GetPayrollClient());
            }
        }

        public ICrudClient<IEmployee> Employee
        {
            get
            {
                return this.currentEmployee ?? (this.currentEmployee = this.GetEmployeeClient());
            }
        }

        #endregion

        #region Methods

        public IEnumerable<GitSettings> Retrieve(Guid? id = null, Guid? companyId = null)
        {
            foreach (var directory in Directory.GetDirectories(this.DataFolder))
            {
                var expectedIndex = Path.GetFileName(directory);

                if (!File.Exists(Path.Combine(directory, @"index.json")))
                {
                    this.OnDebug(
                        @"Skipping ""{0}"" as ""index.json"" does not exist",
                        directory);
                    continue;
                }

                GitSettings pointer;
                try
                {
                    pointer = Path.Combine(directory, @"index.json").LoadIndex();
                }
                catch (Exception e)
                {
                    this.OnError(e);
                    this.OnDebug(
                        @"Skipping ""{0}"" as deserialization of ""index.json"" failed",
                        directory);
                    continue;
                }

                if (!pointer.Index.ToString().Equals(expectedIndex))
                {
                    this.OnDebug(
                        @"Skipping ""{0}"" as ""index.json"" index guid is ""{1}"" (expected ""{2}"")",
                        directory,
                        pointer.Index,
                        expectedIndex);
                    continue;
                }

                GitSettings index;
                try
                {
                    index = Path.Combine(directory, string.Format(@"{0}.json", pointer.Index)).LoadIndex();
                }
                catch (Exception e)
                {
                    this.OnError(e);
                    this.OnDebug(
                        @"Skipping ""{0}"" as deserialization of ""{1}.json"" failed",
                        directory,
                        pointer.Index);
                    continue;
                }

                if (!index.Index.ToString().Equals(expectedIndex))
                {
                    this.OnDebug(@"Skipping ""{0}"" as ""{1}.json"" index guid is ""{2}"" (expected ""{3}"")",
                        directory,
                        pointer.Index,
                        pointer.Index,
                        expectedIndex);
                    continue;
                }

                if (!index.Id.ToString().Equals(expectedIndex))
                {
                    this.OnDebug(@"Skipping ""{0}"" as ""{1}.json"" id guid is ""{2}"" (expected ""{3}"")",
                        directory,
                        pointer.Index,
                        pointer.Index,
                        expectedIndex);
                    continue;
                }

                if ((null == id || index.Id.Equals(id)) && (null == companyId || index.Company.Equals(companyId)))
                {
                    index.RootPath = directory;
                    yield return index;
                }
            }
        }

        public GitSettings Create(ISettings companySettings)
        {
            Guid id;

            do
            {
                id = Guid.NewGuid();
            }
            while (this.Retrieve(id).Any());

            var rootPath = Path.Combine(this.DataFolder, id.ToString());

            var index = new GitSettings
            {
                Index = id,
                Id = id,
                RootPath = rootPath,
                AuthorEmail = companySettings.AuthorEmail,
                AuthorName = companySettings.AuthorName
            };

            using (var atomic = index.Atomic(@"Initial Commit"))
            {
                atomic.Save(index);
            }

            return index;
        }

        private static string GetDataFolder()
        {
            var dataFolder = Environment.ExpandEnvironmentVariables(GitDeaClient.DataFolderPath);

            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            return dataFolder;
        }

        private ICompanyClient GetCompanyClient()
        {
            return new GitCompanyClient(this).HookEvents(this);
        }

        private ISettingsClient GetSettingsClient()
        {
            return new GitSettingsClient(this).HookEvents(this);
        }

        private ICrudClient<IAccount> GetAccountClient()
        {
            return new GitAccountClient(this).HookEvents(this);
        }

        private ICrudClient<IDocument> GetDocumentClient()
        {
            return new GitDocumentClient(this).HookEvents(this);
        }

        private ICrudClient<IPeriod> GetPeriodClient()
        {
            return new GitPeriodClient(this).HookEvents(this);
        }

        private ICrudClient<IEmployee> GetEmployeeClient()
        {
            return new GitEmployeeClient(this).HookEvents(this);
        }

        private GitTxnClient GetTxnClient()
        {
            return new GitTxnClient(this).HookEvents(this);
        }

        private ICrudClient<IPayroll> GetPayrollClient()
        {
            return new GitPayrollClient(this).HookEvents(this);
        }

        #endregion
    }
}