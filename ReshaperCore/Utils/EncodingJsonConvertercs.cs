using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ReshaperCore.Utils
{
	public class EncodingJsonConvertercs : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Encoding);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			Encoding encoding = null;
			try
			{
				JObject jsonObj = JObject.Load(reader);
				JToken value = null;
				if (jsonObj.TryGetValue("Encoding", out value))
				{
					string rawString = value.Value<string>();
					encoding = Encoding.GetEncoding(rawString);
				}
			}
			catch (Exception)
			{
			}
			return encoding;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			Encoding encoding = value as Encoding;
			writer.WriteStartObject();
			writer.WritePropertyName("$type");
			writer.WriteValue("System.Text.Encoding, mscorlib");
			writer.WritePropertyName("Encoding");
			writer.WriteValue(encoding.WebName);
			writer.WriteEndObject();
		}
	}
}
