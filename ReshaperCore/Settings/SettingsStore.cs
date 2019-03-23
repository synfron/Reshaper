using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReshaperCore.Utils;

namespace ReshaperCore.Settings
{
	public abstract class SettingsStore : INotifyPropertyChanged
	{
		private string _filePath;
		private JObject _jsonModel = null;
		private bool _initializing = false;

		public static string StoragePath
		{
			private set;
			get;
		} = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/Synfron/Reshaper";

		public event PropertyChangedEventHandler PropertyChanged;

		public T GetValue<T>(string propertyName)
		{
			T returnVal = default(T);
			if (_jsonModel == null)
			{
				SetJsonModel();
			}
			JToken value = null;
			if (_jsonModel.TryGetValue(propertyName, out value))
			{
				returnVal = value.ToObject<T>();
			}
			else
			{
				returnVal = GetDefaultValue<T>(propertyName);
			}
			return returnVal;
		}

		public void SetValue<T>(string propertyName, T value)
		{
			if (_jsonModel == null)
			{
				SetJsonModel();
			}
			_jsonModel[propertyName] = JToken.FromObject(value);
			if (!_initializing)
			{
				SaveSettings();
				OnPropertyChanged(propertyName);
			}
		}

		private void SetJsonModel()
		{
			Type type = GetType();
			JsonObjectAttribute attr = type.GetCustomAttribute<JsonObjectAttribute>();
			if (!string.IsNullOrEmpty(attr?.Id))
			{
				string filename = attr.Id;
				if (!filename.EndsWith(".json"))
				{
					filename += ".json";
				}
				this._filePath = $@"{StoragePath}/{filename}";
				if (File.Exists(this._filePath))
				{
					string fileText = File.ReadAllText(this._filePath);
					_jsonModel = JObject.Parse(fileText);
				}
			}
			if (_jsonModel == null)
			{
				_jsonModel = new JObject();
				SaveSettings();
			}
		}

		private void SaveSettings()
		{
			FileInfo file = new FileInfo(_filePath);
			file.Directory.Create();
			File.WriteAllText(_filePath, Serializer.Serialize(_jsonModel));
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		protected abstract T GetDefaultValue<T>(string propertyName);
	}
}
