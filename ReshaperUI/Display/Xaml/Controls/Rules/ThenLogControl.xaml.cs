using System.ComponentModel.Composition;
using System.Windows.Controls;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.Rules.Thens;

namespace ReshaperUI.Display.Xaml.Controls.Rules
{
	/// <summary>
	/// Interaction logic for ThenLogControl.xaml
	/// </summary>
	[Export(typeof(IModelPresenter<ThenLogViewModel>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public partial class ThenLogControl : UserControl, IModelPresenter<ThenLogViewModel>
	{
		public ThenLogControl()
		{
			InitializeComponent();
		}

		public void SetModel(object model)
		{
			DataContext = model;
		}
	}
}
