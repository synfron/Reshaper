using System.Windows.Controls;
using ReshaperUI.Display.ViewModels.Base;
using ReshaperUI.Display.ViewModels.EventViews;

namespace ReshaperUI.Display.ViewModels.Settings
{
	public class SettingsListViewModel : ObservableViewModel, IEventViewModel
	{
		private object _selectedSettings;

		public string DisplayName
		{
			get
			{
				return "Settings";
			}
		}

		public object SelectedSettings
		{
			get
			{
				return _selectedSettings;
			}
			set
			{
				_selectedSettings = value;
				OnPropertyChanged(nameof(SelectedSettings));
			}
		}
	}
}
