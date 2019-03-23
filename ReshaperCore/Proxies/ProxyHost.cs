using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using ReshaperCore.Providers;
using ReshaperCore.Utils;

namespace ReshaperCore.Proxies
{
	public class ProxyHost
	{
		private TcpListener _listener;
		private ProxyInfo _proxy;

        public ISystemProxySettings SystemProxySettings { get; set; } = new SystemProxySettingsProvider().GetInstance();

        public bool IsRegisted()
		{
			return (SystemProxySettings?.IsLastRegistered() ?? false) && _proxy.Enabled;
		}

		public ProxyHost(ProxyInfo proxy)
		{
			this._proxy = proxy;
		}

		public void Start()
		{
			try
			{
				//foreach (IPAddress ipAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
				//{
					try
					{
						_listener = new TcpListener(/*ipAddress, */_proxy.Port);
						_listener.Start();
						_proxy.PropertyChanged += Proxy_PropertyChanged;
						_proxy.Enabled = true;
						_listener.AcceptTcpClientAsync().ContinueWith(OnClientConnected);
					}
					catch (Exception/* ex*/)
					{
						//Log.LogError(ex, $"Could not setup proxy on {ipAddress.ToString()} at {_proxy.Port}.");
					}
				//}
				if (_proxy.RegisterAsSystemProxy && _proxy.DataType == ProxyDataType.Http)
				{
					SystemProxySettings.SetProxy("127.0.0.1", _proxy.Port);
				}
			}
			catch (Exception ex)
			{
				Log.LogError(ex, $"Could not setup proxy on port {_proxy.Port}.");
			}
		}

		private void Proxy_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			try
			{
				if (e.PropertyName == "Enabled")
				{
					if (!_proxy.Enabled)
					{
						_listener.Stop();
						SystemProxySettings?.Reset();
					}
				}
			}
			catch (Exception)
			{
			}
		}

		private void OnClientConnected(Task<TcpClient> task)
		{
			try
			{
				if (task.Status == TaskStatus.RanToCompletion)
				{
					ProxyConnection proxyConnection = new ProxyConnection(this, _proxy, task.Result);
					proxyConnection.Init();
					_listener.AcceptTcpClientAsync().ContinueWith(OnClientConnected);
				}
			}
			catch (Exception ex)
			{
				Log.LogError(ex, $"Problem accepting hosted TCP client connection.");
			}
		}
	}
}
