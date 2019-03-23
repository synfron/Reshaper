using System.Windows.Controls;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.EventViews;

namespace ReshaperUI.Display.Xaml.Controls.EventView
{
	/// <summary>
	/// Interaction logic for LogEventViewControl.xaml
	/// </summary>
	public partial class LogEventViewControl : UserControl, IModelPresenter<LogEventsViewModel>
	{
		public LogEventViewControl()
		{
			InitializeComponent();
		}

		public void SetModel(object model)
		{
			DataContext = model;
		}
	}
}
