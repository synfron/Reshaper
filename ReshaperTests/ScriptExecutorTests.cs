using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReshaperCore;
using ReshaperCore.Rules;
using ReshaperCore.Utils;
using ReshaperScript;
using ReshaperScript.Core;

namespace ReshaperTests
{
	[TestClass]
	public class ScriptExecutorTests
	{
		private EventInfo eventInfo;
		private List<Script> scripts;

		[TestInitialize]
		public void Setup()
		{
			ScriptsLifetimeManager scriptsLifetimeManager = new ScriptsLifetimeManager();

			scripts = new List<Script>();

			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();
			Mock<Self> mockSelf = new Mock<Self>();
			Mock<ScriptRegistry> mockScriptRegistry = new Mock<ScriptRegistry>();

			eventInfo = mockEventInfo.Object;
			Self self = mockSelf.Object;
			ScriptRegistry scriptRegistry = mockScriptRegistry.Object;

			mockScriptRegistry.Setup(mock => mock.Scripts).Returns(scripts);

			Singleton<ISelf>.Instance = self;
			Singleton<IScriptRegistry>.Instance = scriptRegistry;
		}

		[TestCleanup]
		public void Teardown()
		{
			Singleton<ISelf>.Instance = null;
			Singleton<IScriptRegistry>.Instance = null;
		}

		[TestMethod]
		public void ScriptExecutor_RunCommand_ResponseReturnTest()
		{

			ScriptHandler scriptHandler = new ScriptHandler();
			Assert.AreEqual("Continue", scriptHandler.RunScript(eventInfo, "return Event.Broadcast()"));
		}

		[TestMethod]
		public void ScriptExecutor_RunStaticScriptAndCommand_ResponseReturnTest()
		{

			scripts.Add(new Script { Name = "Test1", Text = "function test() { return 20; }", IsStaticScript = true });

			ScriptHandler scriptHandler = new ScriptHandler();
			Assert.AreEqual("20", scriptHandler.RunScript(eventInfo, "return test()"));
		}
	}
}
