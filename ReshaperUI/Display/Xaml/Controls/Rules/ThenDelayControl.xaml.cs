using System.ComponentModel.Composition;
using System.Windows.Controls;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.Rules.Thens;

namespace ReshaperUI.Display.Xaml.Controls.Rules
{
	/// <summary>
	/// Interaction logic for ThenDelayControl.xaml
	/// </summary>
	[Export(typeof(IModelPresenter<ThenDelayViewModel>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public partial class ThenDelayControl : UserControl, IModelPresenter<ThenDelayViewModel>
	{
		public ThenDelayControl()
		{
			InitializeComponent();
		}

		public void SetModel(object model)
		{
			DataContext = model;
		}
	}
}
