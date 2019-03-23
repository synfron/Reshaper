using System.Threading;
using ReshaperCore.Vars;

namespace ReshaperCore.Rules.Thens
{
	public class ThenDelay : Then
	{


		public VariableString Delay
		{
			get;
			set;
		}

		public override ThenResponse Perform(EventInfo eventInfo)
		{
			Thread.Sleep(Delay.GetInt(eventInfo.Variables) ?? 0);
			return ThenResponse.Continue;
		}
	}
}
