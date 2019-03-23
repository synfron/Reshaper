namespace ReshaperCore.Rules.Thens
{
	public class ThenDisconnect : Then
	{
		public override ThenResponse Perform(EventInfo eventInfo)
		{
			eventInfo.ProxyConnection.DisconnectChannel(eventInfo.Direction);
			return ThenResponse.Continue;
		}
	}
}
