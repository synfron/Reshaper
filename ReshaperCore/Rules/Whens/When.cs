namespace ReshaperCore.Rules.Whens
{
	public abstract class When : IRuleOperation
	{

		public bool Negate
		{
			set;
			get;
		}

		public bool UseOrCondition
		{
			set;
			get;
		}

		public abstract bool IsMatch(EventInfo eventInfo);
	}
}
