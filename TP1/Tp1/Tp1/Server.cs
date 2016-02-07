using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Tp1
{
    class Server
    {
        public void start()
        {
            /* Créer un socket et attendre la demande du client */
            var server = new UdpListener();

            //start listening for messages and copy the messages back to the client
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    var received = await server.Receive();

                    server.Reply(new Packet(0, received.packet.SequenceNumber, false, true, 0, new byte[0]).BuildPacket(), received.Sender);
                    Console.WriteLine(received.Message);
                    if (received.Message == "quit")
                        break;
                }
            });
            while (true)
            {
                int i = 1;
                i++;
            }
        }
    }
}
