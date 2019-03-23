using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.EventViews;

namespace ReshaperUI.Factories
{
	public interface IInputPromptModelPresenterFactory
	{
		IModelIndependentPresenter<InputPromptViewModel> GetPresenter();
	}
}
