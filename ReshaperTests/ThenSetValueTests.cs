using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReshaperCore.Messages;
using ReshaperCore.Messages.Entities;
using ReshaperCore.Messages.Entities.Http;
using ReshaperCore.Rules;
using ReshaperCore.Rules.Thens;
using ReshaperCore.Settings;
using ReshaperCore.Utils;
using ReshaperCore.Vars;

namespace ReshaperTests
{
	[TestClass]
	public class ThenSetValueTests
	{
		[TestInitialize]
		public void Setup()
		{
			Mock<IGeneralSettings> mockGeneralSettings = new Mock<IGeneralSettings>();
			IGeneralSettings generalSetting = mockGeneralSettings.Object;
			Singleton<IGeneralSettings>.Instance = generalSetting;

			mockGeneralSettings.SetupGet(mock => mock.AutoUpdateContentLength).Returns(false);
		}

		[TestCleanup]
		public void Teardown()
		{
			Singleton<IGeneralSettings>.Instance = null;
		}

		[TestMethod]
		public void ThenSetValue_Perform_SetDataDirectionTest()
		{
			DataDirection direction = DataDirection.Target;

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<VariableString> mockToTargetTextString = new Mock<VariableString>(It.IsAny<string>(), null);

			EventInfo eventInfo = mockEventInfo.Object;
			VariableString toTargetTextString = mockToTargetTextString.Object;

			mockToTargetTextString.Setup(mock => mock.GetText(It.IsAny<Variables>())).Returns(direction.ToString());

			ThenSetValue then = new ThenSetValue()
			{
				Text = toTargetTextString,
				DestinationMessageValue = MessageValue.DataDirection
			};
			Assert.AreEqual(ThenResponse.Continue, then.Perform(eventInfo));

			mockEventInfo.VerifySet(mock => mock.Direction = DataDirection.Target, Times.Once);
		}

		[TestMethod]
		public void ThenSetValue_Perform_SetHttpHeaderTest()
		{
			string identifierValue = "Cookie";
			string headerValue = "value";

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<HttpMessage> mockHttpMessage = new Mock<HttpMessage>();
			Mock<HttpHeaders> mockHttpHeaders = new Mock<HttpHeaders>();
			Mock<VariableString> mockTextString = new Mock<VariableString>(It.IsAny<string>(), null);
			Mock<VariableString> mockIdentifierString = new Mock<VariableString>(It.IsAny<string>(), null);

			EventInfo eventInfo = mockEventInfo.Object;
			HttpMessage httpMessage = mockHttpMessage.Object;
			HttpHeaders httpHeaders = mockHttpHeaders.Object;
			VariableString textString = mockTextString.Object;
			VariableString identifierString = mockIdentifierString.Object;

			mockTextString.Setup(mock => mock.GetText(It.IsAny<Variables>())).Returns(headerValue);
			mockEventInfo.Setup(mock => mock.Message).Returns(httpMessage);
			mockHttpMessage.Setup(mock => mock.Headers).Returns(httpHeaders);
			mockHttpHeaders.SetupSet(mock => mock[identifierValue] = It.IsAny<string>());
			mockIdentifierString.Setup(mock => mock.GetText(It.IsAny<Variables>())).Returns(identifierValue);

			ThenSetValue then = new ThenSetValue()
			{
				Text = textString,
				DestinationIdentifier = identifierString,
				DestinationMessageValue = MessageValue.HttpHeader
			};
			Assert.AreEqual(ThenResponse.Continue, then.Perform(eventInfo));

			mockHttpHeaders.VerifySet(mock => mock[identifierValue] = headerValue, Times.Once);
		}

