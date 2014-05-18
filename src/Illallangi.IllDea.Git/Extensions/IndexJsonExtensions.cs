namespace Illallangi.IllDea
{
    using System;
    using System.Globalization;
    using System.IO;

    using Illallangi.IllDea.Model;

    using Newtonsoft.Json;

    public static class IndexJsonExtensions
    {
        #region Fields

        private static JsonSerializerSettings staticSerializerSettings;

        #endregion

        #region Properties

        private static JsonSerializerSettings SerializerSettings
        {
            get
            {
                return IndexJsonExtensions.staticSerializerSettings
                       ?? (IndexJsonExtensions.staticSerializerSettings = IndexJsonExtensions.GetSerializerSettings());
            }
        }

        #endregion

        #region Methods

        public static GitSettings LoadIndex(this string path)
        {
            return IndexJsonExtensions.Deserialize<GitSettings>(path);
        }

        public static T Load<T>(this GitSettings index, Guid guid) where T : BaseModel
        {
            var result = IndexJsonExtensions.Deserialize<T>(Path.Combine(index.RootPath, string.Format("{0}.json", guid)));

            if (!result.Id.Equals(guid))
            {
                throw new InvalidDataException("ID does not match");
            }

            return result;
        }
        
        private static T Deserialize<T>(string path)
        {
            return JsonConvert.DeserializeObject<T>(
                File.ReadAllText(path),
                IndexJsonExtensions.SerializerSettings);
        }

        private static JsonSerializerSettings GetSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                Culture = CultureInfo.InvariantCulture,
                MissingMemberHandling = MissingMemberHandling.Error,
            };
        }

        #endregion
    }
}