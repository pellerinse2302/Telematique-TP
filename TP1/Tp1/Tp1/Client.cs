using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tp1
{
    class Client
    {
        public void start()
        {
            int choiceTask;
            bool parsed;

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
                /* On vérifie si le fichier existe. Si non, on redemande un nom de fichier existant à l'infini */
                while (!File.Exists(Path.Combine(fileName)))
                {
                    Console.WriteLine("\nFichier inexistant.\nEntrez le nom du fichier à traiter");
                    fileName = Console.ReadLine();
                };

                /* Envoyer fichier */
                Submission submission = new Submission();
                submission.submit(fileName);
            }

            else
            {
                /* Envoyer au serveur qu'on souhaite recevoir un fichier */
                client.Send("Je souhaite recevoir ce fichier " + fileName);
            }
        }
    }
}
