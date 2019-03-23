using System;
using System.Collections.Generic;
using System.Threading;
using ReshaperCore.Providers;
using ReshaperCore.Proxies;
using ReshaperCore.Rules.Thens;
using ReshaperCore.Rules.Whens;
using ReshaperCore.Utils;

namespace ReshaperCore.Rules
{
	public class RulesEngine : Observer
	{
        public virtual MessageQueue<EventInfo> Queue
		{
			private set;
			get;
		}

        public ITextRulesRegistry TextRulesRegistry { get; set; } = new TextRulesRegistryProvider().GetInstance();

        public IHttpRulesRegistry HttpRulesRegistry { get; set; } = new HttpRulesRegistryProvider().GetInstance();

        public RulesEngine()
		{
			Queue = new MessageQueue<EventInfo>();
			Queue.SetObserver(this);
		}

		private bool MatchWhens(IEnumerable<When> whens, EventInfo eventInfo)
		{
			bool isMatch = true;
			bool first = true;
			foreach (When when in whens)
			{
				if (!isMatch && !when.UseOrCondition && !first)
				{
					break;
				}
				if (when.UseOrCondition && !first)
				{
					isMatch |= when.IsMatch(eventInfo) == !when.Negate;
				}
				else
				{
					isMatch &= when.IsMatch(eventInfo) == !when.Negate;
				}
				first = false;
			}
			return isMatch;
		}

		private ThenResponse PerformThens(IEnumerable<Then> thens, EventInfo eventInfo)
		{
			ThenResponse ThenResult = ThenResponse.Continue;
			foreach (Then then in thens)
			{
				ThenResponse result = then.Perform(eventInfo);
				ThenResult |= result;
				if (result.HasFlag(ThenResponse.BreakThens) || result.HasFlag(ThenResponse.BreakRules))
				{
					break;
				}
			}
			return ThenResult;
		}

		private ThenResponse Run(EventInfo eventInfo)
		{
			IReadOnlyList<Rule> rules = null;

			switch (eventInfo.ProxyConnection.ProxyInfo.DataType)
			{
				case ProxyDataType.Http:
					rules = HttpRulesRegistry.GetRules();
					break;
				case ProxyDataType.Text:
					rules = TextRulesRegistry.GetRules();
					break;
				default:
					rules = new List<Rule>();
					break;
			}


			ThenResponse thenResult = ThenResponse.Continue;
			foreach (Rule rule in rules)
			{
				try
				{
					if (rule.Enabled && MatchWhens(rule.Whens, eventInfo))
					{
						thenResult |= PerformThens(rule.Thens, eventInfo);
						if (thenResult.HasFlag(ThenResponse.BreakRules))
						{
							break;
						}
					}
				}
				catch (Exception e)
				{
					Log.LogError(e, "Failure running rule", rule.Name);
				}
			}
			return thenResult;
		}

		public override void OnUpdate()
		{
			if (!Monitor.IsEntered(ObserverKey))
			{
				if (Monitor.TryEnter(ObserverKey) && !ObserverWorking)
				{
					ObserverWorking = true;
					if (Queue != null)
					{
						while (!Queue.IsEmpty())
						{
							Run(Queue.TakeFirst());
						}
					}
					ObserverWorking = false;
					Monitor.Exit(ObserverKey);
				}
			}
		}
	}
}
