using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReshaperTests
{
	[TestClass]
	public class ThenDelimitHttpTests
	{
		//		[TestMethod]
		//		public void Perform_MultiPartialTest()
		//		{
		//			string httpTexts = File.ReadAllText("TestFiles/HttpTexts.txt");

		//			List<string> expectedTexts = (new List<string> {
		//				@"POST http://www.syngames.net/account/login.php HTTP/1.1
		//Host: www.syngames.net
		//User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0
		//Accept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8
		//Accept-Language: en-US,en;q=0.5
		//Accept-Encoding: gzip, deflate
		//Referer: http://www.syngames.net/
		//Cookie: __utma=75527319.894017908.1450359518.1452069589.1454651852.3; __utmz=75527319.1450359520.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); PHPSESSID=be2d1e84eb423f3539f976c3aa914b31; __utmb=75527319.2.10.1454651853; __utmc=75527319; __utmt=1
		//Connection: keep-alive
		//Content-Type: application/x-www-form-urlencoded
		//Content-Length: 25

		//uname=dfsdfd&upass=vdfvfd",
		//				@"GET http://www.syngames.net/addins/shoutbox/ HTTP/1.1
		//Host: www.syngames.net
		//User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0
		//Accept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8
		//Accept-Language: en-US,en;q=0.5
		//Accept-Encoding: gzip, deflate
		//Referer: http://www.syngames.net/
		//Cookie: __utma=75527319.894017908.1450359518.1452069589.1454651852.3; __utmz=75527319.1450359520.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); PHPSESSID=be2d1e84eb423f3539f976c3aa914b31; __utmb=75527319.2.10.1454651853; __utmc=75527319; __utmt=1
		//DNT: 1
		//Connection: keep-alive

		//",
		//				@"HTTP/1.1 200 OK
		//Date: Fri, 05 Feb 2016 06:00:16 GMT
		//Server: Apache
		//X-Powered-By: PHP/5.6.15
		//Expires: Thu, 19 Nov 1981 08:52:00 GMT
		//Cache-Control: no-store, no-cache, must-revalidate, post-check=0, pre-check=0
		//Pragma: no-cache
		//Keep-Alive: timeout=2, max=198
		//Connection: Keep-Alive
		//Content-Type: text/html; charset=UTF-8
		//Content-Length: 149


		//function add_emoticon(emoticon) {
		//	document.getElementById(""shout_message"").value += emoticon;
		//	document.getElementById(""shout_message"").focus();
		//}".Replace("\r", ""),
		//				@"HTTP/1.1 302 Moved Temporarily
		//Date: Fri, 05 Feb 2016 06:00:14 GMT
		//Server: Apache
		//X-Powered-By: PHP/5.6.15
		//Expires: Thu, 19 Nov 1981 08:52:00 GMT
		//Cache-Control: no-store, no-cache, must-revalidate, post-check=0, pre-check=0
		//Pragma: no-cache
		//Location: /
		//Content-Length: 0
		//Keep-Alive: timeout=2, max=200
		//Connection: Keep-Alive
		//Content-Type: text/html; charset=UTF-8

		//",
		//				@"HTTP/1.1 301 Moved Permanently
		//Date: Fri, 05 Feb 2016 05:56:31 GMT
		//Content-Type: text/html
		//Connection: keep-alive
		//Cache-control: no-cache=""set-cookie""
		//Location: http://www.w3counter.com/tracker.js
		//CF-Cache-Status: MISS
		//Vary: Accept-Encoding
		//Server: cloudflare-nginx
		//CF-RAY: 26fc5f5d664509b8-ORD
		//Content-Length: 178

		//<html>
		//<head><title>301 Moved Permanently</title></head>
		//<body bgcolor=""white"">
		//<center><h1>301 Moved Permanently</h1></center>
		//<hr><center>nginx</center>
		//</body>
		//</html>
		//",
		//				@"GET http://code.jquery.com/jquery-1.10.2.js HTTP/1.1
		//Host: code.jquery.com
		//User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0
		//Accept: */*
		//Accept-Language: en-US,en;q=0.5
		//Accept-Encoding: gzip, deflate
		//Cookie: __cfduid=df3aec58db6353bfe4715700e01a15f891443760856
		//DNT: 1
		//Referer: http://code.jquery.com/jquery-1.10.2.js
		//Connection: keep-alive

		//",
		//				@"GET http://www.syngames.net/addins/shoutbox/ HTTP/1.1
		//Host: www.syngames.net
		//User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64; rv:43.0) Gecko/20100101 Firefox/43.0
		//Accept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8
		//Accept-Language: en-US,en;q=0.5
		//Accept-Encoding: gzip, deflate
		//Referer: http://www.syngames.net/
		//Cookie: __utma=75527319.894017908.1450359518.1450359518.1452069589.2; __utmz=75527319.1450359520.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); PHPSESSID=be2d1e84eb423f3539f976c3aa914b31
		//DNT: 1
		//Connection: keep-alive

		//"
		//			}).Select(text => text).ToList();

		//			List<string> chunkedText = new List<string>(Chunk(httpTexts, 3));

		//			List<EventInfo> events = new List<EventInfo>();

		//			Variables variables = new Variables();

		//			ThenDelimitHttp thenDelimitHttp = new ThenDelimitHttp();

		//			Mock<RulesEngine> mockRulesEngine = new Mock<RulesEngine>() { CallBase = true };
		//			Mock<ProxyConnection> mockProxyConnection = new Mock<ProxyConnection>(It.IsAny<ProxyHost>(), It.IsAny<ProxyInfo>(), It.IsAny<TcpClient>());
		//			Mock<ProxyInfo> mockProxyInfo = new Mock<ProxyInfo>();

		//			mockRulesEngine.Setup(mock => mock.OnUpdate());
		//			mockProxyConnection.Setup(mock => mock.ProxyInfo).Returns(mockProxyInfo.Object);

		//			foreach (string chunk in chunkedText)
		//			{
		//				events.Add(new EventInfo()
		//				{
		//					Message = new Message()
		//					{
		//						Complete = false,
		//						TextEncoding = Encoding.UTF8,
		//						RawText = chunk
		//					},
		//					ProxyConnection = mockProxyConnection.Object,
		//					Type = EventType.Message,
		//					Variables = variables,
		//					Engine = mockRulesEngine.Object
		//				});
		//			}

		//			List<string> actualTexts = new List<string>();

		//			events.ForEach(eventInfo =>
		//			{
		//				thenDelimitHttp.Perform(eventInfo);
		//				while (!mockRulesEngine.Object.Queue.IsEmpty())
		//				{
		//					actualTexts.Add(mockRulesEngine.Object.Queue.TakeFirst().Message.ToString());
		//				}
		//			});

		//			CollectionAssert.AreEqual(expectedTexts, actualTexts);
		//		}

		private IEnumerable<string> Chunk(string text, int numChunks)
		{
			for (int index = 0; index < numChunks; index++)
			{
				int chunkStartIndex = (text.Length / numChunks) * index;
				int length;
				if (index == numChunks - 1)
				{
					length = text.Length - chunkStartIndex;
				}
				else
				{
					length = ((text.Length / numChunks) * (index + 1)) - chunkStartIndex;
				}
				if (length > 0)
				{
					yield return text.Substring(chunkStartIndex, length);
				}
				else
				{
					yield break;
				}
			}
		}
	}
}
