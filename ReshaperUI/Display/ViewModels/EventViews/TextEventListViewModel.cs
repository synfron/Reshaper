using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using ReshaperCore.Messages;
using ReshaperCore.Providers;
using ReshaperCore.Rules;
using ReshaperUI.Commands;
using ReshaperUI.Display.ViewModels.Base;
using ReshaperUI.Providers;
using ReshaperUI.Settings;
using ReshaperUI.Utils;

namespace ReshaperUI.Display.ViewModels.EventViews
{
	public class TextEventListViewModel : ObservableViewModel, IEventViewModel
	{
		private readonly ObservableCollection<TextEventViewModel> _events = new ObservableCollection<TextEventViewModel>();
		private TextEventViewModel _selectedEvent;
		private int _trimLinesSize;
		private RelayCommand<ICollection> _deleteCommand;
		private readonly IGeneralInterfaceSettings _generalInterfaceSettings;

		public ICommand DeleteCommand
		{
			get
			{
				if (_deleteCommand == null)
				{
					_deleteCommand = new RelayCommand<ICollection>((deletedItems) =>
					{
						foreach (TextEventViewModel model in deletedItems.OfType<TextEventViewModel>().ToArray())
						{
							_events.Remove(model);
						}
					});
				}
				return _deleteCommand;
			}
		}

		public ObservableCollection<TextEventViewModel> EventList
		{
			get
			{
				return _events;
			}
		}

		public TextEventViewModel SelectedEvent
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
				return "Text";
			}
		}

		public TextEventListViewModel()
		{
			GeneralInterfaceSettingsProvider generalInterfaceSettingsProvider = new GeneralInterfaceSettingsProvider();
			_generalInterfaceSettings = generalInterfaceSettingsProvider.GetInstance();

			SetTrimLineSize();
			_generalInterfaceSettings.PropertyChanged += OnGeneralInterfaceSettingsPropertyChanged;

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
			_trimLinesSize = (int)Math.Min(50, (int)Math.Ceiling((double)_generalInterfaceSettings.MaxEventBufferSize / 8));
		}

		private void NewEventBroadcasted(EventInfo eventInfo)
		{
			ThreadUtils.RunInUiAsync(() =>
			{
				if (!(eventInfo.Message is HttpMessage))
				{
					if (_generalInterfaceSettings.LimitEventBufferSize)
					{
						int maxEventBufferSize = _generalInterfaceSettings.MaxEventBufferSize;
						if (maxEventBufferSize > 0 && maxEventBufferSize < EventList.Count)
						{
							for (int messageIndex = Math.Max(_trimLinesSize, EventList.Count - maxEventBufferSize); messageIndex >= 0 && EventList.Count > 0; messageIndex--)
							{
								EventList.RemoveAt(messageIndex);
							}
						}
					}
					EventList.Add(new TextEventViewModel(eventInfo));
				}
			});
		}
	}
}
