using System.ComponentModel.Composition;
using System.Windows.Controls;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.Rules.Thens;

namespace ReshaperUI.Display.Xaml.Controls.Rules
{
	/// <summary>
	/// Interaction logic for ThenDisconnectControl.xaml
	/// </summary>
	[Export(typeof(IModelPresenter<ThenDisconnectViewModel>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public partial class ThenDisconnectControl : UserControl, IModelPresenter<ThenDisconnectViewModel>
	{
		public ThenDisconnectControl()
		{
			InitializeComponent();
		}

		public void SetModel(object model)
		{
			DataContext = model;
		}
	}
}
