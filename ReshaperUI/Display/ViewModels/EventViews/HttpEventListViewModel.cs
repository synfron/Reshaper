using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using ReshaperCore.Messages;
using ReshaperCore.Messages.Entities.Http;
using ReshaperCore.Providers;
using ReshaperCore.Rules;
using ReshaperUI.Commands;
using ReshaperUI.Display.ViewModels.Base;
using ReshaperUI.Providers;
using ReshaperUI.Settings;
using ReshaperUI.Utils;

namespace ReshaperUI.Display.ViewModels.EventViews
{
	public class HttpEventListViewModel : ObservableViewModel, IEventViewModel
	{
		private readonly ObservableCollection<HttpEventViewModel> _events = new ObservableCollection<HttpEventViewModel>(new HashSet<HttpEventViewModel>());
		private readonly Dictionary<Tuple<long, int>, HttpEventViewModel> _eventsMap = new Dictionary<Tuple<long, int>, HttpEventViewModel>();
		private HttpEventViewModel _selectedEvent;
		private int _trimLinesSize;
		private RelayCommand<ICollection> _deleteCommand;

		public ICommand DeleteCommand
		{
			get
			{
				if (_deleteCommand == null)
				{
					_deleteCommand = new RelayCommand<ICollection>((deletedItems) =>
					{
						foreach (HttpEventViewModel model in deletedItems.OfType<HttpEventViewModel>().ToArray())
						{
							_events.Remove(model);
							_eventsMap.Remove(model.Id);
						}
					});
				}
				return _deleteCommand;
			}
		}

		public ObservableCollection<HttpEventViewModel> EventList
		{
			get
			{
				return _events;
			}
		}

		public HttpEventViewModel SelectedEvent
		{
			get
			{
				return _selectedEvent;
			}

			set
			{
				this._selectedEvent = value;
				OnPropertyChanged(nameof(SelectedEvent));
			}
		}

		public string DisplayName
		{
			get
			{
				return "HTTP";
			}
		}

        public IGeneralInterfaceSettings GeneralInterfaceSettings { get; set; } = new GeneralInterfaceSettingsProvider().GetInstance();

        public HttpEventListViewModel()
		{
			SetTrimLineSize();
			GeneralInterfaceSettings.PropertyChanged += OnGeneralInterfaceSettingsPropertyChanged;

			SelfProvider selfProvider = new SelfProvider();
			selfProvider.GetInstance().NewEventBroadcasted += NewEventBroadcasted;
		}

		private void OnGeneralInterfaceSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case "MaxEventBufferSize":
					SetTrimLineSize();
					break;
			}
		}

		private void SetTrimLineSize()
		{
			_trimLinesSize = (int)Math.Min(50, (int)Math.Ceiling((double)GeneralInterfaceSettings.MaxEventBufferSize / 8));
		}

		private void NewEventBroadcasted(EventInfo eventInfo)
		{
			ThreadUtils.RunInUiAsync(() =>
			{
				if (eventInfo.Message is HttpMessage)
				{
					HttpMessage httpMessage = eventInfo.Message as HttpMessage;
					if (httpMessage != null && httpMessage.Complete)
					{
						HttpEventViewModel model = null;
						if (httpMessage.Type == HttpMessageType.Request)
						{
							Tuple<long, int> id = new Tuple<long, int>(eventInfo.ProxyConnection.ConnectionId, (eventInfo.Message as HttpMessage)?.SyncId ?? 0);
							model = new HttpEventViewModel()
							{
								RequestEventInfo = eventInfo,
								Id = id
							};
							if (GeneralInterfaceSettings.LimitEventBufferSize)
							{
								int maxEventBufferSize = GeneralInterfaceSettings.MaxEventBufferSize;
								if (maxEventBufferSize > 0 && maxEventBufferSize < EventList.Count)
								{
									for (int messageIndex = Math.Max(_trimLinesSize, EventList.Count - maxEventBufferSize); messageIndex >= 0 && EventList.Count > 0; messageIndex--)
									{
										HttpEventViewModel removedModel = EventList[messageIndex];
										EventList.RemoveAt(messageIndex);
										_eventsMap.Remove(removedModel.Id);
									}
								}
							}
							_eventsMap.Add(id, model);
							_events.Add(model);
						}
						else
						{
							if (_eventsMap.TryGetValue(new Tuple<long, int>(eventInfo.ProxyConnection.ConnectionId, (eventInfo.Message as HttpMessage)?.SyncId ?? 0), out model))
							{
								model.ResponseEventInfo = eventInfo;
							}
						}
					}
				}
			});
		}
	}
}
