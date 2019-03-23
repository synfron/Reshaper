namespace ReshaperCore.Messages.Entities.Http
{
	/// <summary>
	/// Represents the status line of a HTTP response
	/// </summary>
	public class HttpResponseStatusLine : HttpStatusLine
	{
		private static long _entityFlag;
		private int _statusCode;
		private string _statusMessage;

		/// <summary>
		/// The status code of the HTTP response
		/// </summary>
		public virtual int StatusCode
		{
			set
			{
				_statusCode = value;
				OnPropertyChanged(nameof(StatusCode));
			}
			get
			{
				return _statusCode;
			}
		}

		/// <summary>
		/// The status message of the HTTP response
		/// </summary>
		public virtual string StatusMessage
		{
			set
			{
				_statusMessage = value;
				OnPropertyChanged(nameof(StatusMessage));
			}
			get
			{
				return _statusMessage;
			}
		}

		public static long EntityFlag
		{
			get
			{
				return _entityFlag;
			}
		}

		static HttpResponseStatusLine()
		{
			_entityFlag = RegisterFlag();
		}

		public override long GetEntityFlag()
		{
			return _entityFlag;
		}

		/// <summary>
		/// <see cref="object.Equals(object)"/>
		/// </summary>
		/// <returns>The full status line as text</returns>
		public override string ToString()
		{
			return $"{Version} {StatusCode} {StatusMessage}";
		}

		/// <summary>
		/// <see cref="object.Equals(object)"/>
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			bool equals = false;
			if (obj is HttpResponseStatusLine)
			{
				equals = (obj as HttpResponseStatusLine)?.ToString() == ToString();
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
