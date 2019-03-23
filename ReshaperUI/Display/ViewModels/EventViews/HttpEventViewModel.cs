using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using ReshaperCore.Messages;
using ReshaperCore.Messages.Entities.Http;
using ReshaperCore.Proxies;
using ReshaperCore.Rules;
using ReshaperCore.Utils;
using ReshaperUI.Commands;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.Base;
using ReshaperUI.Factories;
using ReshaperUI.Providers;
using ReshaperUI.Settings;

namespace ReshaperUI.Display.ViewModels.EventViews
{
	public class HttpEventViewModel : ObservableViewModel
	{
		private readonly IGeneralInterfaceSettings _generalInterfaceSettings;
		private readonly IResendEventModelPresenterFactory _resendEventModelPresenterFactory;
		private EventInfo _responseEventInfo;
		private EventInfo _requestEventInfo;
		private static int _currentOrder = 0;

		public HttpEventViewModel()
		{
			GeneralInterfaceSettingsProvider generalInterfaceSettingsProvider = new GeneralInterfaceSettingsProvider();
			_generalInterfaceSettings = generalInterfaceSettingsProvider.GetInstance();

			ResendEventModelPresenterFactoryProvider resendEventModelPresenterFactoryProvider = new ResendEventModelPresenterFactoryProvider();
			_resendEventModelPresenterFactory = resendEventModelPresenterFactoryProvider.GetInstance();

			Order = _currentOrder++;
		}

		public EventInfo RequestEventInfo
		{
			get
			{
				return _requestEventInfo;
			}
			set
			{
				_requestEventInfo = value;
				OnPropertyChanged(nameof(RequestEventInfo));
			}
		}

		public EventInfo ResponseEventInfo
		{
			get
			{
				return _responseEventInfo;
			}
			set
			{
				RegisterOnEntityChanges(nameof(ResponseEventInfo), value, _responseEventInfo);
				_responseEventInfo = value;
				OnPropertyChanged(nameof(ResponseEventInfo));
			}
		}

		public long Order
		{
			get;
			set;
		}

		public Tuple<long, int> Id
		{
			get;
			set;
		}

		public string Protocol
		{
			get
			{
				return RequestEventInfo.Message.Protocol;
			}
		}

		public string Method
		{
			get
			{
				return ((RequestEventInfo?.Message as HttpMessage)?.StatusLine as HttpRequestStatusLine)?.Method;
			}
		}

		public string ProxyName
		{
			get
			{
				return RequestEventInfo.ProxyConnection.ProxyInfo.Name;
			}
		}


		public string StatusCode
		{
			get
			{
				return ((ResponseEventInfo?.Message as HttpMessage)?.StatusLine as HttpResponseStatusLine)?.StatusCode.ToString();
			}
		}


		public string Url
		{
			get
			{
				return ((RequestEventInfo?.Message as HttpMessage)?.StatusLine as HttpRequestStatusLine)?.Uri;
			}
		}

		public int LocalPort
		{
			get
			{
				return RequestEventInfo.ProxyConnection.ProxyInfo.Port;
			}
		}

		public string DestinationHost
		{
			get
			{
				return RequestEventInfo.ProxyConnection.TargetChannel?.RemoteEndpoint.Address.ToString();
			}
		}

		public int DestinationPort
		{
			get
			{
				return RequestEventInfo.ProxyConnection.TargetChannel?.RemoteEndpoint?.Port ?? -1;
			}
		}

		public string RequestMessage
		{
			get
			{
				string messageText = RequestEventInfo?.Message.ToString() ?? string.Empty;
				if (_generalInterfaceSettings.AutoTruncateMessages)
				{
					int maxLength = Math.Min(messageText.Length, _generalInterfaceSettings.TruncatedMessageMaxSize);
					messageText.Substring(0, maxLength);
				}
				return messageText;
			}
		}

