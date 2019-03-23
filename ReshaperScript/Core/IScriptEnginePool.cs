namespace ReshaperScript.Core
{
	public interface IScriptEnginePool
	{
		void CheckinEngine(IPooledEngine pooledEngine);

		IPooledEngine CheckoutEngine();
	}
}