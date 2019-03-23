using ReshaperCore.Settings;

namespace ReshaperCore.Providers
{
	public class GeneralSettingsProvider : SingletonProvider<IGeneralSettings>
	{
		protected override IGeneralSettings CreateInstance()
		{
			return new GeneralSettings();
		}
	}
}
