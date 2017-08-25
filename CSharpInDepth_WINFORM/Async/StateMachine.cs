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
namespace Demo
{
    class App
    {
        async static Task DisplayWebLength()
        {
            using (HttpClient client = new HttpClient())
            {
                Task<string> getLength = client.GetStringAsync("https://www.facebook.com");
                MessageBox.Show(text: (await getLength).Length.ToString());
            }
        }
        static void Main()
        {
            Task task = DisplayWebLength();
            task.Wait();
        }
    }
}