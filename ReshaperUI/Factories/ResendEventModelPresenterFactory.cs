using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.EventViews;
using ReshaperUI.Display.Xaml.Windows;

namespace ReshaperUI.Factories
{
	public class ResendEventModelPresenterFactory : IResendEventModelPresenterFactory
	{
		public T GetPresenter<T>() where T : IModelIndependentPresenter
		{
			IModelIndependentPresenter presenter = null;
			if (typeof(T) == typeof(IModelIndependentPresenter<HttpResendRequestViewModel>))
			{
				presenter = new HttpResendRequestWindow();
			}
			else if (typeof(T) == typeof(IModelIndependentPresenter<TextResendMessageViewModel>))
			{
				presenter = new TextResendMessageWindow();
			}
			return (T)presenter;
		}
	}
}
