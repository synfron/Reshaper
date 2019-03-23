using ReshaperCore.Rules;
using ReshaperCore.Settings;
using ReshaperCore.Vars;

namespace ReshaperCore
{
	public class Self : ISelf
	{
		private static readonly string _globalVarPath = $@"{SettingsStore.StoragePath}/GlobalVariables.json";

		public event NewEventBroadcastedHandler NewEventBroadcasted;
		public event PromptCanceledHandler PromptCanceled;
		public event PromptRequestedHandler PromptRequested;

		public virtual Variables Variables
		{
			private set;
			get;
		} = new Variables() { PersistPath = _globalVarPath };

		public virtual void BroadcastEvent(EventInfo eventInfo)
		{
			if (NewEventBroadcasted != null)
			{
				NewEventBroadcasted(eventInfo);
			}
		}

		public virtual void Init()
		{
			Variables.LoadPersistables();
		}

		public virtual void Shutdown()
		{
			Variables.SavePersistables();
		}

		public void PromptForInput(PromptRequestedEventArgs args)
		{
			if (PromptRequested != null)
			{
				PromptRequested(args);
			}
		}

		public void CancelPrompt(object id)
		{
			if (PromptCanceled != null)
			{
				PromptCanceled(id);
			}
		}
	}
}
