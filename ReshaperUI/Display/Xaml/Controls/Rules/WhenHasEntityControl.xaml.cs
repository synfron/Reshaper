using System.ComponentModel.Composition;
using System.Windows.Controls;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.Rules.Whens;

namespace ReshaperUI.Display.Xaml.Controls.Rules
{
	/// <summary>
	/// Interaction logic for WhenHasEntityControl.xaml
	/// </summary>
	[Export(typeof(IModelPresenter<WhenHasEntityViewModel>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public partial class WhenHasEntityControl : UserControl, IModelPresenter<WhenHasEntityViewModel>
	{
		public WhenHasEntityControl()
		{
			InitializeComponent();
		}

		public void SetModel(object model)
		{
			DataContext = model;
		}
	}
}
