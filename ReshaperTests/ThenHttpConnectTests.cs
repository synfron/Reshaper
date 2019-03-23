using System.Net.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReshaperCore.Messages;
using ReshaperCore.Messages.Entities.Http;
using ReshaperCore.Proxies;
using ReshaperCore.Rules;
using ReshaperCore.Rules.Thens;

namespace ReshaperTests
{
	[TestClass]
	public class ThenHttpConnectTests
	{
		[TestMethod]
		public void ThenHttpConnect_PerformTest()
		{
			string hostname = "www.google.com";
			int port = 90;

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<ProxyConnection> mockProxyConnection = new Mock<ProxyConnection>(It.IsAny<ProxyHost>(), It.IsAny<ProxyInfo>(), It.IsAny<TcpClient>());
			Mock<HttpMessage> mockHttpMessage = new Mock<HttpMessage>();
			Mock<HttpHeaders> mockHttpHeaders = new Mock<HttpHeaders>();

			EventInfo eventInfo = mockEventInfo.Object;
			ProxyConnection proxyConnection = mockProxyConnection.Object;
			HttpMessage httpMessage = mockHttpMessage.Object;
			HttpHeaders httpHeaders = mockHttpHeaders.Object;

			mockHttpMessage.SetupGet(mock => mock.Headers).Returns(httpHeaders);

			var testCases = new[]
			{
				new
				{
					HasTargetConnection = false,
					DataDirection = DataDirection.Target,
					OverrideCurrentConnection = false,
					HeaderValue = (string)null,
					InitTargetChannelSuccess = false,
					InitTargetChannelCalled = false,
					ExpectedThenResponse = ThenResponse.BreakRules,
					ExpectedHostname = (string)null,
					ExpectedPort = (int?)null
				},
				new
				{
					HasTargetConnection = true,
					DataDirection = DataDirection.Target,
					OverrideCurrentConnection = true,
					HeaderValue = $"{hostname}:{port}",
					InitTargetChannelSuccess = true,
					InitTargetChannelCalled = true,
					ExpectedThenResponse = ThenResponse.Continue,
					ExpectedHostname = hostname,
					ExpectedPort = (int?)port
				},
				new
				{
					HasTargetConnection = false,
					DataDirection = DataDirection.Target,
					OverrideCurrentConnection = false,
					HeaderValue = $"{hostname}:{port}",
					InitTargetChannelSuccess = true,
					InitTargetChannelCalled = true,
					ExpectedThenResponse = ThenResponse.Continue,
					ExpectedHostname = hostname,
					ExpectedPort = (int?)port
				},
				new
				{
					HasTargetConnection = false,
					DataDirection = DataDirection.Target,
					OverrideCurrentConnection = false,
					HeaderValue = $"{hostname}",
					InitTargetChannelSuccess = true,
					InitTargetChannelCalled = true,
					ExpectedThenResponse = ThenResponse.Continue,
					ExpectedHostname = hostname,
					ExpectedPort = (int?)80
				},
				new
				{
					HasTargetConnection = false,
					DataDirection = DataDirection.Target,
					OverrideCurrentConnection = false,
					HeaderValue = $"{hostname}:{port}",
					InitTargetChannelSuccess = false,
					InitTargetChannelCalled = true,
					ExpectedThenResponse = ThenResponse.BreakRules,
					ExpectedHostname = hostname,
					ExpectedPort = (int?)port
				}
			};

			foreach (var testCase in testCases)
			{
				ThenHttpConnect then = new ThenHttpConnect();

				mockEventInfo.Reset();
				mockProxyConnection.Reset();
				mockHttpHeaders.Reset();

				mockEventInfo.Setup(mock => mock.ProxyConnection).Returns(proxyConnection);
				mockEventInfo.Setup(mock => mock.Direction).Returns(testCase.DataDirection);
				mockEventInfo.Setup(mock => mock.Message).Returns(httpMessage);
				mockHttpHeaders.Setup(mock => mock.GetOrDefault("Host")).Returns(testCase.HeaderValue);
				mockProxyConnection.Setup(mock => mock.HasTargetConnection).Returns(testCase.HasTargetConnection);

				if (testCase.InitTargetChannelCalled)
				{
					then.OverrideCurrentConnection = testCase.OverrideCurrentConnection;

					mockProxyConnection.Setup(mock => mock.InitConnection(It.IsAny<DataDirection>(), It.IsAny<string>(), It.IsAny<int>())).Returns(testCase.InitTargetChannelSuccess);

					Assert.AreEqual(testCase.ExpectedThenResponse, then.Perform(eventInfo));

					mockProxyConnection.Verify(mock => mock.InitConnection(testCase.DataDirection, testCase.ExpectedHostname, testCase.ExpectedPort.GetValueOrDefault()), Times.Once);
				}
				else
				{
					then.OverrideCurrentConnection = testCase.OverrideCurrentConnection;

					Assert.AreEqual(testCase.ExpectedThenResponse, then.Perform(eventInfo));

					mockProxyConnection.Verify(mock => mock.InitConnection(It.IsAny<DataDirection>(), It.IsAny<string>(), It.IsAny<int>()), Times.Never);
				}
			}
		}
	}
}
