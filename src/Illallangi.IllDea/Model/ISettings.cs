namespace Illallangi.IllDea.Model
{
    public interface ISettings : IBaseModel
    {
        string AuthorEmail { get; set; }
        string AuthorName { get; set; }
    }
}