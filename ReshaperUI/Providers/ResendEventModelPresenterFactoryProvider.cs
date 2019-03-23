using ReshaperUI.Factories;
using ReshaperCore.Providers;

namespace ReshaperUI.Providers
{
	public class ResendEventModelPresenterFactoryProvider : SingletonProvider<IResendEventModelPresenterFactory>
	{
		protected override IResendEventModelPresenterFactory CreateInstance()
		{
			return new ResendEventModelPresenterFactory();
		}
	}
}