		[TestMethod]
		public void ThenSetValue_Perform_SetHttpBodyTest()
		{
			string textValue = "value";

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<HttpMessage> mockHttpMessage = new Mock<HttpMessage>();
			Mock<HttpBody> mockHttpBody = new Mock<HttpBody>();
			Mock<VariableString> mockTextString = new Mock<VariableString>(It.IsAny<string>(), null);

			EventInfo eventInfo = mockEventInfo.Object;
			HttpMessage httpMessage = mockHttpMessage.Object;
			HttpBody httpBody = mockHttpBody.Object;
			VariableString textString = mockTextString.Object;

			mockTextString.Setup(mock => mock.GetText(It.IsAny<Variables>())).Returns(textValue);
			mockEventInfo.Setup(mock => mock.Message).Returns(httpMessage);
			mockHttpMessage.SetupSet(mock => mock.Body = It.IsAny<HttpBody>()).Callback((HttpBody body) =>
			{
				Assert.AreEqual(textValue, body.Text);
			});

			ThenSetValue then = new ThenSetValue()
			{
				Text = textString,
				DestinationMessageValue = MessageValue.HttpBody
			};
			Assert.AreEqual(ThenResponse.Continue, then.Perform(eventInfo));

			mockHttpMessage.VerifySet(mock => mock.Body = It.IsAny<HttpBody>(), Times.Once);
		}

		[TestMethod]
		public void ThenSetValue_Perform_SetHttpHeadersTest()
		{
			string headersValue = "Cookie: Blah\nHost: www.google.com";

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<HttpMessage> mockHttpMessage = new Mock<HttpMessage>();
			Mock<VariableString> mockTextString = new Mock<VariableString>(It.IsAny<string>(), null);

			EventInfo eventInfo = mockEventInfo.Object;
			HttpMessage httpMessage = mockHttpMessage.Object;
			VariableString textString = mockTextString.Object;

			mockTextString.Setup(mock => mock.GetText(It.IsAny<Variables>())).Returns(headersValue);
			mockEventInfo.Setup(mock => mock.Message).Returns(httpMessage);
			mockHttpMessage.SetupSet(mock => mock.Headers = It.IsAny<HttpHeaders>()).Callback((HttpHeaders httpHeaders) =>
			{
				Assert.AreEqual(headersValue, httpHeaders.ToString());
			});

			ThenSetValue then = new ThenSetValue()
			{
				Text = textString,
				DestinationMessageValue = MessageValue.HttpHeaders
			};
			Assert.AreEqual(ThenResponse.Continue, then.Perform(eventInfo));

			mockHttpMessage.VerifySet(mock => mock.Headers = It.IsAny<HttpHeaders>(), Times.Once);
		}

		[TestMethod]
		public void ThenSetValue_Perform_SetHttpStatusLineTest()
		{
			string textValue = "GET https://www.telerik.com/UpdateCheck.aspx?isBeta=False HTTP/1.1";

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<HttpMessage> mockHttpMessage = new Mock<HttpMessage>();
			Mock<HttpStatusLine> mockHttpStatusLine = new Mock<HttpStatusLine>();
			Mock<VariableString> mockTextString = new Mock<VariableString>(It.IsAny<string>(), null);

			EventInfo eventInfo = mockEventInfo.Object;
			HttpMessage httpMessage = mockHttpMessage.Object;
			HttpStatusLine httpStatusLine = mockHttpStatusLine.Object;
			VariableString textString = mockTextString.Object;

			mockTextString.Setup(mock => mock.GetText(It.IsAny<Variables>())).Returns(textValue);
			mockEventInfo.Setup(mock => mock.Message).Returns(httpMessage);
			mockHttpMessage.SetupSet(mock => mock.StatusLine = It.IsAny<HttpStatusLine>()).Callback((HttpStatusLine statusLine) =>
			{
				Assert.AreEqual(textValue, statusLine.ToString());
			});

			ThenSetValue then = new ThenSetValue()
			{
				Text = textString,
				DestinationMessageValue = MessageValue.HttpStatusLine
			};
			Assert.AreEqual(ThenResponse.Continue, then.Perform(eventInfo));

			mockHttpMessage.VerifySet(mock => mock.StatusLine = It.IsAny<HttpStatusLine>(), Times.Once);
		}