		public string ResponseMessage
		{
			get
			{
				string messageText = ResponseEventInfo?.Message.ToString() ?? string.Empty;
				if (_generalInterfaceSettings.AutoTruncateMessages)
				{
					int maxLength = Math.Min(messageText.Length, _generalInterfaceSettings.TruncatedMessageMaxSize);
					messageText.Substring(0, maxLength);
				}
				return messageText;
			}
		}

		public IEnumerable<MenuItemViewModel> ContextMenuItems
		{
			get
			{
				IList<MenuItemViewModel> menuItems = new List<MenuItemViewModel>();
				menuItems.Add(new MenuItemViewModel
				{
					Label = "Disconnect",
					Command = new RelayCommand(() =>
					{
						if (_requestEventInfo.ProxyConnection.HasConnection(DataDirection.Origin) || _requestEventInfo.ProxyConnection.HasConnection(DataDirection.Target))
						{
							_requestEventInfo.ProxyConnection.Disconnect();
						}
						if (_responseEventInfo.ProxyConnection.HasConnection(DataDirection.Origin) || _responseEventInfo.ProxyConnection.HasConnection(DataDirection.Target))
						{
							_responseEventInfo.ProxyConnection.Disconnect();
						}
					},
					() =>
					{
						return _requestEventInfo.ProxyConnection.HasConnection(DataDirection.Origin) || _requestEventInfo.ProxyConnection.HasConnection(DataDirection.Target) || _responseEventInfo.ProxyConnection.HasConnection(DataDirection.Origin) || _responseEventInfo.ProxyConnection.HasConnection(DataDirection.Target);
					})
				});
				menuItems.Add(new MenuItemViewModel
				{
					Label = "Resend Request",
					Command = new RelayCommand(() =>
					{
						SelfConnector connector = new SelfConnector();
						connector.Connect(RequestEventInfo.ProxyConnection.ProxyInfo).ContinueWith(OnConnectorConnected);
					},
					() =>
					{
						return RequestEventInfo?.Message.RawText != null;
					})
				});
				menuItems.Add(new MenuItemViewModel
				{
					Label = "Edit & Resend Request",
					Command = new RelayCommand(() =>
					{
						IModelIndependentPresenter<HttpResendRequestViewModel> presenter = _resendEventModelPresenterFactory.GetPresenter<IModelIndependentPresenter<HttpResendRequestViewModel>>();
						presenter.SetModel(new HttpResendRequestViewModel(RequestEventInfo)
						{
							Text = RequestEventInfo?.Message.RawText
						});
						presenter.Show();
					},
					() =>
					{
						return RequestEventInfo?.Message.RawText != null;
					})
				});
				return menuItems;
			}
		}

		private void OnConnectorConnected(Task<SelfConnector> task)
		{
			if (task.Status == TaskStatus.RanToCompletion)
			{
				if (RequestEventInfo != null)
				{
					task.Result.SendData(RequestEventInfo.Message.RawBytes);
				}
			}
		}

		protected override void OnPropertyChanged(string propertyName)
		{
			base.OnPropertyChanged(propertyName);
			switch (propertyName)
			{
				case "RequestEventInfo":
					OnPropertyChanged(nameof(RequestMessage));
					OnPropertyChanged(nameof(Method));
					OnPropertyChanged(nameof(Url));
					break;
				case "ResponseEventInfo":
					OnPropertyChanged(nameof(ResponseMessage));
					OnPropertyChanged(nameof(StatusCode));
					break;
			}
		}

		protected virtual void RegisterOnEntityChanges<T>(string propertyName, T newValue, T oldValue) where T : ObservableEntity
		{
			PropertyChangedEventHandler entityChangedEvent = (sender, e) =>
			{
				OnPropertyChanged(propertyName);
			};
			newValue.PropertyChanged += entityChangedEvent;
			if (oldValue != null)
			{
				oldValue.PropertyChanged -= entityChangedEvent;
			}
		}
	}
}
