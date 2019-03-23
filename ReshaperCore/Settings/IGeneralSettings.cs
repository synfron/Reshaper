using System.ComponentModel;

namespace ReshaperCore.Settings
{
	public interface IGeneralSettings : INotifyPropertyChanged
	{
		bool AutoUpdateContentLength { get; set; }
		bool IgnoreContentLength { get; set; }
	}
}