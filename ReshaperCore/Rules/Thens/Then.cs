namespace ReshaperCore.Rules.Thens
{
	public abstract class Then : IRuleOperation
	{
		public abstract ThenResponse Perform(EventInfo eventInfo);
	}
}
