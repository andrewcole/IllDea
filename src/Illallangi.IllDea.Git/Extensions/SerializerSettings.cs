using System.Globalization;
using Newtonsoft.Json;

namespace Illallangi.IllDea
{
    public class SerializerSettings : JsonSerializerSettings
    {
        private static SerializerSettings staticInstance;

        public static SerializerSettings Instance
        {
            get
            {
                return SerializerSettings.staticInstance ??
                       (SerializerSettings.staticInstance = new SerializerSettings());
            }
        }
        public SerializerSettings()
        {
            this.Culture = CultureInfo.InvariantCulture;
            this.MissingMemberHandling = MissingMemberHandling.Error;
            this.NullValueHandling = NullValueHandling.Ignore;
            this.Converters = new JsonConverter[] {new HrefConverter()};
            this.Formatting = Formatting.Indented;
        }
    }
}