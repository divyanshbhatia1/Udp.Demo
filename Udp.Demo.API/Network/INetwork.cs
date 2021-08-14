using System;

namespace Udp.Demo.API.Network
{
	public interface INetwork
	{
		void Connect();

		bool Send(string message);

		//void Disconnect();

		//event EventHandler<string> DataRecevied;

		//event EventHandler Disconnected;
	}
}
