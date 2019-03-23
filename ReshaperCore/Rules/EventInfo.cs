using ReshaperCore.Messages;
using ReshaperCore.Proxies;
using ReshaperCore.Utils;
using ReshaperCore.Vars;

namespace ReshaperCore.Rules
{
	public class EventInfo : ObservableEntity
	{
		private EventType _type;
		private DataDirection _direction;
		private Message _message;
		private ProxyConnection _proxyConnection;
		private Variables _variables;
		private RulesEngine _engine;

		public virtual EventType Type
		{
			set
			{
				_type = value;
				OnPropertyChanged(nameof(Type));
			}
			get
			{
				return _type;
			}
		}

		public virtual DataDirection Direction
		{
			set
			{
				_direction = value;
				OnPropertyChanged(nameof(Direction));
			}
			get
			{
				return _direction;
			}
		}

		public virtual Message Message
		{
			set
			{
				RegisterOnEntityChanges(nameof(Message), value, _message);
				_message = value;
				OnPropertyChanged(nameof(Message));
			}
			get
			{
				return _message;
			}
		}

		public virtual ProxyConnection ProxyConnection
		{
			set
			{
				_proxyConnection = value;
				OnPropertyChanged(nameof(ProxyConnection));
			}
			get
			{
				return _proxyConnection;
			}
		}

		public virtual Variables Variables
		{
			set
			{
				_variables = value;
				OnPropertyChanged(nameof(Variables));
			}
			get
			{
				return _variables;
			}
		}
		public virtual RulesEngine Engine
		{
			set
			{
				_engine = value;
				OnPropertyChanged(nameof(Engine));
			}
			get
			{
				return _engine;
			}
		}

		public EventInfo()
		{

		}

		public virtual EventInfo Clone(RulesEngine engine = null, EventType? type = null, DataDirection? direction = null, Message message = null, ProxyConnection proxyConnection = null, Variables variables = null)
		{
			return new EventInfo()
			{
				Engine = engine ?? Engine,
				Type = type ?? Type,
				Direction = direction ?? Direction,
				Message = message ?? Message,
				ProxyConnection = proxyConnection ?? ProxyConnection,
				Variables = variables ?? Variables
			};
		}
	}
}
