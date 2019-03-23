using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Newtonsoft.Json;
using ReshaperCore;
using ReshaperCore.Providers;
using ReshaperUI.Commands;
using ReshaperUI.Display.Interfaces;
using ReshaperUI.Display.ViewModels.Base;
using ReshaperUI.Display.ViewModels.Rules;
using ReshaperUI.Display.ViewModels.Settings;
using ReshaperUI.Factories;
using ReshaperUI.Providers;
using ReshaperUI.Utils;

namespace ReshaperUI.Display.ViewModels.EventViews
{
	[JsonObject("ViewSettings")]
	public class EventViewViewModel : JsonBasedModel<EventViewViewModel>
	{
		private readonly ObservableCollection<IModelPresenter> _itemPresenters = new ObservableCollection<IModelPresenter>();
		private bool _lockPresenters = false;
		private ICommand _closeCommand;

        public ISelf Self { get; set; } = new SelfProvider().GetInstance();

        public ICommand CloseCommand
		{
			get
			{
				if (_closeCommand == null)
				{
					_closeCommand = new RelayCommand<object>(Close);
				}
				return _closeCommand;
			}
		}

		public ObservableCollection<IModelPresenter> ItemPresenters
		{
			get
			{
				return _itemPresenters;
			}
		}

		public bool ShowHttpEvents
		{
			get
			{
				return GetValue<bool>(nameof(ShowHttpEvents));
			}
			set
			{
				if (ShowHttpEvents != value)
				{
					SetValue(nameof(ShowHttpEvents), value);
					OnPropertyChanged(nameof(ShowHttpEvents));
					SetHttpEventPresenter();
				}
			}
		}

		public bool ShowTextEvents
		{
			get
			{
				return GetValue<bool>(nameof(ShowTextEvents));
			}
			set
			{
				if (ShowTextEvents != value)
				{
					SetValue(nameof(ShowTextEvents), value);
					OnPropertyChanged(nameof(ShowTextEvents));
					SetTextEventPresenter();
				}
			}
		}

		public bool ShowTextRules
		{
			get
			{
				return GetValue<bool>(nameof(ShowTextRules));
			}
			set
			{
				if (ShowTextRules != value)
				{
					SetValue(nameof(ShowTextRules), value);
					OnPropertyChanged(nameof(ShowTextRules));
					SetTextRulesPresenter();
				}
			}
		}

		public bool ShowHttpRules
		{
			get
			{
				return GetValue<bool>(nameof(ShowHttpRules));
			}
			set
			{
				if (ShowHttpRules != value)
				{
					SetValue(nameof(ShowHttpRules), value);
					OnPropertyChanged(nameof(ShowHttpRules));
					SetHttpRulesPresenter();
				}
			}
		}

		public bool ShowLog
		{
			get
			{
				return GetValue<bool>(nameof(ShowLog));
			}
			set
			{
				if (ShowLog != value)
				{
					SetValue(nameof(ShowLog), value);
					OnPropertyChanged(nameof(ShowLog));
					SetLogPresenter();
				}
			}
		}

		public bool ShowSettings
		{
			get
			{
				return GetValue<bool>(nameof(ShowSettings));
			}
			set
			{
				if (ShowSettings != value)
				{
					SetValue(nameof(ShowSettings), value);
					OnPropertyChanged(nameof(ShowSettings));
					SetSettingsPresenter();
				}
			}
		}

        public IInputPromptModelPresenterFactory InputPromptModelPresenterFactory { get; set; } = new InputPromptModelPresenterFactoryProvider().GetInstance();

        public EventViewViewModel()
		{
			SetTextEventPresenter();
			SetHttpEventPresenter();
			SetTextRulesPresenter();
			SetHttpRulesPresenter();
			SetLogPresenter();
			SetSettingsPresenter();
		}

		public void SetPresenter<T>(bool value) where T : IEventViewModel
		{
			if (!_lockPresenters)
			{
				if (value)
				{
					EventViewModelPresenterProvider<IModelPresenter<T>> eventViewModelPresenterProvider = new EventViewModelPresenterProvider<IModelPresenter<T>>();
					IModelPresenter presenter = eventViewModelPresenterProvider.GetInstance();
					_itemPresenters.Add(presenter);
				}
				else
				{
					_itemPresenters.Remove(_itemPresenters.FirstOrDefault(presenter => presenter is IModelPresenter<T>));
				}
			}
		}

		public void SetTextEventPresenter()
		{
			SetPresenter<TextEventListViewModel>(ShowTextEvents);
		}

		public void SetHttpEventPresenter()
		{
			SetPresenter<HttpEventListViewModel>(ShowHttpEvents);
		}

		public void SetLogPresenter()
		{
			SetPresenter<LogEventsViewModel>(ShowLog);
		}

		public void SetTextRulesPresenter()
		{
			SetPresenter<TextRulesViewModel>(ShowTextRules);
		}

		public void SetHttpRulesPresenter()
		{
			SetPresenter<HttpRulesViewModel>(ShowHttpRules);
		}

		public void SetSettingsPresenter()
		{
			SetPresenter<SettingsListViewModel>(ShowSettings);
		}

		private void OnPromptRequested(PromptRequestedEventArgs args)
		{
			ThreadUtils.RunInUiAsync(() =>
			{
				IModelIndependentPresenter<InputPromptViewModel> presenter = InputPromptModelPresenterFactory.GetPresenter();
				presenter.SetModel(new InputPromptViewModel(new Action<string>(args.Callback))
				{
					Id = args.Id,
					Description = args.Description,
					Text = args.Text
				});
				presenter.Show();
			});
		}

		private void Close(object obj)
		{
			if (obj is TextEventListViewModel)
			{
				ShowTextEvents = false;
			}
			else if (obj is HttpEventListViewModel)
			{
				ShowHttpEvents = false;
			}
			else if (obj is TextRulesViewModel)
			{
				ShowTextRules = false;
			}
			else if (obj is HttpRulesViewModel)
			{
				ShowHttpRules = false;
			}
			else if (obj is LogEventsViewModel)
			{
				ShowLog = false;
			}
			else if (obj is SettingsListViewModel)
			{
				ShowSettings = false;
			}
		}

		protected override void Init()
        {
            Self.PromptRequested += OnPromptRequested;

            _lockPresenters = true;

			ShowTextEvents = true;
			ShowHttpEvents = false;
			ShowTextRules = true;
			ShowHttpRules = false;
			ShowLog = false;
			ShowSettings = true;

			_lockPresenters = false;
		}
	}
}
