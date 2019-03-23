using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReshaperCore.Messages;
using ReshaperCore.Messages.Entities;
using ReshaperCore.Messages.Entities.Http;
using ReshaperCore.Proxies;
using ReshaperCore.Rules;
using ReshaperCore.Rules.Whens;
using ReshaperCore.Vars;

namespace ReshaperTests
{
	[TestClass]
	public class WhenHasEntityTests
	{
		[TestMethod]
		public void WhenHasEntity_IsMatch_AlwaysTrue()
		{
			List<MessageValue> alwaysTrueMessageValues = new List<MessageValue>()
			{
				MessageValue.DataDirection,
				MessageValue.LocalAddress,
				MessageValue.LocalPort,
				MessageValue.Protocol,
				MessageValue.SourceRemoteAddress,
				MessageValue.SourceRemotePort
			};
			IEnumerable<MessageValue> notAlwaysTrueMessageValues = Enum.GetValues(typeof(MessageValue)).Cast<MessageValue>().Except(alwaysTrueMessageValues);

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<ProxyConnection> mockProxyConnection = new Mock<ProxyConnection>(It.IsAny<ProxyHost>(), It.IsAny<ProxyInfo>(), It.IsAny<TcpClient>());
			Mock<ProxyInfo> mockProxyInfo = new Mock<ProxyInfo>();

			EventInfo eventInfo = mockEventInfo.Object;
			ProxyConnection proxyConnection = mockProxyConnection.Object;
			ProxyInfo proxyInfo = mockProxyInfo.Object;

			mockEventInfo.Setup(mock => mock.ProxyConnection).Returns(proxyConnection);
			mockProxyConnection.Setup(mock => mock.ProxyInfo).Returns(proxyInfo);

			WhenHasEntity when = new WhenHasEntity();

			foreach (MessageValue value in alwaysTrueMessageValues)
			{
				when.MessageValue = value;
				Assert.IsTrue(when.IsMatch(eventInfo));
			}

			foreach (MessageValue value in notAlwaysTrueMessageValues)
			{
				when.MessageValue = value;
				Assert.IsFalse(when.IsMatch(eventInfo));
			}
		}

		[TestMethod]
		public void WhenHasEntity_IsMatch_DestinationRemoteAddressTest()
		{
			WhenHasEntity when = new WhenHasEntity()
			{
				MessageValue = MessageValue.DestinationRemoteAddress
			};

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<ProxyConnection> mockProxyConnection = new Mock<ProxyConnection>(It.IsAny<ProxyHost>(), It.IsAny<ProxyInfo>(), It.IsAny<TcpClient>());

			EventInfo eventInfo = mockEventInfo.Object;
			ProxyConnection proxyConnection = mockProxyConnection.Object;

			mockEventInfo.Setup(mock => mock.ProxyConnection).Returns(proxyConnection);
			mockProxyConnection.Setup(mock => mock.HasTargetConnection).Returns(true);

			Assert.IsTrue(when.IsMatch(eventInfo));

			mockProxyConnection.Reset();

			mockProxyConnection.Setup(mock => mock.HasTargetConnection).Returns(false);

			Assert.IsFalse(when.IsMatch(eventInfo));

		}

		[TestMethod]
		public void WhenHasEntity_IsMatch_DestinationRemotePortTest()
		{
			WhenHasEntity when = new WhenHasEntity()
			{
				MessageValue = MessageValue.DestinationRemotePort
			};

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<ProxyConnection> mockProxyConnection = new Mock<ProxyConnection>(It.IsAny<ProxyHost>(), It.IsAny<ProxyInfo>(), It.IsAny<TcpClient>());

			EventInfo eventInfo = mockEventInfo.Object;
			ProxyConnection proxyConnection = mockProxyConnection.Object;

			mockEventInfo.Setup(mock => mock.ProxyConnection).Returns(proxyConnection);
			mockProxyConnection.Setup(mock => mock.HasTargetConnection).Returns(true);

			Assert.IsTrue(when.IsMatch(eventInfo));

			mockProxyConnection.Reset();

			mockProxyConnection.Setup(mock => mock.HasTargetConnection).Returns(false);

			Assert.IsFalse(when.IsMatch(eventInfo));

		}

		[TestMethod]
		public void WhenHasEntity_IsMatch_HttpBodyTest()
		{
			WhenHasEntity when = new WhenHasEntity()
			{
				MessageValue = MessageValue.HttpBody
			};

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<HttpMessage> mockHttpMessage = new Mock<HttpMessage>();
			Mock<HttpBody> mockHttpBody = new Mock<HttpBody>();

			EventInfo eventInfo = mockEventInfo.Object;
			HttpMessage httpMessage = mockHttpMessage.Object;
			HttpBody httpBody = mockHttpBody.Object;

			mockEventInfo.Setup(mock => mock.Message).Returns(httpMessage);
			mockHttpMessage.Setup(mock => mock.Body).Returns(httpBody);

			Assert.IsTrue(when.IsMatch(eventInfo));

		}

