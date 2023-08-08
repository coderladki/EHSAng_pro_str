using System;
using System.Text.Json;

namespace CRM.Server.Web.Api.Core.JsonConverters
{
    public class DateTimeConverter : System.Text.Json.Serialization.JsonConverter<System.DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string stringValue = reader.GetString();
                if (DateTime.TryParse(stringValue, out DateTime value))
                {
                    return value.ToLocalTime();
                }
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetDateTime().ToLocalTime();
            }

            throw new System.Text.Json.JsonException();
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }
}
