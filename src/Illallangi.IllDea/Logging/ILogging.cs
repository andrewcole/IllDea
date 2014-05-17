namespace Illallangi.IllDea.Logging
{
    using System;
    using System.IO;

    public interface ILogging
    {
        event EventHandler<LogEventArgs> Debug;

        event EventHandler<LogEventArgs> Verbose;

        event EventHandler<LogEventArgs> Warning;

        event EventHandler<ErrorEventArgs> Error;

        void OnDebug(string message, params object[] args);

        void OnVerbose(string message, params object[] args);

        void OnWarning(string message, params object[] args);

        void OnError(Exception exception);
    }
}