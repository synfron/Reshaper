namespace ReshaperCore.Rules.Thens
{
	public class ThenSkipProcessing : Then
	{

		public override ThenResponse Perform(EventInfo eventInfo)
		{
			return ThenResponse.BreakRules;
		}
	}
}
