using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSahrp_in_depth
{
    class Method
    {
        static void SimpleInvoke(Control control,MethodInvoker invoker)
        {
            control.Invoke(invoker);
        }
        static void Message()
        {
            //MessageBox("Heee", "NAIVE");
        }
        static void Main()
        {
            Form frm = new Form();
            SimpleInvoke(frm, Message);
            MethodInvoker invoker = Message;
            frm.Invoke((MethodInvoker)invoker);
            frm.ShowDialog();
        }
    }
}
