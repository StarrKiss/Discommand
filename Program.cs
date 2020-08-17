using System;

namespace discBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Bot bot = new Bot();
            bot.runAsync().GetAwaiter().GetResult();
            
        }
    }
}
