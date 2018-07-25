using System;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace Lunz.Microservice.AspNetCore
{
    public class UrlEncodeConverter : JsonConverter
    {
        public Encoding Encoding { get; }

        public UrlEncodeConverter()
        {
            Encoding = System.Text.Encoding.UTF8;
        }

        public UrlEncodeConverter(Encoding encoding)
        {
            Encoding = encoding;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var s = (string)reader.Value;
            return s == null ? null : HttpUtility.UrlDecode(s, Encoding);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var s = (string)value;
            writer.WriteValue(s == null ? null : HttpUtility.UrlEncode(s, Encoding));
        }
    }
}