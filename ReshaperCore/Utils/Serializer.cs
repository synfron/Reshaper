using System;
using System.IO;
using Newtonsoft.Json;
using ReshaperCore.Vars;

namespace ReshaperCore.Utils
{
	public class Serializer
	{
		public static String Serialize(Object o)
		{
			return JsonConvert.SerializeObject(o, new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Objects,
				Converters = new JsonConverter[] { new VariableStringJsonConverter(), new EncodingJsonConvertercs() }
			});
		}

		public static void SerializeToFile(string filePath, object o)
		{
			FileInfo file = new FileInfo(filePath);
			file.Directory.Create();
			File.WriteAllText(filePath, Serializer.Serialize(o));
		}

		public static T Deserialize<T>(String serialized)
		{
			return JsonConvert.DeserializeObject<T>(serialized, new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Objects,
				Converters = new JsonConverter[] { new VariableStringJsonConverter(), new EncodingJsonConvertercs() },
				Binder = new SerializationBinder()
			});
		}

		public static T DeserializeFromFile<T>(string filePath)
		{
			T obj = default(T);
			if (File.Exists(filePath))
			{
				string fileText = File.ReadAllText(filePath);
				obj = Deserialize<T>(fileText);
			}
			return obj;
		}
	}
}
