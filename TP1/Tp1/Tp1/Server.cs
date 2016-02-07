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

                    if(received.packet.SOR == false)
                    {
                        var receiver = new UdpListener();

                        receiver.Reply(new Packet(0, received.packet.SequenceNumber, false, false, 0, new byte[0]).BuildPacket(), received.Sender);

                        Reception reception = new Reception();
                        reception.receive(receiver, System.Text.Encoding.UTF8.GetString(received.packet.DATA), received.packet.DataLength);
                    }
                    else
                    {
                        var sender = new UdpUser();

                        byte[] bytes = System.IO.File.ReadAllBytes(System.Text.Encoding.UTF8.GetString(received.packet.DATA));
                        Submission submission = new Submission();
                        submission.submit(bytes, sender);
                    }

                    server.Reply(new Packet(0, received.packet.SequenceNumber, false, true, 0, new byte[0]).BuildPacket(), received.Sender);
                    Console.WriteLine(received.Message);
                    if (received.Message == "quit")
                        break;
                }
            });
            while (true)
            {
            }
        }
    }
}
