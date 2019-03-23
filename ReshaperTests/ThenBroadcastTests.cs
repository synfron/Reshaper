using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReshaperCore;
using ReshaperCore.Rules;
using ReshaperCore.Rules.Thens;
using ReshaperCore.Utils;

namespace ReshaperTests
{
	[TestClass]
	public class ThenBroadcastTests
	{
		[TestCleanup]
		public void Teardown()
		{
			Singleton<ISelf>.Instance = null;
		}

		[TestMethod]
		public void ThenBroadcast_PerformTest()
		{
			var testCases = new[]
			{
				new
				{
					InputEventInfo = new EventInfo()
					{
						Type = EventType.Connected
					},
					WillBroadcast = false,
					HitCount = Times.Never()
				},
				new
				{
					InputEventInfo = new EventInfo()
					{
						Type = EventType.Disconnected
					},
					WillBroadcast = false,
					HitCount = Times.Never()
				},
				new
				{
					InputEventInfo = new EventInfo()
					{
						Type = EventType.Message
					},
					WillBroadcast = true,
					HitCount = Times.Once()
				}
			};
			Mock<Self> selfMock = new Mock<Self>() { CallBase = true };
			Self mockedSelf = selfMock.Object;
			Singleton<ISelf>.Instance = mockedSelf;
			ThenBroadcast then = new ThenBroadcast();
			EventInfo resultEventInfo = null;
			mockedSelf.NewEventBroadcasted += (EventInfo eventInfo) => resultEventInfo = eventInfo;


			foreach (var testCase in testCases)
			{
				ThenResponse response = then.Perform(testCase.InputEventInfo);

				if (testCase.WillBroadcast)
				{
					Assert.AreEqual(testCase.InputEventInfo, resultEventInfo);
				}
				else
				{
					Assert.AreEqual(null, resultEventInfo);
				}
				Assert.AreEqual(ThenResponse.Continue, response);
				resultEventInfo = null;
			}
		}
	}
}
