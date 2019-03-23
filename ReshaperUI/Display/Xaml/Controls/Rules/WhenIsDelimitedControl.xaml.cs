using System.ComponentModel.Composition;
using System.Windows.Controls;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.Rules.Whens;

namespace ReshaperUI.Display.Xaml.Controls.Rules
{
	/// <summary>
	/// Interaction logic for WhenIsDelimitedControl.xaml
	/// </summary>
	[Export(typeof(IModelPresenter<WhenIsDelimitedViewModel>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public partial class WhenIsDelimitedControl : UserControl, IModelPresenter<WhenIsDelimitedViewModel>
	{
		public WhenIsDelimitedControl()
		{
			InitializeComponent();
		}

		public void SetModel(object model)
		{
			DataContext = model;
		}
	}
}
