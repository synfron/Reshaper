namespace ReshaperCore.Messages.Entities.Http
{
	class HttpChunkedBody : HttpBody
	{
		private static long _entityFlag;
		private string _unchunkedText;
		private byte[] _unchunkedBytes;

		public override byte[] RawBytes
		{
			get
			{
				return base.RawBytes;
			}
			set
			{
				base.RawBytes = value;
				UnchunkedBytes = value;
			}
		}

		/// <summary>
		/// The full text of the body
		/// </summary>
		public override string Text
		{
			set
			{
				base.Text = value;
				UnchunkedText = value;
			}
			get
			{
				return base.Text;
			}
		}

		public virtual string UnchunkedText
		{
			set
			{
				_unchunkedText = value;
				_unchunkedBytes = TextEncoding.GetBytes(_unchunkedText);
				OnPropertyChanged(nameof(UnchunkedText));
				OnPropertyChanged(nameof(UnchunkedBytes));
			}
			get
			{
				return _unchunkedText;
			}
		}

		public virtual byte[] UnchunkedBytes
		{
			get
			{
				return _unchunkedBytes;
			}
			set
			{
				_unchunkedBytes = value;
				_unchunkedText = TextEncoding.GetString(_unchunkedBytes);
				OnPropertyChanged(nameof(UnchunkedText));
				OnPropertyChanged(nameof(UnchunkedBytes));
			}
		}

		static HttpChunkedBody()
		{
			_entityFlag = RegisterFlag();
		}

		public override long GetEntityFlag()
		{
			return _entityFlag;
		}

		public static new long EntityFlag
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
			return UnchunkedText;
		}

		/// <summary>
		/// <see cref="object.Equals(object)"/>
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			bool equals = false;
			if (obj is HttpChunkedBody)
			{
				equals = (obj as HttpChunkedBody)?.ToString() == ToString();
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
