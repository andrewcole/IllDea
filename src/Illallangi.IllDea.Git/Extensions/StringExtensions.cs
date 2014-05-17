namespace Illallangi.IllDea.Model
{
    public static class StringExtensions
    {
        public static string TrimStart(this string inp, string trim)
        {
            if (inp.StartsWith(trim))
            {
                inp = inp.Substring(trim.Length);
            }
            return inp;
        }
    }
}