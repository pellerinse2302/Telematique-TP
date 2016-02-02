using System;

namespace Tp1
{
    class Submission
    {
        public void submit(String filename)
        {
            byte[] bytes = System.IO.File.ReadAllBytes(filename);

            Packet packet = new Packet(2, 1, false, false, Convert.ToInt16(bytes.Length), bytes);

            byte[] packetToSend = packet.BuildPacket();
        }
    }
}
