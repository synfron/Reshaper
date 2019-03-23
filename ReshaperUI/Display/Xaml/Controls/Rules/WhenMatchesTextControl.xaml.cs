using System.ComponentModel.Composition;
using System.Windows.Controls;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.Rules.Whens;

namespace ReshaperUI.Display.Xaml.Controls.Rules
{
	/// <summary>
	/// Interaction logic for WhenMatchesTextControl.xaml
	/// </summary>
	[Export(typeof(IModelPresenter<WhenMatchesTextViewModel>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public partial class WhenMatchesTextControl : UserControl, IModelPresenter<WhenMatchesTextViewModel>
	{
		public WhenMatchesTextControl()
		{
			InitializeComponent();
		}

		public void SetModel(object model)
		{
			DataContext = model;
		}
	}
}
