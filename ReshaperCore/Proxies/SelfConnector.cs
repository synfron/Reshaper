using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ReshaperCore.Networking;

namespace ReshaperCore.Proxies
{
	public class SelfConnector
	{
		private Channel _channel;
		private TcpClient _client;
		private Thread _connectionThread;
		private ProxyInfo _proxyInfo;

		public Task<SelfConnector> Connect(ProxyInfo proxyInfo)
		{
			this._proxyInfo = proxyInfo;
			Task<SelfConnector> connectionPromise = new Task<SelfConnector>(() =>
			{
				if (_channel?.Connected ?? false)
				{
					return this;
				}
				else
				{
					throw new SocketException();
				}
			});
			_connectionThread = new Thread(() =>
			{
				_client = new TcpClient();
				_client.ConnectAsync("127.0.0.1", proxyInfo.Port).ContinueWith(OnConnected, connectionPromise);
			});
			_connectionThread.Start();
			return connectionPromise;
		}

		private void OnConnected(Task connectionTask, object taskObj)
		{
			Task<SelfConnector> connectionPromise = (Task<SelfConnector>)taskObj;
			if (connectionTask.Status == TaskStatus.RanToCompletion)
			{
				_channel = new Channel(_client);
				_channel.Init();
			}
			connectionPromise.Start();
		}

		public void SendData(byte[] data)
		{
			_channel.SendData(new Buffer<byte>(data));
		}
	}
}
