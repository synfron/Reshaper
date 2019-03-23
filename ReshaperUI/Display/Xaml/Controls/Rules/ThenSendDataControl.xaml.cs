using System.ComponentModel.Composition;
using System.Windows.Controls;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.Rules.Thens;

namespace ReshaperUI.Display.Xaml.Controls.Rules
{
	/// <summary>
	/// Interaction logic for ThenSendDataControl.xaml
	/// </summary>
	[Export(typeof(IModelPresenter<ThenSendDataViewModel>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public partial class ThenSendDataControl : UserControl, IModelPresenter<ThenSendDataViewModel>
	{
		public ThenSendDataControl()
		{
			InitializeComponent();
		}

		public void SetModel(object model)
		{
			DataContext = model;
		}
	}
}
