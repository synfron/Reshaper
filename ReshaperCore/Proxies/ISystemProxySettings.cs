namespace ReshaperCore.Proxies
{
	public interface ISystemProxySettings
	{
		void ForceReset();
		bool IsLastRegistered();
		void Reset();
		void SetProxy(string hostname, int port);
	}
}