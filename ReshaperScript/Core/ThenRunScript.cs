using System;
using System.ComponentModel.Composition.Hosting;
using ReshaperCore.Providers;
using ReshaperCore.Rules;
using ReshaperCore.Rules.Thens;
using ReshaperCore.Utils;

namespace ReshaperScript.Core
{
	public class ThenRunScript : Then
	{
		public string ScriptName
		{
			get;
			set;
		} = string.Empty;

		public string ScriptText
		{
			get;
			set;
		} = string.Empty;

		public bool UseNamedScript
		{
			get;
			set;
		}

        public CompositionContainer Container { get; set; } = new CompositionContainerProvider().GetInstance();

		public override ThenResponse Perform(EventInfo eventInfo)
		{
			ThenResponse response = ThenResponse.Continue;
			try
			{
				IScriptHandler scriptHandler = Container.GetExportedValue<IScriptHandler>();
				if (scriptHandler != null)
				{
					if (UseNamedScript)
					{
						if (!Enum.TryParse(scriptHandler.RunNamedScript(eventInfo, ScriptName), out response))
						{
							response = ThenResponse.Continue;
						}
					}
					else
					{
						if (!Enum.TryParse(scriptHandler.RunScript(eventInfo, ScriptText), out response))
						{
							response = ThenResponse.Continue;
						}
					}
				}
			}
			catch (Exception e)
			{
				Log.LogError(e, "Unknown Script Error: " + e.Message);
			}
			return response;
		}
	}
}
