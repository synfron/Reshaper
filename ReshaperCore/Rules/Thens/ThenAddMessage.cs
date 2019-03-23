using ReshaperCore.Messages;
using ReshaperCore.Vars;

namespace ReshaperCore.Rules.Thens
{
	public class ThenAddMessage : Then
	{
		public DataDirection Direction
		{
			get;
			set;
		}

		public string Delimiter
		{
			get;
			set;
		} = string.Empty;

		public VariableString MessageText
		{
			get;
			set;
		}

		public bool InsertAtBeginning
		{
			get;
			set;
		}

		public override ThenResponse Perform(EventInfo eventInfo)
		{
			Variables connectionVariables;
			if (Direction == DataDirection.Origin)
			{
				connectionVariables = eventInfo.ProxyConnection.ToOriginConnectionVariables;
			}
			else
			{
				connectionVariables = eventInfo.ProxyConnection.ToTargetConnectionVariables;
			}

			EventInfo newEventInfo = eventInfo.Clone(direction: Direction, type: EventType.Message, message: new Message()
			{
				RawText = MessageText.GetText(eventInfo.Variables) + Delimiter,
				Delimiter = Delimiter,
			}, variables: connectionVariables);

			if (InsertAtBeginning)
			{
				eventInfo.Engine.Queue.AddFirst(newEventInfo);
			}
			else
			{
				eventInfo.Engine.Queue.AddLast(newEventInfo);
			}
			return ThenResponse.Continue;
		}
	}
}
