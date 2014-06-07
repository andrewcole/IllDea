using System;
using System.Linq;

namespace Illallangi.IllDea.PowerShell.Employee
{
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    [Cmdlet(VerbsCommon.New, Nouns.Employee)]
    public sealed class NewEmployeeCmdlet : IdDeaCmdlet, IEmployee
    {
        #region Fields

        #region Parameter Sets

        private const string FilterParameterSet = @"Filter";

        private const string IdParameterSet = @"Id";

        #endregion

        #region SalaryExpenseAccount Fields

        private Guid? currentSalaryExpenseAccount;

        private string currentSalaryExpenseAccountName;

        private WildcardPattern currentSalaryExpenseAccountNameWildcardPattern;

        private string currentSalaryExpenseAccountNumber;

        private WildcardPattern currentSalaryExpenseAccountNumberWildcardPattern;

        #endregion

        #region EmployeeLiabilityAccount Fields

        private Guid? currentEmployeeLiabilityAccount;

        private string currentEmployeeLiabilityAccountName;

        private WildcardPattern currentEmployeeLiabilityAccountNameWildcardPattern;

        private string currentEmployeeLiabilityAccountNumber;

        private WildcardPattern currentEmployeeLiabilityAccountNumberWildcardPattern;

        #endregion

        #region IncomeTaxLiability Fields

        private Guid? currentIncomeTaxLiabilityAccount;

        private string currentIncomeTaxLiabilityAccountName;

        private WildcardPattern currentIncomeTaxLiabilityAccountNameWildcardPattern;

        private string currentIncomeTaxLiabilityAccountNumber;

        private WildcardPattern currentIncomeTaxLiabilityAccountNumberWildcardPattern;

        #endregion

        #region SuperannuationExpenseAccount Fields

        private Guid? currentSuperannuationExpenseAccount;

        private string currentSuperannuationExpenseAccountName;

        private WildcardPattern currentSuperannuationExpenseAccountNameWildcardPattern;

        private string currentSuperannuationExpenseAccountNumber;

        private WildcardPattern currentSuperannuationExpenseAccountNumberWildcardPattern;

        #endregion

        #region SuperannuationLiabilityAccount Fields

        private Guid? currentSuperannuationLiabilityAccount;

        private string currentSuperannuationLiabilityAccountName;

        private WildcardPattern currentSuperannuationLiabilityAccountNameWildcardPattern;

        private string currentSuperannuationLiabilityAccountNumber;

        private WildcardPattern currentSuperannuationLiabilityAccountNumberWildcardPattern;

        #endregion

        #endregion

        #region Simple Parameters

