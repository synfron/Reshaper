using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using ReshaperCore.Utils;

namespace ReshaperCore.Proxies
{
	public class ProxyInfo : ObservableEntity
	{
		private bool _enabled;
		private int _port;
		private string _name;
		private ProxyDataType _dataType;
		private int? _destinationPort;
		private string _destinationHost;
		private List<string> _delimiters;
		private bool _autoActivate;
		private bool _registerAsSystemProxy;
		private bool _useDelimiter = true;
		private Encoding _defaultEncoding = Encoding.UTF8;

		[JsonConverter(typeof(EncodingJsonConvertercs))]
		public Encoding DefaultEncoding
		{
			get
			{
				return _defaultEncoding;
			}
			set
			{
				if (_defaultEncoding != value)
				{
					_defaultEncoding = value;
					OnPropertyChanged(nameof(DefaultEncoding));
				}
			}
		}

		public virtual int Port
		{
			get
			{
				return _port;
			}
			set
			{
				if (_port != value)
				{
					_port = value;
					OnPropertyChanged(nameof(Port));
				}
			}
		}
		public virtual bool UseDelimiter
		{
			get
			{
				return _useDelimiter;
			}
			set
			{
				if (_useDelimiter != value)
				{
					_useDelimiter = value;
					OnPropertyChanged(nameof(UseDelimiter));
				}
			}
		}

		[JsonIgnore]
		public virtual bool Enabled
		{
			get
			{
				return _enabled;
			}
			set
			{
				if (_enabled != value)
				{
					_enabled = value;
					OnPropertyChanged(nameof(Enabled));
				}
			}
		}

		public virtual string Name
		{
			get
			{
				return _name;
			}
			set
			{
				if (_name != value)
				{
					_name = value;
					OnPropertyChanged(nameof(Name));
				}
			}
		}

		public virtual bool RegisterAsSystemProxy
		{
			get
			{
				return _registerAsSystemProxy;
			}
			set
			{
				if (_registerAsSystemProxy != value)
				{
					_registerAsSystemProxy = value;
					OnPropertyChanged(nameof(RegisterAsSystemProxy));
				}
			}
		}

		public virtual bool AutoActivate
		{
			get
			{
				return _autoActivate;
			}
			set
			{
				if (_autoActivate != value)
				{
					_autoActivate = value;
					OnPropertyChanged(nameof(AutoActivate));
				}
			}
		}

		public virtual List<string> Delimiters
		{
			get
			{
				return _delimiters;
			}
			set
			{
				if (_delimiters != value)
				{
					_delimiters = value;
					OnPropertyChanged(nameof(Delimiters));
				}
			}
		}

		public virtual int? DestinationPort
		{
			get
			{
				return _destinationPort;
			}
			set
			{
				if (_destinationPort != value)
				{
					_destinationPort = value;
					OnPropertyChanged(nameof(DestinationPort));
				}
			}
		}

		public virtual string DestinationHost
		{
			get
			{
				return _destinationHost;
			}
			set
			{
				if (_destinationHost != value)
				{
					_destinationHost = value;
					OnPropertyChanged(nameof(DestinationHost));
				}
			}
		}

		public virtual ProxyDataType DataType
		{
			get
			{
				return _dataType;
			}
			set
			{
				if (_dataType != value)
				{
					_dataType = value;
					OnPropertyChanged(nameof(DataType));
				}
			}
		}

		public ProxyInfo()
		{
			Delimiters = new List<string>();
		}
	}
}
