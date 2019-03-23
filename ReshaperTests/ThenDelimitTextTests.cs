using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReshaperCore.Messages;
using ReshaperCore.Proxies;
using ReshaperCore.Rules;
using ReshaperCore.Rules.Thens;
using ReshaperCore.Vars;

namespace ReshaperTests
{
	[TestClass]
	public class ThenDelimitTextTests
	{
		[TestMethod]
		public void Perform_MultiMessagesTest()
		{
			Variables variables = new Variables();

			ThenDelimitText thenDelimitText = new ThenDelimitText();

			ProxyInfo proxyInfo = new ProxyInfo();
			proxyInfo.Delimiters.Add("\n");

			Mock<ProxyConnection> mockProxyConnection = new Mock<ProxyConnection>(It.IsAny<ProxyHost>(), It.IsAny<ProxyInfo>(), It.IsAny<TcpClient>());

			Mock<RulesEngine> rulesEngineMock = new Mock<RulesEngine>() { CallBase = true };
			rulesEngineMock.Setup(engine => engine.OnUpdate());
			mockProxyConnection.SetupGet(mock => mock.ProxyInfo).Returns(proxyInfo);

			List<string> expectedTexts = new List<string>()
			{
				":NickServ!service@rizon.net NOTICE Trex :    ACCESS     Modify the list of authorized addresses",
				":NickServ!service@rizon.net NOTICE Trex: SET        Set options, including kill protection",
				":NickServ!service@rizon.net NOTICE Trex: DROP       Cancel the registration of a nickname",
				""
			};

			EventInfo eventInfo = new EventInfo()
			{
				Message = new Message()
				{
					Complete = false,
					TextEncoding = Encoding.UTF8,
					RawText = string.Join("\n", expectedTexts)
				},
				Type = EventType.Message,
				Variables = variables,
				Engine = rulesEngineMock.Object,
				ProxyConnection = mockProxyConnection.Object
			};

			thenDelimitText.Perform(eventInfo);

			Assert.AreEqual(eventInfo.Engine.Queue.TakeFirst().Message.ToString(), expectedTexts[0]);
			Assert.AreEqual(eventInfo.Engine.Queue.TakeFirst().Message.ToString(), expectedTexts[1]);
			Assert.AreEqual(eventInfo.Engine.Queue.TakeFirst().Message.ToString(), expectedTexts[2]);

		}

		[TestMethod]
		public void Perform_MulitPartialMessagesTest()
		{
			Variables variables = new Variables();

			ThenDelimitText thenDelimitText = new ThenDelimitText();

			string firstPartial = ":NickServ!service@rizon.net NOTICE Trex : ";
			string lastPartial = "\n";

			List<string> expectedTexts = new List<string>()
			{
				"   ACCESS     Modify the list of authorized addresses",
				":NickServ!service@rizon.net NOTICE Trex: SET        Set options, including kill protection",
				":NickServ!service@rizon.net NOTICE Trex: DROP       Cancel the registration of a nickname",
			};

			Mock<ProxyInfo> mockProxyInfo = new Mock<ProxyInfo>() { CallBase = true };
			Mock<ProxyConnection> mockProxyConnection = new Mock<ProxyConnection>(It.IsAny<ProxyHost>(), It.IsAny<ProxyInfo>(), It.IsAny<TcpClient>());
			Mock<RulesEngine> mockRulesEngine = new Mock<RulesEngine>() { CallBase = true };

			mockProxyConnection.Setup(mock => mock.ProxyInfo).Returns(mockProxyInfo.Object);
			mockProxyInfo.Setup(mock => mock.UseDelimiter).Returns(true);
			mockRulesEngine.Setup(mock => mock.OnUpdate());

			mockProxyInfo.Object.Delimiters.Add("\n");

			List<EventInfo> events = new List<EventInfo>()
			{
				new EventInfo()
				{
					Message = new Message()
					{
						Complete = false,
						TextEncoding = Encoding.UTF8,
						RawText = firstPartial
					},
					Type = EventType.Message,
					Variables = variables,
					Engine = mockRulesEngine.Object,
					ProxyConnection = mockProxyConnection.Object
				},
				new EventInfo()
				{
					Message = new Message()
					{
						Complete = false,
						TextEncoding = Encoding.UTF8,
						RawText = string.Join("\n", expectedTexts)
					},
					Type = EventType.Message,
					Variables = variables,
					Engine = mockRulesEngine.Object,
					ProxyConnection = mockProxyConnection.Object
				},
				new EventInfo()
				{
					Message = new Message()
					{
						Complete = false,
						TextEncoding = Encoding.UTF8,
						RawText = lastPartial
					},
					Type = EventType.Message,
					Variables = variables,
					Engine = mockRulesEngine.Object,
					ProxyConnection = mockProxyConnection.Object
				}
			};

			thenDelimitText.Perform(events[0]);
			thenDelimitText.Perform(events[1]);

			Assert.AreEqual(mockRulesEngine.Object.Queue.TakeFirst().Message.ToString(), firstPartial + expectedTexts[0]);
			Assert.AreEqual(mockRulesEngine.Object.Queue.TakeFirst().Message.ToString(), expectedTexts[1]);

			thenDelimitText.Perform(events[2]);

			Assert.AreEqual(mockRulesEngine.Object.Queue.TakeFirst().Message.ToString(), expectedTexts[2]);
		}
	}
}
