using System.Windows.Controls;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.Rules;

namespace ReshaperUI.Display.Xaml.Controls.EventView
{
	/// <summary>
	/// Interaction logic for RulesEventViewControl.xaml
	/// </summary>
	public partial class TextRulesEventViewControl : UserControl, IModelPresenter<TextRulesViewModel>
	{
		public TextRulesEventViewControl()
		{
			InitializeComponent();
		}

		public void SetModel(object model)
		{
			DataContext = model;
		}
	}
}
