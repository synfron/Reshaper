using System.Windows.Input;
using ReshaperUI.Display.ViewModels.Base;

namespace ReshaperUI.Display.ViewModels.EventViews
{
	public class MenuItemViewModel : ObservableViewModel
	{
		private ICommand _command;
		private string _label;
		private bool _enabled = true;

		public ICommand Command
		{
			get
			{
				return _command;
			}
			set
			{
				_command = value;
				OnPropertyChanged(nameof(Command));
			}
		}

		public string Label
		{
			get
			{
				return _label;
			}
			set
			{
				_label = value;
				OnPropertyChanged(nameof(Label));
			}
		}

		public bool Enabled
		{
			get
			{
				return _enabled;
			}
			set
			{
				_enabled = value;
				OnPropertyChanged(nameof(Enabled));
			}
		}

		public override string ToString()
		{
			return Label;
		}
	}
}
