using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SBCEAK.Util
{
  public class DateTimeConverter : JsonConverter<DateTime>
  {
    public override DateTime Read(
        ref Utf8JsonReader reader, 
        Type typeToConvert, 
        JsonSerializerOptions options
        )
    {
      return DateTime.Parse(reader.GetString());
    }
 
    public override void Write(
        Utf8JsonWriter writer, 
        DateTime value, 
        JsonSerializerOptions options
        )
    {
      if ((value.Hour != 0) || (value.Minute != 0) || (value.Second != 0)) 
        writer.WriteStringValue(value.ToString("dd/MM/yyyy HH:mm:ss"));
      else
        writer.WriteStringValue(value.ToString("dd/MM/yyyy"));
    }
  }
}
