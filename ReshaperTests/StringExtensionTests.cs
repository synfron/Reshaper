using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReshaperCore.Utils.Extensions;

namespace ReshaperTests
{
	[TestClass]
	public class StringExtensionTests
	{
		[TestMethod]
		public void StringExtension_StartsWith_WithoutOutTest()
		{
			var testCases = new[]
			{
				new
				{
					Text = "Auto Actions are a powerful feature that Foo IRC",
					SubString = "are",
					ExpectedFound = true,
					Index = 13

				},
				new
				{
					Text = "allow users to have more control over there IRC",
					SubString = "allow",
					ExpectedFound = true,
					Index = 0
				},
				new
				{
					Text = "allow users to have more control over there IRC",
					SubString = "a",
					ExpectedFound = false,
					Index = 1
				},
				new
				{
					Text = "allow users to have more control over there IRC",
					SubString = "l",
					ExpectedFound = false,
					Index = 0
				},
				new
				{
					Text = "",
					SubString = "l",
					ExpectedFound = false,
					Index = 0
				},
				new
				{
					Text = "users",
					SubString = "users",
					ExpectedFound = true,
					Index = 0
				},
				new
				{
					Text = "user",
					SubString = "users",
					ExpectedFound = false,
					Index = 0
				},
				new
				{
					Text = "allow users to have more control over there IRC",
					SubString = "allos",
					ExpectedFound = false,
					Index = 0
				},
				new
				{
					Text = "allow users to have more control over there IRC",
					SubString = "aliow",
					ExpectedFound = false,
					Index = 0
				}
			};

			foreach (var testCase in testCases)
			{
				Assert.AreEqual(testCase.ExpectedFound, testCase.Text.StartsWith(testCase.SubString, testCase.Index));
			}
		}

		[TestMethod]
		public void StringExtension_StartsWith_WithOutTest()
		{
			var testCases = new[]
			{
				new
				{
					Text = "are a powerful feature that Foo IRC",
					SubStrings = new List<string>
					{
						"Suto",
						"are",
						"Tll"
					},
					ExpectedFound = true,
					ExpectedDelimiterFound = "are"

				},
				new
				{
					Text = "allow users to have more control over there IRC",
					SubStrings = new List<string>
					{
						"Auto",
						"are",
						"allow"
					},
					ExpectedFound = true,
					ExpectedDelimiterFound = "allow"
				},
				new
				{
					Text = "allow users to have more control over there IRC",
					SubStrings = new List<string>
					{
						"a"
					},
					ExpectedFound = true,
					ExpectedDelimiterFound = "a"
				},
				new
				{
					Text = "allow users to have more control over there IRC",
					SubStrings = new List<string>
					{
						"l"
					},
					ExpectedFound = false,
					ExpectedDelimiterFound = (string)null
				},
				new
				{
					Text = "",
					SubStrings = new List<string>
					{
						"l"
					},
					ExpectedFound = false,
					ExpectedDelimiterFound = (string)null
				},
				new
				{
					Text = "users",
					SubStrings = new List<string>
					{
						"users"
					},
					ExpectedFound = true,
					ExpectedDelimiterFound = "users"
				},
				new
				{
					Text = "user",
					SubStrings = new List<string>
					{
						"users"
					},
					ExpectedFound = false,
					ExpectedDelimiterFound = (string)null
				},
				new
				{
					Text = "allow users to have more control over there IRC",
					SubStrings = new List<string>
					{
						"allos"
					},
					ExpectedFound = false,
					ExpectedDelimiterFound = (string)null
				},
				new
				{
					Text = "allow users to have more control over there IRC",
					SubStrings = new List<string>
					{
						"aliow"
					},
					ExpectedFound = false,
					ExpectedDelimiterFound = (string)null
				}
			};

			foreach (var testCase in testCases)
			{
				string foundDelimiter = null;
				bool result = testCase.Text.StartsWith(testCase.SubStrings, out foundDelimiter);
				Assert.AreEqual(testCase.ExpectedFound, result);
				Assert.AreEqual(testCase.ExpectedDelimiterFound, foundDelimiter);
			}
		}

