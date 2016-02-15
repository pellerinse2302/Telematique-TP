using System;
using System.IO;
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
      
      if (choiceTask == 1)
      {
        UdpUser socket = UdpUser.ConnectTo(Credentials.IPAddress, 32123);
        byte[] fileNameString = Encoding.ASCII.GetBytes(Path.GetFileName(Path.Combine(fileName)));
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

        /* On vérifie si le fichier existe. Si non, on redemande un nom de fichier existant à l'infini */
        while (!File.Exists(Path.Combine(fileName)))
        {
          Console.WriteLine("\nFichier inexistant.\nEntrez le nom du fichier à traiter");
          fileName = Console.ReadLine();
        };

        /* Envoyer fichier */
        byte[] bytes = System.IO.File.ReadAllBytes(fileName);
        
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
        UdpListener receiver = new UdpListener();
        bool endReception = false;
        byte[] fileNameString = Encoding.ASCII.GetBytes(fileName);

        Packet packet = new Packet(0, 0, false, true, fileNameString.Length, fileNameString);
        receiver.Send(packet.BuildPacket(), Credentials.IPAddress, 32123);
        Task.Factory.StartNew(async () =>
        {
          Received received = await receiver.Receive();
          if (received.packet.SOR == false)
          {
            receiver.Reply(new Packet(0, received.packet.SequenceNumber, false, false, 0, new byte[0]).BuildPacket(), received.Sender);

            Reception reception = new Reception();
            endReception = reception.receive(receiver, System.Text.Encoding.UTF8.GetString(received.packet.DATA), received.packet.DataLength);
          }
        });
        while (!endReception)
        {

        }
      }
    }
  }
}
