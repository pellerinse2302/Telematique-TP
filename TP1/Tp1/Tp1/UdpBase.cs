using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Tp1
{
    abstract class UdpBase
    {
        protected UdpClient Client;

        protected UdpBase()
        {
            Client = new UdpClient();
        }

        public async Task<Received> Receive()
        {
            var result = await Client.ReceiveAsync();
            return new Received()
            {
                //Message = Encoding.ASCII.GetString(result.Buffer, 0, result.Buffer.Length),
                Message = string.Empty,
                packet = new Packet(result.Buffer),
                Sender = result.RemoteEndPoint
            };
        }
    }
}
