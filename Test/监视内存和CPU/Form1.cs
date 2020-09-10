using ComputerSteward.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 监视内存和CPU
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
    class MonitorThread
    {
        public delegate void refresh(string mem, string cpu, string up, string down);
        public event refresh refreshEvent;
        public ThreadStart ts;
        public Thread thread;
        public SystemInfo systeminfo;
        public MonitorThread()
        {
            systeminfo = new SystemInfo();
            ts = new ThreadStart(run);
            thread = new Thread(ts);
            thread.Start();
        }
        public void run()
        {
            while (true)
            {
                Thread.Sleep(500);
                string mem = string.Format("{0:##}%", ((float)systeminfo.MemoryAvailable / (float)systeminfo.PhysicalMemory) * 100);
                string cpu = string.Format("{0:##}%", systeminfo.CpuLoad);
                refreshEvent(mem, cpu, "", "");
            }
        }
    }
}
