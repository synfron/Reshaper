using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReshaperCore.Rules;
using ReshaperCore.Rules.Thens;

namespace ReshaperTests
{
	[TestClass]
	public class ThenSkipProcessingTests
	{
		[TestMethod]
		public void ThenSkip_PerformTest()
		{
			ThenSkipProcessing then = new ThenSkipProcessing();
			Assert.AreEqual(ThenResponse.BreakRules, then.Perform(null));
		}
	}
}
