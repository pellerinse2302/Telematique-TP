﻿using System;
using System.Threading.Tasks;
using System.Timers;
using System.Linq;
using System.Globalization;

namespace Tp1
{
  class Submission
  {

    public void submit(byte[] bytes, UdpUser socket)
    {
      SlidingWindow window = new SlidingWindow();

      int extraBytes = bytes.Length % 1024;
      int loop = bytes.Length / 1024;

      //wait for reply messages from server and send them to console 
      Task.Factory.StartNew(async () =>
      {
        while (true)
        {
          var received = await socket.Receive();
          //lock (window)
          {
            if (received.packet.AckNumber == window.Window.Peek().SequenceNumber)
            {

              window.Forward();
              window.LastAck = received.packet.AckNumber;
              Console.WriteLine(String.Format((received.packet.AckNumber / (Decimal)(loop + 1)).ToString("P", CultureInfo.InvariantCulture)));
            }
          }

        }
      });

      for (int i = 0; i <= loop; i++)
      {
        Packet packet = new Packet();
        if (i == loop)
        {
          packet = new Packet(loop + 1, 0, true, false, extraBytes, Extensions.SubArray(bytes, 1024 * (loop), extraBytes));
        }
        else
        {
          packet = new Packet(i + 1, 0, false, false, Convert.ToInt32(1024), Extensions.SubArray(bytes, 1024 * i, 1024));
        }

        socket.Send(packet.BuildPacket());

        Timer timer = new Timer(1000);
        timer.Elapsed += OnTimedEvent;
        timer.Start();
        while (!window.CanForward && timer.Enabled == true)
        {

        }
        if (timer.Enabled == false)
        {
          i = window.Window.Peek().SequenceNumber - 2;
          //i = i - 5;
          //if (i < -1) { i = -1; }
        }
        else
        {
          lock (window)
          {
            if ((window.Window.Count == 0) ||
              (packet.SequenceNumber > window.Window.Min().SequenceNumber))
            {
              window.InsertPacket(packet);
            }
          }


        }
        timer.Close();
      }
    }

    private static void OnTimedEvent(Object source, ElapsedEventArgs e)
    {
      ((Timer)source).Stop();
    }
  }
}
