using ReshaperUI.Settings;
using ReshaperCore.Providers;
using ReshaperCore.Settings;

namespace ReshaperUI.Providers
{
	public class GeneralInterfaceSettingsProvider : SingletonProvider<IGeneralSettings, IGeneralInterfaceSettings>
	{
		protected override IGeneralInterfaceSettings CreateInstance()
		{
			return new GeneralInterfaceSettings();
		}
	}
}
