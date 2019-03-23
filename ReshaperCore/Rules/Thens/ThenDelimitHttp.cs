using System;
using System.Collections.Generic;
using ReshaperCore.Messages;
using ReshaperCore.Messages.Parsers;
using ReshaperCore.Networking;
using ReshaperCore.Utils.Extensions;
using ReshaperCore.Vars;

namespace ReshaperCore.Rules.Thens
{
	public class ThenDelimitHttp : Then
	{
		public override ThenResponse Perform(EventInfo eventInfo)
		{
			if (eventInfo.Type == EventType.Message)
			{
				ThenResponse thenResult = ThenResponse.BreakRules;
				if (!eventInfo.Message.CheckedEntity(HttpMessage.EntityFlag))
				{
					IVariable<byte[]> carryOverVar = eventInfo.Variables.GetOrDefault<byte[]>($"ThenDelimitHttp_CarryOverBytes") ?? eventInfo.Variables.Add<byte[]>($"ThenDelimitHttp_CarryOverBytes");
					byte[] carryOverBytes = carryOverVar.Value ?? new byte[0];
					carryOverVar.Value = new byte[0];

					byte[] fullBytes = carryOverBytes.Combine(eventInfo.Message.RawBytes);

					List<EventInfo> childEvents = new List<EventInfo>();

					Tuple<int, HttpMessage> messageInfo = null;
					IVariable<int> syncId = eventInfo.Variables.GetOrDefault<int>("ThenDelimitHttp_SyncId") ?? eventInfo.Variables.Add<int>("ThenDelimitHttp_SyncId");

					do
					{
						HttpMessageParser parser = new HttpMessageParser()
						{
							TextEncoding = eventInfo.ProxyConnection.ProxyInfo.DefaultEncoding
						};
						messageInfo = parser.Parse(fullBytes);
						if (messageInfo.Item2 != null)
						{
							carryOverBytes = new byte[0];
							messageInfo.Item2.SyncId = syncId.Value++;
							childEvents.Add(eventInfo.Clone(message: messageInfo.Item2));
							if (messageInfo.Item1 <= fullBytes.Length)
							{
								fullBytes = new Buffer<byte>(fullBytes, messageInfo.Item1, fullBytes.Length - messageInfo.Item1).GetBytes();
							}
							else
							{
								fullBytes = new byte[0];
							}
						}
						else
						{
							carryOverBytes = fullBytes;
						}
					}
					while (fullBytes.Length > 0 && messageInfo.Item2 != null);

					carryOverVar.Value = carryOverBytes;

					eventInfo.Engine.Queue.AddFirst(childEvents);

					eventInfo.Message.SetEntityFlag(HttpMessage.EntityFlag, false);
				}
				else if (eventInfo.Message is HttpMessage)
				{
					thenResult = ThenResponse.Continue;
				}
				return thenResult;
			}
			else
			{
				return ThenResponse.Continue;
			}
		}
	}
}
