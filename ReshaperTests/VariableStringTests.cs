using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReshaperCore;
using ReshaperCore.Utils;
using ReshaperCore.Vars;

namespace ReshaperTests
{
	[TestClass]
	public class VariableStringTests
	{

		[TestCleanup]
		public void Teardown()
		{
			Singleton<ISelf>.Instance = null;
		}

		[TestMethod]
		public void VariableString_GetFormattedStringTest()
		{
			var testCases = new[]
			{
				new
				{
					FormattedString = "{global:var1}{global:var2}{channel:var1}{channel:var2}{global:var3}{channel:var3}?",
					ExpectedString = "{global:var1}{global:var2}{channel:var1}{channel:var2}{global:var3}{channel:var3}?",
					ExpectException = false
				},
				new
				{
					FormattedString = "{var1}{var2}{channel:var1}{channel:var2}{var3}{channel:var3}?",
					ExpectedString = "{global:var1}{global:var2}{channel:var1}{channel:var2}{global:var3}{channel:var3}?",
					ExpectException = false
				},
				new
				{
					FormattedString = "{{var1}}{{{{var1}}}}{{channel:var1}}{{channel:var1}}{{var1}}{{channel:var1}}?",
					ExpectedString = "{var1}{{var1}}{channel:var1}{channel:var1}{var1}{channel:var1}?",
					ExpectException = false
				},
				new
				{
					FormattedString = "{global:var1}{global:var1}{channel:var1}{local:var1}{global:var1}{channel:var1}?",
					ExpectedString = (string)null,
					ExpectException = true
				},
				new
				{
					FormattedString = "{global:var1?",
					ExpectedString = (string)null,
					ExpectException = true
				},
				new
				{
					FormattedString = "global}:var1?",
					ExpectedString = (string)null,
					ExpectException = true
				},
				new
				{
					FormattedString = "glo{bal{:var1?",
					ExpectedString = (string)null,
					ExpectException = true
				}
			};

			foreach (var testCase in testCases)
			{
				if (testCase.ExpectException)
				{
					try
					{
						VariableString varString = VariableString.GetAsVariableString(testCase.FormattedString);
						Assert.Fail("Expected an exception. No exception thrown");
					}
					catch (FormatException)
					{

					}
				}
				else
				{
					VariableString varString = VariableString.GetAsVariableString(testCase.FormattedString);
					Assert.AreEqual(testCase.ExpectedString, varString.GetFormattedString());
				}
			}

		}

		[TestMethod]
		public void VariableString_GetTextTest()
		{
			Mock<Self> selfMock = new Mock<Self>() { CallBase = true };
			Self mockedSelf = selfMock.Object;
			Singleton<ISelf>.Instance = mockedSelf;

			Variables connectionVariables = new Variables();

			mockedSelf.Variables.Add<string>("var1").Value = "Hello,";
			mockedSelf.Variables.Add<string>("var2").Value = " how";
			connectionVariables.Add<string>("var1").Value = " are";
			connectionVariables.Add<string>("var2").Value = " you";

			VariableString varString1 = VariableString.GetAsVariableString("{global:var1}{global:var2}{channel:var1}{channel:var2}{global:var3}{channel:var3}?");
			VariableString varString2 = VariableString.GetAsVariableString("Hello, how are you?");

			Assert.AreEqual("Hello, how are you?", varString1.GetText(connectionVariables));
			Assert.AreEqual("Hello, how are you?", varString2.GetText(connectionVariables));
		}
	}
}
