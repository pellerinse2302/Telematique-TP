using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Tp1
{
  abstract class UdpBase
  {
    public UdpClient Client;

    protected UdpBase()
    {
      Client = new UdpClient();
    }

    public async Task<Received> Receive()
    {
      var result = await Client.ReceiveAsync();
      return new Received()
      {
        Message = string.Empty,
        packet = new Packet(result.Buffer),
        Sender = result.RemoteEndPoint
      };
    }

    public void Reply(byte[] message, IPEndPoint endpoint)
    {
      Client.Send(message, message.Length, endpoint);
    }

    public void Send(byte[] message)
    {
      Client.Send(message, message.Length);
    }
    public void Send(byte[] message, string ipAddress, int port)
    {
      Client.Send(message, message.Length, new IPEndPoint(IPAddress.Parse(ipAddress), port));
    }
  }
}
