using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;

namespace BackgroundWorkers
{
    class Program
    {
        static void Main(string[] args)
        {
            var bw = new BackgroundWorker();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;

            bw.DoWork += Worker_Dowork;
            bw.ProgressChanged += Work_ProgressChanged;
            bw.RunWorkerCompleted += Work_completet;
            bw.RunWorkerAsync();
            Console.WriteLine("Press C to cancel work");
            do
            {
                if (Console.ReadKey(true).KeyChar == 'C')
                {
                    bw.CancelAsync();
                }
            } while (bw.IsBusy);
        }

        public static void Worker_Dowork(object sender, DoWorkEventArgs e)
        {
            var bw = (BackgroundWorker)sender;
            for (int i = 0; i < 100; i++)
            {
                if (bw.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                if (i % 10 == 0)
                {
                    bw.ReportProgress(i);
                }
                Thread.Sleep(TimeSpan.FromSeconds(0.1));
            }
            e.Result = 42;
        }
        public static void Work_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Console.WriteLine(e.ProgressPercentage + " completed.");
        }
        public static void Work_completet(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("Complete");
            if (e.Error != null)
            {
                Console.WriteLine("异常");
            }
            else if (e.Cancelled)
            {
                Console.WriteLine("被取消");
            }
            else
            {
                Console.WriteLine(e.Result + "结果");
            }
        }
    }
}
