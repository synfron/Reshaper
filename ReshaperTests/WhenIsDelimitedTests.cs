using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReshaperCore.Messages;
using ReshaperCore.Rules;
using ReshaperCore.Rules.Whens;

namespace ReshaperTests
{
	[TestClass]
	public class WhenIsDelimitedTests
	{
		[TestMethod]
		public void WhenIsDelimited_IsMatchTest()
		{
			WhenIsDelimited when = new WhenIsDelimited();

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<Message> mockMessage = new Mock<Message>();

			EventInfo eventInfo = mockEventInfo.Object;
			Message message = mockMessage.Object;

			mockEventInfo.Setup(mock => mock.Message).Returns(message);

			Assert.IsFalse(when.IsMatch(eventInfo));

			mockMessage.Setup(mock => mock.Complete).Returns(true);

			Assert.IsTrue(when.IsMatch(eventInfo));
		}
	}
}
