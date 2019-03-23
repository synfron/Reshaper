using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using ReshaperCore.Providers;
using ReshaperCore.Proxies;
using ReshaperUI.Attributes;
using ReshaperUI.Commands;
using ReshaperUI.Converters;
using ReshaperUI.Display.ViewModels.Base;
using ReshaperUI.Utils;

namespace ReshaperUI.Display.ViewModels.Settings
{
	public class ProxyViewModel : SourceModelViewModel
	{
		private string _proxyName;
		private int? _port;
		private bool? _autoActivate;
		private ObservableCollection<string> _delimiters;
		private int? _destinationPort;
		private string _destinationHost;
		private ProxyDataType? _dataType;
		private ICommand _saveCommand;
		private bool? _enabled;
		private bool? _useDelimiter;
		private RelayCommand _deleteCommand;
		private bool? _registerAsSystemProxy;
		private readonly IProxyRegistry _proxyRegistry;

		public ICommand SaveCommand
		{
			get
			{
				if (_saveCommand == null)
				{
					_saveCommand = new SourceModelSaveCommand<ProxyInfo>(_proxyRegistry.Add, CanSave);
				}
				return _saveCommand;
			}
		}

		public ICommand DeleteCommand
		{
			get
			{
				if (_deleteCommand == null)
				{
					_deleteCommand = new RelayCommand(() => _proxyRegistry.Remove(ProxyInfo.Name), () => { return !IsNew; });
				}
				return _deleteCommand;
			}
		}

		protected override string PartialDisplayName
		{
			get
			{
				return (IsNew) ? "New..." : ProxyName;
			}
		}

		[SourceModel]
		public ProxyInfo ProxyInfo { get; private set; }

		[SourceModelProperty("Name")]
		[Required(ErrorMessage = "'Proxy Name' is required.")]
		public string ProxyName
		{
			get
			{
				return Flyweight.Get<string>(_proxyName, ProxyInfo.Name);
			}
			set
			{
				this._proxyName = value;
				this.OnPropertyChanged(nameof(ProxyName));
				this.OnPropertyChanged(nameof(DisplayName));
			}
		}

		[SourceModelProperty("Port")]
		[Required(ErrorMessage = "'Port' is required.")]
		[Range(1, 49151, ErrorMessage = "'Port' is out of range.")]
		public int? Port
		{
			get
			{
				return Flyweight.Get<int>(_port, ProxyInfo.Port);
			}
			set
			{
				this._port = value;
				this.OnPropertyChanged(nameof(Port));
			}
		}

		[SourceModelProperty("AutoActivate")]
		[Required(ErrorMessage = "'Auto-Activate' is required.")]
		public bool? AutoActivate
		{
			get
			{
				return Flyweight.Get<bool>(_autoActivate, ProxyInfo.AutoActivate);
			}
			set
			{
				this._autoActivate = value;
				this.OnPropertyChanged(nameof(AutoActivate));
			}
		}

		[SourceModelProperty("RegisterAsSystemProxy")]
		public bool RegisterAsSystemProxy
		{
			get
			{
				return Flyweight.Get<bool>(_registerAsSystemProxy, ProxyInfo.RegisterAsSystemProxy);
			}
			set
			{
				this._registerAsSystemProxy = value;
				this.OnPropertyChanged(nameof(RegisterAsSystemProxy));
			}
		}

		[SourceModelProperty("Enabled")]
		[Required(ErrorMessage = "'Enabled' is required.")]
		public bool? Enabled
		{
			get
			{
				return Flyweight.Get<bool>(_enabled, ProxyInfo.Enabled);
			}
			set
			{
				this._enabled = value;
				this.OnPropertyChanged(nameof(Enabled));
			}
		}

		[SourceModelProperty("UseDelimiter")]
		[Required(ErrorMessage = "'Use Delimiter' is required.")]
		public bool? UseDelimiter
		{
			get
			{
				return Flyweight.Get<bool>(_useDelimiter, ProxyInfo.UseDelimiter);
			}
			set
			{
				this._useDelimiter = value;
				this.OnPropertyChanged(nameof(UseDelimiter));
			}
		}

		[SourceModelProperty("Delimiters", ConverterType = typeof(ObservableCollectionToListConverter))]
		[MinLengthDependent(1, ErrorMessage = "There must be at least one delimiter.", JsonDependentPropertyValuePairs = "{\"DataType\":\"Text\",\"UseDelimiter\":true}")]
		public ObservableCollection<string> Delimiters
		{
			get
			{
				if (_delimiters == null)
				{
					_delimiters = (ObservableCollection<string>)new ListToObservableCollectionConverter().Convert(ProxyInfo.Delimiters, typeof(ObservableCollection<string>));

					_delimiters.CollectionChanged += (sender, e) =>
					{
						this.OnPropertyChanged(nameof(Delimiters));
					};
				}
				return _delimiters;
			}
			set
			{
				if (_delimiters != null && _delimiters != value)
				{
					_delimiters.CollectionChanged += (sender, e) =>
					{
						this.OnPropertyChanged(nameof(Delimiters));
					};
					this._delimiters = value;
					this.OnPropertyChanged(nameof(Delimiters));
				}
			}
		}

		[SourceModelProperty("DestinationPort")]
		[RequiredDependent(ErrorMessage = "'Destination Port' is required.", DependentProperty = "DataType", DependentValue = "Text")]
		[RangeDependent(1, 49151, ErrorMessage = "'Destination Port' is out of range.", DependentProperty = "DataType", DependentValue = "Text")]
		public int? DestinationPort
		{
			get
			{
				return Flyweight.Get<int>(_destinationPort, ProxyInfo.DestinationPort);
			}
			set
			{
				this._destinationPort = value;
				this.OnPropertyChanged(nameof(DestinationPort));
			}
		}

		[SourceModelProperty("DestinationHost")]
		[RequiredDependent(ErrorMessage = "'Destination Host' is required.", DependentProperty = "DataType", DependentValue = "Text")]
		[RegularExpressionDependent(@"\S+", ErrorMessage = "The given value for 'Destination Host' is not valid.", DependentProperty = "DataType", DependentValue = "Text")]
		public string DestinationHost
		{
			get
			{
				return Flyweight.Get<string>(_destinationHost, ProxyInfo.DestinationHost);
			}
			set
			{
				this._destinationHost = value;
				this.OnPropertyChanged(nameof(DestinationHost));
			}
		}

		public IEnumerable<string> DataTypes
		{
			get
			{
				return (Enum.GetValues(typeof(ProxyDataType)) as ProxyDataType[]).Select(dataType => (string)(new EnumToStringConverter().Convert(dataType, typeof(ProxyDataType))));
			}
		}

		[SourceModelProperty("DataType")]
		[Required(ErrorMessage = "'Proxy Type' is required.")]
		public ProxyDataType? DataType
		{
			get
			{
				return Flyweight.Get<ProxyDataType>(_dataType, ProxyInfo.DataType);
			}
			set
			{
				this._dataType = value;
				this.OnPropertyChanged(nameof(DataType));
			}
		}

		public ProxyViewModel(ProxyInfo proxyInfo = null)
		{
			ProxyRegistryProvider proxyRegistryProvider = new ProxyRegistryProvider();
			_proxyRegistry = proxyRegistryProvider.GetInstance();

			this.ProxyInfo = proxyInfo ?? new ProxyInfo();
			IsNew = proxyInfo == null;
		}
	}
}
