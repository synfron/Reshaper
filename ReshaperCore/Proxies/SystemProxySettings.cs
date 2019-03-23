namespace ReshaperCore.Proxies
{
	public class SystemProxySettings : ISystemProxySettings
	{
		private readonly object _proxyMutex = new object();
		private bool _changedProxy;
		private int _lastIdentity;
		private int _identity;
		private WinINetAdapter.INTERNET_PER_CONN_OPTION_LIST _defaultProxyOptions;

		public SystemProxySettings()
		{
			_identity = GetIdentity();
		}

		private int GetIdentity()
		{
			return ++_lastIdentity;
		}

		public bool IsLastRegistered()
		{
			return _identity == _lastIdentity;
		}

		public void Reset()
		{
			if (_identity == _lastIdentity)
			{
				ForceReset();
			}
		}

		public void ForceReset()
		{
			lock (_proxyMutex)
			{
				if (_changedProxy)
				{
					WinINetAdapter.SetConnectionProxy(_defaultProxyOptions);

					_changedProxy = false;
				}
			}
		}

		private void SaveCurrent()
		{
			_defaultProxyOptions = WinINetAdapter.GetSystemProxy();
		}

		public void SetProxy(string hostname, int port)
		{
			lock (_proxyMutex)
			{
				if (!_changedProxy)
				{
					SaveCurrent();
				}
				WinINetAdapter.SetConnectionProxy(true, $"{hostname}:{port}");
				_changedProxy = true;
			}
		}
	}
}
