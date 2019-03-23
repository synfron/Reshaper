using System.Collections.Generic;
using System.Net;
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
	public class WhenMatchesTextTests
	{
		private VariableString GetMockVariableString(string returnValue)
		{
			Mock<VariableString> mockMatchTextString = new Mock<VariableString>(It.IsAny<string>(), null);
			VariableString matchTextString = mockMatchTextString.Object;
			mockMatchTextString.Setup(mock => mock.GetText(It.IsAny<Variables>())).Returns(returnValue);
			return matchTextString;
		}

		[TestMethod]
		public void WhenMatchesText_IsMatch_MessageValue_SimpleStringTests()
		{
			string sourceString = "TestString";

			List<MessageValue> testableValues = new List<MessageValue>
			{
				MessageValue.DestinationRemoteAddress,
				MessageValue.HttpMethod,
				MessageValue.HttpRequestUri,
				MessageValue.LocalAddress,
				MessageValue.Protocol,
				MessageValue.SourceRemoteAddress,
				MessageValue.HttpBody,
				MessageValue.Message,
				MessageValue.HttpHeaders,
				MessageValue.HttpStatusLine,
				MessageValue.HttpVersion,
				MessageValue.HttpStatusMessage
			};

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<ProxyInfo> mockProxyInfo = new Mock<ProxyInfo>();
			Mock<ProxyConnection> mockProxyConnection = new Mock<ProxyConnection>(It.IsAny<ProxyHost>(), It.IsAny<ProxyInfo>(), It.IsAny<TcpClient>());
			Mock<HttpMessage> mockHttpMessage = new Mock<HttpMessage>();
			Mock<HttpResponseStatusLine> mockHttpResponseStatusLine = new Mock<HttpResponseStatusLine>();
			Mock<HttpRequestStatusLine> mockHttpRequestStatusLine = new Mock<HttpRequestStatusLine>();
			Mock<HttpBody> mockHttpBody = new Mock<HttpBody>();
			Mock<HttpHeaders> mockHttpHeaders = new Mock<HttpHeaders>();
			Mock<Channel> mockChannel = new Mock<Channel>(It.IsAny<TcpClient>());
			Mock<IPEndPoint> mockIPEndPoint = new Mock<IPEndPoint>(It.IsAny<long>(), It.IsAny<int>());
			Mock<IPAddress> mockIPAddress = new Mock<IPAddress>(It.IsAny<long>());

			EventInfo eventInfo = mockEventInfo.Object;
			ProxyConnection proxyConnection = mockProxyConnection.Object;
			ProxyInfo proxyInfo = mockProxyInfo.Object;
			HttpMessage httpMessage = mockHttpMessage.Object;
			HttpResponseStatusLine responseStatusLine = mockHttpResponseStatusLine.Object;
			HttpRequestStatusLine requestStatusLine = mockHttpRequestStatusLine.Object;
			HttpBody httpBody = mockHttpBody.Object;
			HttpHeaders httpHeaders = mockHttpHeaders.Object;
			Channel channel = mockChannel.Object;
			IPEndPoint ipEndpoint = mockIPEndPoint.Object;
			IPAddress ipAddress = mockIPAddress.Object;

			mockEventInfo.Setup(mock => mock.ProxyConnection).Returns(proxyConnection);
			mockEventInfo.Setup(mock => mock.Message).Returns(httpMessage);
			mockProxyConnection.Setup(mock => mock.HasTargetConnection).Returns(true);
			mockProxyConnection.Setup(mock => mock.HasOriginConnection).Returns(true);
			mockProxyConnection.Setup(mock => mock.OriginChannel).Returns(channel);
			mockProxyConnection.Setup(mock => mock.TargetChannel).Returns(channel);
			mockProxyConnection.Setup(mock => mock.ProxyInfo).Returns(proxyInfo);
			mockChannel.Setup(mock => mock.LocalEndpoint).Returns(ipEndpoint);
			mockChannel.Setup(mock => mock.RemoteEndpoint).Returns(ipEndpoint);
			ipEndpoint.Address = ipAddress;
			mockHttpHeaders.Setup(mock => mock.ToString()).Returns(sourceString);
			mockHttpBody.Setup(mock => mock.ToString()).Returns(sourceString);
			mockIPAddress.Setup(mock => mock.ToString()).Returns(sourceString);
			mockHttpResponseStatusLine.Setup(mock => mock.ToString()).Returns(sourceString);
			mockHttpResponseStatusLine.Setup(mock => mock.StatusMessage).Returns(sourceString);
			mockHttpResponseStatusLine.Setup(mock => mock.Version).Returns(sourceString);
			mockHttpRequestStatusLine.Setup(mock => mock.ToString()).Returns(sourceString);
			mockHttpRequestStatusLine.Setup(mock => mock.Method).Returns(sourceString);
			mockHttpRequestStatusLine.Setup(mock => mock.Uri).Returns(sourceString);
			mockHttpRequestStatusLine.Setup(mock => mock.Version).Returns(sourceString);

			var testCases = new[]
			{
				new
				{
					MatchString = GetMockVariableString("Test"),
					MatchType = MatchType.BeginsWith,
					Matches = true
				},
				new
				{
					MatchString = GetMockVariableString("est"),
					MatchType = MatchType.Contains,
					Matches = true
				},
				new
				{
					MatchString = GetMockVariableString("ing"),
					MatchType = MatchType.EndsWith,
					Matches = true
				},
				new
				{
					MatchString = GetMockVariableString("TestString"),
					MatchType = MatchType.Equals,
					Matches = true
				},
				new
				{
					MatchString = GetMockVariableString(".*Str.*"),
					MatchType = MatchType.Regex,
					Matches = true
				},
				new
				{
					MatchString = GetMockVariableString("MatchString"),
					MatchType = MatchType.BeginsWith,
					Matches = false
				},
				new
				{
					MatchString = GetMockVariableString("MatchString"),
					MatchType = MatchType.Contains,
					Matches = false
				},
				new
				{
					MatchString = GetMockVariableString("MatchString"),
					MatchType = MatchType.EndsWith,
					Matches = false
				},
				new
				{
					MatchString = GetMockVariableString("MatchString"),
					MatchType = MatchType.Equals,
					Matches = false
				},
				new
				{
					MatchString = GetMockVariableString("MatchString"),
					MatchType = MatchType.Regex,
					Matches = false
				}
			};

			foreach (MessageValue messageValue in testableValues)
			{
				mockHttpMessage.Reset();

				switch (messageValue)
				{
					case MessageValue.HttpStatusLine:
					case MessageValue.HttpVersion:
					case MessageValue.HttpStatusMessage:
						mockHttpMessage.Setup(mock => mock.Body).Returns(httpBody);
						mockHttpMessage.Setup(mock => mock.Headers).Returns(httpHeaders);
						mockHttpMessage.Setup(mock => mock.RawText).Returns(sourceString);
						mockHttpMessage.Setup(mock => mock.Protocol).Returns(sourceString);
						mockHttpMessage.Setup(mock => mock.ToString()).Returns(sourceString);
						mockHttpMessage.Setup(mock => mock.StatusLine).Returns(responseStatusLine);
						break;
					default:
						mockHttpMessage.Setup(mock => mock.Body).Returns(httpBody);
						mockHttpMessage.Setup(mock => mock.Headers).Returns(httpHeaders);
						mockHttpMessage.Setup(mock => mock.RawText).Returns(sourceString);
						mockHttpMessage.Setup(mock => mock.Protocol).Returns(sourceString);
						mockHttpMessage.Setup(mock => mock.ToString()).Returns(sourceString);
						mockHttpMessage.Setup(mock => mock.StatusLine).Returns(requestStatusLine);
						break;
				}

				foreach (var testCase in testCases)
				{
					WhenMatchesText when = new WhenMatchesText()
					{
						MessageValue = messageValue,
						UseMessageValue = true,
						MatchText = testCase.MatchString,
						MatchType = testCase.MatchType
					};

					Assert.AreEqual(testCase.Matches, when.IsMatch(eventInfo));
				}
			}
		}

		[TestMethod]
		public void WhenMatchesText_IsMatch_MessageValue_IntTests()
		{
			int sourceValue = 40494;

			List<MessageValue> testableValues = new List<MessageValue>
			{
				MessageValue.DestinationRemotePort,
				MessageValue.SourceRemotePort,
				MessageValue.LocalPort,
				MessageValue.HttpStatusCode
			};

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<HttpMessage> mockHttpMessage = new Mock<HttpMessage>();
			Mock<ProxyConnection> mockProxyConnection = new Mock<ProxyConnection>(It.IsAny<ProxyHost>(), It.IsAny<ProxyInfo>(), It.IsAny<TcpClient>());
			Mock<ProxyInfo> mockProxyInfo = new Mock<ProxyInfo>();
			Mock<Channel> mockChannel = new Mock<Channel>(It.IsAny<TcpClient>());
			Mock<IPEndPoint> mockIPEndPoint = new Mock<IPEndPoint>(It.IsAny<long>(), It.IsAny<int>());
			Mock<IPAddress> mockIPAddress = new Mock<IPAddress>(It.IsAny<long>());
			Mock<HttpResponseStatusLine> mockHttpResponseStatusLine = new Mock<HttpResponseStatusLine>();

			EventInfo eventInfo = mockEventInfo.Object;
			ProxyConnection proxyConnection = mockProxyConnection.Object;
			ProxyInfo proxyInfo = mockProxyInfo.Object;
			HttpMessage httpMessage = mockHttpMessage.Object;
			HttpResponseStatusLine responseStatusLine = mockHttpResponseStatusLine.Object;
			Channel channel = mockChannel.Object;
			IPEndPoint ipEndpoint = mockIPEndPoint.Object;
			IPAddress ipAddress = mockIPAddress.Object;

			mockEventInfo.Setup(mock => mock.ProxyConnection).Returns(proxyConnection);
			mockEventInfo.Setup(mock => mock.Message).Returns(httpMessage);
			mockProxyConnection.Setup(mock => mock.HasTargetConnection).Returns(true);
			mockProxyConnection.Setup(mock => mock.OriginChannel).Returns(channel);
			mockProxyConnection.Setup(mock => mock.TargetChannel).Returns(channel);
			mockProxyConnection.Setup(mock => mock.ProxyInfo).Returns(proxyInfo);
			mockChannel.Setup(mock => mock.LocalEndpoint).Returns(ipEndpoint);
			mockChannel.Setup(mock => mock.RemoteEndpoint).Returns(ipEndpoint);
			mockHttpMessage.Setup(mock => mock.StatusLine).Returns(responseStatusLine);
			ipEndpoint.Port = sourceValue;
			mockHttpResponseStatusLine.Setup(mock => mock.StatusCode).Returns(sourceValue);

			var testCases = new[]
			{
				new
				{
					MatchString = GetMockVariableString("40"),
					MatchType = MatchType.BeginsWith,
					Matches = true
				},
				new
				{
					MatchString = GetMockVariableString("49"),
					MatchType = MatchType.Contains,
					Matches = true
				},
				new
				{
					MatchString = GetMockVariableString("94"),
					MatchType = MatchType.EndsWith,
					Matches = true
				},
				new
				{
					MatchString = GetMockVariableString("40494"),
					MatchType = MatchType.Equals,
					Matches = true
				},
				new
				{
					MatchString = GetMockVariableString(".*49.*"),
					MatchType = MatchType.Regex,
					Matches = true
				},
				new
				{
					MatchString = GetMockVariableString("65"),
					MatchType = MatchType.BeginsWith,
					Matches = false
				},
				new
				{
					MatchString = GetMockVariableString("65"),
					MatchType = MatchType.Contains,
					Matches = false
				},
				new
				{
					MatchString = GetMockVariableString("65"),
					MatchType = MatchType.EndsWith,
					Matches = false
				},
				new
				{
					MatchString = GetMockVariableString("65"),
					MatchType = MatchType.Equals,
					Matches = false
				},
				new
				{
					MatchString = GetMockVariableString("65"),
					MatchType = MatchType.Regex,
					Matches = false
				}
			};

			foreach (MessageValue messageValue in testableValues)
			{
				foreach (var testCase in testCases)
				{
					WhenMatchesText when = new WhenMatchesText()
					{
						MessageValue = messageValue,
						UseMessageValue = true,
						MatchText = testCase.MatchString,
						MatchType = testCase.MatchType
					};

					Assert.AreEqual(testCase.Matches, when.IsMatch(eventInfo));
				}
			}
		}

		[TestMethod]
		public void WhenMatchesText_IsMatch_MessageValue_WithIdentifierTests()
		{
			string sourceString = "TestString";
			string identifier = "Ident";

			List<MessageValue> testableValues = new List<MessageValue>
			{
				MessageValue.HttpHeader
			};

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<ProxyInfo> mockProxyInfo = new Mock<ProxyInfo>();
			Mock<ProxyConnection> mockProxyConnection = new Mock<ProxyConnection>(It.IsAny<ProxyHost>(), It.IsAny<ProxyInfo>(), It.IsAny<TcpClient>());
			Mock<HttpMessage> mockHttpMessage = new Mock<HttpMessage>();
			Mock<HttpHeaders> mockHttpHeaders = new Mock<HttpHeaders>();
			Mock<VariableString> mockIdentiferString = new Mock<VariableString>(It.IsAny<string>(), null);

			EventInfo eventInfo = mockEventInfo.Object;
			ProxyConnection proxyConnection = mockProxyConnection.Object;
			ProxyInfo proxyInfo = mockProxyInfo.Object;
			HttpMessage httpMessage = mockHttpMessage.Object;
			HttpHeaders httpHeaders = mockHttpHeaders.Object;
			VariableString identiferString = mockIdentiferString.Object;

			mockEventInfo.Setup(mock => mock.ProxyConnection).Returns(proxyConnection);
			mockEventInfo.Setup(mock => mock.Message).Returns(httpMessage);
			mockProxyConnection.Setup(mock => mock.ProxyInfo).Returns(proxyInfo);
			mockHttpHeaders.Setup(mock => mock.GetOrDefault(identifier)).Returns(sourceString);
			mockIdentiferString.Setup(mock => mock.GetText(It.IsAny<Variables>())).Returns(identifier);

			var testCases = new[]
			{
				new
				{
					MatchString = GetMockVariableString("Test"),
					MatchType = MatchType.BeginsWith,
					Matches = true
				},
				new
				{
					MatchString = GetMockVariableString("est"),
					MatchType = MatchType.Contains,
					Matches = true
				},
				new
				{
					MatchString = GetMockVariableString("ing"),
					MatchType = MatchType.EndsWith,
					Matches = true
				},
				new
				{
					MatchString = GetMockVariableString("TestString"),
					MatchType = MatchType.Equals,
					Matches = true
				},
				new
				{
					MatchString = GetMockVariableString(".*Str.*"),
					MatchType = MatchType.Regex,
					Matches = true
				},
				new
				{
					MatchString = GetMockVariableString("MatchString"),
					MatchType = MatchType.BeginsWith,
					Matches = false
				},
				new
				{
					MatchString = GetMockVariableString("MatchString"),
					MatchType = MatchType.Contains,
					Matches = false
				},
				new
				{
					MatchString = GetMockVariableString("MatchString"),
					MatchType = MatchType.EndsWith,
					Matches = false
				},
				new
				{
					MatchString = GetMockVariableString("MatchString"),
					MatchType = MatchType.Equals,
					Matches = false
				},
				new
				{
					MatchString = GetMockVariableString("MatchString"),
					MatchType = MatchType.Regex,
					Matches = false
				}
			};

			foreach (MessageValue messageValue in testableValues)
			{
				mockHttpMessage.Reset();
				mockHttpMessage.Setup(mock => mock.Headers).Returns(httpHeaders);

				foreach (var testCase in testCases)
				{
					WhenMatchesText when = new WhenMatchesText()
					{
						MessageValue = messageValue,
						UseMessageValue = true,
						MatchText = testCase.MatchString,
						MatchType = testCase.MatchType,
						Identifier = identiferString
					};

					Assert.AreEqual(testCase.Matches, when.IsMatch(eventInfo));
				}
			}
		}

		[TestMethod]
		public void WhenMatchesText_IsMatch_NoMessageValueTests()
		{
			string sourceString = "TestString";

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<VariableString> mockSourceTextString = new Mock<VariableString>(It.IsAny<string>(), null);

			EventInfo eventInfo = mockEventInfo.Object;
			VariableString sourceTextString = mockSourceTextString.Object;

			mockSourceTextString.Setup(mock => mock.GetText(It.IsAny<Variables>())).Returns(sourceString);

			var testCases = new[]
			{
				new
				{
					MatchString = GetMockVariableString("Test"),
					MatchType = MatchType.BeginsWith,
					Matches = true
				},
				new
				{
					MatchString = GetMockVariableString("est"),
					MatchType = MatchType.Contains,
					Matches = true
				},
				new
				{
					MatchString = GetMockVariableString("ing"),
					MatchType = MatchType.EndsWith,
					Matches = true
				},
				new
				{
					MatchString = GetMockVariableString("TestString"),
					MatchType = MatchType.Equals,
					Matches = true
				},
				new
				{
					MatchString = GetMockVariableString(".*Str.*"),
					MatchType = MatchType.Regex,
					Matches = true
				},
				new
				{
					MatchString = GetMockVariableString("MatchString"),
					MatchType = MatchType.BeginsWith,
					Matches = false
				},
				new
				{
					MatchString = GetMockVariableString("MatchString"),
					MatchType = MatchType.Contains,
					Matches = false
				},
				new
				{
					MatchString = GetMockVariableString("MatchString"),
					MatchType = MatchType.EndsWith,
					Matches = false
				},
				new
				{
					MatchString = GetMockVariableString("MatchString"),
					MatchType = MatchType.Equals,
					Matches = false
				},
				new
				{
					MatchString = GetMockVariableString("MatchString"),
					MatchType = MatchType.Regex,
					Matches = false
				}
			};

			foreach (var testCase in testCases)
			{
				WhenMatchesText when = new WhenMatchesText()
				{
					UseMessageValue = false,
					MatchText = testCase.MatchString,
					MatchType = testCase.MatchType,
					SourceText = sourceTextString
				};

				Assert.AreEqual(testCase.Matches, when.IsMatch(eventInfo));
			}

		}
	}
}
