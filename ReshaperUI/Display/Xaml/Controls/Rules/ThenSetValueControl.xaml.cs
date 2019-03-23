using System.ComponentModel.Composition;
using System.Windows.Controls;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.Rules.Thens;

namespace ReshaperUI.Display.Xaml.Controls.Rules
{
	/// <summary>
	/// Interaction logic for ThenSetValueControl.xaml
	/// </summary>
	[Export(typeof(IModelPresenter<ThenSetValueViewModel>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public partial class ThenSetValueControl : UserControl, IModelPresenter<ThenSetValueViewModel>
	{
		public ThenSetValueControl()
		{
			InitializeComponent();
		}

		public void SetModel(object model)
		{
			DataContext = model;
		}
	}
}
