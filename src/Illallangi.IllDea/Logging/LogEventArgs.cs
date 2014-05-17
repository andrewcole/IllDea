namespace Illallangi.IllDea.Logging
{
    using System;
    using System.Globalization;

    public sealed class LogEventArgs : EventArgs
    {
        private readonly string currentMessage;

        private readonly object[] currentArgs;

        public LogEventArgs(string message, params object[] args)
        {
            this.currentMessage = message;
            this.currentArgs = args;
        }

        public string Message
        {
            get
            {
                return this.currentMessage;
            }
        }

        public object[] Args
        {
            get
            {
                return this.currentArgs;
            }
        }

        public override string ToString()
        {
            return string.Format(this.Message, this.Args);
        }
    }
}