using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ReshaperCore.Messages.Entities.Http;
using ReshaperCore.Networking;
using ReshaperCore.Providers;
using ReshaperCore.Settings;
using ReshaperCore.Utils.Extensions;

namespace ReshaperCore.Messages.Parsers
{
	public class HttpMessageParser
	{
		private int _fullBytesPos;
		private string _newLineStr;
		private HttpMessageType _type;
		private byte[] _bytes;
		private HttpStatusLine _statusLine = null;
		private HttpHeaders _headers = null;
		private HttpBody _body = null;
		private HttpMessage _message = null;
		private bool _autoUpdateContentLength;

		public Encoding TextEncoding
		{
			get;
			set;
		} = Encoding.UTF8;

        public IGeneralSettings GeneralSettings { get; set; } = new GeneralSettingsProvider().GetInstance();

		public Tuple<int, HttpMessage> Parse(string text, bool useFullText = false)
		{
			return Parse(TextEncoding.GetBytes(text), useFullText);
		}

		public Tuple<int, HttpMessage> Parse(byte[] bytes, bool useFullBody = false)
		{
			_fullBytesPos = 0;
			this._bytes = bytes;
			bool complete = false;
			useFullBody |= GeneralSettings.IgnoreContentLength;
			_autoUpdateContentLength = GeneralSettings.AutoUpdateContentLength;
			_statusLine = ParseStatusLine();
			if (_statusLine != null)
			{
				_headers = ParseHeaders();
				if (_headers != null)
				{
					int rawBodyByteSize = bytes.Length - _fullBytesPos;


					string definedContentLengthStr = _headers.GetOrDefault("Content-Length");
					int definedContentLength = 0;

					if (definedContentLength == 0)
					{
						_autoUpdateContentLength = false;
					}

					int.TryParse(definedContentLengthStr, out definedContentLength);

					if (definedContentLength <= rawBodyByteSize)
					{
						if (_headers.GetOrDefault("Transfer-Encoding") == "chunked")
						{
							_body = ParseBodyChunks();
							if (_body != null)
							{
								complete = true;
							}
						}
						else
						{
							if (useFullBody)
							{
								definedContentLength = rawBodyByteSize;
							}

							if (definedContentLength > 0)
							{
								_body = ParseBody<HttpBody>(definedContentLength);
								_fullBytesPos += definedContentLength;
								if (_body != null)
								{
									if (_autoUpdateContentLength)
									{
										_headers["Content-Length"] = definedContentLength.ToString();
									}
									complete = true;
								}
							}
							else
							{
								complete = true;
							}
						}
					}
				}
			}
			if (complete)
			{
				_message = new HttpMessage()
				{
					Body = _body,
					Complete = complete,
					TextEncoding = TextEncoding,
					Headers = _headers,
					StatusLine = _statusLine,
					NewLine = _newLineStr,
					Type = _type
				};
			}
			return new Tuple<int, HttpMessage>(_fullBytesPos, _message);
		}

		private HttpChunkedBody ParseBodyChunks()
		{
			int startBytesPos = _fullBytesPos;
			bool valid = false;
			IList<byte[]> chunks = new List<byte[]>();

			int chunkLength = 0;
			do
			{
				int newLineOffset = GetNextNewLineInfo()?.Item1 ?? -1;
				if (newLineOffset > 2)
				{
					string chunkLengthStr = Encoding.ASCII.GetString(_bytes, _fullBytesPos, newLineOffset).Trim();
					if (!string.IsNullOrEmpty(chunkLengthStr))
					{
						if (int.TryParse(chunkLengthStr, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out chunkLength))
						{
							if (chunkLength == 0)
							{
								_fullBytesPos += newLineOffset;
								newLineOffset = GetNextNewLineInfo()?.Item1 ?? -1;
								if (newLineOffset != 2)
								{
									break;
								}
								else
								{
									_fullBytesPos += newLineOffset;
									valid = true;
								}
							}
							else if (chunkLength > 0)
							{
								_fullBytesPos += newLineOffset;
								if (chunkLength + _fullBytesPos < _bytes.Length)
								{
									chunks.Add(new Buffer<byte>(_bytes, _fullBytesPos, chunkLength).GetBytes());
									_fullBytesPos += chunkLength;
									newLineOffset = GetNextNewLineInfo()?.Item1 ?? -1;
									if (newLineOffset < 2)
									{
										break;
									}
									else
									{
										_fullBytesPos += newLineOffset;
									}
								}
								else
								{
									break;
								}
							}
							else
							{
								break;
							}
						}
					}
					else
					{
						break;
					}
				}
				else
				{
					break;
				}
			}
			while (chunkLength > 0);

			HttpChunkedBody body = null;
			if (valid)
			{
				body = ParseBody<HttpChunkedBody>(_fullBytesPos - startBytesPos, startBytesPos);
				body.UnchunkedBytes = chunks.Combine();
			}
			return body;
		}

