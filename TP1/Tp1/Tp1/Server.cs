using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Tp1
{
  class Server
  {
    public void start()
    {
      /* Créer un socket et attendre la demande du client */
      var server = new UdpListener(new IPEndPoint(IPAddress.Any, 32123));

      //start listening for messages and copy the messages back to the client
      Task.Factory.StartNew(async () =>
      {
        while (true)
        {
          Received received = await server.Receive();

          if (received.packet.SOR == false)
          {
            UdpListener receiver = new UdpListener(new IPEndPoint(IPAddress.Any, Extensions.FreeUdpPort()));
            Byte[] portNumber = new Byte[4];
            portNumber = BitConverter.GetBytes(((IPEndPoint)receiver.Client.Client.LocalEndPoint).Port);
            server.Reply(new Packet(0, received.packet.SequenceNumber, false, false, portNumber.Length, portNumber).BuildPacket(), received.Sender);

            Reception reception = new Reception();
            reception.receive(receiver, System.Text.Encoding.UTF8.GetString(received.packet.DATA), received.packet.DataLength);
          }
          else
          {
            UdpUser sender = UdpUser.ConnectTo(received.Sender.Address.ToString(), received.Sender.Port);
            string fileName = System.Text.Encoding.UTF8.GetString(received.packet.DATA);

            await Task.Factory.StartNew(async () =>
             {
               var ack = await sender.Receive();
               if (ack.packet.AckNumber == 0)
               {
                 sender = UdpUser.ConnectTo(Credentials.IPAddress, received.Sender.Port);
                 CommunicationBase.Handshake = true;
               }
             });

            /* Envoyer fichier */
            byte[] bytes = System.IO.File.ReadAllBytes(fileName);
            byte[] fileNameString = Encoding.ASCII.GetBytes(Path.GetFileName(Path.Combine(fileName)));
            

            Packet packet = new Packet(0, 0, false, false, fileNameString.Length, fileNameString);
            sender.Send(packet.BuildPacket());

            while (!CommunicationBase.Handshake)
            {

            }

            Submission submission = new Submission();
            submission.submit(bytes, sender);
                  /*UdpUser sender = UdpUser.ConnectTo(received.Sender.Address.ToString(), received.Sender.Port);

                  byte[] bytes = System.IO.File.ReadAllBytes(System.Text.Encoding.UTF8.GetString(received.packet.DATA));
                  Submission submission = new Submission();
                  submission.submit(bytes, sender);*/
          }

                //server.Reply(new Packet(0, received.packet.SequenceNumber, false, true, 0, new byte[0]).BuildPacket(), received.Sender);
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
