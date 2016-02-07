using System;
using System.Threading.Tasks;

namespace Tp1
{
    class Submission
    {
        UdpUser socket = UdpUser.ConnectTo(Credentials.IPAddress, 32123);

        public void submit(String filename)
        {
            SlidingWindow window = new SlidingWindow();
            byte[] bytes = System.IO.File.ReadAllBytes(filename);

            int extraBytes = bytes.Length % 1024;
            int loop = bytes.Length / 1024;

            //wait for reply messages from server and send them to console 
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    var received = await socket.Receive();
                    if (received.packet.AckNumber == window.Window.Peek().SequenceNumber)
                    {
                        window.Forward();
                        Console.WriteLine(received.packet.AckNumber);
                    }
                }
            });

            for(int i = 0; i < loop; i++)
            {
                Packet packet = new Packet(i+1, 0, false, false, Convert.ToInt32(1024), Extensions.SubArray(bytes, 1024*i, 1024));
                //byte[] packetToSend = packet.BuildPacket();
                while (!window.CanForward)
                {

                }
                window.InsertPacket(packet);
                socket.Send(packet.BuildPacket());
            }

            Packet extraPacket = new Packet(loop+1, 0, true, false, extraBytes, Extensions.SubArray(bytes, 1024*(loop), extraBytes));
            while (!window.CanForward)
            {

            }
            window.InsertPacket(extraPacket);
            socket.Send(extraPacket.BuildPacket());
            
        }
    }
}