        [Parameter(Mandatory = true, ParameterSetName = NewEmployeeCmdlet.IdParameterSet)]
        [Parameter(Mandatory = true, ParameterSetName = NewEmployeeCmdlet.FilterParameterSet)]
        public string Name { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = NewEmployeeCmdlet.IdParameterSet)]
        [Parameter(Mandatory = true, ParameterSetName = NewEmployeeCmdlet.FilterParameterSet)]
        public string Address { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = NewEmployeeCmdlet.IdParameterSet)]
        [Parameter(Mandatory = true, ParameterSetName = NewEmployeeCmdlet.FilterParameterSet)]
        public string City { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = NewEmployeeCmdlet.IdParameterSet)]
        [Parameter(Mandatory = true, ParameterSetName = NewEmployeeCmdlet.FilterParameterSet)]
        public string State { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = NewEmployeeCmdlet.IdParameterSet)]
        [Parameter(Mandatory = true, ParameterSetName = NewEmployeeCmdlet.FilterParameterSet)]
        public string PostCode { get; set; }

        #endregion

        #region Account Parameters

        public Guid SalaryExpenseAccount
        {
            get
            {
                if (!this.currentSalaryExpenseAccount.HasValue)
                {
                    this.currentSalaryExpenseAccount =
                        this.Client.Account.Retrieve(this.CompanyId).Single(this.IsSalaryExpenseAccountMatch).Id;
                }

                return this.currentSalaryExpenseAccount.Value;
            }
            set
            {
                this.currentSalaryExpenseAccount = value;
            }
        }

        public Guid EmployeeLiabilityAccount
        {
            get
            {
                if (!this.currentEmployeeLiabilityAccount.HasValue)
                {
                    this.currentEmployeeLiabilityAccount =
                        this.Client.Account.Retrieve(this.CompanyId).Single(this.IsEmployeeLiabilityAccountMatch).Id;
                }

                return this.currentEmployeeLiabilityAccount.Value;
            }
            set
            {
                this.currentEmployeeLiabilityAccount = value;
            }
        }


        public Guid IncomeTaxLiabilityAccount
        {
            get
            {
                if (!this.currentIncomeTaxLiabilityAccount.HasValue)
                {
                    this.currentIncomeTaxLiabilityAccount =
                        this.Client.Account.Retrieve(this.CompanyId).Single(this.IsIncomeTaxLiabilityAccountMatch).Id;
                }

                return this.currentIncomeTaxLiabilityAccount.Value;
            }
            set
            {
                this.currentIncomeTaxLiabilityAccount = value;
            }
        }

       

        public Guid SuperannuationExpenseAccount
        {
            get
            {
                if (!this.currentSuperannuationExpenseAccount.HasValue)
                {
                    this.currentSuperannuationExpenseAccount =
                        this.Client.Account.Retrieve(this.CompanyId).Single(this.IsSuperannuationExpenseAccountMatch).Id;
                }

                return this.currentSuperannuationExpenseAccount.Value;
            }
            set
            {
                this.currentSuperannuationExpenseAccount = value;
            }
        }
        

        public Guid SuperannuationLiabilityAccount
        {
            get
            {
                if (!this.currentSuperannuationLiabilityAccount.HasValue)
                {
                    this.currentSuperannuationLiabilityAccount =
                        this.Client.Account.Retrieve(this.CompanyId).Single(this.IsSuperannuationLiabilityAccountMatch).Id;
                }

                return this.currentSuperannuationLiabilityAccount.Value;
            }
            set
            {
                this.currentSuperannuationLiabilityAccount = value;
            }
        }


        #endregion

        #region IsMatch Methods

        private bool IsSalaryExpenseAccountMatch(IAccount account)
        {
            switch (this.ParameterSetName)
            {
                case NewEmployeeCmdlet.IdParameterSet:
                    return this.SalaryExpenseAccountId.HasValue &&
                           this.SalaryExpenseAccountId.Value.Equals(account.Id);

                case NewEmployeeCmdlet.FilterParameterSet:
                    return this.SalaryExpenseAccountNumberWildcardPattern.IsMatch(account.Number)
                           && this.SalaryExpenseAccountNameWildcardPattern.IsMatch(account.Name);

                default:
                    throw new NotImplementedException(this.ParameterSetName);
            }
        }

        private bool IsEmployeeLiabilityAccountMatch(IAccount account)
        {
            switch (this.ParameterSetName)
            {
                case NewEmployeeCmdlet.IdParameterSet:
                    return this.EmployeeLiabilityAccountId.HasValue &&
                           this.EmployeeLiabilityAccountId.Value.Equals(account.Id);

                case NewEmployeeCmdlet.FilterParameterSet:
                    return this.EmployeeLiabilityAccountNumberWildcardPattern.IsMatch(account.Number)
                           && this.EmployeeLiabilityAccountNameWildcardPattern.IsMatch(account.Name);

                default:
                    throw new NotImplementedException(this.ParameterSetName);
            }
        }

        private bool IsIncomeTaxLiabilityAccountMatch(IAccount account)
        {
            switch (this.ParameterSetName)
            {
                case NewEmployeeCmdlet.IdParameterSet:
                    return this.IncomeTaxLiabilityAccountId.HasValue &&
                           this.IncomeTaxLiabilityAccountId.Value.Equals(account.Id);

                case NewEmployeeCmdlet.FilterParameterSet:
                    return this.IncomeTaxLiabilityAccountNumberWildcardPattern.IsMatch(account.Number)
                           && this.IncomeTaxLiabilityAccountNameWildcardPattern.IsMatch(account.Name);

                default:
                    throw new NotImplementedException(this.ParameterSetName);
            }
        }

        private bool IsSuperannuationExpenseAccountMatch(IAccount account)
        {
            switch (this.ParameterSetName)
            {
                case NewEmployeeCmdlet.IdParameterSet:
                    return this.SuperannuationExpenseAccountId.HasValue &&
                           this.SuperannuationExpenseAccountId.Value.Equals(account.Id);

                case NewEmployeeCmdlet.FilterParameterSet:
                    return this.SuperannuationExpenseAccountNumberWildcardPattern.IsMatch(account.Number)
                           && this.SuperannuationExpenseAccountNameWildcardPattern.IsMatch(account.Name);

                default:
                    throw new NotImplementedException(this.ParameterSetName);
            }
        }

        private bool IsSuperannuationLiabilityAccountMatch(IAccount account)
        {
            switch (this.ParameterSetName)
            {
                case NewEmployeeCmdlet.IdParameterSet:
                    return this.SuperannuationLiabilityAccountId.HasValue &&
                           this.SuperannuationLiabilityAccountId.Value.Equals(account.Id);

                case NewEmployeeCmdlet.FilterParameterSet:
                    return this.SuperannuationLiabilityAccountNumberWildcardPattern.IsMatch(account.Number)
                           && this.SuperannuationLiabilityAccountNameWildcardPattern.IsMatch(account.Name);

                default:
                    throw new NotImplementedException(this.ParameterSetName);
            }
        }

        #endregion

        #region Number WildcardPattern Properties

        private WildcardPattern SalaryExpenseAccountNumberWildcardPattern
        {
            get
            {
                return this.currentSalaryExpenseAccountNumberWildcardPattern
                       ?? (this.currentSalaryExpenseAccountNumberWildcardPattern =
                           new WildcardPattern(this.SalaryExpenseAccountNumber ?? @"*", WildcardOptions.Compiled | WildcardOptions.IgnoreCase));
            }
        }

        private WildcardPattern EmployeeLiabilityAccountNumberWildcardPattern
        {
            get
            {
                return this.currentEmployeeLiabilityAccountNumberWildcardPattern
                       ?? (this.currentEmployeeLiabilityAccountNumberWildcardPattern =
                           new WildcardPattern(this.EmployeeLiabilityAccountNumber ?? @"*", WildcardOptions.Compiled | WildcardOptions.IgnoreCase));
            }
        }

        private WildcardPattern IncomeTaxLiabilityAccountNumberWildcardPattern
        {
            get
            {
                return this.currentIncomeTaxLiabilityAccountNumberWildcardPattern
                       ?? (this.currentIncomeTaxLiabilityAccountNumberWildcardPattern =
                           new WildcardPattern(this.IncomeTaxLiabilityAccountNumber ?? @"*", WildcardOptions.Compiled | WildcardOptions.IgnoreCase));
            }
        }

        private WildcardPattern SuperannuationExpenseAccountNumberWildcardPattern
        {
            get
            {
                return this.currentSuperannuationExpenseAccountNumberWildcardPattern
                       ?? (this.currentSuperannuationExpenseAccountNumberWildcardPattern =
                           new WildcardPattern(this.SuperannuationExpenseAccountNumber ?? @"*", WildcardOptions.Compiled | WildcardOptions.IgnoreCase));
            }
        }

        private WildcardPattern SuperannuationLiabilityAccountNumberWildcardPattern
        {
            get
            {
                return this.currentSuperannuationLiabilityAccountNumberWildcardPattern
                       ?? (this.currentSuperannuationLiabilityAccountNumberWildcardPattern =
                           new WildcardPattern(this.SuperannuationLiabilityAccountNumber ?? @"*", WildcardOptions.Compiled | WildcardOptions.IgnoreCase));
            }
        }

        #endregion

        #region Name WildcardPattern Properties

        private WildcardPattern SalaryExpenseAccountNameWildcardPattern
        {
            get
            {
                return this.currentSalaryExpenseAccountNameWildcardPattern
                       ?? (this.currentSalaryExpenseAccountNameWildcardPattern =
                           new WildcardPattern(this.SalaryExpenseAccountName ?? @"*", WildcardOptions.Compiled | WildcardOptions.IgnoreCase));
            }
        }

        private WildcardPattern EmployeeLiabilityAccountNameWildcardPattern
        {
            get
            {
                return this.currentEmployeeLiabilityAccountNameWildcardPattern
                       ?? (this.currentEmployeeLiabilityAccountNameWildcardPattern =
                           new WildcardPattern(this.EmployeeLiabilityAccountName ?? @"*", WildcardOptions.Compiled | WildcardOptions.IgnoreCase));
            }
        }

        private WildcardPattern IncomeTaxLiabilityAccountNameWildcardPattern
        {
            get
            {
                return this.currentIncomeTaxLiabilityAccountNameWildcardPattern
                       ?? (this.currentIncomeTaxLiabilityAccountNameWildcardPattern =
                           new WildcardPattern(this.IncomeTaxLiabilityAccountName ?? @"*", WildcardOptions.Compiled | WildcardOptions.IgnoreCase));
            }
        }

        private WildcardPattern SuperannuationExpenseAccountNameWildcardPattern
        {
            get
            {
                return this.currentSuperannuationExpenseAccountNameWildcardPattern
                       ?? (this.currentSuperannuationExpenseAccountNameWildcardPattern =
                           new WildcardPattern(this.SuperannuationExpenseAccountName ?? @"*", WildcardOptions.Compiled | WildcardOptions.IgnoreCase));
            }
        }

        private WildcardPattern SuperannuationLiabilityAccountNameWildcardPattern
        {
            get
            {
                return this.currentSuperannuationLiabilityAccountNameWildcardPattern
                       ?? (this.currentSuperannuationLiabilityAccountNameWildcardPattern =
                           new WildcardPattern(this.SuperannuationLiabilityAccountName ?? @"*", WildcardOptions.Compiled | WildcardOptions.IgnoreCase));
            }
        }

        #endregion

        [Parameter(Mandatory = true, ParameterSetName = NewEmployeeCmdlet.IdParameterSet)]
        public Guid? SalaryExpenseAccountId { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = NewEmployeeCmdlet.IdParameterSet)]
        public Guid? EmployeeLiabilityAccountId { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = NewEmployeeCmdlet.IdParameterSet)]
        public Guid? IncomeTaxLiabilityAccountId { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = NewEmployeeCmdlet.IdParameterSet)]
        public Guid? SuperannuationExpenseAccountId { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = NewEmployeeCmdlet.IdParameterSet)]
        public Guid? SuperannuationLiabilityAccountId { get; set; }

        [SupportsWildcards]
        [Parameter(Mandatory = false, ParameterSetName = NewEmployeeCmdlet.FilterParameterSet)]
        public string SalaryExpenseAccountName
        {
            get
            {
                return this.currentSalaryExpenseAccountName;
            }
            set
            {
                this.currentSalaryExpenseAccountName = value;
                this.currentSalaryExpenseAccountNameWildcardPattern = null;
            }
        }

        [SupportsWildcards]
        [Parameter(Mandatory = false, ParameterSetName = NewEmployeeCmdlet.FilterParameterSet)]
        public string SalaryExpenseAccountNumber
        {
            get
            {
                return this.currentSalaryExpenseAccountNumber;
            }
            set
            {
                this.currentSalaryExpenseAccountNumber = value;
                this.currentSalaryExpenseAccountNumberWildcardPattern = null;
            }
        }

        [SupportsWildcards]
        [Parameter(Mandatory = false, ParameterSetName = NewEmployeeCmdlet.FilterParameterSet)]
        public string EmployeeLiabilityAccountName
        {
            get
            {
                return this.currentEmployeeLiabilityAccountName;
            }
            set
            {
                this.currentEmployeeLiabilityAccountName = value;
                this.currentEmployeeLiabilityAccountNameWildcardPattern = null;
            }
        }

        [SupportsWildcards]
        [Parameter(Mandatory = false, ParameterSetName = NewEmployeeCmdlet.FilterParameterSet)]
        public string EmployeeLiabilityAccountNumber
        {
            get
            {
                return this.currentEmployeeLiabilityAccountNumber;
            }
            set
            {
                this.currentEmployeeLiabilityAccountNumber = value;
                this.currentEmployeeLiabilityAccountNumberWildcardPattern = null;
            }
        }

        [SupportsWildcards]
        [Parameter(Mandatory = false, ParameterSetName = NewEmployeeCmdlet.FilterParameterSet)]
        public string IncomeTaxLiabilityAccountName
        {
            get
            {
                return this.currentIncomeTaxLiabilityAccountName;
            }
            set
            {
                this.currentIncomeTaxLiabilityAccountName = value;
                this.currentIncomeTaxLiabilityAccountNameWildcardPattern = null;
            }
        }

        [SupportsWildcards]
        [Parameter(Mandatory = false, ParameterSetName = NewEmployeeCmdlet.FilterParameterSet)]
        public string IncomeTaxLiabilityAccountNumber
        {
            get
            {
                return this.currentIncomeTaxLiabilityAccountNumber;
            }
            set
            {
                this.currentIncomeTaxLiabilityAccountNumber = value;
                this.currentIncomeTaxLiabilityAccountNumberWildcardPattern = null;
            }
        }

        [SupportsWildcards]
        [Parameter(Mandatory = false, ParameterSetName = NewEmployeeCmdlet.FilterParameterSet)]
        public string SuperannuationExpenseAccountName
        {
            get
            {
                return this.currentSuperannuationExpenseAccountName;
            }
            set
            {
                this.currentSuperannuationExpenseAccountName = value;
                this.currentSuperannuationExpenseAccountNameWildcardPattern = null;
            }
        }

        [SupportsWildcards]
        [Parameter(Mandatory = false, ParameterSetName = NewEmployeeCmdlet.FilterParameterSet)]
        public string SuperannuationExpenseAccountNumber
        {
            get
            {
                return this.currentSuperannuationExpenseAccountNumber;
            }
            set
            {
                this.currentSuperannuationExpenseAccountNumber = value;
                this.currentSuperannuationExpenseAccountNumberWildcardPattern = null;
            }
        }

        [SupportsWildcards]
        [Parameter(Mandatory = false, ParameterSetName = NewEmployeeCmdlet.FilterParameterSet)]
        public string SuperannuationLiabilityAccountName
        {
            get
            {
                return this.currentSuperannuationLiabilityAccountName;
            }
            set
            {
                this.currentSuperannuationLiabilityAccountName = value;
                this.currentSuperannuationLiabilityAccountNameWildcardPattern = null;
            }
        }

        [SupportsWildcards]
        [Parameter(Mandatory = false, ParameterSetName = NewEmployeeCmdlet.FilterParameterSet)]
        public string SuperannuationLiabilityAccountNumber
        {
            get
            {
                return this.currentSuperannuationLiabilityAccountNumber;
            }
            set
            {
                this.currentSuperannuationLiabilityAccountNumber = value;
                this.currentSuperannuationLiabilityAccountNumberWildcardPattern = null;
            }
        }

        protected override void ProcessRecord()
        {
            this.WriteObject(this.Client.Employee.Create(this.CompanyId, this, this.ToString()));
        }

        public override string ToString()
        {
            return string.Format(
                @"New-Employee -Name ""{0}""",
                this.Name);
        }
    }
}