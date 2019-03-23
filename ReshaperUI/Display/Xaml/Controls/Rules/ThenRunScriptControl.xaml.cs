using System.ComponentModel.Composition;
using System.Windows.Controls;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.Rules.Thens;

namespace ReshaperUI.Display.Xaml.Controls.Rules
{
	/// <summary>
	/// Interaction logic for ThenRunScriptControl.xaml
	/// </summary>
	[Export(typeof(IModelPresenter<ThenRunScriptViewModel>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public partial class ThenRunScriptControl : UserControl, IModelPresenter<ThenRunScriptViewModel>
	{
		public ThenRunScriptControl()
		{
			InitializeComponent();
		}

		public void SetModel(object model)
		{
			DataContext = model;
		}
	}
}
