using System;
using System.Text.RegularExpressions;
using ReshaperCore.Messages;
using ReshaperCore.Messages.Entities;
using ReshaperCore.Utils.Extensions;
using ReshaperCore.Vars;

namespace ReshaperCore.Rules.Whens
{
	public class WhenMatchesText : When
	{
		private MessageValueHandler _messageValueRetriever = new MessageValueHandler();

		public VariableString Identifier
		{
			get;
			set;
		}
		public bool UseMessageValue
		{
			get;
			set;
		}

		public MessageValue MessageValue
		{
			get;
			set;
		}

		public MessageValueType MessageValueType
		{
			get;
			set;
		} = MessageValueType.Text;

		public MatchType MatchType
		{
			get;
			set;
		}

		public VariableString RegexPattern
		{
			get;
			set;
		}

		public VariableString SourceText
		{
			get;
			set;
		}

		public VariableString MatchText
		{
			get;
			set;
		}

		public override bool IsMatch(EventInfo eventInfo)
		{
			bool isMatch = false;
			string sourceText = null;
			if (UseMessageValue)
			{
				sourceText = _messageValueRetriever.GetValue(eventInfo, MessageValue, MessageValueType, Identifier);
			}
			else
			{
				if (MessageValueType != MessageValueType.Text && Identifier != null)
				{
					switch (MessageValueType)
					{
						case MessageValueType.Json:
							sourceText = SourceText.GetText(eventInfo.Variables).GetJsonValue(Identifier.GetText(eventInfo.Variables)) ?? string.Empty;
							break;
						case MessageValueType.Xml:
							sourceText = SourceText.GetText(eventInfo.Variables).GetXmlValue(Identifier.GetText(eventInfo.Variables)) ?? string.Empty;
							break;
					}
				}
				else
				{
					sourceText = SourceText.GetText(eventInfo.Variables);
				}
			}
			string matchText = MatchText.GetText(eventInfo.Variables);

			switch (MatchType)
			{
				case MatchType.BeginsWith:
					isMatch = sourceText.StartsWith(matchText);
					break;
				case MatchType.EndsWith:
					isMatch = sourceText.EndsWith(matchText);
					break;
				case MatchType.Contains:
					isMatch = sourceText.Contains(matchText);
					break;
				case MatchType.Equals:
					isMatch = sourceText == matchText;
					break;
				case MatchType.Regex:
					try
					{
						isMatch = Regex.IsMatch(sourceText, matchText);
					}
					catch (Exception)
					{
					}
					break;
			}
			return isMatch;
		}
	}
}
