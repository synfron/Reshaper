using System.ComponentModel.Composition;
using System.Windows.Controls;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.Rules.Thens;

namespace ReshaperUI.Display.Xaml.Controls.Rules
{
	/// <summary>
	/// Interaction logic for ThenSkipProcessingControl.xaml
	/// </summary>
	[Export(typeof(IModelPresenter<ThenSkipProcessingViewModel>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public partial class ThenSkipProcessingControl : UserControl, IModelPresenter<ThenSkipProcessingViewModel>
	{
		public ThenSkipProcessingControl()
		{
			InitializeComponent();
		}

		public void SetModel(object model)
		{
			DataContext = model;
		}
	}
}
