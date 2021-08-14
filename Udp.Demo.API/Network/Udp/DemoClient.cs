using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Udp.Demo.API.Configs;

namespace Udp.Demo.API.Network.Udp
{
	public class DemoClient : IUdpNetwork
	{
		private readonly UdpClient _clientSend;
		private readonly IPEndPoint _endpoint;
		private readonly ILogger<DemoClient> _logger;
		private const int _defaultRetries = 3;

		public DemoClient(UdpConfig settings, ILogger<DemoClient> logger)
		{
			_clientSend = new UdpClient();
			_endpoint = new IPEndPoint(IPAddress.Parse(settings.IpAddress), settings.Port);
			_logger = logger;
		}

		//public event EventHandler<string> DataRecevied;
		//public event EventHandler Disconnected;

		public void Connect()
		{
			_clientSend.Client.ExclusiveAddressUse = false;
			_clientSend.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			_clientSend.Client.ReceiveTimeout = 1000;

			SendCommandAsync("zed func_ping");
		}

		//public void Disconnect()
		//{
		//	throw new NotImplementedException();
		//}

		public bool Send(string message)
		{
			return SendCommandAsync(message);
		}

		#region Private Methods
		private bool SendCommandAsync(string command)
		{
			try
			{
				SendCommandAsync(command, _endpoint);
			}
			catch (Exception e)
			{
				_logger.LogError($"Error sending message {e}");
				return false;
			}
			return true;
		}

		private void SendCommandAsync(string command, IPEndPoint endpoint)
		{
			UdpState state = new UdpState
			{
				IPEndPoint = endpoint,
				UdpClient = _clientSend,
				Retries = 0,
				BytesToSend = Encoding.ASCII.GetBytes(command),
				BytesReceived = Array.Empty<byte>()
			};

			SendAndReceiveAsync(state);
		}

		private void SendAndReceiveAsync(UdpState state)
		{
			try
			{
				_clientSend.SendAsync(state.BytesToSend, state.BytesToSend.Length, state.IPEndPoint);
				_clientSend.BeginReceive(new AsyncCallback(ReceiveCallback), state);
			}
			catch (Exception e)
			{
				_logger.LogError($"Udp Error {e}");
			}

		}

		private void ReceiveCallback(IAsyncResult ar)
		{
			UdpState state = ar.AsyncState as UdpState;

			try
			{
				byte[] receiveBytes = state.UdpClient.EndReceive(ar, ref state.IPEndPoint);
				string receiveString = Encoding.ASCII.GetString(receiveBytes);

				_logger.LogDebug($"[Carrier Board] Socket UDP send {Encoding.ASCII.GetString(state.BytesToSend)} | Retries : {state.Retries}");
				_logger.LogDebug($"[Carrier Board] Socket UDP receive {receiveString}");
			}
			catch (Exception e)
			{
				_logger.LogError($"[Carrier Board] {e}");
				if (state.Retries < _defaultRetries)
				{
					++state.Retries;
					SendAndReceiveAsync(state);
				}

				_logger.LogError($"[Carrier Board] Socket UDP can't send {Encoding.ASCII.GetString(state.BytesToSend)} | Retries : {state.Retries}");
			}
		}

		#endregion
	}
}
