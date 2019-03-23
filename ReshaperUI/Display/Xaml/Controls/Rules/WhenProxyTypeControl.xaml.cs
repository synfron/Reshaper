using System;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.Rules.Whens;

namespace ReshaperUI.Display.Xaml.Controls.Rules
{
	/// <summary>
	/// Interaction logic for WhenProxyTypeControl.xaml
	/// </summary>
	[Export(typeof(IModelPresenter<WhenProxyTypeViewModel>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public partial class WhenProxyTypeControl : UserControl, IModelPresenter<WhenProxyTypeViewModel>
	{
		public WhenProxyTypeControl()
		{
			InitializeComponent();
		}

		public void SetModel(object model)
		{
			DataContext = model;
		}

		public void Show()
		{
			throw new NotImplementedException();
		}
	}
}
