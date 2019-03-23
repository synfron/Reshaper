using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReshaperCore.Messages;
using ReshaperCore.Rules;
using ReshaperCore.Rules.Whens;

namespace ReshaperTests
{
	[TestClass]
	public class WhenEventDirectionTest
	{
		[TestMethod]
		public void WhenEventDirection_IsMatchTest()
		{
			var testCases = new[]
			{
				new
				{
					InputEventInfo = new EventInfo()
					{
						Direction = DataDirection.Origin
					},
					Direction = DataDirection.Origin,
					WillMatch = true
				},
				new
				{
					InputEventInfo = new EventInfo()
					{
						Direction = DataDirection.Origin
					},
					Direction = DataDirection.Target,
					WillMatch = false
				},
				new
				{
					InputEventInfo = new EventInfo()
					{
						Direction = DataDirection.Target
					},
					Direction = DataDirection.Origin,
					WillMatch = false
				},
				new
				{
					InputEventInfo = new EventInfo()
					{
						Direction = DataDirection.Target
					},
					Direction = DataDirection.Target,
					WillMatch = true
				}
			};

			foreach (var testCase in testCases)
			{

				WhenEventDirection when = new WhenEventDirection()
				{
					Direction = testCase.Direction
				};
				Assert.AreEqual(testCase.WillMatch, when.IsMatch(testCase.InputEventInfo));
			}
		}
	}
}
