using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading;
using ReshaperCore.Messages;
using ReshaperCore.Networking;
using ReshaperCore.Rules;
using ReshaperCore.Vars;

namespace ReshaperCore.Proxies
{
	public class ProxyConnection
	{
		private static long _connectionId;

		public virtual long ConnectionId
		{
			get;
			private set;
		}

		private TcpClient targetClient;
		private TcpClient originClient;

		public virtual ProxyInfo ProxyInfo
		{
			private set;
			get;
		}

		public virtual bool HasTargetConnection
		{
			get
			{
				return TargetChannel?.Connected ?? false;
			}
		}

		public virtual bool HasOriginConnection
		{
			get
			{
				return OriginChannel?.Connected ?? false;
			}
		}

		public virtual Variables ToTargetConnectionVariables
		{
			private set;
			get;
		} = new Variables();

		public virtual Variables ToOriginConnectionVariables
		{
			private set;
			get;
		} = new Variables();

		private RulesEngine rulesEngine;
		private Thread connectionThread;

		public virtual Channel TargetChannel
		{
			private set;
			get;
		}

		public virtual Channel OriginChannel
		{
			private set;
			get;
		}
		public ProxyHost Host
		{
			get;
			private set;
		}

		public ProxyConnection(ProxyHost host, ProxyInfo proxy, TcpClient client)
		{
			this.Host = host;
			this.ProxyInfo = proxy;
			this.originClient = client;
			rulesEngine = new RulesEngine();
			ConnectionId = Interlocked.Increment(ref _connectionId);
		}

		public virtual bool HasConnection(DataDirection direction)
		{
			return (direction == DataDirection.Origin) ? HasOriginConnection : HasTargetConnection;
		}

		public virtual bool InitConnection(DataDirection direction, string hostname, int port)
		{
			return (direction == DataDirection.Origin) ? InitOriginConnection(hostname, port) : InitTargetConnection(hostname, port);
		}

		public virtual bool InitTargetConnection(string hostname, int port)
		{
			bool connected = false;
			try
			{
				targetClient = new TcpClient();
				targetClient.Connect(hostname, port);
				if (TargetChannel != null)
				{
					TargetChannel.DataReceived -= OnTargetChannelDataReceived;
					//Disconnect code
				}
				TargetChannel = new Channel(targetClient);
				TargetChannel.Disconnected += OnTargetChannelDisconnected;
				TargetChannel.DataReceived += OnTargetChannelDataReceived;
				TargetChannel.Init();
				connected = true;
			}
			catch (Exception)
			{
			}
			return connected;
		}

		public virtual bool InitOriginConnection(string hostname, int port)
		{
			bool connected = false;
			try
			{
				originClient = new TcpClient();
				originClient.Connect(hostname, port);
				if (OriginChannel != null)
				{
					OriginChannel.DataReceived -= OnOriginChannelDataReceived;
					//Disconnect code
				}
				OriginChannel = new Channel(originClient);
				OriginChannel.Disconnected += OnOriginChannelDisconnected;
				OriginChannel.DataReceived += OnOriginChannelDataReceived;
				OriginChannel.Init();
				connected = true;
			}
			catch (Exception)
			{
			}
			return connected;
		}

		public void Disconnect()
		{
			DisconnectTargetChannel();
			DisconnectOriginChannel();
		}

		public void DisconnectChannel(DataDirection direction)
		{
			if (direction == DataDirection.Origin)
			{
				DisconnectOriginChannel();
			}
			else
			{
				DisconnectTargetChannel();
			}
		}

		public void DisconnectOriginChannel()
		{
			if (HasOriginConnection)
			{
				OriginChannel.Dispose();
			}
		}

		public void DisconnectTargetChannel()
		{
			if (HasTargetConnection)
			{
				TargetChannel.Dispose();
			}
		}

		private void OnOriginChannelDisconnected()
		{
			if (TargetChannel?.Connected ?? false)
			{
				rulesEngine.Queue.AddLast(new EventInfo { ProxyConnection = this, Engine = rulesEngine, Type = EventType.Disconnected, Direction = DataDirection.Target, Variables = ToTargetConnectionVariables });
			}
		}

		private void OnTargetChannelDisconnected()
		{
			if (OriginChannel?.Connected ?? false)
			{
				rulesEngine.Queue.AddLast(new EventInfo { ProxyConnection = this, Engine = rulesEngine, Type = EventType.Disconnected, Direction = DataDirection.Origin, Variables = ToOriginConnectionVariables });
			}
		}

		public virtual void Init()
		{
			if (ProxyInfo.Enabled)
			{
				connectionThread = new Thread(() =>
				{
					OriginChannel = new Channel(originClient);
					OriginChannel.Disconnected += OnOriginChannelDisconnected;
					OriginChannel.DataReceived += OnOriginChannelDataReceived;
					OriginChannel.Init();

					ProxyInfo.PropertyChanged += OnProxyInfoPropertyChanged;

					rulesEngine.Queue.AddLast(new EventInfo { ProxyConnection = this, Engine = rulesEngine, Type = EventType.Connected, Direction = DataDirection.Target, Variables = ToTargetConnectionVariables });
				});
				connectionThread.Start();
			}
		}

		private void OnProxyInfoPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			try
			{
				if (e.PropertyName == "Enabled")
				{
					if (!ProxyInfo.Enabled)
					{
						OriginChannel.Dispose();
						ProxyInfo.PropertyChanged -= OnProxyInfoPropertyChanged;
					}
				}
			}
			catch (Exception)
			{
			}
		}

		private void OnTargetChannelDataReceived(Buffer<byte> buffer)
		{
			AddToOriginData(buffer.GetBytes());
		}

		private void OnOriginChannelDataReceived(Buffer<byte> buffer)
		{
			AddToTargetData(buffer.GetBytes());
		}

		public virtual void SendData(EventInfo eventInfo)
		{
			switch (eventInfo.Direction)
			{
				case Messages.DataDirection.Target:
					if (eventInfo.ProxyConnection.HasTargetConnection)
					{
						TargetChannel.SendData(new Buffer<byte>(eventInfo.Message.RawBytes));
					}
					break;
				case DataDirection.Origin:
					if (eventInfo.ProxyConnection.HasOriginConnection)
					{
						OriginChannel.SendData(new Buffer<byte>(eventInfo.Message.RawBytes));
					}
					break;
			}
		}

		private void AddToOriginData(byte[] data)
		{
			if (data != null)
			{
				rulesEngine.Queue.AddLast(new EventInfo { ProxyConnection = this, Engine = rulesEngine, Type = EventType.Message, Direction = DataDirection.Origin, Message = new Message() { RawBytes = data }, Variables = ToOriginConnectionVariables });
			}
		}

		private void AddToTargetData(byte[] data)
		{
			if (data != null)
			{
				rulesEngine.Queue.AddLast(new EventInfo { ProxyConnection = this, Engine = rulesEngine, Type = EventType.Message, Direction = DataDirection.Target, Message = new Message() { RawBytes = data }, Variables = ToTargetConnectionVariables });
			}
		}

		public void AddData(DataDirection direction, byte[] data)
		{
			if (direction == DataDirection.Origin)
			{
				AddToOriginData(data);
			}
			else
			{
				AddToTargetData(data);
			}
		}
	}
}