		[TestMethod]
		public void WhenHasEntity_IsMatch_HttpHeadersTest()
		{
			WhenHasEntity when = new WhenHasEntity()
			{
				MessageValue = MessageValue.HttpHeaders
			};

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<HttpMessage> mockHttpMessage = new Mock<HttpMessage>();
			Mock<HttpHeaders> mockHttpHeaders = new Mock<HttpHeaders>();

			EventInfo eventInfo = mockEventInfo.Object;
			HttpMessage httpMessage = mockHttpMessage.Object;
			HttpHeaders httpHeaders = mockHttpHeaders.Object;

			mockEventInfo.Setup(mock => mock.Message).Returns(httpMessage);
			mockHttpMessage.Setup(mock => mock.Headers).Returns(httpHeaders);

			mockEventInfo.Setup(mock => mock.Message).Returns(httpMessage);
			mockHttpMessage.Setup(mock => mock.Headers).Returns(httpHeaders);

			Assert.IsFalse(when.IsMatch(eventInfo));

			mockHttpHeaders.Setup(mock => mock.Count).Returns(1);

			Assert.IsTrue(when.IsMatch(eventInfo));

		}

		[TestMethod]
		public void WhenHasEntity_IsMatch_HttpHeaderTest()
		{
			string expectedKey = "key";

			WhenHasEntity when = new WhenHasEntity()
			{
				MessageValue = MessageValue.HttpHeader
			};

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<HttpMessage> mockHttpMessage = new Mock<HttpMessage>();
			Mock<HttpHeaders> mockHttpHeaders = new Mock<HttpHeaders>();
			Mock<VariableString> mockVariableString = new Mock<VariableString>(It.IsAny<string>(), null);

			EventInfo eventInfo = mockEventInfo.Object;
			HttpMessage httpMessage = mockHttpMessage.Object;
			HttpHeaders httpHeaders = mockHttpHeaders.Object;
			VariableString variableString = mockVariableString.Object;

			mockEventInfo.Setup(mock => mock.Message).Returns(httpMessage);
			mockHttpMessage.Setup(mock => mock.Headers).Returns(httpHeaders);
			mockVariableString.Setup(mock => mock.GetText(It.IsAny<Variables>())).Returns(expectedKey);
			mockHttpHeaders.Setup(mock => mock.Contains(It.IsAny<string>())).Returns(false);

			when.Identifier = variableString;

			Assert.IsFalse(when.IsMatch(eventInfo));

			mockHttpHeaders.Verify(mock => mock.Contains(expectedKey), Times.Once);

			mockHttpHeaders.Reset();

			mockHttpHeaders.Setup(mock => mock.Contains(It.IsAny<string>())).Returns(true);

			Assert.IsTrue(when.IsMatch(eventInfo));

			mockHttpHeaders.Verify(mock => mock.Contains(expectedKey), Times.Once);
		}

		[TestMethod]
		public void WhenHasEntity_IsMatch_HttpMethodTest()
		{
			WhenHasEntity when = new WhenHasEntity()
			{
				MessageValue = MessageValue.HttpMethod
			};

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<HttpMessage> mockHttpMessage = new Mock<HttpMessage>();
			Mock<HttpRequestStatusLine> mockHttpRequestStatusLine = new Mock<HttpRequestStatusLine>();

			EventInfo eventInfo = mockEventInfo.Object;
			HttpMessage httpMessage = mockHttpMessage.Object;
			HttpRequestStatusLine httpRequestStatusLine = mockHttpRequestStatusLine.Object;

			mockEventInfo.Setup(mock => mock.Message).Returns(httpMessage);
			mockHttpMessage.Setup(mock => mock.StatusLine).Returns(httpRequestStatusLine);
			mockHttpRequestStatusLine.Setup(mock => mock.Method).Returns((string)null);

			Assert.IsFalse(when.IsMatch(eventInfo));

			mockHttpRequestStatusLine.Reset();

			mockHttpRequestStatusLine.Setup(mock => mock.Method).Returns("GET");

			Assert.IsTrue(when.IsMatch(eventInfo));

		}

		[TestMethod]
		public void WhenHasEntity_IsMatch_HttpRequestUriTest()
		{
			WhenHasEntity when = new WhenHasEntity()
			{
				MessageValue = MessageValue.HttpRequestUri
			};

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<HttpMessage> mockHttpMessage = new Mock<HttpMessage>();
			Mock<HttpRequestStatusLine> mockHttpRequestStatusLine = new Mock<HttpRequestStatusLine>();

			EventInfo eventInfo = mockEventInfo.Object;
			HttpMessage httpMessage = mockHttpMessage.Object;
			HttpRequestStatusLine httpRequestStatusLine = mockHttpRequestStatusLine.Object;

			mockEventInfo.Setup(mock => mock.Message).Returns(httpMessage);
			mockHttpMessage.Setup(mock => mock.StatusLine).Returns(httpRequestStatusLine);
			mockHttpRequestStatusLine.Setup(mock => mock.Uri).Returns((string)null);

			Assert.IsFalse(when.IsMatch(eventInfo));

			mockHttpRequestStatusLine.Reset();

			mockHttpRequestStatusLine.Setup(mock => mock.Uri).Returns("http");

			Assert.IsTrue(when.IsMatch(eventInfo));

		}

