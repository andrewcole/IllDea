namespace Illallangi.IllDea
{
    using Illallangi.IllDea.Model;

    public static class IndexGitAtomicExtensions
    {
        public static Atomic Atomic(this GitSettings index, string message, params object[] args)
        {
            return new Atomic(index, message, args);
        }        
    }
}