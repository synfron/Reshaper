using System.Threading.Tasks;
using System.Windows.Input;
using ReshaperCore.Proxies;
using ReshaperCore.Rules;
using ReshaperUI.Commands;

namespace ReshaperUI.Display.ViewModels.EventViews
{
	public class HttpResendRequestViewModel
	{
		private RelayCommand _saveCommand;
		private EventInfo _eventInfo;

		public virtual event CloseRequestedHandler CloseRequested;
		public delegate void CloseRequestedHandler();

		public ICommand SaveCommand
		{
			get
			{
				if (_saveCommand == null)
				{
					_saveCommand = new RelayCommand(() =>
					{
						SelfConnector connector = new SelfConnector();
						connector.Connect(_eventInfo.ProxyConnection.ProxyInfo).ContinueWith(OnConnectorConnected);
						if (CloseRequested != null)
						{
							CloseRequested();
						}
					});
				}
				return _saveCommand;
			}
		}

		private void OnConnectorConnected(Task<SelfConnector> task)
		{
			if (task.Status == TaskStatus.RanToCompletion)
			{
				task.Result.SendData(_eventInfo.Message.TextEncoding.GetBytes(Text));
			}
		}

		public string Text
		{
			get;
			set;
		}

		public HttpResendRequestViewModel(EventInfo requestEventInfo)
		{
			this._eventInfo = requestEventInfo;
		}
	}
}
