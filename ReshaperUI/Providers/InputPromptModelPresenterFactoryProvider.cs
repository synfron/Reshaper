using ReshaperUI.Factories;
using ReshaperCore.Providers;

namespace ReshaperUI.Providers
{
	public class InputPromptModelPresenterFactoryProvider : SingletonProvider<IInputPromptModelPresenterFactory>
	{
		protected override IInputPromptModelPresenterFactory CreateInstance()
		{
			return new InputPromptModelPresenterFactory();
		}
	}
}
