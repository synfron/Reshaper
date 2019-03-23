using System.Windows.Controls;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.Settings;

namespace ReshaperUI.Display.Xaml.Controls.EventView
{
	/// <summary>
	/// Interaction logic for SettingsEventViewControl.xaml
	/// </summary>
	public partial class SettingsEventViewControl : UserControl, IModelPresenter<SettingsListViewModel>
	{

		public SettingsEventViewControl()
		{
			InitializeComponent();
		}

		public void SetModel(object model)
		{
			DataContext = model;
		}
	}
}
