using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth_WINFORM
{
    class CA16
    {
        static void Main()
        {
            string[] strings = new string[] { "x", "y", "z" };
            List<Action> actions = new List<Action>();
            foreach(string value in strings)
            {
                actions.Add(() => Console.WriteLine(value));

            }
            foreach(var action in actions)
            {
                action();
            }
            Console.ReadKey();
        }
    }
}
