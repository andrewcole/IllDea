namespace Illallangi.IllDea.Client
{
    using System;
    using System.IO;

    using Illallangi.IllDea.Logging;

    public abstract class BaseClient : ILogging
    {
        public virtual event EventHandler<LogEventArgs> Debug;

        public virtual event EventHandler<LogEventArgs> Verbose;

        public virtual event EventHandler<LogEventArgs> Warning;

        public virtual event EventHandler<ErrorEventArgs> Error;

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