		[TestMethod]
		public void ThenSetValue_Perform_SetHttpMethodTest()
		{
			string textValue = "GET";

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<HttpMessage> mockHttpMessage = new Mock<HttpMessage>();
			Mock<HttpRequestStatusLine> mockHttpStatusLine = new Mock<HttpRequestStatusLine>();
			Mock<VariableString> mockTextString = new Mock<VariableString>(It.IsAny<string>(), null);

			EventInfo eventInfo = mockEventInfo.Object;
			HttpMessage httpMessage = mockHttpMessage.Object;
			HttpRequestStatusLine httpStatusLine = mockHttpStatusLine.Object;
			VariableString textString = mockTextString.Object;

			mockTextString.Setup(mock => mock.GetText(It.IsAny<Variables>())).Returns(textValue);
			mockEventInfo.Setup(mock => mock.Message).Returns(httpMessage);
			mockHttpMessage.Setup(mock => mock.StatusLine).Returns(httpStatusLine);

			ThenSetValue then = new ThenSetValue()
			{
				Text = textString,
				DestinationMessageValue = MessageValue.HttpMethod
			};
			Assert.AreEqual(ThenResponse.Continue, then.Perform(eventInfo));

			mockHttpStatusLine.VerifySet(mock => mock.Method = textValue);
		}

		[TestMethod]
		public void ThenSetValue_Perform_SetHttpRequestUriTest()
		{
			string textValue = "/Path";

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<HttpMessage> mockHttpMessage = new Mock<HttpMessage>();
			Mock<HttpRequestStatusLine> mockHttpStatusLine = new Mock<HttpRequestStatusLine>();
			Mock<VariableString> mockTextString = new Mock<VariableString>(It.IsAny<string>(), null);

			EventInfo eventInfo = mockEventInfo.Object;
			HttpMessage httpMessage = mockHttpMessage.Object;
			HttpRequestStatusLine httpStatusLine = mockHttpStatusLine.Object;
			VariableString textString = mockTextString.Object;

			mockTextString.Setup(mock => mock.GetText(It.IsAny<Variables>())).Returns(textValue);
			mockEventInfo.Setup(mock => mock.Message).Returns(httpMessage);
			mockHttpMessage.Setup(mock => mock.StatusLine).Returns(httpStatusLine);


			ThenSetValue then = new ThenSetValue()
			{
				Text = textString,
				DestinationMessageValue = MessageValue.HttpRequestUri
			};
			Assert.AreEqual(ThenResponse.Continue, then.Perform(eventInfo));

			mockHttpStatusLine.VerifySet(mock => mock.Uri = textValue);
		}

		[TestMethod]
		public void ThenSetValue_Perform_SetHttpStatusMessageTest()
		{
			string textValue = "Failed";

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<HttpMessage> mockHttpMessage = new Mock<HttpMessage>();
			Mock<HttpResponseStatusLine> mockHttpStatusLine = new Mock<HttpResponseStatusLine>();
			Mock<VariableString> mockTextString = new Mock<VariableString>(It.IsAny<string>(), null);

			EventInfo eventInfo = mockEventInfo.Object;
			HttpMessage httpMessage = mockHttpMessage.Object;
			HttpResponseStatusLine httpStatusLine = mockHttpStatusLine.Object;
			VariableString textString = mockTextString.Object;

			mockTextString.Setup(mock => mock.GetText(It.IsAny<Variables>())).Returns(textValue);
			mockEventInfo.Setup(mock => mock.Message).Returns(httpMessage);
			mockHttpMessage.Setup(mock => mock.StatusLine).Returns(httpStatusLine);

			ThenSetValue then = new ThenSetValue()
			{
				Text = textString,
				DestinationMessageValue = MessageValue.HttpStatusMessage
			};
			Assert.AreEqual(ThenResponse.Continue, then.Perform(eventInfo));

			mockHttpStatusLine.VerifySet(mock => mock.StatusMessage = textValue, Times.Once);
		}

		[TestMethod]
		public void ThenSetValue_Perform_SetHttpStatusCodeTest()
		{
			string textValue = "90";

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<HttpMessage> mockHttpMessage = new Mock<HttpMessage>();
			Mock<HttpResponseStatusLine> mockHttpStatusLine = new Mock<HttpResponseStatusLine>();
			Mock<VariableString> mockNumString = new Mock<VariableString>(It.IsAny<string>(), null);

			EventInfo eventInfo = mockEventInfo.Object;
			HttpMessage httpMessage = mockHttpMessage.Object;
			HttpResponseStatusLine httpStatusLine = mockHttpStatusLine.Object;
			VariableString numString = mockNumString.Object;

			mockNumString.Setup(mock => mock.GetText(It.IsAny<Variables>())).Returns(textValue);
			mockEventInfo.Setup(mock => mock.Message).Returns(httpMessage);
			mockHttpMessage.Setup(mock => mock.StatusLine).Returns(httpStatusLine);

			ThenSetValue then = new ThenSetValue()
			{
				Text = numString,
				DestinationMessageValue = MessageValue.HttpStatusCode
			};
			Assert.AreEqual(ThenResponse.Continue, then.Perform(eventInfo));

			mockHttpStatusLine.VerifySet(mock => mock.StatusCode = int.Parse(textValue), Times.Once);
		}

