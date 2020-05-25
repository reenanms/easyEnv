using System;

namespace EasyEnv
{
    public class ConsoleNotifier : INotifier
    {
        public void Notify(string message)
        {
            Console.WriteLine(message);
        }
    }
}
