using System.Text;

namespace ReshaperCore.Rules
{
	public class HttpRulesRegistry : RulesRegistry, IHttpRulesRegistry
	{
		public HttpRulesRegistry() : base("HTTP")
		{
		}

		protected override string DefaultRulesJson
		{
			get
			{
				return Encoding.UTF8.GetString(Properties.Resources.DefaultHttpRules);
			}
		}
	}
}
