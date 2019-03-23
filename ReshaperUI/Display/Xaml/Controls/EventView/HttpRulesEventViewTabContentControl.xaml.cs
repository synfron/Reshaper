using System.Windows.Controls;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.Rules;

namespace ReshaperUI.Display.Xaml.Controls.EventView
{
	/// <summary>
	/// Interaction logic for RulesEventViewControl.xaml
	/// </summary>
	public partial class HttpRulesEventViewControl : UserControl, IModelPresenter<HttpRulesViewModel>
	{
		public HttpRulesEventViewControl()
		{
			InitializeComponent();
		}

		public void SetModel(object model)
		{
			DataContext = model;
		}
	}
}
