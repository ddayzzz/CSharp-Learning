using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net;
using System.Threading;

namespace Async
{
    public partial class AsyncForm : Form
    {
        public AsyncForm()
        {
            InitializeComponent();
            //添加事件
            //代码15-1
            button1.Click += DisplayWebLength;
            //button1.Click += NoAsyncDisplayWebLength;//非异步
            //异步流程
            //button1.Click += Button1_Click;
        }
        //按钮1的Click方法
        async void Button1_Click(object sender, EventArgs e)
        {
            //label1.Text = "Fetching...";
            //await FetchFirstSuccessfulAsync(new[] { "http://11111ddayzzz.win", "https://111www.baidu.com" });
            DoWork(SynchronizationContext.Current, progressBar1, sender, e);
        }
        //异步显示网页的长度
        async static void DisplayWebLength(object sender,EventArgs e)
        {
            //label1.Text = "Fetching...";
            using (HttpClient client = new HttpClient())
            {
                Task<string> getLength = client.GetStringAsync("https://www.facebook.com");
                //label1.Text = (await getLength).Length.ToString();
                MessageBox.Show(text: (await getLength).Length.ToString());


            }
        }
        //非异步显示网页的长度
        void NoAsyncDisplayWebLength(object sender, EventArgs e)
        {
            label1.Text = "Fetching...";
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                string text = client.DownloadString("http://csharpindepth.com");
                label1.Text = text.Length.ToString();

            }
        }
        //调用方法
        static void PrintPageLength(object sender, EventArgs e)
        {
            Task<int> lengthTask = GetPageLengthAsync("https://www.baidu.com");
            MessageBox.Show(lengthTask.Result.ToString());
        }
        //异步方法:返回值包装为Task<int>
        static async Task<int> GetPageLengthAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                Task<string> fetchTextTask = client.GetStringAsync(url);
                int length = (await fetchTextTask).Length;
                return length;
            }
        }
        //异步方法：查找第一个可以访问的URL
        async Task<string> FetchFirstSuccessfulAsync(IEnumerable<string> urls)
        {
            foreach(string url in urls)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        return await client.GetStringAsync(url);
                    }
                }
                catch(WebException exception)
                {
                    label1.Text = label1.Text + string.Format("\n{0} failed", url);
                }
            }
            throw new WebException("No URLs succeeded");
        }
        //SynchronizationContext和ExecutionContext
        public static void DoWork(SynchronizationContext sc,object prog,object sender,EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                //在线程池上工作
                DisplayWebLength(sender, e);
                ChangeProgressBar(sc, prog);
            });
        }
        public static void ChangeProgressBar(SynchronizationContext sc,object prog)
        {
            sc.Post(delegate
            {

                //在UI上工作
                //Thread.Sleep(2000);
                ProgressBar progressBar = prog as ProgressBar;
                progressBar.Value = 20;
            }, prog);
        }
    }
}
