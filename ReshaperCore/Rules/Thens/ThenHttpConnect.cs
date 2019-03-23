using ReshaperCore.Messages;

namespace ReshaperCore.Rules.Thens
{
	public class ThenHttpConnect : Then
	{

		public bool OverrideCurrentConnection
		{
			get;
			set;
		}

		public override ThenResponse Perform(EventInfo eventInfo)
		{
			ThenResponse thenResponse = ThenResponse.BreakRules;

			if (eventInfo.Type == EventType.Message && (OverrideCurrentConnection || !eventInfo.ProxyConnection.HasConnection(eventInfo.Direction)))
			{
				HttpMessage httpMessage = eventInfo.Message as HttpMessage;
				if (httpMessage != null)
				{
					string[] host = httpMessage.Headers.GetOrDefault("Host")?.Split(':');
					if (host != null)
					{
						string hostname = host[0];
						int port;
						if (host.Length <= 1 || !int.TryParse(host[1], out port))
						{
							port = 80;
						}
						if (eventInfo.ProxyConnection.InitConnection(eventInfo.Direction, hostname, port))
						{
							thenResponse = ThenResponse.Continue;
						}
					}
				}
			}
			else
			{
				thenResponse = ThenResponse.Continue;
			}
			return thenResponse;
		}
	}
}
