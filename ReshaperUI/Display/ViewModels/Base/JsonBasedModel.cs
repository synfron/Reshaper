using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReshaperCore.Settings;
using ReshaperCore.Utils;

namespace ReshaperUI.Display.ViewModels.Base
{
	public abstract class JsonBasedModel<U> : ObservableViewModel where U : JsonBasedModel<U>
	{
		private string _filePath;
		private JObject _jsonModel = null;
		private bool _initializing = false;

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
				SaveJsonModel();
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
				this._filePath = $@"{SettingsStore.StoragePath}/{filename}";
				if (File.Exists(this._filePath))
				{
					string fileText = File.ReadAllText(this._filePath);
					_jsonModel = JObject.Parse(fileText);
				}
			}
			if (_jsonModel == null)
			{
				_jsonModel = new JObject();
				_initializing = true;
				Init();
				_initializing = false;
				SaveJsonModel();
			}
		}

		private void SaveJsonModel()
		{
			FileInfo file = new FileInfo(_filePath);
			file.Directory.Create();
			File.WriteAllText(_filePath, Serializer.Serialize(_jsonModel));
		}

		protected override void OnPropertyChanged(string propertyName)
		{
			if (!_initializing)
			{
				base.OnPropertyChanged(propertyName);
			}
		}

		protected abstract void Init();
	}
}
