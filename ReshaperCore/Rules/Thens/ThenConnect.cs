using System;
using ReshaperCore.Vars;

namespace ReshaperCore.Rules.Thens
{
	public class ThenConnect : Then
	{
		public VariableString DestinationHost
		{
			get;
			set;
		}

		public VariableString DestinationPort
		{
			get;
			set;
		}

		public bool OverrideCurrentConnection
		{
			get;
			set;
		}

		public override ThenResponse Perform(EventInfo eventInfo)
		{
			ThenResponse thenResponse = ThenResponse.Continue;

			string destinationHost = DestinationHost?.GetText(eventInfo.Variables) ?? eventInfo.ProxyConnection.ProxyInfo.DestinationHost;
			int? destinationPort = DestinationPort?.GetInt(eventInfo.Variables) ?? eventInfo.ProxyConnection.ProxyInfo.DestinationPort;

			if (eventInfo.Direction == Messages.DataDirection.Target)
			{
				if (!String.IsNullOrEmpty(destinationHost) && destinationPort != null && (OverrideCurrentConnection || !eventInfo.ProxyConnection.HasTargetConnection))
				{
					if (!eventInfo.ProxyConnection.InitTargetConnection(destinationHost, destinationPort.Value))
					{
						thenResponse = ThenResponse.BreakRules;
					}
				}
			}
			else
			{
				if (!String.IsNullOrEmpty(destinationHost) && destinationPort != null && (OverrideCurrentConnection || !eventInfo.ProxyConnection.HasOriginConnection))
				{
					if (!eventInfo.ProxyConnection.InitOriginConnection(destinationHost, destinationPort.Value))
					{
						thenResponse = ThenResponse.BreakRules;
					}
				}
			}

			return thenResponse;
		}
	}
}
