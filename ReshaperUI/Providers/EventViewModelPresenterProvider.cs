using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.EventViews;
using ReshaperUI.Display.ViewModels.Rules;
using ReshaperUI.Display.ViewModels.Settings;
using ReshaperUI.Display.Xaml.Controls.EventView;
using ReshaperCore.Providers;

namespace ReshaperUI.Providers
{
	public class EventViewModelPresenterProvider<T> : SingletonProvider<T> where T : IModelPresenter
	{
		protected override T CreateInstance()
		{
			IModelPresenter presenter = null;
			if (typeof(T) == typeof(IModelPresenter<TextEventListViewModel>))
			{
				presenter = new TextEventViewControl();
			}
			else if (typeof(T) == typeof(IModelPresenter<HttpEventListViewModel>))
			{
				presenter = new HttpEventViewControl();
			}
			else if (typeof(T) == typeof(IModelPresenter<TextRulesViewModel>))
			{
				presenter = new TextRulesEventViewControl();
			}
			else if (typeof(T) == typeof(IModelPresenter<HttpRulesViewModel>))
			{
				presenter = new HttpRulesEventViewControl();
			}
			else if (typeof(T) == typeof(IModelPresenter<LogEventsViewModel>))
			{
				presenter = new LogEventViewControl();
			}
			else if (typeof(T) == typeof(IModelPresenter<SettingsListViewModel>))
			{
				presenter = new SettingsEventViewControl();
			}
			return (T)presenter;
		}
	}
}
