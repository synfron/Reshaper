using System.ComponentModel.Composition;
using System.Windows.Controls;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.Rules.Whens;

namespace ReshaperUI.Display.Xaml.Controls.Rules
{
	/// <summary>
	/// Interaction logic for WhenEventDirectionControl.xaml
	/// </summary>
	[Export(typeof(IModelPresenter<WhenEventDirectionViewModel>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public partial class WhenEventDirectionControl : UserControl, IModelPresenter<WhenEventDirectionViewModel>
	{
		public WhenEventDirectionControl()
		{
			InitializeComponent();
		}

		public void SetModel(object model)
		{
			DataContext = model;
		}
	}
}
