using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using ReshaperCore.Networking;

namespace ReshaperCore.Proxies
{
	public class Channel
	{
		private TcpClient _client;
		private NetworkStream _clientStream;
		private DataReader _clientDataReader;
		private bool _signaledDisconnection = false;

		public event DataReceivedHandler DataReceived;
		public delegate void DataReceivedHandler(Buffer<byte> buffer);

		public event DisconnectedHandler Disconnected;
		public delegate void DisconnectedHandler();

		public bool Connected
		{
			get
			{
				bool connected = false;
				try
				{
					IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();

					connected = ipProperties.GetActiveTcpConnections().FirstOrDefault(connectionInformation => connectionInformation.LocalEndPoint.Equals(_client.Client.LocalEndPoint) && connectionInformation.RemoteEndPoint.Equals(_client.Client.RemoteEndPoint))?.State == TcpState.Established;
				}
				catch (Exception)
				{
				}
				return connected;
			}
		}

		public virtual IPEndPoint LocalEndpoint
		{
			get;
			private set;
		}

		public virtual IPEndPoint RemoteEndpoint
		{
			get;
			private set;
		}

		public Channel(TcpClient client)
		{
			this._client = client;
		}

		public void Init()
		{
			try
			{
				if (Connected)
				{
					_signaledDisconnection = false;
					LocalEndpoint = _client.Client.LocalEndPoint as IPEndPoint;
					RemoteEndpoint = _client.Client.RemoteEndPoint as IPEndPoint;
					_clientStream = _client.GetStream();
					_clientDataReader = new DataReader(_clientStream);

					_clientDataReader.ReadAsync().ContinueWith(OnClientData);
				}
			}
			catch (Exception)
			{
				SignalDisconnection();
			}
		}

		private void OnClientData(Task<Buffer<byte>> task)
		{
			try
			{
				if (task.Status == TaskStatus.RanToCompletion && task.Result.Length > 0)
				{
					if (DataReceived != null)
					{
						DataReceived(task.Result);
					}
					_clientDataReader.ReadAsync().ContinueWith(OnClientData);
				}
				else
				{
					SignalDisconnection();
				}
			}
			catch (Exception)
			{
				SignalDisconnection();
			}
		}

		public void SendData(Buffer<byte> rawBuffer)
		{
			try
			{
				_clientStream.WriteAsync(rawBuffer.Array, rawBuffer.Position, rawBuffer.Length);
				_clientStream.FlushAsync();
			}
			catch (Exception)
			{
				SignalDisconnection();
			}
		}

		private void SignalDisconnection()
		{
			if (!Connected && !_signaledDisconnection && Disconnected != null)
			{
				Disconnected();
				_signaledDisconnection = true;
			}
		}

		public void Dispose()
		{
			try
			{
				_client.Close();
			}
			catch (Exception)
			{
			}
		}
	}
}
