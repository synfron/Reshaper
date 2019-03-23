using System.Net.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReshaperCore.Messages;
using ReshaperCore.Proxies;
using ReshaperCore.Rules;
using ReshaperCore.Rules.Thens;
using ReshaperCore.Utils;
using ReshaperCore.Vars;

namespace ReshaperTests
{
	/// <summary>
	/// Summary description for ThenAddMessageTest
	/// </summary>
	[TestClass]
	public class ThenAddMessageTest
	{

		[TestMethod]
		public void ThenAddMessage_PerformTest()
		{
			string messageText = "blah blah blah";
			Variables toTargetVariables = new Variables();
			Variables toOriginVariables = new Variables();

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>() { CallBase = true };
			Mock<ProxyConnection> mockProxyConnection = new Mock<ProxyConnection>(It.IsAny<ProxyHost>(), It.IsAny<ProxyInfo>(), It.IsAny<TcpClient>());
			Mock<VariableString> mockMessageTextString = new Mock<VariableString>(It.IsAny<string>(), null);
			Mock<RulesEngine> mockRulesEngine = new Mock<RulesEngine>() { CallBase = true };
			Mock<MessageQueue<EventInfo>> mockMessageQueue = new Mock<MessageQueue<EventInfo>>();

			EventInfo eventInfo = mockEventInfo.Object;
			VariableString messageTextString = mockMessageTextString.Object;
			RulesEngine rulesEngine = mockRulesEngine.Object;
			MessageQueue<EventInfo> messageQueue = mockMessageQueue.Object;
			ProxyConnection proxyConnection = mockProxyConnection.Object;

			mockMessageTextString.Setup(mock => mock.GetText(It.IsAny<Variables>())).Returns(messageText);
			mockProxyConnection.Setup(mock => mock.ToTargetConnectionVariables).Returns(toTargetVariables);
			mockProxyConnection.Setup(mock => mock.ToOriginConnectionVariables).Returns(toOriginVariables);
			mockRulesEngine.Setup(mock => mock.Queue).Returns(messageQueue);
			mockEventInfo.Setup(mock => mock.Engine).Returns(rulesEngine);
			mockEventInfo.Setup(mock => mock.ProxyConnection).Returns(proxyConnection);

			var testCases = new[]
			{
				new
				{
					Direction = DataDirection.Origin,
					InsertAtBeginning = true,
					ExpectedVariables = toOriginVariables
                },
				new
				{
					Direction = DataDirection.Origin,
					InsertAtBeginning = false,
					ExpectedVariables = toOriginVariables
                },
				new
				{
					Direction = DataDirection.Target,
					InsertAtBeginning = true,
					ExpectedVariables = toTargetVariables
                },
				new
				{
					Direction = DataDirection.Target,
					InsertAtBeginning = false,
					ExpectedVariables = toTargetVariables
                }
			};

			foreach (var testCase in testCases)
			{
				ThenAddMessage then = new ThenAddMessage()
				{
					Direction = testCase.Direction,
					InsertAtBeginning = testCase.InsertAtBeginning,
					MessageText = messageTextString
				};


				mockMessageQueue.Reset();

				if (testCase.InsertAtBeginning)
				{
					mockMessageQueue.Setup(mock => mock.AddFirst(It.IsAny<EventInfo>())).Callback((EventInfo eventInfoParam) =>
					{
						Assert.AreEqual(testCase.ExpectedVariables, eventInfoParam.Variables);
						Assert.AreEqual(messageText, eventInfoParam.Message.RawText);
					});

					Assert.AreEqual(ThenResponse.Continue, then.Perform(eventInfo));

					mockMessageQueue.Verify(mock => mock.AddFirst(It.IsAny<EventInfo>()), Times.Once);
				}
				else
				{
					mockMessageQueue.Setup(mock => mock.AddLast(It.IsAny<EventInfo>())).Callback((EventInfo eventInfoParam) =>
					{
						Assert.AreEqual(testCase.ExpectedVariables, eventInfoParam.Variables);
						Assert.AreEqual(messageText, eventInfoParam.Message.RawText);
					});

					Assert.AreEqual(ThenResponse.Continue, then.Perform(eventInfo));

					mockMessageQueue.Verify(mock => mock.AddLast(It.IsAny<EventInfo>()), Times.Once);
				}
			}
		}
	}
}
