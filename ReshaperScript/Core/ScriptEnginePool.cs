using System.Threading;
using MsieJavaScriptEngine;
using ReshaperCore.Utils;
using ReshaperScript.Providers;
using ReshaperScript.Settings;

namespace ReshaperScript.Core
{
	public class ScriptEnginePool : IScriptEnginePool
	{
		private int _enginesCheckedOut = 0;
		private object _poolMutex = new object();
		private object _checkoutMutex = new object();
		private readonly IScriptEngineSettings _scriptEngineSettings;

		private Deque<PooledEngine> Engines
		{
			get;
			set;
		}

		public ScriptEnginePool()
		{
			ScriptEngineSettingsProvider scriptEngineSettingsProvider = new ScriptEngineSettingsProvider();
			_scriptEngineSettings = scriptEngineSettingsProvider.GetInstance();
			Engines = new Deque<PooledEngine>();
		}

		public IPooledEngine CheckoutEngine()
		{
			PooledEngine pooledEngine = null;
			lock (_poolMutex)
			{
				if (_enginesCheckedOut < _scriptEngineSettings.MaxEnginesInPool)
				{
					if (Engines.Count > 0)
					{
						pooledEngine = Engines.TakeFirst();
					}
					else
					{
						pooledEngine = CreateNewEngine();
					}
				}
			}
			if (pooledEngine == null)
			{
				Monitor.Wait(_checkoutMutex);
				pooledEngine = CheckoutEngine() as PooledEngine;
			}
			else
			{
				pooledEngine.UseCount++;
				_enginesCheckedOut++;
				pooledEngine.CheckedIn = true;
			}
			return pooledEngine;
		}

		private PooledEngine CreateNewEngine()
		{

			PooledEngine pooledEngine = new PooledEngine()
			{
				ScriptEngine = new MsieJsEngineAdapter(new MsieJsEngine()),
				PoolId = _enginesCheckedOut
			};

			pooledEngine.ExpirationTimer = new Timer((state) =>
			{
				lock (_poolMutex)
				{
					if (!pooledEngine.Expired && pooledEngine.CheckedIn)
					{
						Engines.Remove(pooledEngine);
						DestroyEngine(pooledEngine);
					}
					pooledEngine.Expired = true;
					pooledEngine.ExpirationTimer = null;
				}
			}, null, _scriptEngineSettings.PoolEngineExpirationInMinutes, Timeout.Infinite);

			return pooledEngine;
		}

		private void DestroyEngine(PooledEngine pooledEngine)
		{
			pooledEngine.ScriptEngine.Dispose();
			pooledEngine.ExpirationTimer?.Dispose();
			pooledEngine.ExpirationTimer = null;
		}

		public void CheckinEngine(IPooledEngine pooledEngine)
		{
			lock (_poolMutex)
			{
				PooledEngine unwrappedPooledEngine = (PooledEngine)pooledEngine;
				if (unwrappedPooledEngine != null && !unwrappedPooledEngine.CheckedIn)
				{
					unwrappedPooledEngine.Expired |= unwrappedPooledEngine.UseCount == _scriptEngineSettings.PoolEngineExpirationUseCount;
					if (unwrappedPooledEngine.Expired)
					{
						DestroyEngine(unwrappedPooledEngine);
					}
					else
					{
						Engines.AddLast(unwrappedPooledEngine);
					}
					_enginesCheckedOut--;
					unwrappedPooledEngine.CheckedIn = true;
					Monitor.Pulse(_checkoutMutex);
				}
			}
		}
	}
}
