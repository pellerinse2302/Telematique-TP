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

                    server.Reply("ack " + received.Message, received.Sender);
                    Console.WriteLine(received.Message);
                    Process.Start(Path.Combine(received.Message.Replace("Je t'envoie ce fichier ", string.Empty).Replace("Je souhaite recevoir ce fichier ", string.Empty)));
                    if (received.Message == "quit")
                        break;
                }
            });
            while (true)
            {
                int i = 1;
            }
        }
    }
}
