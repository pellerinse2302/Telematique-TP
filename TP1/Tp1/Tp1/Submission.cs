using System;

namespace Tp1
{
    class Submission
    {
        public void submit(String filename)
        {
            byte[] bytes = System.IO.File.ReadAllBytes(filename);
        }
    }
}
