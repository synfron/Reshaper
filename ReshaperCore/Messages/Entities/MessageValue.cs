using System.ComponentModel;

namespace ReshaperCore.Messages.Entities
{
	/// <summary>
	/// Enumeration of all possible values of a text or HTTP message
	/// </summary>
	public enum MessageValue
	{
		[Description("Source Remote Address")]
		SourceRemoteAddress,
		[Description("Destination Remote Address")]
		DestinationRemoteAddress,
		[Description("Local Address")]
		LocalAddress,
		[Description("Destination Remote Port")]
		DestinationRemotePort,
		[Description("Source Remote Port")]
		SourceRemotePort,
		[Description("Local Port")]
		LocalPort,
		[Description("Protocol")]
		Protocol,
		[Description("Data Direction")]
		DataDirection,
		[Description("HTTP Request URI")]
		HttpRequestUri,
		[Description("HTTP Method")]
		HttpMethod,
		[Description("HTTP Header")]
		HttpHeader,
		[Description("HTTP Headers")]
		HttpHeaders,
		[Description("HTTP Body")]
		HttpBody,
		[Description("Message")]
		Message,
		[Description("HTTP Status Line")]
		HttpStatusLine,
		[Description("HTTP Status Message")]
		HttpStatusMessage,
		[Description("HTTP Status Code")]
		HttpStatusCode,
		[Description("HTTP Version")]
		HttpVersion,
	}
}
