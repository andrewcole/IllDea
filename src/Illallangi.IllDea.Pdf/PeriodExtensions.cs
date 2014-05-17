namespace Illallangi.IllDea.Pdf
{
    using Illallangi.IllDea.Model;

    internal static class PeriodExtensions
    {
        public static string ToTitle(this IPeriod period)
        {
            if (period.End.Year == period.Start.Year && period.End.Month == period.Start.Month)
            {
                return string.Format("{0} to {1}", period.Start.ToString(@"dddd, d"), period.End.ToLongDateString());
            }

            return string.Format("{0} to {1}", period.Start.ToLongDateString(), period.End.ToLongDateString());
        }
    }
}