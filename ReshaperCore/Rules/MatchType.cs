using System.ComponentModel;

namespace ReshaperCore.Rules
{
	public enum MatchType
	{
		Equals,
		Contains,
		[Description("Begins With")]
		BeginsWith,
		[Description("Ends With")]
		EndsWith,
		Regex
	}
}
