using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace CSharpInDepth_WINFORM
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
    namespace MethodGroup
    {
        class Event
        {
            static void LogPlainEvent(object sender, EventArgs e)
            {
                //Console.WriteLine("An event occurred");
                MessageBox.Show("Naive");
            }
            static void Main()
            {
                Button btn = new Button();
                btn.Text = "Click me";
                btn.MouseClick += LogPlainEvent;
                Form form = new Form();
                form.Controls.Add(btn);
                Application.Run(form);
            }
        }
        namespace DifferentMethod
        {
            public class Base
            {
                public void Sample(int s)
                {
                    Console.WriteLine("Base");
                }
            }
            public class Derived:Base
            {
                public void Sample(string s)//不是覆盖，与Base.Sample成为方法组
                {
                    Console.WriteLine("Derived");
                }
            }
            public class FileSortDemo
            {
                //匿名方法：文件排序
                public static void SortAndShowFile(string title, Comparison<FileInfo> sortOrderPred)
                {
                    FileInfo[] files = new DirectoryInfo(@"C:\Program Files\Dolby\Dolby DAX2\DAX2_APP").GetFiles();
                    Array.Sort(files, sortOrderPred);
                    Console.WriteLine(title);
                    foreach (FileInfo fi in files)
                    {
                        Console.WriteLine("{0} ({1} bytes)", fi.Name, fi.Length);
                    }
                }
                public static void Main()
                {
                    SortAndShowFile("Sort by name", delegate (FileInfo f1, FileInfo f2) { return f1.Name.CompareTo(f2.Name); });
                    SortAndShowFile("Sort by size", delegate (FileInfo f1, FileInfo f2) { return f1.Length.CompareTo(f2.Length); });
                    Console.ReadKey();
                }
            }
            class Prgoram
            {
                public static void Main()
                {
                    Base b = new Base();
                    Derived d = new Derived();
                    b.Sample(25);
                    d.Sample(25);
                    //将匿名方法用于Action<T>委托类型
                    Action<string> printReverse = delegate (string text)//IL创建方法instance void CSharpInDepth_WINFORM.MethodGroup.DifferentMethod.Prgoram/'<>c'::'<Main>b__0_0'(string)

                    {
                        char[] chars = text.ToCharArray();
                          Array.Reverse(chars);
                          Console.WriteLine(new string(chars));
                      };
                    Action<int> printRoot = delegate (int number)//Action<object>就不行，匿名方法不支持逆变性
                    {
                        Console.WriteLine(Math.Sqrt(number));//求一个数字的平方根
                    };
                    Action<IList<double>> printMean = delegate (IList<double> numbers)
                      {
                          
                          double total = 0.0;
                          foreach (double value in numbers)
                          {
                              total += value;
                          }
                          Console.WriteLine(total / numbers.Count);
                      };
                    printReverse("Hello");
                    printRoot(2);
                    printMean(new double[] { 1, 2, 3, 4, 5, 6 });
                    //独占一行的匿名方法
                    List<int> numbers2 = new List<int>();//printMean的numbers参数的作用域与变量级别相同
                    numbers2.Add(1);
                    numbers2.Add(2);
                    numbers2.ForEach(delegate (int n) { Console.WriteLine(n); });
                    //省略的委托参数
                    //var th = new System.Threading.Thread(delegate {; });//二义性 需要制定委托的类型
                    var th_enforce = new System.Threading.Thread((ThreadStart)(delegate {; }));
                    Console.ReadKey();
                }
                
            }
            public class ClosureDemo
            {
                //多个变量捕获多个对象的实例
                static void Main()
                {
                    List<MethodInvoker> list = new List<MethodInvoker>();
                    for(int index=0;index < 5;++index)
                    {
                        int counter = index * 10;
                        list.Add(delegate
                        {
                            Console.WriteLine($"{counter}");
                            counter++;
                        });
                    }
                    foreach(MethodInvoker invoker in list)
                    {
                        invoker();
                    }
                    list[0]();
                    list[0]();
                    list[0]();
                    list[1]();
                    Console.ReadKey();
                }
            }
        }
    }
}
