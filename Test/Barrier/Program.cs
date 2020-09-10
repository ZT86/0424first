using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Barriers
{
    class Program
    {
       static Barrier bs = new Barrier(2, b => Console.WriteLine(b.CurrentPhaseNumber));
        static void Main(string[] args)
        {
            var t1 = new Thread(() => PlayMusic1("thess", "sdf", 1));
            var t2 = new Thread(() => PlayMusic2("qwes", "zxdf", 2));
            t1.Start();
            t2.Start();
        }
        static void PlayMusic1(string name ,string message,int second)
        {
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine(name + "------------------");
                Thread.Sleep(TimeSpan.FromSeconds(second));
                Console.WriteLine("name "+name+" start massage:"+message);
                Thread.Sleep(TimeSpan.FromSeconds(second));
                Console.WriteLine("name " + name + " finish to:" + message);
                bs.SignalAndWait();
                Console.WriteLine(name+"###"+bs.ParticipantsRemaining);
                if (i == 2 && bs.ParticipantsRemaining > 1)
                {
                    bs.RemoveParticipant();
                }
            }
        }
        static void PlayMusic2(string name, string message, int second)
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(name + "------------------");
                Thread.Sleep(TimeSpan.FromSeconds(second));
                Console.WriteLine("name " + name + " start massage:" + message);
                Thread.Sleep(TimeSpan.FromSeconds(second));
                Console.WriteLine("name " + name + " finish to:" + message);
                bs.SignalAndWait();
                Console.WriteLine(name + "###" + bs.ParticipantsRemaining);
                if (i == 2 && bs.ParticipantsRemaining > 1)
                {
                    bs.RemoveParticipant();//不减少会永远等待
                }
            }
        }
    }
}
