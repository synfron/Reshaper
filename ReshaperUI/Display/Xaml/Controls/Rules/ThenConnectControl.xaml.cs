﻿using System.ComponentModel.Composition;
using System.Windows.Controls;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.Rules.Thens;

namespace ReshaperUI.Display.Xaml.Controls.Rules
{
	/// <summary>
	/// Interaction logic for ThenConnectControl.xaml
	/// </summary>
	[Export(typeof(IModelPresenter<ThenConnectViewModel>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public partial class ThenConnectControl : UserControl, IModelPresenter<ThenConnectViewModel>
	{
		public ThenConnectControl()
		{
			InitializeComponent();
		}

		public void SetModel(object model)
		{
			DataContext = model;
		}
	}
}
