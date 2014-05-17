namespace Illallangi.IllDea.Model
{
    using System;

    public interface IPeriod : IBaseModel
    {
        DateTime Start { get; set; }
        
        DateTime End { get; set; }
    }
}