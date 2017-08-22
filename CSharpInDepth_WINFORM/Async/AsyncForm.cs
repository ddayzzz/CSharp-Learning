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
        }
        //异步显示网页的长度
        async void DisplayWebLength(object sender,EventArgs e)
        {
            label1.Text = "Fetching...";
            using (HttpClient client = new HttpClient())
            {
                string text = await client.GetStringAsync("http://csharpindepth.com");
                label1.Text = text.Length.ToString();
                
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
    }
}
