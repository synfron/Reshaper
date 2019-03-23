using System.ComponentModel.Composition;
using System.Windows.Controls;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.Rules.Thens;

namespace ReshaperUI.Display.Xaml.Controls.Rules
{
	/// <summary>
	/// Interaction logic for ThenBroadcastControl.xaml
	/// </summary>
	[Export(typeof(IModelPresenter<ThenBroadcastViewModel>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public partial class ThenBroadcastControl : UserControl, IModelPresenter<ThenBroadcastViewModel>
	{
		public ThenBroadcastControl()
		{
			InitializeComponent();
		}

		public void SetModel(object model)
		{
			DataContext = model;
		}
	}
}
