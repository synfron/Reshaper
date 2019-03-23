namespace ReshaperCore.Messages.Entities.Http
{
	/// <summary>
	/// Represents the HTTP status line of a HTTP request.
	/// </summary>
	public class HttpRequestStatusLine : HttpStatusLine
	{
		private static long _entityFlag;
		private string _method;
		private string _uri;

		/// <summary>
		/// The HTTP method
		/// </summary>
		public virtual string Method
		{
			set
			{
				_method = value;
				OnPropertyChanged(nameof(Method));
			}
			get
			{
				return _method;
			}
		}

		/// <summary>
		/// The URI of the HTTP request
		/// </summary>
		public virtual string Uri
		{
			set
			{
				_uri = value;
				OnPropertyChanged(nameof(Uri));
			}
			get
			{
				return _uri;
			}
		}

		static HttpRequestStatusLine()
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
		/// <returns>The full status line as text</returns>
		public override string ToString()
		{
			return $"{Method} {Uri} {Version}";
		}

		/// <summary>
		/// <see cref="object.Equals(object)"/>
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			bool equals = false;
			if (obj is HttpRequestStatusLine)
			{
				equals = (obj as HttpRequestStatusLine)?.ToString() == ToString();
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
