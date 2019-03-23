using ReshaperCore.Messages;
using ReshaperCore.Messages.Entities;
using ReshaperCore.Vars;

namespace ReshaperCore.Rules.Thens
{
	public class ThenSetValue : ThenSet
	{
		private MessageValueHandler messageValueHandler;

		public MessageValue DestinationMessageValue
		{
			get;
			set;
		} = MessageValue.DataDirection;

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

		public ThenSetValue()
		{
			messageValueHandler = new MessageValueHandler();
		}

		public override ThenResponse Perform(EventInfo eventInfo)
		{
			string replacementText = GetReplacementValue(eventInfo);
			SetValue(eventInfo, replacementText);
			return ThenResponse.Continue;
		}

		private void SetValue(EventInfo eventInfo, string replacementText)
		{
			messageValueHandler.SetValue(eventInfo, DestinationMessageValue, DestinationMessageValueType, DestinationIdentifier, replacementText);
		}
	}
}
