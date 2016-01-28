using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tp1
{
    class Service
    {
        static void Main(string[] args)
        {
            int choiceRole;
            bool parsed;

            do
            {
                Console.WriteLine("Quel rôle souhaitez-vous jouer?\n1) Client\n2) Serveur");
                parsed = Int32.TryParse(Console.ReadLine(), out choiceRole);
            } while (!parsed || (choiceRole != 1 && choiceRole != 2));

            if (choiceRole == 1)
            {
                int choiceTask;
                do
                {
                    Console.WriteLine("\nQue souhaitez-vous faire?\n1) Envoyer un fichier\n2) Recevoir un fichier");
                    parsed = Int32.TryParse(Console.ReadLine(), out choiceTask);
                } while (!parsed || (choiceTask != 1 && choiceTask != 2));

                string fileName;
                Console.WriteLine("\nEntrez le nom du fichier à traiter");
                fileName = Console.ReadLine();

                /* Créer un socket */
                var client = UdpUser.ConnectTo(Credentials.IPAddress, 32123);

                if (choiceTask == 1)
                {
                    /* Envoyer au serveur qu'on souhaite lui envoyer un fichier */
                    client.Send("Je t'envoie ce fichier " + fileName);
                }

                else
                {
                    /* Envoyer au serveur qu'on souhaite recevoir un fichier */
                    client.Send("Je souhaite recevoir ce fichier " + fileName);
                }
            }

            else
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
}
