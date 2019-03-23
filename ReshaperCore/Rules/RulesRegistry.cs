using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReshaperCore.Settings;
using ReshaperCore.Utils;

namespace ReshaperCore.Rules
{
	public abstract class RulesRegistry : IRulesRegistry
	{
		private readonly string _rulesFilePath;
		private readonly string _rulesType;

		public virtual event RulesListChangedHandler RulesListChanged;

		private List<Rule> Rules
		{
			get;
			set;
		} = new List<Rule>();

		protected abstract string DefaultRulesJson
		{
			get;
		}

		public RulesRegistry(string rulesType)
		{
			_rulesType = rulesType;
			_rulesFilePath = $@"{SettingsStore.StoragePath}/{rulesType}Rules.json";
		}

		public virtual void Init()
		{
			LoadRules();
		}

		public virtual IReadOnlyList<Rule> GetRules()
		{
			return Rules.ToList();
		}

		public virtual void DeleteRule(Rule rule)
		{
			if (!rule.Locked)
			{
				Rules.Remove(rule);
				OnRulesListChanged();
			}
		}

		public virtual bool CanDelete(Rule rule)
		{
			return rule != null && !rule.Locked;
		}

		public virtual void AddRule(Rule rule)
		{
			int index = Rules.Count;
			if (Rules.Count > 0)
			{
				int lastOpenPlace = Rules.FindIndex(r => r.Placement == RunPosition.End);
				if (lastOpenPlace >= 0)
				{
					index = lastOpenPlace;
				}
			}
			Rules.Insert(index, rule);
			OnRulesListChanged();
		}

		public virtual void SaveRules()
		{
			try
			{
				String serializedRules = Serializer.Serialize(Rules);

				FileInfo file = new FileInfo(_rulesFilePath);
				file.Directory.Create();
				File.WriteAllText(_rulesFilePath, serializedRules);
			}
			catch (Exception e)
			{
				Log.LogError(e, $"Could not save {_rulesType} Rules");
			}
		}

		public virtual void RestoreDefaultRules()
		{
			if (File.Exists(_rulesFilePath))
			{
				File.Delete(_rulesFilePath);
			}
			LoadRules();
		}

		private void LoadRules()
		{
			try
			{
				String serializedRules;
				if (File.Exists(_rulesFilePath))
				{
					serializedRules = File.ReadAllText(_rulesFilePath);
				}
				else
				{
					serializedRules = DefaultRulesJson;
				}
				Rules = Serializer.Deserialize<List<Rule>>(serializedRules);
			}
			catch (Exception e)
			{
				Log.LogError(e, $"Could not load {_rulesType} Rules");
			}
			if (Rules == null)
			{
				Rules = new List<Rule>();
			}
			OnRulesListChanged();
		}

		public virtual void MovePrevious(Rule rule)
		{
			if (rule.Placement == RunPosition.Undefined)
			{
				int currentIndex = Rules.IndexOf(rule);
				if (currentIndex > 0)
				{
					Rule previousRule = Rules[currentIndex];
					if (previousRule.Placement != RunPosition.Beginning)
					{
						Rules.RemoveAt(currentIndex);
						Rules.Insert(--currentIndex, rule);
						OnRulesListChanged();
					}
				}
			}
		}

		public virtual bool CanMovePrevious(Rule rule)
		{
			bool canMove = false;
			if (rule != null && rule.Placement == RunPosition.Undefined)
			{
				int currentIndex = Rules.IndexOf(rule);
				if (currentIndex > 0)
				{
					Rule previousRule = Rules[currentIndex];
					if (previousRule.Placement != RunPosition.Beginning)
					{
						canMove = true;
					}
				}
			}
			return canMove;
		}

		public virtual void MoveNext(Rule rule)
		{
			if (rule.Placement == RunPosition.Undefined)
			{
				int currentIndex = Rules.IndexOf(rule);
				if (currentIndex < Rules.Count - 1)
				{
					Rule nextRule = Rules[currentIndex];
					if (nextRule.Placement != RunPosition.End)
					{
						Rules.RemoveAt(currentIndex);
						Rules.Insert(++currentIndex, rule);
						OnRulesListChanged();
					}
				}
			}
		}

		public virtual bool CanMoveNext(Rule rule)
		{
			bool canMove = false;
			if (rule != null && rule.Placement == RunPosition.Undefined)
			{
				int currentIndex = Rules.IndexOf(rule);
				if (currentIndex >= 0 && currentIndex < Rules.Count - 1)
				{
					Rule nextRule = Rules[currentIndex];
					if (nextRule.Placement != RunPosition.End)
					{
						canMove = true;
					}
				}
			}
			return canMove;
		}

		private void OnRulesListChanged()
		{
			if (RulesListChanged != null)
			{
				RulesListChanged();
			}
		}
	}
}
