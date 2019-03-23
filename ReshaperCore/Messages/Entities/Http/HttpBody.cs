using System.Text;

namespace ReshaperCore.Messages.Entities.Http
{
	/// <summary>
	/// Represents the body of an HTTP request or response
	/// </summary>
	public class HttpBody : EntityContainer
	{
		private static long _entityFlag;
		private string _text;
		private byte[] _rawBytes;

		public Encoding TextEncoding
		{
			get;
			set;
		} = Encoding.UTF8;

		public virtual byte[] RawBytes
		{
			get
			{
				return _rawBytes;
			}
			set
			{
				_rawBytes = value;
				_text = TextEncoding.GetString(_rawBytes);
				OnPropertyChanged(nameof(RawBytes));
				OnPropertyChanged(nameof(Text));
				OnPropertyChanged(nameof(ContentLength));
			}
		}

		/// <summary>
		/// The full text of the body
		/// </summary>
		public virtual string Text
		{
			set
			{
				_text = value;
				_rawBytes = TextEncoding.GetBytes(_text);
				OnPropertyChanged(nameof(Text));
				OnPropertyChanged(nameof(RawBytes));
				OnPropertyChanged(nameof(ContentLength));
			}
			get
			{
				return _text;
			}
		}

		/// <summary>
		/// The size of the body
		/// </summary>
		public int ContentLength
		{
			get
			{
				return this._rawBytes.Length;
			}
		}

		static HttpBody()
		{
			_entityFlag = RegisterFlag();
		}

		public override long GetEntityFlag()
		{
			return _entityFlag;
		}

		public static long EntityFlag
		{
			get
			{
				return _entityFlag;
			}
		}

		/// <summary>
		/// <see cref="object.ToString"/>
		/// </summary>
		/// <returns>The text of the body</returns>
		public override string ToString()
		{
			return Text;
		}

		/// <summary>
		/// <see cref="object.Equals(object)"/>
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			bool equals = false;
			if (obj is HttpBody)
			{
				equals = (obj as HttpBody)?.ToString() == ToString();
			}
			return equals;
		}

		/// <summary>
		/// <see cref="object.GetHashCode"/>
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}
	}
}
