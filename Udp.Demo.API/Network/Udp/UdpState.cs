using System.Net;
using System.Net.Sockets;

namespace Udp.Demo.API.Network.Udp
{
	public class UdpState
	{
		public UdpClient UdpClient { get; set; }
		public IPEndPoint IPEndPoint;
		public int Retries { get; set; }
		public byte[] BytesToSend { get; set; }
		public byte[] BytesReceived { get; set; }
	}
}
