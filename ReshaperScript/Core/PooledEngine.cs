using MsieJavaScriptEngine;
using System.Threading;

namespace ReshaperScript.Core
{
	public class PooledEngine : IPooledEngine
	{
		public Timer ExpirationTimer
		{
			get;
			set;
		}

		public bool CheckedIn
		{
			get;
			set;
		} = true;

		public int PoolId
		{
			get;
			set;
		}

		public bool Expired
		{
			get;
			set;
		}

		public int UseCount
		{
			get;
			set;
		}

		public IScriptEngineAdapter ScriptEngine
		{
			get;
			set;
		}
	}
}
