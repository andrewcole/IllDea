using System;
using Newtonsoft.Json;

namespace Illallangi.IllDea
{
    /// <summary>
    /// http://stackoverflow.com/questions/16972364/c-sharp-json-deserialization-can-i-intercept-the-deserialization-and-optionally
    /// </summary>
    public sealed class HrefConverter : JsonConverter
    {
        private const string Json = @".json";

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, string.Concat(value, Json));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.String:
                    return ((string)reader.Value).EndsWith(Json, StringComparison.InvariantCultureIgnoreCase) ?
                        Guid.Parse(((string)reader.Value).Substring(0, ((string)reader.Value).Length - Json.Length)) :
                        Guid.Parse((string)reader.Value);
                default:
                    return reader.Value;
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Guid);
        }
    }
}
