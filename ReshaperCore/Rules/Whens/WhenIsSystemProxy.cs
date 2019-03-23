namespace ReshaperCore.Rules.Whens
{
	public class WhenIsSystemProxy : When
	{
		public override bool IsMatch(EventInfo eventInfo)
		{
			return eventInfo.ProxyConnection.Host.IsRegisted();
		}
	}
}
