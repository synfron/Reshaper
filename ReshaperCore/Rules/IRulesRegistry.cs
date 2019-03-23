using System.Collections.Generic;

namespace ReshaperCore.Rules
{

	public delegate void RulesListChangedHandler();

	public interface IRulesRegistry
	{
		event RulesListChangedHandler RulesListChanged;

		void AddRule(Rule rule);
		bool CanDelete(Rule rule);
		bool CanMoveNext(Rule rule);
		bool CanMovePrevious(Rule rule);
		void DeleteRule(Rule rule);
		IReadOnlyList<Rule> GetRules();
		void Init();
		void MoveNext(Rule rule);
		void MovePrevious(Rule rule);
		void RestoreDefaultRules();
		void SaveRules();
	}
}