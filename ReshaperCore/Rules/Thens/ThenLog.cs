using ReshaperCore.Utils;
using ReshaperCore.Vars;

namespace ReshaperCore.Rules.Thens
{
	public class ThenLog : Then
	{

		public VariableString Text
		{
			get;
			set;
		}

		public override ThenResponse Perform(EventInfo eventInfo)
		{
			Log.LogInfo(Text.GetText(eventInfo.Variables));
			return ThenResponse.Continue;
		}
	}
}
