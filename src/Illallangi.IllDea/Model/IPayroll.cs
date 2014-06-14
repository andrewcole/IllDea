using System;

namespace Illallangi.IllDea.Model
{
    public interface IPayroll : IBaseModel
    {
        DateTime Start { get; set; }

        DateTime End { get; set; }

        Guid Employee { get; set; }

        Guid PayTxn { get; set; }

        Guid SuperTxn { get; set; }

        decimal GrossPay { get; set; }
        decimal Tax { get; set; }
        decimal Super { get; set; }
    }
}