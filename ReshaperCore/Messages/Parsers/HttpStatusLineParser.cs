using ReshaperCore.Messages.Entities.Http;

namespace ReshaperCore.Messages.Parsers
{
	public class HttpStatusLineParser
	{

		public HttpStatusLine Parse(string line)
		{
			HttpStatusLine statusLine = null;
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
				}
			}
			return statusLine;
		}
	}
}
