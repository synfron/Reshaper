using ReshaperCore.Messages;

namespace ReshaperCore.Rules.Whens
{
	public class WhenEventDirection : When
	{

		public DataDirection Direction
		{
			get;
			set;
		}

		public override bool IsMatch(EventInfo eventInfo)
		{
			return eventInfo.Direction == Direction;
		}
	}
}
