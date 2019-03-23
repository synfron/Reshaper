using ReshaperCore.Rules;

namespace ReshaperCore.Providers
{
	public class HttpRulesRegistryProvider : SingletonProvider<IHttpRulesRegistry>
	{
		protected override IHttpRulesRegistry CreateInstance()
		{
			return new HttpRulesRegistry();
		}
	}
}
