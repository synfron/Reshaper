using System;
using System.Net.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReshaperCore.Proxies;
using ReshaperCore.Rules;
using ReshaperCore.Rules.Whens;

namespace ReshaperTests
{
	[TestClass]
	public class WhenProxyTypeTests
	{
		[TestMethod]
		public void WhenProxyType_IsMatchTest()
		{
			var testCases = new[]
			{
				new
				{
					InputEventInfo = ((Func<EventInfo>)(() =>
					{
						Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
						Mock<ProxyConnection> mockProxyConnection = new Mock<ProxyConnection>(It.IsAny<ProxyHost>(), It.IsAny<ProxyInfo>(), It.IsAny<TcpClient>());
						Mock<ProxyInfo> mockProxyInfo = new Mock<ProxyInfo>();

						EventInfo eventInfo = mockEventInfo.Object;
						ProxyConnection proxyConnection = mockProxyConnection.Object;
						ProxyInfo proxyInfo = mockProxyInfo.Object;

						mockEventInfo.Setup(mock => mock.ProxyConnection).Returns(proxyConnection);
						mockProxyConnection.SetupGet(mock => mock.ProxyInfo).Returns(proxyInfo);
						mockProxyInfo.SetupGet(mock => mock.DataType).Returns(ProxyDataType.Http);
						return eventInfo;
					})).Invoke(),
					Type = ProxyDataType.Http,
					WillMatch = true
				},
				new
				{
					InputEventInfo = ((Func<EventInfo>)(() =>
					{
						Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
						Mock<ProxyConnection> mockProxyConnection = new Mock<ProxyConnection>(It.IsAny<ProxyHost>(), It.IsAny<ProxyInfo>(), It.IsAny<TcpClient>());
						Mock<ProxyInfo> mockProxyInfo = new Mock<ProxyInfo>();

						EventInfo eventInfo = mockEventInfo.Object;
						ProxyConnection proxyConnection = mockProxyConnection.Object;
						ProxyInfo proxyInfo = mockProxyInfo.Object;

						mockEventInfo.Setup(mock => mock.ProxyConnection).Returns(proxyConnection);
						mockProxyConnection.SetupGet(mock => mock.ProxyInfo).Returns(proxyInfo);
						mockProxyInfo.SetupGet(mock => mock.DataType).Returns(ProxyDataType.Text);
						return eventInfo;
					})).Invoke(),
					Type = ProxyDataType.Text,
					WillMatch = true
				},
				new
				{
					InputEventInfo = ((Func<EventInfo>)(() =>
					{
						Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
						Mock<ProxyConnection> mockProxyConnection = new Mock<ProxyConnection>(It.IsAny<ProxyHost>(), It.IsAny<ProxyInfo>(), It.IsAny<TcpClient>());
						Mock<ProxyInfo> mockProxyInfo = new Mock<ProxyInfo>();

						EventInfo eventInfo = mockEventInfo.Object;
						ProxyConnection proxyConnection = mockProxyConnection.Object;
						ProxyInfo proxyInfo = mockProxyInfo.Object;

						mockEventInfo.Setup(mock => mock.ProxyConnection).Returns(proxyConnection);
						mockProxyConnection.SetupGet(mock => mock.ProxyInfo).Returns(proxyInfo);
						mockProxyInfo.SetupGet(mock => mock.DataType).Returns(ProxyDataType.Http);
						return eventInfo;
					})).Invoke(),
					Type = ProxyDataType.Text,
					WillMatch = false
				},
				new
				{
					InputEventInfo = ((Func<EventInfo>)(() =>
					{
						Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
						Mock<ProxyConnection> mockProxyConnection = new Mock<ProxyConnection>(It.IsAny<ProxyHost>(), It.IsAny<ProxyInfo>(), It.IsAny<TcpClient>());
						Mock<ProxyInfo> mockProxyInfo = new Mock<ProxyInfo>();

						EventInfo eventInfo = mockEventInfo.Object;
						ProxyConnection proxyConnection = mockProxyConnection.Object;
						ProxyInfo proxyInfo = mockProxyInfo.Object;

						mockEventInfo.Setup(mock => mock.ProxyConnection).Returns(proxyConnection);
						mockProxyConnection.SetupGet(mock => mock.ProxyInfo).Returns(proxyInfo);
						mockProxyInfo.SetupGet(mock => mock.DataType).Returns(ProxyDataType.Text);
						return eventInfo;
					})).Invoke(),
					Type = ProxyDataType.Http,
					WillMatch = false
				}
			};

			foreach (var testCase in testCases)
			{
				WhenProxyType when = new WhenProxyType()
				{
					ProxyType = testCase.Type
				};
				Assert.AreEqual(testCase.WillMatch, when.IsMatch(testCase.InputEventInfo));
			}
		}
	}
}
