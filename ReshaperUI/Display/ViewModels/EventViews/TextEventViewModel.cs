using System;
using System.Collections.Generic;
using System.ComponentModel;
using ReshaperCore.Messages;
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
	public class TextEventViewModel : ObservableViewModel
	{
		private static int _currentOrder = 0;
		private EventInfo _eventInfo;
		private readonly IGeneralInterfaceSettings _generalInterfaceSettings;
		private readonly IResendEventModelPresenterFactory _resendEventModelPresenterFactory;

		public TextEventViewModel(EventInfo eventInfo)
		{
			GeneralInterfaceSettingsProvider generalInterfaceSettingsProvider = new GeneralInterfaceSettingsProvider();
			_generalInterfaceSettings = generalInterfaceSettingsProvider.GetInstance();

			ResendEventModelPresenterFactoryProvider resendEventModelPresenterFactoryProvider = new ResendEventModelPresenterFactoryProvider();
			_resendEventModelPresenterFactory = resendEventModelPresenterFactoryProvider.GetInstance();

			EventInfo = eventInfo;
			Order = _currentOrder++;
		}

		public EventInfo EventInfo
		{
			get
			{
				return _eventInfo;
			}
			set
			{
				RegisterOnEntityChanges(nameof(EventInfo), value, _eventInfo);
				_eventInfo = value;
				OnPropertyChanged(nameof(EventInfo));
			}
		}

		public long Order
		{
			get;
			set;
		}

		public string Message
		{
			get
			{
				string messageText = EventInfo.Message.ToString() ?? string.Empty;
				if (_generalInterfaceSettings.AutoTruncateMessages)
				{
					int maxLength = Math.Min(messageText.Length, _generalInterfaceSettings.TruncatedMessageMaxSize);
					messageText.Substring(0, maxLength);
				}
				return messageText;
			}
		}

		public string ProxyName
		{
			get
			{
				return EventInfo.ProxyConnection.ProxyInfo.Name;
			}
		}

		public int LocalPort
		{
			get
			{
				return EventInfo.ProxyConnection.ProxyInfo.Port;
			}
		}

		public string DestinationHost
		{
			get
			{
				return EventInfo.ProxyConnection.TargetChannel?.RemoteEndpoint.Address.ToString();
			}
		}

		public int DestinationPort
		{
			get
			{
				return EventInfo.ProxyConnection.TargetChannel?.RemoteEndpoint?.Port ?? -1;
			}
		}

		public string DirectionSymbol
		{
			get
			{
				return (EventInfo.Direction == DataDirection.Origin) ? "▼" : "▲";
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
						EventInfo.ProxyConnection.Disconnect();
					},
					() =>
					{
						return EventInfo.ProxyConnection.HasConnection(DataDirection.Origin) || EventInfo.ProxyConnection.HasConnection(DataDirection.Target);
					})
				});
				menuItems.Add(new MenuItemViewModel
				{
					Label = "Resend Message",
					Command = new RelayCommand(() =>
					{
						EventInfo.ProxyConnection.AddData(EventInfo.Direction, EventInfo.Message?.RawBytes);
					},
					() =>
					{
						return EventInfo.ProxyConnection.HasConnection(EventInfo.Direction);
					})
				});
				menuItems.Add(new MenuItemViewModel
				{
					Label = "Edit & Resend Message",
					Command = new RelayCommand(() =>
					{
						IModelIndependentPresenter<TextResendMessageViewModel> presenter = _resendEventModelPresenterFactory.GetPresenter<IModelIndependentPresenter<TextResendMessageViewModel>>();
						presenter.SetModel(new TextResendMessageViewModel(EventInfo)
						{
							Text = EventInfo.Message?.ToString() ?? string.Empty
						});
						presenter.Show();
					},
					() =>
					{
						return EventInfo.ProxyConnection.HasConnection(DataDirection.Origin) || EventInfo.ProxyConnection.HasConnection(DataDirection.Target);
					})
				});
				return menuItems;
			}
		}

		protected override void OnPropertyChanged(string propertyName)
		{
			base.OnPropertyChanged(propertyName);
			switch (propertyName)
			{
				case "EventInfo":
					OnPropertyChanged(nameof(Message));
					break;
			}
		}

		protected virtual void RegisterOnEntityChanges<T>(string propertyName, T newValue, T oldValue) where T : ObservableEntity
		{
			PropertyChangedEventHandler entityChangedEvent = (sender, e) =>
			{
				OnPropertyChanged(propertyName);
			};
			if (newValue != null)
			{
				newValue.PropertyChanged += entityChangedEvent;
			}
			if (oldValue != null)
			{
				oldValue.PropertyChanged -= entityChangedEvent;
			}
		}
	}
}
