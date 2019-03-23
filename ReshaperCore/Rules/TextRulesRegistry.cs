using System.Text;

namespace ReshaperCore.Rules
{
	public class TextRulesRegistry : RulesRegistry, ITextRulesRegistry
	{
		public TextRulesRegistry() : base("Text")
		{
		}

		protected override string DefaultRulesJson
		{
			get
			{
				return Encoding.UTF8.GetString(Properties.Resources.DefaultTextRules);
			}
		}
	}
}
