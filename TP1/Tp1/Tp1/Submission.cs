using System;

namespace Tp1
{
    class Submission
    {
        public void submit(String filename)
        {
            byte[] bytes = System.IO.File.ReadAllBytes(filename);

            int extraBytes = bytes.Length % 1024;
            int loop = bytes.Length / 1024;

            for(int i = 0; i < loop; i++)
            {
                Packet packet = new Packet(i+1, 0, false, false, Convert.ToInt16(bytes.Length), Extensions.SubArray(bytes, i, 1024*(i+1)));
                byte[] packetToSend = packet.BuildPacket();
            }
        }
    }
}
