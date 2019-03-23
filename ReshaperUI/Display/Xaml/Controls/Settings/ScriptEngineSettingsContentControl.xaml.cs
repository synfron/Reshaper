using System.Windows.Controls;

namespace ReshaperUI.Display.Xaml.Controls.Settings
{
	/// <summary>
	/// Interaction logic for ScriptEngineSettingsContentControl.xaml
	/// </summary>
	public partial class ScriptEngineSettingsContentControl : UserControl
	{
		public ScriptEngineSettingsContentControl()
		{
			InitializeComponent();
		}

		public override string ToString()
		{
			return "Script Engine";
		}
	}
}
