using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiskFiller
{
    public partial class Form1 : Form
    {
        private string rootPath;
        private bool stop = false;
        private List<Thread> Threads = new List<Thread>();
        private Stopwatch stopwatch;
        private int bytesWritten = 0;
        private System.Timers.Timer timer;

        public Form1()
        {
            InitializeComponent();
            this.textBox1.Text = Directory.GetCurrentDirectory();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.stop = false;
            this.bytesWritten = 0;
            rootPath = this.textBox1.Text;
            var numThreads = System.Environment.ProcessorCount;

            for (int i = 0; i < numThreads; i++)
            {
                this.spawnThread();
            }

            MessageBox.Show("Spawned " + numThreads + " threads.");

            this.stopwatch = new Stopwatch();
            this.timer = new System.Timers.Timer();
            this.timer.Interval = TimeSpan.FromSeconds(1).TotalMilliseconds;
            this.timer.Elapsed += (o, e2) =>
            {
                int mb = 60 * this.bytesWritten / 1024 / 1024;
                this.lblStatus.Text = string.Format("Writing {0}mb per minute", mb);
                this.bytesWritten = 0;
            };
            this.timer.Start();
        }

        private void spawnThread()
        {
            var thread = new Thread(new ThreadStart(() =>
            {
                // Random path
                string myPath = Path.Combine(rootPath, Guid.NewGuid().ToString());
                while (stop == false)
                {
                    using (FileStream f = new FileStream(myPath, FileMode.Append, FileAccess.Write))
                    {
                        using (StreamWriter s = new StreamWriter(f))
                        {
                            s.Write(RandomString(1024378)); // seems to give the best performance in terms of bytes per second
                            this.bytesWritten += 1024378;
                        }
                    }
                }
            }));

            thread.Start();
            this.Threads.Add(thread);
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.timer.Stop();
            this.stop = true;
            foreach (var thread in this.Threads)
            {
                thread.Abort();
            }

            MessageBox.Show("Stopped.");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
