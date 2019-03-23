using ReshaperCore.Providers;

namespace ReshaperCore.Rules.Thens
{
	public class ThenBroadcast : Then
	{
        public ISelf Self { get; set; } = new SelfProvider().GetInstance();

		public override ThenResponse Perform(EventInfo eventInfo)
		{
			if (eventInfo.Type == EventType.Message)
			{
                Self.BroadcastEvent(eventInfo);
			}
			return ThenResponse.Continue;
		}
	}
}
