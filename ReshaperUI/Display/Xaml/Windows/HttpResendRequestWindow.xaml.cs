using System.Windows;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels;
using ReshaperUI.Display.ViewModels.EventViews;

namespace ReshaperUI.Display.Xaml.Windows
{
	/// <summary>
	/// Interaction logic for HttpResendRequestWindow.xaml
	/// </summary>
	public partial class HttpResendRequestWindow : Window, IModelIndependentPresenter<HttpResendRequestViewModel>
	{
		public HttpResendRequestWindow()
		{
			InitializeComponent();
		}

		public void SetModel(object model)
		{
			DataContext = model;
			((HttpResendRequestViewModel)model).CloseRequested += OnCloseRequested;
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
