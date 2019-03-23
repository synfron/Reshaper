namespace ReshaperCore.Messages.Entities.Http
{
	/// <summary>
	/// Enumerates the type of HTTP messages. Used to differentiate between a HTTP request and a HTTP response
	/// </summary>
	public enum HttpMessageType
	{
		/// <summary>
		/// HTTP request
		/// </summary>
		Request,
		/// <summary>
		/// HTTP response
		/// </summary>
		Response
	}
}
