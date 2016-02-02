using System;

namespace Tp1
{
    class Submission
    {
        public void submit(String filename)
        {
            var client = UdpUser.ConnectTo(Credentials.IPAddress, 32123);

            byte[] bytes = System.IO.File.ReadAllBytes(filename);

            Packet packet = new Packet(2, 1, false, false, Convert.ToInt16(bytes.Length), bytes);

            packet.BuildPacket();

            byte[] packetToSend = packet.BuildPacket();
        }
    }
}
