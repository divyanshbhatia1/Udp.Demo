using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Udp.Demo.Sender
{
	class Program
	{
		static void Main(string[] args)
		{
			var client = new UdpClient();

			IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000);

			client.Connect(endpoint);

			byte[] bytes = Encoding.UTF8.GetBytes("Hello");

			client.Send(bytes, bytes.Length);
		}
	}
}
