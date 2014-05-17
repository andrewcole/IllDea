namespace Illallangi.IllDea.Model
{
    using System;
    using System.IO;

    public interface IDocument : IBaseModel
    {
        DateTime Date { get; set; }

        Guid Period { get; set; }

        string Title { get; set; }

        bool Compilation { get; set; }

        Uri Uri { get; set; }
    }
}