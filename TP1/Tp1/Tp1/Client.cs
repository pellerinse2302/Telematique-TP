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

      UdpUser socket = UdpUser.ConnectTo(Credentials.IPAddress, 32123);

      //wait for reply messages from server and send them to console 
      Task.Factory.StartNew(async () =>
      {
          var received = await socket.Receive();
          if (received.packet.AckNumber == 0)
          {
              socket = UdpUser.ConnectTo(Credentials.IPAddress, BitConverter.ToInt32(received.packet.DATA, 0));
            CommunicationBase.Handshake = true;
          }
      });

      if (choiceTask == 1)
      {
        /* On vérifie si le fichier existe. Si non, on redemande un nom de fichier existant à l'infini */
        while (!File.Exists(Path.Combine(fileName)))
        {
          Console.WriteLine("\nFichier inexistant.\nEntrez le nom du fichier à traiter");
          fileName = Console.ReadLine();
        };

        /* Envoyer fichier */
        byte[] bytes = System.IO.File.ReadAllBytes(fileName);
        byte[] fileNameString = Encoding.ASCII.GetBytes(Path.GetFileName(Path.Combine(fileName)));
                Submission submission = new Submission();

        Packet packet = new Packet(0, 0, false, false, fileNameString.Length, fileNameString);
        socket.Send(packet.BuildPacket());

        while (!CommunicationBase.Handshake)
        {

        }

        submission.submit(bytes, socket);
      }

      else
      {
        /* Envoyer au serveur qu'on souhaite recevoir un fichier */
        //client.Send("Je souhaite recevoir ce fichier " + fileName);
      }
    }
  }
}
