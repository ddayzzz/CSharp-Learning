using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//linq
using System.Linq.Expressions;
//反射
using System.Reflection;
namespace CSharpInDepth_WINFORM
{
    namespace CA9_Film
    {
        class Film
        {
            public string Name { get; set; }
            public int Year { get; set; }
        }
        class Program
        {
            static void Main()
            {
                var films = new List<Film>
                {
                    new Film{Name="Warwolf 2",Year=2017},
                    new Film{Name="Wonders of solar system",Year=2013},
                    new Film{Year=2016,Name="The Chinese New Year"}
                };
                Action<Film> print = film => Console.WriteLine($"Name={film.Name},Year={film.Year}");
                films.ForEach(print);
                films.FindAll(film => film.Year >= 2016).ForEach(print);
                films.Sort((f1, f2) => f1.Year.CompareTo(f2.Year) );
                films.ForEach(print);
                Console.ReadKey();
            }
        }
    }
    namespace CA9_Log
    {
        class LogDemo
        {
            static void Log(string title, object sender, EventArgs e)
            {
                Console.WriteLine("Event:{0}\n Sender:{1}\n Arguments:{2}", title, sender, e.GetType());
                foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(e))
                {
                    string name = prop.DisplayName;
                    object value = prop.GetValue(e);
                    Console.WriteLine("\t{0}={1}", name, value);
                }
               
            }
            static void Main()
            {
                Button button = new Button { Text = "Click me" };
                button.Click += (src, e) => Log("Click", src, e);
                button.KeyPress += (src, e) => Log("KeyPress", src, e);
                button.MouseClick += (src, e) => Log("MouseClick", src, e);
                Form frm = new Form { AutoSize = true, Controls = { button } };
                Application.Run(frm);
                Console.ReadKey();
            }
        }
    }
    namespace CA9_ExpressionTree
    {
        class Prgoram
        {
            static void Main()
            {
                //Expression first = Expression.Constant(2);
                //Expression second = Expression.Constant(3);
                //Expression add = Expression.Add(first, second);
                //Console.WriteLine(add);
                //表达式树编译为委托
                //Expression first = Expression.Constant(2);
                //Expression second = Expression.Constant(3);
                //Expression add = Expression.Add(first, second);
                //Func<int> compiled = Expression.Lambda<Func<int>>(add).Compile();
                //Console.WriteLine(compiled());
                //编译表达式
                //Expression<Func<int>> return5 = ()=>5;// () => { return 5; };//注意不能使语句块cs0834
                //Func<int> compiled_return5 = return5.Compile();
                //Console.WriteLine(compiled_return5());
                //用代码构造一个方法调用表达式树
                MethodInfo method = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });//通过反射获取string类型的方法StartsWith
                var target = Expression.Parameter(typeof(string), "x");
                var methodArg = Expression.Parameter(typeof(string), "y");
                Expression[] methodArgs = new Expression[] { methodArg };
                Expression call = Expression.Call(target, method, methodArgs);
                var lambdaParameters = new[] { target, methodArg };
                var lambda = Expression.Lambda<Func<string, string, bool>>(call, lambdaParameters);
                var compiled = lambda.Compile();
                
                Console.WriteLine(compiled("First", "Second"));
                Console.WriteLine(compiled("First", "Fir"));
                Console.ReadKey();
            }
        }
    }
    namespace CA9_AnonymousTypeDeduce
    {
        class Program
        {
            delegate T MyFunc<T>();
            static void WriteResult<T>(MyFunc<T> function)
            {
                Console.WriteLine(function());

            }
            //例子9-11
            static void PrintConvertedValue<TInput,TOutput>(TInput input, Converter<TInput,TOutput> converter)
            {
                Console.WriteLine(converter(input));
            }
            //例子9-15 多级类型推断
            static void ConvertTwice<TInput,TMiddle,TOutput>(
                TInput input,
                Converter<TInput,TMiddle> first,
                Converter<TMiddle,TOutput> second)
            {
                TMiddle middle = first(input);
                TOutput output = second(middle);
                Console.WriteLine(output);
            }
            //lambda返回不同的类型
            static T ReturnDifferentType<T>(Func<T> fcn)
            {
                return fcn();
            }
            static void Main()
            {
                //WriteResult(delegate { return 5; });
                //将没有指定参数名的lambda表达式传给泛型的方法
                //PrintConvertedValue("20", x => x.Length);
                //多级推断
                ConvertTwice("Stri",
                    text => text.Length,
                    length => Math.Sqrt(length));
                var oo=ReturnDifferentType(delegate
                {
                    if (DateTime.Now.Hour < 11)
                        return 10;
                    else
                        return 10.2;
                });
                Console.ReadKey();
            }
        }
    }
}
