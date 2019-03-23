using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReshaperCore.Rules;
using ReshaperCore.Rules.Whens;

namespace ReshaperTests
{
	[TestClass]
	public class WhenEventTypeTests
	{
		[TestMethod]
		public void WhenEventType_IsMatchTest()
		{
			var testCases = new[]
			{
				new
				{
					InputEventInfo = new EventInfo()
					{
						Type = EventType.Connected
					},
					Type = EventType.Connected,
					WillMatch = true
				},
				new
				{
					InputEventInfo = new EventInfo()
					{
						Type = EventType.Connected
					},
					Type = EventType.Disconnected,
					WillMatch = false
				},
				new
				{
					InputEventInfo = new EventInfo()
					{
						Type = EventType.Message
					},
					Type = EventType.Message,
					WillMatch = true
				}
			};

			foreach (var testCase in testCases)
			{
				WhenEventType when = new WhenEventType()
				{
					Type = testCase.Type
				};
				Assert.AreEqual(testCase.WillMatch, when.IsMatch(testCase.InputEventInfo));
			}
		}
	}
}
