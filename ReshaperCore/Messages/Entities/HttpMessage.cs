using ReshaperCore.Messages.Entities.Http;
using ReshaperCore.Utils.Extensions;

namespace ReshaperCore.Messages
{
	public class HttpMessage : Message
	{

		private static long _entityFlag;
		private HttpMessageType _type;
		private HttpStatusLine _statusLine;
		private HttpHeaders _headers;
		private HttpBody _body;
		private int _syncId;
		private string _newLine;

		public virtual HttpMessageType Type
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

		public virtual HttpStatusLine StatusLine
		{
			set
			{
				RegisterOnEntityChanges(nameof(StatusLine), value, _statusLine);
				_statusLine = value;
				OnPropertyChanged(nameof(StatusLine));
				OnPropertyChanged(nameof(RawText));
			}
			get
			{
				return _statusLine;
			}
		}

		public virtual HttpHeaders Headers
		{
			set
			{
				RegisterOnEntityChanges(nameof(Headers), value, _headers);
				_headers = value;
				OnPropertyChanged(nameof(Headers));
				OnPropertyChanged(nameof(RawText));
			}
			get
			{
				return _headers;
			}
		}

		public virtual HttpBody Body
		{
			set
			{
				RegisterOnEntityChanges(nameof(StatusLine), value, _body);
				_body = value;
				OnPropertyChanged(nameof(Body));
				OnPropertyChanged(nameof(RawText));
			}
			get
			{
				return _body;
			}
		}

		public virtual int SyncId
		{
			set
			{
				_syncId = value;
				OnPropertyChanged(nameof(SyncId));
			}
			get
			{
				return _syncId;
			}
		}

		public override string RawText
		{
			get
			{
				return $"{StatusLine}{NewLine}{Headers}{NewLine}{NewLine}{Body?.ToString() ?? string.Empty}";
			}

			set
			{
			}
		}

		public override byte[] RawBytes
		{
			get
			{
				byte[] bytes = TextEncoding.GetBytes($"{StatusLine}{NewLine}{Headers}{NewLine}{NewLine}");
				if (Body != null)
				{
					bytes = bytes.Combine(Body.RawBytes);
				}
				return bytes;
			}
		}

		public override string Protocol
		{
			get
			{
				return "Http";
			}
		}

		public new static long EntityFlag
		{
			get
			{
				return _entityFlag;
			}
		}

		public virtual string NewLine
		{
			set
			{
				_newLine = value;
				OnPropertyChanged(nameof(NewLine));
				OnPropertyChanged(nameof(RawText));
			}
			get
			{
				return _newLine;
			}
		}

		static HttpMessage()
		{
			_entityFlag = RegisterFlag();
		}

		public override long GetEntityFlag()
		{
			return _entityFlag;
		}

		public override string ToString()
		{
			return RawText;
		}

		public override bool Equals(object obj)
		{
			bool equals = false;
			if (obj is HttpMessage)
			{
				equals = (obj as HttpMessage)?.ToString() == ToString();
			}
			return equals;
		}

		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}
	}
}