		[TestMethod]
		public void ThenSetValue_Perform_SetHttpVersionTest()
		{
			string textValue = "Http/2";

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<HttpMessage> mockHttpMessage = new Mock<HttpMessage>();
			Mock<HttpStatusLine> mockHttpStatusLine = new Mock<HttpStatusLine>();
			Mock<VariableString> mockTextString = new Mock<VariableString>(It.IsAny<string>(), null);

			EventInfo eventInfo = mockEventInfo.Object;
			HttpMessage httpMessage = mockHttpMessage.Object;
			HttpStatusLine httpStatusLine = mockHttpStatusLine.Object;
			VariableString textString = mockTextString.Object;

			mockTextString.Setup(mock => mock.GetText(It.IsAny<Variables>())).Returns(textValue);
			mockEventInfo.Setup(mock => mock.Message).Returns(httpMessage);
			mockHttpMessage.Setup(mock => mock.StatusLine).Returns(httpStatusLine);
			mockHttpStatusLine.SetupSet(mock => mock.Version = It.IsAny<string>());

			ThenSetValue then = new ThenSetValue()
			{
				Text = textString,
				DestinationMessageValue = MessageValue.HttpVersion
			};
			Assert.AreEqual(ThenResponse.Continue, then.Perform(eventInfo));


			mockHttpStatusLine.VerifySet(mock => mock.Version = It.IsAny<string>(), Times.Once);
		}

		[TestMethod]
		public void ThenSetValue_Perform_SetHttpMessageTest()
		{
			string httpTextValue = File.ReadAllText(@"TestFiles\HttpTestRequest.txt");

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<HttpMessage> mockHttpMessage = new Mock<HttpMessage>();
			Mock<VariableString> mockHttpTextString = new Mock<VariableString>(It.IsAny<string>(), null);

			EventInfo eventInfo = mockEventInfo.Object;
			HttpMessage httpMessage = mockHttpMessage.Object;
			VariableString httpTextString = mockHttpTextString.Object;

			mockHttpTextString.Setup(mock => mock.GetText(It.IsAny<Variables>())).Returns(httpTextValue);
			mockEventInfo.SetupGet(mock => mock.Message).Returns(httpMessage);
			mockEventInfo.SetupSet(mock => mock.Message = It.IsAny<Message>()).Callback((Message message) =>
			{
				Assert.AreEqual(httpTextValue, message.ToString());
			});

			ThenSetValue then = new ThenSetValue()
			{
				Text = httpTextString,
				DestinationMessageValue = MessageValue.Message
			};
			Assert.AreEqual(ThenResponse.Continue, then.Perform(eventInfo));

			mockEventInfo.VerifySet(mock => mock.Message = It.IsAny<Message>(), Times.Once);
		}

		[TestMethod]
		public void ThenSetValue_Perform_SetMessageTest()
		{
			string textValue = "Blah blah blah";

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<Message> mockMessage = new Mock<Message>();
			Mock<VariableString> mockTextString = new Mock<VariableString>(It.IsAny<string>(), null);

			EventInfo eventInfo = mockEventInfo.Object;
			Message message = mockMessage.Object;
			VariableString textString = mockTextString.Object;

			mockTextString.Setup(mock => mock.GetText(It.IsAny<Variables>())).Returns(textValue);
			mockEventInfo.Setup(mock => mock.Message).Returns(message);

			ThenSetValue then = new ThenSetValue()
			{
				Text = textString,
				DestinationMessageValue = MessageValue.Message
			};
			Assert.AreEqual(ThenResponse.Continue, then.Perform(eventInfo));

			mockMessage.VerifySet(mock => mock.RawText = textValue, Times.Once);
			mockMessage.Verify(mock => mock.ResetCheckedEntity(HttpMessage.EntityFlag), Times.Once);
		}
	}
}
