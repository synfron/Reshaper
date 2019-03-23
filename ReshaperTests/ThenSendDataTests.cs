using System.Net.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReshaperCore.Proxies;
using ReshaperCore.Rules;
using ReshaperCore.Rules.Thens;

namespace ReshaperTests
{
	[TestClass]
	public class ThenSendDataTests
	{
		[TestMethod]
		public void PerformTest()
		{
			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<ProxyConnection> mockProxyConnection = new Mock<ProxyConnection>(It.IsAny<ProxyHost>(), It.IsAny<ProxyInfo>(), It.IsAny<TcpClient>());

			EventInfo eventInfo = mockEventInfo.Object;
			ProxyConnection proxyConnection = mockProxyConnection.Object;

			var testCases = new[]
			{
				new
				{
					EventType = EventType.Connected,
					SendDataCalled = false,
					ExpectedThenResponse = ThenResponse.Continue
				},
				new
				{
					EventType = EventType.Disconnected,
					SendDataCalled = false,
					ExpectedThenResponse = ThenResponse.Continue
				},
				new
				{
					EventType = EventType.Message,
					SendDataCalled = true,
					ExpectedThenResponse = ThenResponse.Continue
				}
			};

			foreach (var testCase in testCases)
			{
				ThenSendData then = new ThenSendData();

				mockEventInfo.Reset();
				mockProxyConnection.Reset();

				mockEventInfo.Setup(mock => mock.ProxyConnection).Returns(proxyConnection);
				mockEventInfo.Setup(mock => mock.Type).Returns(testCase.EventType);

				Assert.AreEqual(testCase.ExpectedThenResponse, then.Perform(eventInfo));

				if (testCase.SendDataCalled)
				{
					mockProxyConnection.Verify(mock => mock.SendData(eventInfo), Times.Once);
				}
				else
				{
					mockProxyConnection.Verify(mock => mock.SendData(It.IsAny<EventInfo>()), Times.Never);
				}
			}
		}
	}
}
