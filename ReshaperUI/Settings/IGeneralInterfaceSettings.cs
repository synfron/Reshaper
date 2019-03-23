using ReshaperCore.Settings;

namespace ReshaperUI.Settings
{
	public interface IGeneralInterfaceSettings : IGeneralSettings
	{
		bool AutoTruncateMessages { get; set; }
		bool LimitEventBufferSize { get; set; }
		int MaxEventBufferSize { get; set; }
		int TruncatedMessageMaxSize { get; set; }
	}
}