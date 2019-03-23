using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using ReshaperCore.Providers;
using ReshaperCore.Rules;
using ReshaperCore.Rules.Thens;
using ReshaperCore.Rules.Whens;
using ReshaperCore.Utils.Extensions;
using ReshaperUI.Attributes;
using ReshaperUI.Commands;
using ReshaperUI.Display.ViewModels.Base;
using ReshaperUI.Display.ViewModels.Rules.Thens;
using ReshaperUI.Display.ViewModels.Rules.Whens;
using ReshaperUI.Utils;

namespace ReshaperUI.Display.ViewModels.Rules
{
	public abstract class RuleViewModel : SourceModelViewModel
	{
		private readonly ObservableCollection<WhenViewModel> _whens = new ObservableCollection<WhenViewModel>();
		private readonly ObservableCollection<ThenViewModel> _thens = new ObservableCollection<ThenViewModel>();
		private string _ruleName;
		private WhenViewModel _selectedWhen;
		private ThenViewModel _selectedThen;
		private RelayCommand<string> _addWhenCommand;
		private RelayCommand<string> _addThenCommand;
		private string _newWhenName;
		private string _newThenName;
		private bool? _enabled;
		private RelayCommand<WhenViewModel> _deleteWhenCommand;
		private RelayCommand<ThenViewModel> _deleteThenCommand;

		[SourceModel]
		public Rule Rule { get; private set; }

		public abstract ICommand SaveCommand
		{
			get;
		}

		public ICommand DeleteWhenCommand
		{
			get
			{
				if (_deleteWhenCommand == null)
				{
					_deleteWhenCommand = new RelayCommand<WhenViewModel>(when =>
					{
						if (!Rule.Whens.Remove(when.GetModel() as When))
						{
							Whens.Remove(when);
						}
					}, (when) => when != null);
				}
				return _deleteWhenCommand;
			}
		}

		public ICommand DeleteThenCommand
		{
			get
			{
				if (_deleteThenCommand == null)
				{
					_deleteThenCommand = new RelayCommand<ThenViewModel>(then =>
					{
						if (!Rule.Thens.Remove(then.GetModel() as Then))
						{
							Thens.Remove(then);
						}
					}, (then) => then != null);
				}
				return _deleteThenCommand;
			}
		}

		public ICommand AddWhenCommand
		{
			get
			{
				if (_addWhenCommand == null)
				{
					_addWhenCommand = new RelayCommand<string>((name) =>
					{
						if (name != null)
						{
							WhenViewModel newWhen = (WhenViewModel)Activator.CreateInstance(Container.GetDistinctExportsTypes(typeof(WhenViewModel)).Where(type => type.GetCustomAttribute<DescriptionAttribute>()?.Description == name).First());

							newWhen.GetType().GetMethod("SetModels").Invoke(newWhen, new object[] { this, null });

							if (newWhen.IsNew)
							{
								_whens.Add(newWhen);
							}
							SelectedWhen = newWhen;
						}
					});
				}
				return _addWhenCommand;
			}
		}

		public ICommand AddThenCommand
		{
			get
			{
				if (_addThenCommand == null)
				{
					_addThenCommand = new RelayCommand<string>((name) =>
					{
						if (name != null)
						{
							ThenViewModel newThen = (ThenViewModel)Activator.CreateInstance(Container.GetDistinctExportsTypes(typeof(ThenViewModel)).Where(type => type.GetCustomAttribute<DescriptionAttribute>()?.Description == name).First());

							newThen.GetType().GetMethod("SetModels").Invoke(newThen, new object[] { this, null });

							if (newThen.IsNew)
							{
								_thens.Add(newThen);
							}
							SelectedThen = newThen;
						}
					});
				}
				return _addThenCommand;
			}
		}

		public abstract List<string> WhenNames
		{
			get;
		}

		public abstract List<string> ThenNames
		{
			get;
		}

		[SourceModelProperty("Name")]
		[Required(ErrorMessage = "'Rule Name' is required.")]
		public string RuleName
		{
			get
			{
				return Flyweight.Get<string>(_ruleName, Rule.Name);
			}
			set
			{
				this._ruleName = value;
				this.OnPropertyChanged(nameof(RuleName));
				this.OnPropertyChanged(nameof(DisplayName));
			}
		}

		[SourceModelProperty("Enabled")]
		[Required(ErrorMessage = "'Enabled' is required.")]
		public bool? Enabled
		{
			get
			{
				return Flyweight.Get<bool>(_enabled, Rule.Enabled);
			}
			set
			{
				this._enabled = value;
				this.OnPropertyChanged(nameof(Enabled));
			}
		}

		protected override string PartialDisplayName
		{
			get
			{
				return (IsNew) ? "New..." : RuleName;
			}
		}

		public string NewWhenName
		{
			get
			{
				return _newWhenName;
			}
			set
			{
				_newWhenName = value;
				OnPropertyChanged(nameof(NewWhenName));
			}
		}

		public string NewThenName
		{
			get
			{
				return _newThenName;
			}
			set
			{
				_newThenName = value;
				OnPropertyChanged(nameof(NewThenName));
			}
		}

		public WhenViewModel SelectedWhen
		{
			get
			{
				return _selectedWhen;
			}
			set
			{
				_selectedWhen = value;
				OnPropertyChanged(nameof(SelectedWhen));
			}
		}

		public ObservableCollection<WhenViewModel> Whens
		{
			get
			{
				return _whens;
			}
		}

		public ThenViewModel SelectedThen
		{
			get
			{
				return _selectedThen;
			}
			set
			{
				_selectedThen = value;
				OnPropertyChanged(nameof(SelectedThen));
			}
		}

		public ObservableCollection<ThenViewModel> Thens
		{
			get
			{
				return _thens;
			}
        }

        public CompositionContainer Container { get; set; } = new CompositionContainerProvider().GetInstance();

        public RuleViewModel(Rule rule = null)
		{
			this.Rule = rule ?? new Rule();
			IsNew = rule == null;
			Rule.Whens.CollectionChanged += OnWhensCollectionChanged;
			Rule.Thens.CollectionChanged += OnThensCollectionChanged;
		}

        public RuleViewModel Init()
        {
            UpdateWhensList();
            UpdateThensList();
            return this;
        }

		private void OnWhensCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			UpdateWhensList();
		}

		private void OnThensCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			UpdateThensList();
		}

		private void UpdateWhensList()
		{
			Whens.Clear();
			foreach (When when in Rule.Whens)
			{
				Type whenModelType = typeof(WhenViewModel<>).MakeGenericType(when.GetType());
				WhenViewModel whenViewModel = Container.GetExports(typeof(WhenViewModel<>).MakeGenericType(when.GetType()), null, null).FirstOrDefault()?.Value as WhenViewModel;
				if (whenViewModel != null)
				{
					whenModelType.GetMethod("SetModels").Invoke(whenViewModel, new object[] { this, when });
					Whens.Add(whenViewModel);
				}
			}
		}

		private void UpdateThensList()
		{
			Thens.Clear();
			foreach (Then then in Rule.Thens)
			{
				Type thenModelType = typeof(ThenViewModel<>).MakeGenericType(then.GetType());
				ThenViewModel thenViewModel = Container.GetExports(typeof(ThenViewModel<>).MakeGenericType(then.GetType()), null, null).FirstOrDefault()?.Value as ThenViewModel;
				if (thenViewModel != null)
				{
					thenModelType.GetMethod("SetModels").Invoke(thenViewModel, new object[] { this, then });
					Thens.Add(thenViewModel);
				}
			}
		}

		public override string ToString()
		{
			return DisplayName;
		}
	}
}
