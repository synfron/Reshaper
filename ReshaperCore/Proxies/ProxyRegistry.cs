using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using ReshaperCore.Settings;
using ReshaperCore.Utils;

namespace ReshaperCore.Proxies
{
	public class ProxyRegistry : IProxyRegistry
	{
		private ObservableCollection<ProxyInfo> _proxies;
		private static readonly string _proxiesFile = $@"{SettingsStore.StoragePath}/Proxies.json";

		public virtual ObservableCollection<ProxyInfo> Proxies
		{
			get
			{
				return _proxies;
			}
		}

		public virtual void Add(ProxyInfo proxy)
		{
			_proxies.Add(proxy);
			proxy.PropertyChanged += ProxyInfo_PropertyChanged;
			if (proxy.Enabled)
			{
				new ProxyHost(proxy).Start();
			}
		}

		public virtual void Remove(string name)
		{
			ProxyInfo proxyInfo = _proxies.FirstOrDefault(proxy => proxy.Name == name);
			if (proxyInfo != null)
			{
				proxyInfo.Enabled = false;
				_proxies.Remove(proxyInfo);
				proxyInfo.PropertyChanged -= ProxyInfo_PropertyChanged;
			}
		}

		public virtual void LoadProxies()
		{
			try
			{
				if (File.Exists(_proxiesFile))
				{
					string serializedProxies = File.ReadAllText(_proxiesFile);
					_proxies = Serializer.Deserialize<ObservableCollection<ProxyInfo>>(serializedProxies);
				}
			}
			catch (Exception e)
			{
				Log.LogError(e, "Could not load Proxies");
			}
			if (_proxies == null)
			{
				_proxies = new ObservableCollection<ProxyInfo>();
			}
		}

		public virtual void SaveProxies()
		{
			try
			{
				String serializedProxies = Serializer.Serialize(_proxies);

				FileInfo file = new FileInfo(_proxiesFile);
				file.Directory.Create();
				File.WriteAllText(_proxiesFile, serializedProxies);
			}
			catch (Exception e)
			{
				Log.LogError(e, "Could not save Proxies");
			}
		}

		public virtual void Init()
		{
			LoadProxies();

			foreach (ProxyInfo proxyInfo in Proxies)
			{
				proxyInfo.PropertyChanged += ProxyInfo_PropertyChanged;
				if (proxyInfo.AutoActivate)
				{
					proxyInfo.Enabled = true;
				}
			}
		}

		private void ProxyInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Enabled")
			{
				ProxyInfo proxyInfo = sender as ProxyInfo;
				if (proxyInfo.Enabled)
				{
					new ProxyHost(proxyInfo).Start();
				}
			}
		}
	}
}
