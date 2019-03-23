using ReshaperCore.Messages;
using ReshaperCore.Messages.Entities;
using ReshaperCore.Messages.Entities.Http;
using ReshaperCore.Utils.Extensions;
using ReshaperCore.Vars;

namespace ReshaperCore.Rules.Whens
{
	public class WhenHasEntity : When
	{

		public MessageValue MessageValue
		{
			get;
			set;
		}

		public VariableString Identifier
		{
			get;
			set;
		}

		public MessageValueType MessageValueType
		{
			get;
			set;
		} = MessageValueType.Text;

		public override bool IsMatch(EventInfo eventInfo)
		{
			bool matches = false;
			switch (MessageValue)
			{
				case MessageValue.DataDirection:
				case MessageValue.LocalAddress:
				case MessageValue.LocalPort:
				case MessageValue.Protocol:
				case MessageValue.SourceRemoteAddress:
				case MessageValue.SourceRemotePort:
					matches = true;
					break;
				case MessageValue.DestinationRemoteAddress:
					matches = eventInfo.ProxyConnection.HasTargetConnection;
					break;
				case MessageValue.DestinationRemotePort:
					matches = eventInfo.ProxyConnection.HasTargetConnection;
					break;
				case MessageValue.HttpBody:
					{
						HttpBody httpBody = (eventInfo.Message as HttpMessage)?.Body;
						if (httpBody != null)
						{
							if (MessageValueType != MessageValueType.Text && Identifier != null)
							{
								switch (MessageValueType)
								{
									case MessageValueType.Json:
										matches = httpBody.Text.GetJsonValue(Identifier.GetText(eventInfo.Variables)) != null;
										break;
									case MessageValueType.Xml:
										matches = httpBody.Text.GetXmlValue(Identifier.GetText(eventInfo.Variables)) != null;
										break;
								}
							}
							else
							{
								matches = true;
							}
						}
						else
						{
							matches = false;
						}
					}
					break;
				case MessageValue.HttpHeaders:
					matches = ((eventInfo.Message as HttpMessage)?.Headers?.Count ?? 0) > 0;
					break;
				case MessageValue.HttpHeader:
					matches = (eventInfo.Message as HttpMessage)?.Headers.Contains(Identifier.GetText(eventInfo.Variables)) ?? false;
					break;
				case MessageValue.HttpMethod:
					matches = !string.IsNullOrEmpty(((eventInfo.Message as HttpMessage)?.StatusLine as HttpRequestStatusLine)?.Method);
					break;
				case MessageValue.HttpRequestUri:
					matches = !string.IsNullOrEmpty(((eventInfo.Message as HttpMessage)?.StatusLine as HttpRequestStatusLine)?.Uri);
					break;
				case MessageValue.HttpVersion:
					HttpStatusLine requestStatusLine = (eventInfo.Message as HttpMessage)?.StatusLine;
					if (requestStatusLine is HttpRequestStatusLine)
					{
						matches = !string.IsNullOrEmpty((requestStatusLine as HttpRequestStatusLine)?.Version);
					}
					else
					{
						matches = !string.IsNullOrEmpty((requestStatusLine as HttpResponseStatusLine)?.Version);
					}
					break;
				case MessageValue.HttpStatusLine:
					matches = (eventInfo.Message as HttpMessage)?.StatusLine != null;
					break;
				case MessageValue.HttpStatusCode:
					matches = ((eventInfo.Message as HttpMessage)?.StatusLine as HttpResponseStatusLine)?.StatusCode != null;
					break;
				case MessageValue.HttpStatusMessage:
					matches = !string.IsNullOrEmpty(((eventInfo.Message as HttpMessage)?.StatusLine as HttpResponseStatusLine)?.StatusMessage);
					break;
				case MessageValue.Message:
					matches = !string.IsNullOrEmpty(eventInfo.Message?.ToString());
					break;
			}

			return matches;
		}
	}
}
