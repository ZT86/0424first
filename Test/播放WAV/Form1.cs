using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 播放WAV
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

        private void button1_Click(object sender, EventArgs e)
        {
            string path = @"F:\NG.wav";//.wav音频文件路径
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(path);
            player.Play();//简单播放一遍
            //player.PlayLooping();//循环播放
            //player.PlaySync();//另起线程播放
        }
    }
}
