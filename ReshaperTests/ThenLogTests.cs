using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReshaperCore.Rules;
using ReshaperCore.Rules.Thens;
using ReshaperCore.Utils;
using ReshaperCore.Vars;

namespace ReshaperTests
{
	[TestClass]
	public class ThenLogTests
	{
		[TestMethod]
		public void ThenLog_PerformTest()
		{
			string textValue = "log value";

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<VariableString> mockTextString = new Mock<VariableString>(It.IsAny<string>(), null);

			EventInfo eventInfo = mockEventInfo.Object;
			VariableString textString = mockTextString.Object;

			mockTextString.Setup(mock => mock.GetText(It.IsAny<Variables>())).Returns(textValue);

			Log.InfoLogged += (info, extraInfo) =>
			{
				Assert.AreEqual(textValue, info);
			};

			ThenLog then = new ThenLog()
			{
				Text = textString
			};
			Assert.AreEqual(ThenResponse.Continue, then.Perform(eventInfo));
		}
	}
}
