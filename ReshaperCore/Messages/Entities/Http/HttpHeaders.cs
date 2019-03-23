using System;
using System.Collections.Generic;
using System.Linq;

namespace ReshaperCore.Messages.Entities.Http
{
	/// <summary>
	/// Represents the headers of an HTTP request or response
	/// </summary>
	public class HttpHeaders : EntityContainer
	{
		private static long _entityFlag;
		private int _lastIndex = 0;
		private Dictionary<string, Tuple<int, string>> _headers = new Dictionary<string, Tuple<int, string>>(StringComparer.OrdinalIgnoreCase);

		/// <summary>
		/// The number of individual HTTP headers
		/// </summary>
		public virtual int Count
		{
			get
			{
				return _headers.Count;
			}
		}

		/// <summary>
		/// Get or set the value of an HTTP header
		/// </summary>
		/// <param name="name">The header's name</param>
		/// <returns></returns>
		public virtual string this[string name]
		{
			get
			{
				return _headers[name].Item2;
			}
			set
			{
				if (value != null)
				{
					Tuple<int, string> currentVal = null;
					if (_headers.TryGetValue(name, out currentVal))
					{
						_headers[name] = new Tuple<int, string>(currentVal.Item1, value);
					}
					else
					{
						_headers[name] = new Tuple<int, string>(_lastIndex++, value);
					}
				}
				else
				{
					_headers.Remove(name);
				}

			}
		}

		static HttpHeaders()
		{
			_entityFlag = RegisterFlag();
		}

		public override long GetEntityFlag()
		{
			return _entityFlag;
		}

		/// <summary>
		/// <see cref="object.ToString"/>
		/// </summary>
		/// <returns>All headers combined into a single string</returns>
		public override string ToString()
		{
			return string.Join(NewLine, _headers.OrderBy(pair => pair.Value.Item1).Select(pair => pair.Key + ": " + pair.Value.Item2));
		}

		/// <summary>
		/// Gets the value of the HTTP header or null if the header does not exist.
		/// </summary>
		/// <param name="name">The header's name</param>
		/// <returns>The HTTP header value</returns>
		public virtual string GetOrDefault(string name)
		{
			Tuple<int, string> tupleVal = null;
			_headers.TryGetValue(name, out tupleVal);
			return tupleVal?.Item2;
		}

		/// <summary>
		/// Determines if the specified header exists
		/// </summary>
		/// <param name="name">The header's name</param>
		/// <returns>True if the header exists, false otherwise</returns>
		public virtual bool Contains(string name)
		{
			return _headers.ContainsKey(name);
		}

		/// <summary>
		/// <see cref="object.Equals(object)"/>
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			bool equals = false;
			if (obj is HttpHeaders)
			{
				equals = (obj as HttpHeaders)?.ToString() == ToString();
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

		public static long EntityFlag
		{
			get
			{
				return _entityFlag;
			}
		}

		/// <summary>
		/// The string that is used a newline separating each header.
		/// </summary>
		public string NewLine
		{
			get;
			set;
		}
	}
}
