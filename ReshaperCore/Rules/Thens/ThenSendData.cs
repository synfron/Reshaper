namespace ReshaperCore.Rules.Thens
{
	public class ThenSendData : Then
	{
		public override ThenResponse Perform(EventInfo eventInfo)
		{
			switch (eventInfo.Type)
			{
				case EventType.Message:
					eventInfo.ProxyConnection.SendData(eventInfo);
					break;
			}
			return ThenResponse.Continue;
		}
	}
}
