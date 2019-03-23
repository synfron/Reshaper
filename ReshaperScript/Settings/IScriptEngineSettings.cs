namespace ReshaperScript.Settings
{
	public interface IScriptEngineSettings
	{
		int MaxEnginesInPool { get; set; }
		int PoolEngineExpirationInMinutes { get; set; }
		int PoolEngineExpirationUseCount { get; set; }
		int ScriptTimeoutInSeconds { get; set; }
	}
}