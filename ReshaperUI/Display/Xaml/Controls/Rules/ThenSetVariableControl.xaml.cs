using System.ComponentModel.Composition;
using System.Windows.Controls;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.Rules.Thens;

namespace ReshaperUI.Display.Xaml.Controls.Rules
{
	/// <summary>
	/// Interaction logic for ThenSetVariableControl.xaml
	/// </summary>
	[Export(typeof(IModelPresenter<ThenSetVariableViewModel>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public partial class ThenSetVariableControl : UserControl, IModelPresenter<ThenSetVariableViewModel>
	{
		public ThenSetVariableControl()
		{
			InitializeComponent();
		}

		public void SetModel(object model)
		{
			DataContext = model;
		}
	}
}
