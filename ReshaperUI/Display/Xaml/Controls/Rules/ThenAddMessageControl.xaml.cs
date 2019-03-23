using System.ComponentModel.Composition;
using System.Windows.Controls;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.Rules.Thens;

namespace ReshaperUI.Display.Xaml.Controls.Rules
{
	/// <summary>
	/// Interaction logic for ThenAddMessageControl.xaml
	/// </summary>
	[Export(typeof(IModelPresenter<ThenAddMessageViewModel>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public partial class ThenAddMessageControl : UserControl, IModelPresenter<ThenAddMessageViewModel>
	{
		public ThenAddMessageControl()
		{
			InitializeComponent();
		}

		public void SetModel(object model)
		{
			DataContext = model;
		}
	}
}
