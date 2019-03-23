namespace ReshaperCore.Rules.Whens
{
	public class WhenEventType : When
	{

		public EventType Type
		{
			get;
			set;
		}

		public override bool IsMatch(EventInfo eventInfo)
		{
			return eventInfo.Type == Type;
		}
	}
}