		[TestMethod]
		public void WhenHasEntity_IsMatch_HttpRequestVersionTest()
		{
			WhenHasEntity when = new WhenHasEntity()
			{
				MessageValue = MessageValue.HttpVersion
			};

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<HttpMessage> mockHttpMessage = new Mock<HttpMessage>();
			Mock<HttpRequestStatusLine> mockHttpRequestStatusLine = new Mock<HttpRequestStatusLine>();

			EventInfo eventInfo = mockEventInfo.Object;
			HttpMessage httpMessage = mockHttpMessage.Object;
			HttpRequestStatusLine httpRequestStatusLine = mockHttpRequestStatusLine.Object;

			mockEventInfo.Setup(mock => mock.Message).Returns(httpMessage);
			mockHttpMessage.Setup(mock => mock.StatusLine).Returns(httpRequestStatusLine);
			mockHttpRequestStatusLine.Setup(mock => mock.Uri).Returns((string)null);

			Assert.IsFalse(when.IsMatch(eventInfo));

			mockHttpRequestStatusLine.Reset();

			mockHttpRequestStatusLine.Setup(mock => mock.Version).Returns("http");

			Assert.IsTrue(when.IsMatch(eventInfo));

		}

		[TestMethod]
		public void WhenHasEntity_IsMatch_HttpStatusLineTest()
		{
			WhenHasEntity when = new WhenHasEntity()
			{
				MessageValue = MessageValue.HttpStatusLine
			};

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<HttpMessage> mockHttpMessage = new Mock<HttpMessage>();
			Mock<HttpRequestStatusLine> mockHttpRequestStatusLine = new Mock<HttpRequestStatusLine>();

			EventInfo eventInfo = mockEventInfo.Object;
			HttpMessage httpMessage = mockHttpMessage.Object;
			HttpRequestStatusLine httpRequestStatusLine = mockHttpRequestStatusLine.Object;

			mockEventInfo.Setup(mock => mock.Message).Returns(httpMessage);

			Assert.IsFalse(when.IsMatch(eventInfo));

			mockHttpRequestStatusLine.Reset();

			mockHttpMessage.Setup(mock => mock.StatusLine).Returns(httpRequestStatusLine);

			Assert.IsTrue(when.IsMatch(eventInfo));

		}

		[TestMethod]
		public void WhenHasEntity_IsMatch_HttpStatusCodeTest()
		{
			WhenHasEntity when = new WhenHasEntity()
			{
				MessageValue = MessageValue.HttpStatusCode
			};

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<HttpMessage> mockHttpMessage = new Mock<HttpMessage>();
			Mock<HttpResponseStatusLine> mockHttpResponseStatusLine = new Mock<HttpResponseStatusLine>();

			EventInfo eventInfo = mockEventInfo.Object;
			HttpMessage httpMessage = mockHttpMessage.Object;
			HttpResponseStatusLine httpResponseStatusLine = mockHttpResponseStatusLine.Object;

			mockEventInfo.Setup(mock => mock.Message).Returns(httpMessage);

			Assert.IsFalse(when.IsMatch(eventInfo));

			mockHttpMessage.Setup(mock => mock.StatusLine).Returns(httpResponseStatusLine);
			mockHttpResponseStatusLine.Setup(mock => mock.StatusCode).Returns(65);

			Assert.IsTrue(when.IsMatch(eventInfo));

		}

		[TestMethod]
		public void WhenHasEntity_IsMatch_HttpStatusMessageTest()
		{
			WhenHasEntity when = new WhenHasEntity()
			{
				MessageValue = MessageValue.HttpStatusMessage
			};

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<HttpMessage> mockHttpMessage = new Mock<HttpMessage>();
			Mock<HttpResponseStatusLine> mockHttpResponseStatusLine = new Mock<HttpResponseStatusLine>();

			EventInfo eventInfo = mockEventInfo.Object;
			HttpMessage httpMessage = mockHttpMessage.Object;
			HttpResponseStatusLine httpResponseStatusLine = mockHttpResponseStatusLine.Object;

			mockEventInfo.Setup(mock => mock.Message).Returns(httpMessage);
			mockHttpMessage.Setup(mock => mock.StatusLine).Returns(httpResponseStatusLine);
			mockHttpResponseStatusLine.Setup(mock => mock.StatusMessage).Returns((string)null);

			Assert.IsFalse(when.IsMatch(eventInfo));

			mockHttpResponseStatusLine.Reset();

			mockHttpResponseStatusLine.Setup(mock => mock.StatusMessage).Returns("http");

			Assert.IsTrue(when.IsMatch(eventInfo));

		}
	}
}
