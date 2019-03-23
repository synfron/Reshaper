using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ReshaperCore.Vars
{
	public class VariableStringJsonConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(VariableString);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			VariableString str = null;
			try
			{
				JObject jsonObj = JObject.Load(reader);
				JToken value = null;
				if (jsonObj.TryGetValue("FormattedString", out value))
				{
					string rawString = value.Value<string>();
					if (rawString != null)
					{
						str = VariableString.GetAsVariableString(rawString);
					}
				}
			}
			catch (Exception)
			{
			}
			return str;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			VariableString varString = value as VariableString;
			writer.WriteStartObject();
			writer.WritePropertyName("$type");
			writer.WriteValue("ReshaperCore.Vars.VariableString, ReshaperCore");
			writer.WritePropertyName("FormattedString");
			writer.WriteValue(varString?.GetFormattedString());
			writer.WriteEndObject();
		}
	}
}
