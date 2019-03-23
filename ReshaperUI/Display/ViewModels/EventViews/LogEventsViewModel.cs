using System;
using ReshaperCore.Utils;
using ReshaperUI.Display.ViewModels.Base;

namespace ReshaperUI.Display.ViewModels.EventViews
{
	public class LogEventsViewModel : ObservableViewModel, IEventViewModel
	{
		public string DisplayName
		{
			get
			{
				return "Log";
			}
		}

		public LogEventsViewModel()
		{
			Log.ErrorLogged += this.OnErrorLogged;
			Log.InfoLogged += this.OnInfoLogged;
		}

		public string LogText
		{
			get
			{
				return Log.LogText;
			}
		}

		private void OnErrorLogged(Exception e, string info, string extraInfo)
		{
			OnPropertyChanged(nameof(LogText));
		}

		private void OnInfoLogged(string info, string extraInfo)
		{
			OnPropertyChanged(nameof(LogText));
		}
	}
}
