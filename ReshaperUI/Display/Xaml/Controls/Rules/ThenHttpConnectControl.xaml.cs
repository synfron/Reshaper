using System.ComponentModel.Composition;
using System.Windows.Controls;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.Rules.Thens;

namespace ReshaperUI.Display.Xaml.Controls.Rules
{
	/// <summary>
	/// Interaction logic for ThenHttpConnectControl.xaml
	/// </summary>
	[Export(typeof(IModelPresenter<ThenHttpConnectViewModel>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public partial class ThenHttpConnectControl : UserControl, IModelPresenter<ThenHttpConnectViewModel>
	{
		public ThenHttpConnectControl()
		{
			InitializeComponent();
		}

		public void SetModel(object model)
		{
			DataContext = model;
		}
	}
}
