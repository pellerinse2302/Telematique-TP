using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tp1
{
    class Reception
    {
        public void receive(UdpListener receiver, string fileName, Int32 fileSize)
        {
            Byte[][] bytes = new Byte[fileSize/1024][];

            
            Task.Factory.StartNew(async () =>
            {
                var FIN = false;

                while (!FIN)
                {
                    var received = await receiver.Receive();

                    bytes[received.packet.SequenceNumber] = received.packet.DATA;
                    receiver.Reply(new Packet(0, received.packet.SequenceNumber, false, false, 0, new byte[0]).BuildPacket(), received.Sender);

                    if (checkIfCompleted(bytes))
                    {
                        FIN = true;

                        var file = Extensions.Combine(bytes);

                        System.IO.FileStream fileStream = new System.IO.FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                        fileStream.Write(file, 0, file.Length);
                        fileStream.Close();
                    }
                }
            });    
        }

        public Boolean checkIfCompleted(Byte[][] bytes)
        {
            for(int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i].Length == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
