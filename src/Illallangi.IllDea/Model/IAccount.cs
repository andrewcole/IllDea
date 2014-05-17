namespace Illallangi.IllDea.Model
{
    public interface IAccount : IBaseModel
    {
        string Name { get; set; }

        AccountType Type { get; set; }

        string Number { get; set; }
    }
}