﻿using System.ComponentModel.Composition;
using System.Windows.Controls;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.Rules.Thens;

namespace ReshaperUI.Display.Xaml.Controls.Rules
{
	/// <summary>
	/// Interaction logic for ThenDelimitTextControl.xaml
	/// </summary>
	[Export(typeof(IModelPresenter<ThenDelimitTextViewModel>)), PartCreationPolicy(CreationPolicy.NonShared)]
	public partial class ThenDelimitTextControl : UserControl, IModelPresenter<ThenDelimitTextViewModel>
	{
		public ThenDelimitTextControl()
		{
			InitializeComponent();
		}

		public void SetModel(object model)
		{
			DataContext = model;
		}
	}
}
