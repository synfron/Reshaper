using System.Windows;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels;
using ReshaperUI.Display.ViewModels.EventViews;

namespace ReshaperUI.Display.Xaml.Windows
{
	/// <summary>
	/// Interaction logic for TextResendMessageWindow.xaml
	/// </summary>
	public partial class TextResendMessageWindow : Window, IModelIndependentPresenter<TextResendMessageViewModel>
	{
		public TextResendMessageWindow()
		{
			InitializeComponent();
		}

		public void SetModel(object model)
		{
			DataContext = model;
			((TextResendMessageViewModel)model).CloseRequested += OnCloseRequested;
		}

		private void OnCloseRequested()
		{
			Close();
		}

		void IModelIndependentPresenter.Show()
		{
			Show();
			Activate();
		}
	}
}
