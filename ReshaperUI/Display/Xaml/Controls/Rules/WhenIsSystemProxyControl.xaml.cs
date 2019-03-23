using System.ComponentModel.Composition;
using System.Windows.Controls;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.Rules.Whens;

namespace ReshaperUI.Display.Xaml.Controls.Rules
{
	/// <summary>
	/// Interaction logic for WhenIsSystemProxyControl.xaml
	/// </summary>
	[Export(typeof(IModelPresenter<WhenIsSystemProxyViewModel>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public partial class WhenIsSystemProxyControl : UserControl, IModelPresenter<WhenIsSystemProxyViewModel>
	{
		public WhenIsSystemProxyControl()
		{
			InitializeComponent();
		}

		public void SetModel(object model)
		{
			DataContext = model;
		}
	}
}
