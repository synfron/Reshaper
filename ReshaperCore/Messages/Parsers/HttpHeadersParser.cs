using ReshaperCore.Messages.Entities.Http;
using ReshaperCore.Rules;
using ReshaperCore.Utils;

namespace ReshaperCore.Messages.Parsers
{
	public class HttpHeadersParser
	{
		private string _text;
		private int _pos;
		private string _newLineStr;
		private bool _acceptEofAsLine;

		public void Parse(EventInfo eventInfo, string replacementText, bool acceptEofAsLine = false)
		{
			this._acceptEofAsLine = acceptEofAsLine;
			HttpMessage httpMessage = eventInfo.Message as HttpMessage;
			if (httpMessage != null)
			{
				HttpHeaders headers = Parse(replacementText);
				if (headers != null)
				{
					httpMessage.Headers = headers;
				}
				else
				{
					Log.LogError(null, "Invalid header. Could not set.", replacementText);
				}
			}
		}

		public HttpHeaders Parse(string text, bool acceptEofAsLine = false)
		{
			this._text = text;
			_pos = 0;
			return ParseHeaders();
		}

		private HttpHeaders ParseHeaders()
		{
			string line;
			string[] sections;
			HttpHeaders headers = new HttpHeaders();
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
			if (headers != null && !string.IsNullOrEmpty(_newLineStr))
			{
				headers.NewLine = _newLineStr;
			}
			return headers;
		}

		private string ReadLine()
		{
			int currentPos = _pos;
			char currentChar = '\r';
			char nextChar = '\n';
			while (currentPos < _text.Length)
			{
				switch (_text[currentPos])
				{
					case '\n':
						currentChar = '\n';
						nextChar = '\r';
						goto case '\r';
					case '\r':
						string line = _text.Substring(_pos, currentPos - _pos);
						if (++currentPos < _text.Length && _text[currentPos] == nextChar)
						{
							_pos = ++currentPos;
							_newLineStr = currentChar.ToString() + nextChar.ToString();
						}
						else
						{
							_pos = currentPos;
							_newLineStr = currentChar.ToString();
						}
						return line;
				}
				currentPos++;
			}
			string returnVal = null;
			if (_acceptEofAsLine)
			{
				returnVal = _text.Substring(_pos, currentPos - _pos);
				_pos = currentPos;
			}
			return returnVal;
		}
	}
}
