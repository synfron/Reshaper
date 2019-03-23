using System.ComponentModel.Composition;
using System.Windows.Controls;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.Rules.Whens;

namespace ReshaperUI.Display.Xaml.Controls.Rules
{
	/// <summary>
	/// Interaction logic for WhenEventTypeControl.xaml
	/// </summary>
	[Export(typeof(IModelPresenter<WhenEventTypeViewModel>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public partial class WhenEventTypeControl : UserControl, IModelPresenter<WhenEventTypeViewModel>
	{
		public WhenEventTypeControl()
		{
			InitializeComponent();
		}

		public void SetModel(object model)
		{
			DataContext = model;
		}
	}
}
