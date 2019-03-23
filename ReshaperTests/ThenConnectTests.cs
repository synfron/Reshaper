using System.Net.Sockets;
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
	public class ThenConnectTests
	{
		[TestMethod]
		public void ThenConnect_PerformTest()
		{
			string proxyHostname = "www.google.com";
			int proxyPort = 90;
			string thenHostname = "www.msn.com";
			int thenPort = 100;

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<ProxyConnection> mockProxyConnection = new Mock<ProxyConnection>(It.IsAny<ProxyHost>(), It.IsAny<ProxyInfo>(), It.IsAny<TcpClient>());
			Mock<ProxyInfo> mockProxyInfo = new Mock<ProxyInfo>();
			Mock<VariableString> mockThenHostnameString = new Mock<VariableString>(It.IsAny<string>(), null);
			Mock<VariableString> mockThenPortString = new Mock<VariableString>(It.IsAny<string>(), null);

			EventInfo eventInfo = mockEventInfo.Object;
			ProxyConnection proxyConnection = mockProxyConnection.Object;
			ProxyInfo proxyInfo = mockProxyInfo.Object;
			VariableString thenHostnameString = mockThenHostnameString.Object;
			VariableString thenPortString = mockThenPortString.Object;

			mockThenHostnameString.Setup(mock => mock.GetText(It.IsAny<Variables>())).Returns(thenHostname);
			mockThenPortString.Setup(mock => mock.GetInt(It.IsAny<Variables>())).Returns(thenPort);

			var testCases = new[]
			{
				new
				{
					HasTargetConnection = true,
					DataDirection = DataDirection.Target,
					OverrideCurrentConnection = false,
					InitTargetConnectionSuccess = false,
					InitTargetConnectionCalled = false,
					ExpectedThenResponse = ThenResponse.Continue,
					UseProxyPort = true,
					UseProxyHostname = true,
					UseThenPort = false,
					UseThenHostname = false,
					ExpectedHostname = (string)null,
					ExpectedPort = (int?)null
				},
				new
				{
					HasTargetConnection = true,
					DataDirection = DataDirection.Origin,
					OverrideCurrentConnection = false,
					InitTargetConnectionSuccess = false,
					InitTargetConnectionCalled = false,
					ExpectedThenResponse = ThenResponse.BreakRules,
					UseProxyPort = true,
					UseProxyHostname = true,
					UseThenPort = false,
					UseThenHostname = false,
					ExpectedHostname = (string)null,
					ExpectedPort = (int?)null
				},
				new
				{
					HasTargetConnection = true,
					DataDirection = DataDirection.Target,
					OverrideCurrentConnection = false,
					InitTargetConnectionSuccess = false,
					InitTargetConnectionCalled = false,
					ExpectedThenResponse = ThenResponse.Continue,
					UseProxyPort = false,
					UseProxyHostname = false,
					UseThenPort = false,
					UseThenHostname = false,
					ExpectedHostname = (string)null,
					ExpectedPort = (int?)null
				},
				new
				{
					HasTargetConnection = true,
					DataDirection = DataDirection.Target,
					OverrideCurrentConnection = true,
					InitTargetConnectionSuccess = true,
					InitTargetConnectionCalled = true,
					ExpectedThenResponse = ThenResponse.Continue,
					UseProxyPort = true,
					UseProxyHostname = true,
					UseThenPort = false,
					UseThenHostname = false,
					ExpectedHostname = proxyHostname,
					ExpectedPort = (int?)proxyPort
				},
				new
				{
					HasTargetConnection = false,
					DataDirection = DataDirection.Target,
					OverrideCurrentConnection = false,
					InitTargetConnectionSuccess = true,
					InitTargetConnectionCalled = true,
					ExpectedThenResponse = ThenResponse.Continue,
					UseProxyPort = true,
					UseProxyHostname = true,
					UseThenPort = true,
					UseThenHostname = true,
					ExpectedHostname = thenHostname,
					ExpectedPort = (int?)thenPort
				},
				new
				{
					HasTargetConnection = false,
					DataDirection = DataDirection.Target,
					OverrideCurrentConnection = false,
					InitTargetConnectionSuccess = true,
					InitTargetConnectionCalled = true,
					ExpectedThenResponse = ThenResponse.Continue,
					UseProxyPort = false,
					UseProxyHostname = false,
					UseThenPort = true,
					UseThenHostname = true,
					ExpectedHostname = thenHostname,
					ExpectedPort = (int?)thenPort
				},
				new
				{
					HasTargetConnection = false,
					DataDirection = DataDirection.Target,
					OverrideCurrentConnection = false,
					InitTargetConnectionSuccess = false,
					InitTargetConnectionCalled = true,
					ExpectedThenResponse = ThenResponse.BreakRules,
					UseProxyPort = false,
					UseProxyHostname = false,
					UseThenPort = true,
					UseThenHostname = true,
					ExpectedHostname = thenHostname,
					ExpectedPort = (int?)thenPort
				}
			};

			foreach (var testCase in testCases)
			{
				ThenConnect then = new ThenConnect();

				mockEventInfo.Reset();
				mockProxyConnection.Reset();
				mockProxyInfo.Reset();

				mockEventInfo.Setup(mock => mock.ProxyConnection).Returns(proxyConnection);
				mockEventInfo.Setup(mock => mock.Direction).Returns(testCase.DataDirection);
				mockProxyConnection.Setup(mock => mock.ProxyInfo).Returns(proxyInfo);
				mockProxyConnection.Setup(mock => mock.HasTargetConnection).Returns(testCase.HasTargetConnection);
				mockProxyConnection.Setup(mock => mock.InitTargetConnection(It.IsAny<string>(), It.IsAny<int>())).Returns(testCase.InitTargetConnectionSuccess);

                mockProxyInfo.SetupGet(mock => mock.DestinationHost).Returns(testCase.UseProxyHostname ? proxyHostname : null);
                mockProxyInfo.SetupGet(mock => mock.DestinationPort).Returns(testCase.UseProxyPort ? proxyPort : (int?)null);

				then.OverrideCurrentConnection = testCase.OverrideCurrentConnection;
				then.DestinationHost = testCase.UseThenHostname ? thenHostnameString : null;
				then.DestinationPort = testCase.UseThenPort ? thenPortString : null;

				Assert.AreEqual(testCase.ExpectedThenResponse, then.Perform(eventInfo));

				if (testCase.InitTargetConnectionCalled)
				{
					mockProxyConnection.Verify(mock => mock.InitTargetConnection(testCase.ExpectedHostname, testCase.ExpectedPort.GetValueOrDefault()), Times.Once);
				}
				else
				{
					mockProxyConnection.Verify(mock => mock.InitTargetConnection(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
				}
			}
		}
	}
}
