namespace Illallangi.IllDea
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Net;

    using Illallangi.IllDea.Model;

    using Newtonsoft.Json;

    public sealed class Atomic : IDisposable
    {
        #region Fields

        private static JsonSerializerSettings staticSerializerSettings;

        #endregion

        #region Properties

        private static JsonSerializerSettings SerializerSettings
        {
            get
            {
                return Atomic.staticSerializerSettings
                       ?? (Atomic.staticSerializerSettings = Atomic.GetSerializerSettings());
            }
        }

        #endregion

        #region Fields

        private readonly GitSettings currentIndex;

        private readonly string currentMessage;

        private readonly object[] currentArgs;

        private readonly ICollection<BaseModel> currentAdds;

        private readonly ICollection<BaseModel> currentDeletes;

        private readonly ICollection<string> currentFiles; 

        #endregion

        #region Constructor

        public Atomic(GitSettings index, string message, params object[] args)
        {
            this.currentIndex = index;
            this.currentMessage = message;
            this.currentArgs = args;
            this.currentAdds = new Collection<BaseModel>();
            this.currentDeletes = new Collection<BaseModel>();
            this.currentFiles = new Collection<string>();
        }

        #endregion

        public T Save<T>(T obj) where T : BaseModel
        {
            this.Adds.Add(obj);
            return obj;
        }

        public void Delete<T>(T obj) where T : BaseModel
        {
            this.Deletes.Add(obj);
        }

        public void Dispose()
        {
            if (!Directory.Exists(this.Index.RootPath))
            {
                Directory.CreateDirectory(this.Index.RootPath);
            }

            if (null == GitSharp.Repository.FindRepository(this.Index.RootPath))
            {
                GitSharp.Repository.Init(this.Index.RootPath);
            }

            using (var repo = new GitSharp.Repository(this.Index.RootPath))
            {
                foreach (var obj in this.Adds)
                {
                    repo.Index.Add(Atomic.Save(this.Index, obj));
                }

                foreach (var obj in this.Deletes)
                {
                    repo.Index.Delete(Atomic.Save(this.Index, obj));
                }

                foreach (var file in this.Files)
                {
                    repo.Index.Add(file);
                }

                repo.Index.Add(Atomic.Save(this.Index));
                repo.Index.Add(Atomic.Save(this.Index, this.Index));

                repo.Commit(
                    string.Format(this.Message, this.Args),
                    new GitSharp.Author(this.Index.AuthorName, this.Index.AuthorEmail));
            }
        }

        private GitSettings Index
        {
            get
            {
                return this.currentIndex;
            }
        }

        private ICollection<BaseModel> Adds
        {
            get
            {
                return this.currentAdds;
            }
        }

        private ICollection<BaseModel> Deletes
        {
            get
            {
                return this.currentDeletes;
            }
        }

        private ICollection<string> Files
        {
            get
            {
                return this.currentFiles;
            }
        }

        private string Message
        {
            get
            {
                return this.currentMessage;
            }
        }

        private object[] Args
        {
            get
            {
                return this.currentArgs;
            }
        }

        private static string Save(GitSettings index)
        {
            const string File = "index.json";
            Atomic.Serialize(new { index = index.Id }, Path.Combine(index.RootPath, File));
            return File;
        }

        private static string Save<T>(GitSettings index, T obj) where T : BaseModel
        {
            var file = string.Format("{0}.json", obj.Id);
            obj.Index = index.Id;
            Atomic.Serialize(obj, Path.Combine(index.RootPath, file));
            return file;
        }

        private static T Serialize<T>(T obj, string path)
        {
            File.WriteAllText(
                path,
                JsonConvert.SerializeObject(
                    obj,
                    Formatting.Indented,
                    Atomic.SerializerSettings));
            return obj;
        }

        private static JsonSerializerSettings GetSerializerSettings()
        {
            return new JsonSerializerSettings
                       {
                           Culture = CultureInfo.InvariantCulture,
                           MissingMemberHandling = MissingMemberHandling.Error,
                       };
        }

        public void Download(Uri uri, string file)
        {
            using (var wc = new WebClient())
            {
                wc.DownloadFile(uri, Path.Combine(Index.RootPath, file));
            }
            this.Files.Add(file);
        }
    }
}