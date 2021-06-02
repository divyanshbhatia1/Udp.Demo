using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Udp.Demo.Reciever
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Reciever started");

			UdpClient udpServer = new UdpClient(11000);

			while(true)
			{
				var remoteEp = new IPEndPoint(IPAddress.Any, 11000);
				var data = udpServer.Receive(ref remoteEp);
				Console.WriteLine("Recieved: " + Encoding.UTF8.GetString(data));
			}
		}
	}
}
