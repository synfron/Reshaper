using ReshaperCore.Rules;

namespace ReshaperCore.Providers
{
	public class TextRulesRegistryProvider : SingletonProvider<ITextRulesRegistry>
	{
		protected override ITextRulesRegistry CreateInstance()
		{
			return new TextRulesRegistry();
		}
	}
}