		[TestMethod]
		public void StringExtension_EndsWith_WithOutTest()
		{
			var testCases = new[]
			{
				new
				{
					Text = "are a powerful feature that Foo IRC",
					SubStrings = new List<string>
					{
						"Suto",
						"IRC",
						"Tll"
					},
					ExpectedFound = true,
					ExpectedDelimiterFound = "IRC"

				},
				new
				{
					Text = "allow users to have more control over there",
					SubStrings = new List<string>
					{
						"Auto",
						"are",
						"there"
					},
					ExpectedFound = true,
					ExpectedDelimiterFound = "there"
				},
				new
				{
					Text = "allow users to have more control over there IRC",
					SubStrings = new List<string>
					{
						"C"
					},
					ExpectedFound = true,
					ExpectedDelimiterFound = "C"
				},
				new
				{
					Text = "allow users to have more control over there IRC",
					SubStrings = new List<string>
					{
						"R"
					},
					ExpectedFound = false,
					ExpectedDelimiterFound = (string)null
				},
				new
				{
					Text = "",
					SubStrings = new List<string>
					{
						"l"
					},
					ExpectedFound = false,
					ExpectedDelimiterFound = (string)null
				},
				new
				{
					Text = "users",
					SubStrings = new List<string>
					{
						"users"
					},
					ExpectedFound = true,
					ExpectedDelimiterFound = "users"
				},
				new
				{
					Text = "user",
					SubStrings = new List<string>
					{
						"users"
					},
					ExpectedFound = false,
					ExpectedDelimiterFound = (string)null
				},
				new
				{
					Text = "allow users to have more control over there",
					SubStrings = new List<string>
					{
						"theru"
					},
					ExpectedFound = false,
					ExpectedDelimiterFound = (string)null
				},
				new
				{
					Text = "allow users to have more control over there",
					SubStrings = new List<string>
					{
						"thore"
					},
					ExpectedFound = false,
					ExpectedDelimiterFound = (string)null
				}
			};

			foreach (var testCase in testCases)
			{
				string foundDelimiter = null;
				bool result = testCase.Text.EndsWith(testCase.SubStrings, out foundDelimiter);
				Assert.AreEqual(testCase.ExpectedFound, result);
				Assert.AreEqual(testCase.ExpectedDelimiterFound, foundDelimiter);
			}
		}

		[TestMethod]
		public void StringExtension_SplitWithDelimitersTest()
		{
			var testCases = new[]
			{
				new
				{
					Text = "are a powerful feature that Foo IRC",
					Delimiters = new List<string>
					{
						" ",
						"t"
					},
					ExpectedSplit = new List<Tuple<string, string>>()
					{
						new Tuple<string, string>("are", " "),
						new Tuple<string, string>("a", " "),
						new Tuple<string, string>("powerful", " "),
						new Tuple<string, string>("fea", "t"),
						new Tuple<string, string>("ure", " "),
						new Tuple<string, string>("", "t"),
						new Tuple<string, string>("ha", "t"),
						new Tuple<string, string>("", " "),
						new Tuple<string, string>("Foo", " "),
						new Tuple<string, string>("IRC", null)
					}
				},
				new
				{
					Text = "allow users to have more control over there",
					Delimiters = new List<string>
					{
						"z",
						"e"
					},
					ExpectedSplit = new List<Tuple<string, string>>()
					{
						new Tuple<string, string>("allow us", "e"),
						new Tuple<string, string>("rs to hav", "e"),
						new Tuple<string, string>(" mor", "e"),
						new Tuple<string, string>(" control ov", "e"),
						new Tuple<string, string>("r th", "e"),
						new Tuple<string, string>("r", "e")
					}
				},
				new
				{
					Text = "allow users to have more control over there",
					Delimiters = new List<string>
					{
						"er"
					},
					ExpectedSplit = new List<Tuple<string, string>>()
					{
						new Tuple<string, string>("allow us", "er"),
						new Tuple<string, string>("s to have more control ov", "er"),
						new Tuple<string, string>(" th", "er"),
						new Tuple<string, string>("e", null)
					}
				},
				new
				{
					Text = "allow users to have more control over there",
					Delimiters = new List<string>
					{
						";"
					},
					ExpectedSplit = new List<Tuple<string, string>>()
					{
						new Tuple<string, string>("allow users to have more control over there", null)
					}
				},
				new
				{
					Text = "",
					Delimiters = new List<string>
					{
						";"
					},
					ExpectedSplit = new List<Tuple<string, string>>()
				},
				new
				{
					Text = "a",
					Delimiters = new List<string>
					{
						";"
					},
					ExpectedSplit = new List<Tuple<string, string>>()
					{
						new Tuple<string, string>("a", null)
					}
				},
				new
				{
					Text = ";",
					Delimiters = new List<string>
					{
						";"
					},
					ExpectedSplit = new List<Tuple<string, string>>()
					{
						new Tuple<string, string>("", ";")
					}
				}
			};

			foreach (var testCase in testCases)
			{
				List<Tuple<string, string>> result = testCase.Text.SplitWithDelimiters(testCase.Delimiters);
				CollectionAssert.AreEqual(testCase.ExpectedSplit, result);
			}
		}
	}
}
