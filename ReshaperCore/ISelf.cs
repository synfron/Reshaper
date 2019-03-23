using ReshaperCore.Rules;
using ReshaperCore.Vars;

namespace ReshaperCore
{
	public delegate void NewEventBroadcastedHandler(EventInfo eventInfo);
	public delegate void PromptRequestedHandler(PromptRequestedEventArgs args);
	public delegate void PromptCanceledHandler(object id);
	public delegate void PromptCallback(string text);

	public class PromptRequestedEventArgs
	{
		public string Description { get; set; }
		public object Id { get; set; }
		public string Text { get; set; }
		public PromptCallback Callback { get; set; }
	}

	public interface ISelf
	{
		Variables Variables { get; }

		event NewEventBroadcastedHandler NewEventBroadcasted;
		event PromptCanceledHandler PromptCanceled;
		event PromptRequestedHandler PromptRequested;

		void Init();
		void Shutdown();
		void BroadcastEvent(EventInfo eventInfo);
		void PromptForInput(PromptRequestedEventArgs args);
		void CancelPrompt(object id);
	}
}