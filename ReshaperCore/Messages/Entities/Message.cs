using System.Text;
using ReshaperCore.Utils.Extensions;

namespace ReshaperCore.Messages
{
	public class Message : EntityContainer
	{
		private long _checkedEntityFlags = 0;
		private long _hasEntityFlags = 0;
		private static long _entityFlag;
		private string _delimiter = string.Empty;
		private bool _complete;
		private Encoding _textEncoding = Encoding.UTF8;
		private string _rawText;
		private byte[] _rawBytes;

		public virtual string Delimiter
		{
			set
			{
				_delimiter = value;
				OnPropertyChanged(nameof(Delimiter));
			}
			get
			{
				return _delimiter;
			}
		}

		public virtual bool Complete
		{
			set
			{
				_complete = value;
				OnPropertyChanged(nameof(Complete));
			}
			get
			{
				return _complete;
			}
		}

		public static long EntityFlag
		{
			get
			{
				return _entityFlag;
			}
		}

		public virtual Encoding TextEncoding
		{
			set
			{
				_textEncoding = value;
				OnPropertyChanged(nameof(TextEncoding));
			}
			get
			{
				return _textEncoding;
			}
		}

		public virtual string RawText
		{
			set
			{
				_rawText = value;
				_rawBytes = TextEncoding.GetBytes(_rawText);
				OnPropertyChanged(nameof(RawText));
				OnPropertyChanged(nameof(RawBytes));
				OnPropertyChanged(nameof(Size));
			}
			get
			{
				return _rawText;
			}
		}

		public virtual string Protocol
		{
			get
			{
				return "Raw";
			}
		}

		public virtual byte[] RawBytes
		{
			get
			{
				return _rawBytes;
			}
			set
			{
				_rawBytes = value;
				_rawText = TextEncoding.GetString(_rawBytes);
				OnPropertyChanged(nameof(RawText));
				OnPropertyChanged(nameof(RawBytes));
				OnPropertyChanged(nameof(Size));
			}
		}

		public virtual long Size
		{
			get
			{
				return RawBytes.Length;
			}
		}

		static Message()
		{
			_entityFlag = RegisterFlag();
		}

		public Message()
		{
			SetEntityFlag(GetEntityFlag(), true);
		}

		public bool HasEntity(long entityFlag)
		{
			return (_hasEntityFlags & entityFlag) != 0;
		}

		public bool CheckedEntity(long entityFlag)
		{
			return (_checkedEntityFlags & entityFlag) != 0;
		}

		public virtual void ResetCheckedEntity(long entityFlag)
		{
			_checkedEntityFlags &= ~entityFlag;
		}

		public void SetEntityFlag(long entityFlag, bool hasEntity)
		{
			_checkedEntityFlags |= entityFlag;
			if (hasEntity)
			{
				_hasEntityFlags |= entityFlag;
			}
		}

		public override long GetEntityFlag()
		{
			return _entityFlag;
		}

		public override string ToString()
		{
			return RawText?.TrimEnd(Delimiter);
		}

		public override bool Equals(object obj)
		{
			bool equals = false;
			if (obj is Message)
			{
				equals = (obj as Message)?.ToString() == ToString();
			}
			return equals;
		}

		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}
	}
}
