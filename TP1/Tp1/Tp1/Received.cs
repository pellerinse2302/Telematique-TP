using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Tp1
{
    struct Received
    {
        public IPEndPoint Sender;
        public string Message;
        public Packet packet;
    }
}
