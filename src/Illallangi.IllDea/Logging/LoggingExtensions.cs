namespace Illallangi.IllDea.Logging
{
    public static class LoggingExtensions
    {
        public static T HookEvents<T>(this T source, ILogging target) where T : ILogging
        {
            source.Debug += (sender, args) => target.OnDebug(args.Message, args.Args);
            source.Verbose += (sender, args) => target.OnVerbose(args.Message, args.Args);
            source.Warning += (sender, args) => target.OnWarning(args.Message, args.Args);
            source.Error += (sender, args) => target.OnError(args.GetException());
            return source;
        }
    }
}