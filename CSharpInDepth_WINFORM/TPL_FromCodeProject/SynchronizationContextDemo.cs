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
namespace TPL_FromCodeProject
{
    public partial class SynchronizationContextDemo : Form
    {
        public SynchronizationContextDemo()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task taskWithFactoryAndState1 = Task.Factory.StartNew((stateObj) =>
              {
                  List<int> ints = new List<int>();
                  for (int i = 0; i < (int)stateObj; ++i)
                  {
                      //Thread.Sleep(1000);
                      ints.Add(i);
                  }
                  return ints;
              }, 10000).ContinueWith(ant =>
             {
                  listBox1.DataSource = ant.Result;//延续的任务
              }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
