using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReshaperCore.Rules;
using ReshaperCore.Rules.Thens;
using ReshaperCore.Rules.Whens;
using ReshaperCore.Utils;
using ReshaperScript.Core;

namespace ReshaperTests
{
	[TestClass, Ignore]
	public class UnitTest1
	{

		[TestMethod]
		public void TestMethod3()
		{
			ScriptHandler scriptHandler = new ScriptHandler();
			string result = scriptHandler.RunScript(new EventInfo(), "return true");
			//string httpTexts = File.ReadAllText("TestFiles/HttpTestResponses.txt");

			//byte[] textBytes = Encoding.UTF8.GetBytes(httpTexts);
			//int val = 0;
			//while (val < textBytes.Length)
			//{
			//	try
			//	{
			//		HttpParser.HttpParser parser = new HttpParser.HttpParser();
			//		using (MemoryStream stream = new MemoryStream(textBytes))
			//		{
			//			HttpParserHandler handler = new HttpParserHandler(stream);
			//			parser.Execute(handler.Settings, stream);
			//		}
			//	}
			//	catch (InvalidOperationException)
			//	{
			//	}
			//}

		}


		[TestMethod]
        public void TestMethod1()
		{
			//Thread thread = new Thread(new ThreadStart(delegate ()
			//{
			//	Proxy proxy = new Proxy("bb", 7001);
			//}));
			//thread.Start();
			HttpRulesRegistry httpRulesRegistry = new HttpRulesRegistry();
			TextRulesRegistry textRulesRegistry = new TextRulesRegistry();

			#region Connect Text Rule
			Rule connectRule = new Rule()
			{
				Name = "Establish Text Connection",
				Placement = RunPosition.Beginning,
				Locked = true
			};
			connectRule.Whens.Add(new WhenEventType()
			{
				Type = EventType.Connected
			});
			connectRule.Thens.Add(new ThenConnect());
			connectRule.Thens.Add(new ThenSkipProcessing());
			#endregion

			#region Disconnect Text Rule
			Rule disconnectRule = new Rule()
			{
				Name = "Close Text Connection",
				Placement = RunPosition.Beginning,
				Locked = true
			};
			disconnectRule.Whens.Add(new WhenEventType()
			{
				Type = EventType.Disconnected
			});
			disconnectRule.Thens.Add(new ThenDisconnect());
			disconnectRule.Thens.Add(new ThenSkipProcessing());

			#endregion

			#region Disconnect HTTP Rule
			Rule disconnectHttpRule = new Rule()
			{
				Name = "Close HTTP Connection",
				Placement = RunPosition.Beginning,
				Locked = true
			};
			disconnectHttpRule.Whens.Add(new WhenEventType()
			{
				Type = EventType.Disconnected
			});
			disconnectHttpRule.Thens.Add(new ThenDisconnect());
			#endregion

			#region Skip Processing Rule
			Rule skipRule = new Rule()
			{
				Name = "Skip Connect/Disconnect Events",
				Placement = RunPosition.Beginning,
				Locked = true
			};
			skipRule.Whens.Add(new WhenEventType()
			{
				Type = EventType.Connected,
			});
			skipRule.Whens.Add(new WhenEventType()
			{
				Type = EventType.Disconnected,
				UseOrCondition = true
			});
			skipRule.Thens.Add(new ThenSkipProcessing());
			#endregion

			#region Delimit HTTP Rule
			Rule processHttpRule = new Rule()
			{
				Name = "Delimit HTTP Message",
				Placement = RunPosition.Beginning,
				Locked = true
			};
			processHttpRule.Thens.Add(new ThenDelimitHttp());
			#endregion

			#region Delimit Text Rule
			Rule processTextRule = new Rule()
			{
				Name = "Process Text Message",
				Placement = RunPosition.Beginning,
				Locked = true
			};
			processTextRule.Thens.Add(new ThenDelimitText());
			#endregion

			#region Connect HTTP Rule
			Rule connectHttpRule = new Rule()
			{
				Name = "Establish HTTP Connection",
				Locked = true
			};
			connectHttpRule.Thens.Add(new ThenHttpConnect());
			#endregion

			#region Broadcast Rule
			Rule sendRule = new Rule()
			{
				Name = "Send Message",
				Placement = RunPosition.End,
				Locked = true
			};
			sendRule.Thens.Add(new ThenBroadcast());
			sendRule.Thens.Add(new ThenSendData());
			#endregion

			textRulesRegistry.AddRule(connectRule);
			textRulesRegistry.AddRule(disconnectRule);
			httpRulesRegistry.AddRule(disconnectHttpRule);
			httpRulesRegistry.AddRule(skipRule);
			httpRulesRegistry.AddRule(processHttpRule);
			textRulesRegistry.AddRule(processTextRule);
			httpRulesRegistry.AddRule(connectHttpRule);
			textRulesRegistry.AddRule(sendRule);
			httpRulesRegistry.AddRule(sendRule);

			string httpText = Serializer.Serialize(httpRulesRegistry.GetRules());
			string textText = Serializer.Serialize(textRulesRegistry.GetRules());

			File.WriteAllText("DefaultHttpRules.json", httpText);
			File.WriteAllText("DefaultTextRules.json", textText);

			List<Rule> httpRules = Serializer.Deserialize<List<Rule>>(httpText);
			List<Rule> textRules = Serializer.Deserialize<List<Rule>>(textText);

		}
	}
}
