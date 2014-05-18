namespace Illallangi.IllDea.PowerShell
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Management.Automation;

    using Illallangi.IllDea.Client;
    using Illallangi.IllDea.Logging;
    using Illallangi.IllDea.Model;

    public abstract class DeaCmdlet : PSCmdlet, ILogging
    {
        protected DeaCmdlet()
        {
            this.Debug += (sender, args) => this.WriteDebug(args.ToString());
            this.Verbose += (sender, args) => this.WriteVerbose(args.ToString());
            this.Warning += (sender, args) => this.WriteWarning(args.ToString());
            this.Error += (sender, args) => this.WriteError(new ErrorRecord(args.GetException(), string.Empty, ErrorCategory.NotSpecified, sender));
        }

        private IDeaClient currentClient;

        private Guid? currentCompanyId;

        protected IDeaClient Client
        {
            get
            {
                return this.currentClient ?? (this.currentClient = GetClient());
            }
        }

        protected ICompany OpenCompany(ICompany company)
        {
            this.SessionState.PSVariable.Set(new PSVariable("DeaCompany", company, ScopedItemOptions.AllScope));
            return company;
        }

        protected ICompany GetOpenCompany()
        {
            var deaCompany = this.SessionState.PSVariable.Get(@"DeaCompany");
            if (null == deaCompany)
            {
                var companies = this.Client.Company.Retrieve().ToList();
                if (1 != companies.Count())
                {
                    throw new Exception("No company is open - use Open-Company first");
                }

                this.OnWarning("No company is open - opening {0}", companies.Single().Name);
                return this.OpenCompany(companies.Single());
            }

            var company = (ICompany)(deaCompany.Value);
            if (null != company)
            {
                return company;
            }
            
            throw new Exception("No company is open - use Open-Company first");
        }

        [Parameter(DontShow = true)]
        public Guid CompanyId
        {
            get
            {
                return
                    (this.currentCompanyId.HasValue
                         ? this.currentCompanyId
                         : (this.currentCompanyId = this.GetOpenCompany().Id)).Value;

            }
            set
            {
                this.currentCompanyId = value;
            }
        }

        private IDeaClient GetClient()
        {
            return new GitDeaClient().HookEvents(this);
        }

        public event EventHandler<LogEventArgs> Debug;

        public event EventHandler<LogEventArgs> Verbose;

        public event EventHandler<LogEventArgs> Warning;

        public event EventHandler<ErrorEventArgs> Error;

        public virtual void OnDebug(string message, params object[] args)
        {
            var debug = this.Debug;

            if (null != debug)
            {
                debug(this, new LogEventArgs(message, args));
            }
        }

        public virtual void OnVerbose(string message, params object[] args)
        {
            var info = this.Verbose;

            if (null != info)
            {
                info(this, new LogEventArgs(message, args));
            }
        }

        public virtual void OnWarning(string message, params object[] args)
        {
            var warning = this.Warning;

            if (null != warning)
            {
                warning(this, new LogEventArgs(message, args));
            }
        }

        public virtual void OnError(Exception exception)
        {
            var error = this.Error;

            if (null != error)
            {
                error(this, new ErrorEventArgs(exception));
            }
        }
    }
}