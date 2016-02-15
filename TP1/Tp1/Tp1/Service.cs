using System;

namespace Tp1
{
  class Service
  {
    static void Main(string[] args)
    {
      int choiceRole;
      bool parsed;

      do
      {
        Console.WriteLine("Quel rôle souhaitez-vous jouer?\n1) Client\n2) Serveur");
        parsed = Int32.TryParse(Console.ReadLine(), out choiceRole);
      } while (!parsed || (choiceRole != 1 && choiceRole != 2));

      if (choiceRole == 1)
      {
        Client client = new Client();
        client.start();
      }

      else
      {
        Server server = new Server();
        server.start();
      }
      Console.ReadLine();
    }
  }
}
