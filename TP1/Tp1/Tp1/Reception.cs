using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tp1
{
  class Reception
  {
    public bool receive(UdpListener receiver, string fileName, Int32 fileSize)
    {
      //Byte[][] bytes = new Byte[fileSize/1024][];
      SortedDictionary<Int32, Byte[]> bytes = new SortedDictionary<int, byte[]>();
      bool endReception = false;

      Task.Factory.StartNew(async () =>
      {
        var FIN = false;
        bool endFile = false;
        int counter = 0;

        while (!FIN)
        {
          var received = await receiver.Receive();

          if (!bytes.Keys.Contains(received.packet.SequenceNumber))
          {
            bytes[received.packet.SequenceNumber] = received.packet.DATA;
            counter++;
          }
          
          receiver.Reply(new Packet(0, received.packet.SequenceNumber, false, false, 0, new byte[0]).BuildPacket(), received.Sender);

          if(received.packet.FIN == true)
          {
            endFile = true;
          }

          if (bytes.Keys.Max() == counter && endFile)
          {
            FIN = true;

            byte[] file = Extensions.Combine(bytes.Values.ToArray());

            System.IO.FileStream fileStream = new System.IO.FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            fileStream.Write(file, 0, file.Length);
            fileStream.Close();
            endReception = true;
          }
        }
      });
      while (!endReception) { }
      return endReception;
    }
  }
}
