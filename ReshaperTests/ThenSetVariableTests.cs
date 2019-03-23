using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReshaperCore;
using ReshaperCore.Rules;
using ReshaperCore.Rules.Thens;
using ReshaperCore.Utils;
using ReshaperCore.Vars;

namespace ReshaperTests
{
	[TestClass]
	public class ThenSetVariableTests
	{
		[TestCleanup]
		public void Teardown()
		{
			Singleton<ISelf>.Instance = null;
		}

		[TestMethod]
		public void ThenSetVariable_PerformTest()
		{
			string newConnValue = "Conn New Value";
			string newGlobValue = "Glo New Value";
			string existingConnValue = "Conn Old Value";
			string existingGlobValue = "Glo Old Value";
			string connExistingVarName = "ConnExistingVar";
			string globExistingVarName = "GlobExistingVar";
			string connNewVarName = "ConnNewVar";
			string globNewVarName = "GlobNewVar";

			Variables connectionVars = new Variables();
			Variables globalVars = new Variables();
			IVariable<string> existingConnVar = connectionVars.Add<string>(connExistingVarName);
			IVariable<string> existingGlobVar = globalVars.Add<string>(globExistingVarName);
			existingConnVar.Value = existingConnValue;
			existingGlobVar.Value = existingGlobValue;


			Mock<VariableString> mockNewConnValueString = new Mock<VariableString>(It.IsAny<string>(), null);
			Mock<VariableString> mockNewGlobValueString = new Mock<VariableString>(It.IsAny<string>(), null);
			Mock<VariableString> mockConnNewVarNameString = new Mock<VariableString>(It.IsAny<string>(), null);
			Mock<VariableString> mockGlobNewVarNameString = new Mock<VariableString>(It.IsAny<string>(), null);
			Mock<VariableString> mockConnExistingVarNameString = new Mock<VariableString>(It.IsAny<string>(), null);
			Mock<VariableString> mockGlobExistingVarNameString = new Mock<VariableString>(It.IsAny<string>(), null);
			Mock<Self> mockSelf = new Mock<Self>();
			Mock<EventInfo> mockEventInfo = new Mock<EventInfo>();

			VariableString newConnValueString = mockNewConnValueString.Object;
			VariableString newGlobValueString = mockNewGlobValueString.Object;
			VariableString connNewVarNameString = mockConnNewVarNameString.Object;
			VariableString globNewVarNameString = mockGlobNewVarNameString.Object;
			VariableString connExistingVarNameString = mockConnExistingVarNameString.Object;
			VariableString globExistingVarNameString = mockGlobExistingVarNameString.Object;
			Self self = mockSelf.Object;
			EventInfo eventInfo = mockEventInfo.Object;

			mockNewConnValueString.Setup(mock => mock.GetText(It.IsAny<Variables>())).Returns(newConnValue);
			mockNewGlobValueString.Setup(mock => mock.GetText(It.IsAny<Variables>())).Returns(newGlobValue);
			mockConnNewVarNameString.Setup(mock => mock.GetText(It.IsAny<Variables>())).Returns(connNewVarName);
			mockGlobNewVarNameString.Setup(mock => mock.GetText(It.IsAny<Variables>())).Returns(globNewVarName);
			mockConnExistingVarNameString.Setup(mock => mock.GetText(It.IsAny<Variables>())).Returns(connExistingVarName);
			mockGlobExistingVarNameString.Setup(mock => mock.GetText(It.IsAny<Variables>())).Returns(globExistingVarName);
			mockSelf.Setup(mock => mock.Variables).Returns(globalVars);
			mockEventInfo.Setup(mock => mock.Variables).Returns(connectionVars);
			
			Singleton<ISelf>.Instance = self;

			{
				ThenSetVariable then = new ThenSetVariable()
				{
					Text = newConnValueString,
					VariableName = connNewVarNameString,
					TargetSource = VariableSource.Channel
				};

				Assert.AreEqual(ThenResponse.Continue, then.Perform(eventInfo));

				IVariable<string> newVar = connectionVars.GetOrDefault<string>(connNewVarName);
				Assert.IsNotNull(newVar);
				Assert.AreEqual(newConnValue, newVar.Value);
			}
			{
				ThenSetVariable then = new ThenSetVariable()
				{
					Text = newGlobValueString,
					VariableName = globNewVarNameString,
					TargetSource = VariableSource.Global
				};

				Assert.AreEqual(ThenResponse.Continue, then.Perform(eventInfo));

				IVariable<string> newVar = globalVars.GetOrDefault<string>(globNewVarName);
				Assert.IsNotNull(newVar);
				Assert.AreEqual(newGlobValue, newVar.Value);
			}
			{
				ThenSetVariable then = new ThenSetVariable()
				{
					Text = newConnValueString,
					VariableName = connExistingVarNameString,
					TargetSource = VariableSource.Channel
				};

				Assert.AreEqual(ThenResponse.Continue, then.Perform(eventInfo));

				Assert.AreEqual(newConnValue, existingConnVar.Value);
			}
			{
				ThenSetVariable then = new ThenSetVariable()
				{
					Text = newGlobValueString,
					VariableName = globExistingVarNameString,
					TargetSource = VariableSource.Global
				};

				Assert.AreEqual(ThenResponse.Continue, then.Perform(eventInfo));

				Assert.AreEqual(newGlobValue, existingGlobVar.Value);
			}
		}
	}
}
