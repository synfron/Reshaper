using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.EventViews;
using ReshaperUI.Display.Xaml.Windows;

namespace ReshaperUI.Factories
{
	public class InputPromptModelPresenterFactory : IInputPromptModelPresenterFactory
	{
		public IModelIndependentPresenter<InputPromptViewModel> GetPresenter()
		{
			return new InputPromptWindow();
		}
	}
}
