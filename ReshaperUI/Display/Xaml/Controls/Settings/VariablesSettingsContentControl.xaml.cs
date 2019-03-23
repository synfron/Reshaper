using System.Windows.Controls;

namespace ReshaperUI.Display.Xaml.Controls.Settings
{
	/// <summary>
	/// Interaction logic for VariablesSettingsContentControl.xaml
	/// </summary>
	public partial class VariablesSettingsContentControl : UserControl
	{
		public VariablesSettingsContentControl()
		{
			InitializeComponent();
		}

		public override string ToString()
		{
			return "Global Variables";
		}
	}
}
