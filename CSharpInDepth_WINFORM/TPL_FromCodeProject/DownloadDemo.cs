using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Net.Http;

namespace TPL_FromCodeProject
{
    public partial class DownloadDemo : Form
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            
        public DownloadDemo()
        {
            InitializeComponent();
        }
        //ref1:https://blogs.msdn.microsoft.com/dotnet/2012/06/06/async-in-4-5-enabling-progress-and-cancellation-in-async-apis/
        private async Task GetPageLength(string[] urls, CancellationToken ct, IProgress<int> progress)
        {
            //async和await和UI线程相同
            try
            {

                for (int i = 0; i < urls.Length; ++i)
                {
                    if(ct.IsCancellationRequested)
                    {
                        //MessageBox.Show("取消");
                        ct.ThrowIfCancellationRequested();
                    }
                    else
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            int size = (await client.GetStringAsync(urls[i])).Length;
                            this.listBox1.Items.Add(string.Format("{0}:{1}字节", urls[i], size));
                        }
                        if (progress != null)
                        {
                            progress.Report((int)((float)(i + 1) * 100 / urls.Length));
                        }
                    }
                }
                MessageBox.Show("完成");


            }
            catch (OperationCanceledException ex)
            {
                MessageBox.Show("成功取消了");
            }

        }
        void ReportProgress(int value)
        {
            progressBar1.Value = value;
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Value = 0;
            listBox1.Items.Clear();
            CancellationToken token = cancellationTokenSource.Token;
            string[] strs = this.textBox1.Lines;
            Progress<int> progress = new Progress<int>(ReportProgress);
            await GetPageLength(strs, token, progress);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();
        }
    }
}
