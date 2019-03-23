using System;
using System.Net.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReshaperCore.Messages;
using ReshaperCore.Messages.Entities;
using ReshaperCore.Proxies;
using ReshaperCore.Rules;
using ReshaperCore.Rules.Thens;
using ReshaperCore.Vars;

namespace ReshaperTests
{
	[TestClass]
	public class ThenSetTests : ThenSet
	{
		[TestMethod]
		public void GetReplacementValueTest()
		{

			string messageString = "Hello guy";
			MessageValue messageValue = MessageValue.Protocol;

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<HttpMessage> mockHttpMessage = new Mock<HttpMessage>();
			Mock<VariableString> mockReplacementTextString = new Mock<VariableString>(It.IsAny<string>(), null);
			Mock<VariableString> mockTextString = new Mock<VariableString>(It.IsAny<string>(), null);
			Mock<VariableString> mockRegexPattenString = new Mock<VariableString>(It.IsAny<string>(), null);
			Mock<ProxyInfo> mockProxyInfo = new Mock<ProxyInfo>();
			Mock<ProxyConnection> mockProxyConnection = new Mock<ProxyConnection>(It.IsAny<ProxyHost>(), It.IsAny<ProxyInfo>(), It.IsAny<TcpClient>());

			EventInfo eventInfo = mockEventInfo.Object;
			HttpMessage httpMessage = mockHttpMessage.Object;
			VariableString replacementTextString = mockReplacementTextString.Object;
			VariableString textString = mockTextString.Object;
			VariableString regexPattenString = mockRegexPattenString.Object;
			ProxyConnection proxyConnection = mockProxyConnection.Object;
			ProxyInfo proxyInfo = mockProxyInfo.Object;

			mockReplacementTextString.Setup(mock => mock.GetText(It.IsAny<Variables>())).Returns("$1 girl");
			mockTextString.Setup(mock => mock.GetText(It.IsAny<Variables>())).Returns("Bye guy");
			mockRegexPattenString.Setup(mock => mock.GetText(It.IsAny<Variables>())).Returns(@"(\w+) \w+");
			mockEventInfo.Setup(mock => mock.Message).Returns(httpMessage);
			mockHttpMessage.Setup(mock => mock.Protocol).Returns(messageString);
			mockEventInfo.Setup(mock => mock.ProxyConnection).Returns(proxyConnection);
			mockProxyConnection.Setup(mock => mock.ProxyInfo).Returns(proxyInfo);

			var testCases = new[]
			{
				new
				{
					UseReplace = false,
					UseMessageValue = false,
					ExpectedReturn = "Bye guy"
				},
				new
				{
					UseReplace = true,
					UseMessageValue = false,
					ExpectedReturn = "Bye girl"
				},
				new
				{
					UseReplace = false,
					UseMessageValue = true,
					ExpectedReturn = "Hello guy"
				},
				new
				{
					UseReplace = true,
					UseMessageValue = true,
					ExpectedReturn = "Hello girl"
				}
			};

			foreach (var testCase in testCases)
			{
				SourceMessageValue = messageValue;
				UseMessageValue = testCase.UseMessageValue;
				Text = textString;
				UseReplace = testCase.UseReplace;
				RegexPattern = regexPattenString;
				ReplacementText = replacementTextString;

				Assert.AreEqual(testCase.ExpectedReturn, GetReplacementValue(eventInfo));
			}
		}

		public override ThenResponse Perform(EventInfo eventInfo)
		{
			throw new NotImplementedException();
		}
	}
}
