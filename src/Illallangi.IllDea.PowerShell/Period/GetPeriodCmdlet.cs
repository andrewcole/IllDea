namespace Illallangi.IllDea.PowerShell.Period
{
    using System;
    using System.Linq;
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    [Cmdlet(VerbsCommon.Get, Nouns.Period)]
    public class GetPeriodCmdlet : DeaCmdlet
    {
        private const string FilterParameterSet = @"Filter";
        private const string IdParameterSet = @"Id";

        [Parameter(Mandatory = true, ParameterSetName = GetPeriodCmdlet.IdParameterSet, ValueFromPipelineByPropertyName = true)]
        public Guid? Id { get; set; }

        [Parameter(Position = 1, ParameterSetName = GetPeriodCmdlet.FilterParameterSet)]
        public DateTime? Date { get; set; }

        [Parameter(ParameterSetName = GetPeriodCmdlet.FilterParameterSet)]
        public DateTime? Start { get; set; }

        [Parameter(ParameterSetName = GetPeriodCmdlet.FilterParameterSet)]
        public DateTime? End { get; set; }

        protected override void ProcessRecord()
        {
            this.WriteObject(this.Client.Period.Retrieve(this.CompanyId).Where(this.IsMatch), true);
        }

        protected virtual bool IsMatch(IPeriod period)
        {
            switch (this.ParameterSetName)
            {
                case GetPeriodCmdlet.IdParameterSet:
                    return this.Id.HasValue &&
                           this.Id.Value.Equals(period.Id);

                case GetPeriodCmdlet.FilterParameterSet:
                    return (null == this.Start || period.Start.Equals(this.Start)) &&
                           (null == this.End || period.End.Equals(this.End)) &&
                           (null == this.Date || (period.Start < this.Date && period.End > this.Date));

                default:
                    throw new NotImplementedException(this.ParameterSetName);
            }
        }
    }
}
