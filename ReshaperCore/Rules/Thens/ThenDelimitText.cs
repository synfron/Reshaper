using System;
using System.Collections.Generic;
using ReshaperCore.Messages;
using ReshaperCore.Utils.Extensions;
using ReshaperCore.Vars;

namespace ReshaperCore.Rules.Thens
{
	public class ThenDelimitText : Then
	{

		public override ThenResponse Perform(EventInfo eventInfo)
		{
			ThenResponse thenResult = ThenResponse.Continue;
			if (eventInfo.Type == EventType.Message)
			{
				if (!eventInfo.Message.Complete && eventInfo.Type == EventType.Message)
				{
					IVariable<string> carryOverVar = eventInfo.Variables.GetOrDefault<string>($"ThenDelimitText_CarryOverText_{eventInfo.Direction}") ?? eventInfo.Variables.Add<string>($"ThenDelimitText_CarryOverText_{eventInfo.Direction}");
					string carryOverText = carryOverVar.Value ?? string.Empty;
					List<Tuple<string, string>> sections = null;

					if (eventInfo.ProxyConnection.ProxyInfo.UseDelimiter)
					{
						sections = eventInfo.Message.RawText.SplitWithDelimiters(eventInfo.ProxyConnection.ProxyInfo.Delimiters);
					}
					else
					{
						sections = new List<Tuple<string, string>> { new Tuple<string, string>(eventInfo.Message.RawText, string.Empty) };
					}

					List<EventInfo> childEvents = new List<EventInfo>();

					carryOverVar.Value = String.Empty;
					string foundDelimiter = null;
					if (eventInfo.Message.RawText.StartsWith(eventInfo.ProxyConnection.ProxyInfo.Delimiters, out foundDelimiter))
					{
						childEvents.Add(eventInfo.Clone(message: new Message()
						{
							TextEncoding = eventInfo.Message.TextEncoding,
							RawText = carryOverText + foundDelimiter,
							Delimiter = foundDelimiter,
							Complete = true
						}));
					}
					if (sections.Count > 0)
					{
						for (int sectionIndex = 0; sectionIndex < sections.Count - 1; sectionIndex++)
						{
							childEvents.Add(eventInfo.Clone(message: new Message()
							{
								TextEncoding = eventInfo.Message.TextEncoding,
								RawText = carryOverText + sections[sectionIndex].Item1 + sections[sectionIndex].Item2,
								Delimiter = sections[sectionIndex].Item2,
								Complete = true
							}));
							carryOverText = string.Empty;
						}
						int lastIndex = sections.Count - 1;
						if (sections[lastIndex].Item2 != null)
						{
							childEvents.Add(eventInfo.Clone(message: new Message()
							{
								TextEncoding = eventInfo.Message.TextEncoding,
								RawText = carryOverText + sections[lastIndex].Item1 + sections[lastIndex].Item2,
								Delimiter = sections[lastIndex].Item2,
								Complete = true
							}));
							carryOverText = string.Empty;
						}
						else
						{
							carryOverVar.Value = carryOverText + sections[lastIndex].Item1;
						}
					}
					else
					{
						carryOverVar.Value = carryOverText;
					}
					thenResult = ThenResponse.BreakRules;
					eventInfo.Engine.Queue.AddFirst(childEvents);
				}
			}
			return thenResult;
		}

	}
}