		private Tuple<int, string> GetNextNewLineInfo()
		{
			int offsetPos = _fullBytesPos;
			while (offsetPos + 1 < _bytes.Length)
			{
				if (_bytes[offsetPos] == '\r' && _bytes[offsetPos + 1] == '\n')
				{
					offsetPos += 2;
					return new Tuple<int, string>(offsetPos - _fullBytesPos, "\r\n");
				}
				else if (_bytes[offsetPos] == '\n')
				{
					offsetPos++;
					return new Tuple<int, string>(offsetPos - _fullBytesPos, "\n");
				}
				else
				{
					offsetPos++;
				}
			}
			return null;
		}

		private T ParseBody<T>(int contentSize, int? startPos = null) where T : HttpBody, new()
		{
			if (startPos == null)
			{
				startPos = _fullBytesPos;
			}
			byte[] bodyBytes = new byte[contentSize];
			Buffer.BlockCopy(_bytes, startPos.Value, bodyBytes, 0, contentSize);
			T body = new T()
			{
				TextEncoding = TextEncoding,
				RawBytes = bodyBytes
			};
			return body;
		}

		private HttpHeaders ParseHeaders()
		{
			string line;
			string[] sections;
			HttpHeaders headers = new HttpHeaders();
			headers.NewLine = _newLineStr;
			do
			{
				line = ReadLine();

				if (line != null)
				{
					sections = line.Split(new char[] { ':' }, 2);
					if (sections.Length == 2)
					{
						headers[sections[0]] = sections[1].TrimStart();
					}
					else
					{
						break;

					}
				}
				else
				{
					headers = null;
				}
			}
			while (!string.IsNullOrWhiteSpace(line));

			if (headers != null)
			{
				//Encodings are currently not supported.
				headers["Accept-Encoding"] = null;
			}

			return headers;
		}

		private HttpStatusLine ParseStatusLine()
		{
			HttpStatusLine statusLine = null;
			string line = ReadLine();
			if (!string.IsNullOrEmpty(line))
			{
				string[] sections = line.Split(new char[] { ' ' }, 3);
				if (sections.Length == 3)
				{
					statusLine = ParseResponseStatusLine(sections) ?? ParseRequestStatusLine(sections);
				}
			}
			return statusLine;
		}

		private HttpStatusLine ParseRequestStatusLine(string[] sections)
		{
			HttpStatusLine statusLine = new HttpRequestStatusLine
			{
				Method = sections[0],
				Uri = sections[1],
				Version = sections[2]
			};
			_type = HttpMessageType.Request;
			return statusLine;
		}

		private HttpStatusLine ParseResponseStatusLine(string[] sections)
		{
			HttpStatusLine statusLine = null;
			if (sections[0].StartsWith("http", System.StringComparison.InvariantCultureIgnoreCase))
			{
				string version = sections[0];
				int statusCode = 0;
				string statusMessage = sections[2];

				if (int.TryParse(sections[1], out statusCode))
				{
					statusLine = new HttpResponseStatusLine()
					{
						Version = sections[0],
						StatusCode = statusCode,
						StatusMessage = sections[2]
					};
					_type = HttpMessageType.Response;
				}
			}
			return statusLine;
		}

		private string ReadLine()
		{
			string line = null;
			Tuple<int, string> newLineInfo = GetNextNewLineInfo();
			if (newLineInfo != null)
			{
				line = TextEncoding.GetString(_bytes, _fullBytesPos, newLineInfo.Item1);
				_newLineStr = newLineInfo.Item2;
				_fullBytesPos += newLineInfo.Item1;
			}
			return line?.TrimEnd();
		}
	}
}
