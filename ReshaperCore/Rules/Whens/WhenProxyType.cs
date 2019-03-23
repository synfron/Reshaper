using ReshaperCore.Proxies;

namespace ReshaperCore.Rules.Whens
{
	public class WhenProxyType : When
	{


		public ProxyDataType ProxyType
		{
			get;
			set;
		}

		public override bool IsMatch(EventInfo eventInfo)
		{
			return ProxyType == eventInfo.ProxyConnection.ProxyInfo.DataType;
		}
	}
}
