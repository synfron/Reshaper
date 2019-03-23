using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ReshaperCore.Messages;
using ReshaperCore.Messages.Entities;
using ReshaperCore.Providers;
using ReshaperCore.Utils.Extensions;
using ReshaperCore.Vars;
using Timer = System.Timers.Timer;

namespace ReshaperCore.Rules.Thens
{
	public abstract class ThenSet : Then
	{
		private MessageValueHandler messageValueRetriever = new MessageValueHandler();

        public ISelf Self { get; set; } = new SelfProvider().GetInstance();

        public bool UseMessageValue
		{
			get;
			set;
		}

		public bool PromptValue
		{
			get;
			set;
		}

		public VariableString PromptDescription
		{
			get;
			set;
		}

		public MessageValue SourceMessageValue
		{
			get;
			set;
		}

		public MessageValueType SourceMessageValueType
		{
			get;
			set;
		} = MessageValueType.Text;

		public VariableString SourceIdentifier
		{
			get;
			set;
		}

		public bool UseReplace
		{
			get;
			set;
		}

		public VariableString RegexPattern
		{
			get;
			set;
		}

		public VariableString Text
		{
			get;
			set;
		}

		public VariableString ReplacementText
		{
			get;
			set;
		}

		public int MaxPromptWaitTime
		{
			get;
			set;
		} = 30;

		public ThenSet()
		{

		}

		private string GetValue(EventInfo eventInfo)
		{
			return messageValueRetriever.GetValue(eventInfo, SourceMessageValue, SourceMessageValueType, SourceIdentifier);
		}

		protected virtual string GetReplacementValue(EventInfo eventInfo)
		{
			string text = string.Empty;
			if (UseMessageValue)
			{
				text = GetValue(eventInfo);
			}
			else
			{
				if (SourceMessageValueType != MessageValueType.Text && SourceIdentifier != null)
				{
					switch (SourceMessageValueType)
					{
						case MessageValueType.Json:
							text = (Text?.GetText(eventInfo.Variables) ?? text).GetJsonValue(SourceIdentifier.GetText(eventInfo.Variables)) ?? string.Empty;
							break;
						case MessageValueType.Xml:
							text = (Text?.GetText(eventInfo.Variables) ?? text).GetXmlValue(SourceIdentifier.GetText(eventInfo.Variables)) ?? string.Empty;
							break;
					}
				}
				else
				{
					text = Text?.GetText(eventInfo.Variables) ?? text;
				}
			}

			if (PromptValue)
			{
				text = GetPromptValue(eventInfo, PromptDescription.GetText(eventInfo.Variables), text);
			}

			if (UseReplace && ReplacementText != null)
			{
				Regex regex = new Regex(RegexPattern.GetText(eventInfo.Variables));
				text = regex.Replace(text, ReplacementText.GetText(eventInfo.Variables));
			}
			return text;
		}

		private string GetPromptValue(EventInfo eventInfo, string description = "", string text = "")
		{
			object id = new object();
			Barrier promptBarrier = new Barrier(2);
			CancellationTokenSource tokenSource = new CancellationTokenSource();
			Timer timer = new Timer()
			{
				AutoReset = false,
				Interval = MaxPromptWaitTime * 1000
			};
			timer.Elapsed += (sender, e) =>
			{
				Self.CancelPrompt(id);
				tokenSource.Cancel();
				promptBarrier.RemoveParticipants(promptBarrier.ParticipantsRemaining);
			};
			timer.Start();
			Task promptTask = new Task(() =>
			{
				Self.PromptForInput(new PromptRequestedEventArgs()
				{
					Description = description,
					Text = text,
					Id = id,
					Callback = (newText) =>
				{
					tokenSource.Token.ThrowIfCancellationRequested();
					text = newText;
					timer.Dispose();
					promptBarrier.RemoveParticipants(promptBarrier.ParticipantsRemaining);
				}
				});
			}, tokenSource.Token);
			promptTask.Start();
			promptBarrier.SignalAndWait();
			return text;
		}
	}
}
