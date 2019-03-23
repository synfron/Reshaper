using ReshaperUI.Display.Interfaces;

namespace ReshaperUI.Factories
{
	public interface IResendEventModelPresenterFactory
	{
		T GetPresenter<T>() where T : IModelIndependentPresenter;
	}
}
