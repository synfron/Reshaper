using System.ComponentModel.Composition;
using System.Windows.Controls;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.Rules.Thens;

namespace ReshaperUI.Display.Xaml.Controls.Rules
{
	/// <summary>
	/// Interaction logic for ThenDelimitHttpControl.xaml
	/// </summary>
	[Export(typeof(IModelPresenter<ThenDelimitHttpViewModel>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public partial class ThenDelimitHttpControl : UserControl, IModelPresenter<ThenDelimitHttpViewModel>
	{
		public ThenDelimitHttpControl()
		{
			InitializeComponent();
		}

		public void SetModel(object model)
		{
			DataContext = model;
		}
	}
}
