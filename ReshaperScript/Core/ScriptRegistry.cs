using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReshaperCore.Settings;
using ReshaperCore.Utils;

namespace ReshaperScript.Core
{
	public class ScriptRegistry : IScriptRegistry
	{
		private static readonly string _scriptsFile = $@"{SettingsStore.StoragePath}/Scripts.json";
		public virtual event ScriptsListChangedHandler ScriptsListChanged;

		public virtual IList<Script> Scripts
		{
			get;
			private set;
		} = new List<Script>();

		public void Load()
		{
			try
			{
				if (File.Exists(_scriptsFile))
				{
					String serializedScripts = File.ReadAllText(_scriptsFile);
					Scripts = Serializer.Deserialize<IList<Script>>(serializedScripts);
				}
				OnScriptsListChanged();
			}
			catch (Exception e)
			{
				Log.LogError(e, "Could not load Scripts");
			}
		}

		private void OnScriptChanged(Script script)
		{
		}

		public void Save()
		{
			try
			{
				String serializedScripts = Serializer.Serialize(Scripts);

				FileInfo file = new FileInfo(_scriptsFile);
				file.Directory.Create();
				File.WriteAllText(_scriptsFile, serializedScripts);
			}
			catch (Exception e)
			{
				Log.LogError(e, "Could not save Scripts");
			}
		}

		public virtual IReadOnlyList<Script> GetScripts()
		{
			return Scripts.ToList();
		}

		public virtual void DeleteScript(Script script)
		{
			Scripts.Remove(script);
			OnScriptsListChanged();
		}

		public virtual void AddScript(Script script)
		{
			Scripts.Add(script);
			OnScriptsListChanged();
		}

		private void OnScriptsListChanged()
		{
			if (ScriptsListChanged != null)
			{
				ScriptsListChanged();
			}
		}
	}
}
