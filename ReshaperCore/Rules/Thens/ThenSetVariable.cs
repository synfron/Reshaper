using ReshaperCore.Messages.Entities;
using ReshaperCore.Providers;
using ReshaperCore.Utils.Extensions;
using ReshaperCore.Vars;

namespace ReshaperCore.Rules.Thens
{
	public class ThenSetVariable : ThenSet
	{

        public VariableSource TargetSource
		{
			get;
			set;
		}

		public VariableString VariableName
		{
			get;
			set;
		}

		public VariableString DestinationIdentifier
		{
			get;
			set;
		}

		public MessageValueType DestinationMessageValueType
		{
			get;
			set;
		} = MessageValueType.Text;

		public override ThenResponse Perform(EventInfo eventInfo)
		{
			string replacementText = GetReplacementValue(eventInfo);
			SetValue(eventInfo, replacementText);
			return ThenResponse.Continue;
		}

		private void SetValue(EventInfo eventInfo, string replacementText)
		{
			Variables variables = null;
			switch (TargetSource)
			{
				case VariableSource.Channel:
					variables = eventInfo.Variables;
					break;
				case VariableSource.Global:
					variables = Self.Variables;
					break;
			}
			if (variables != null)
			{
				IVariable<string> variable = variables.GetOrDefault<string>(VariableName.GetText(eventInfo.Variables)) ?? variables.Add<string>(VariableName.GetText(eventInfo.Variables));
				if (DestinationIdentifier != null && DestinationMessageValueType != MessageValueType.Text && variable?.Value != null)
				{
					switch (DestinationMessageValueType)
					{
						case MessageValueType.Json:
							variable.Value = variable.Value.SetJsonValue(DestinationIdentifier.GetText(eventInfo.Variables), replacementText);
							break;
						case MessageValueType.Xml:
							variable.Value = variable.Value.SetXmlValue(DestinationIdentifier.GetText(eventInfo.Variables), replacementText);
							break;
					}
				}
				else
				{
					variable.Value = replacementText;
				}
			}
		}
	}
}
