using System.ComponentModel;

namespace ReshaperCore.Messages.Entities
{
	/// <summary>
	/// Enumeration of the type of data store in a message value
	/// </summary>
	public enum MessageValueType
	{
		[Description("Text")]
		Text,
		[Description("JSON")]
		Json,
		[Description("XML")]
		Xml
	}
}
