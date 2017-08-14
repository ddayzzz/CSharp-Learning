using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

namespace CSharpInDepth_WINFORM
{
    namespace IterationSample
    {
        public class Collection:IEnumerable
        {
            private readonly object[] values;
            private readonly int startPoint;
            public Collection(object[] values,int startPoint)
            {
                this.values = values;
                this.startPoint = startPoint;
            }
            public IEnumerator GetEnumerator()
            {
                return new CollectionIterator(this);
            }
            public object[] Values { get { return values; } }
            public int StartPoint { get { return startPoint; } }
        }
        //集合类型的迭代器
        public class CollectionIterator:IEnumerator
        {
            Collection parent;//存储数据类型的引用
            int postition;
            internal CollectionIterator(Collection parent)
            {
                this.parent = parent;
                postition = -1;//从-1开始
            }
            public bool MoveNext()
            {
                if (postition != parent.Values.Length)
                    postition++;
                return postition < parent.Values.Length;
            }
            public object Current
            {
                get
                {
                    if(postition == -1 || postition==parent.Values.Length)
                    {
                        throw new InvalidOperationException();
                    }
                    int index = postition + parent.StartPoint;
                    index = index % parent.Values.Length;
                    return parent.Values[index];
                }
            }
            public void Reset()
            {
                postition = -1;
            }
        }
        //使用yield return 的集合
        public class Collection2
        {
            static readonly string Padding = new string(' ', 30);
            static IEnumerable<int> CreateEnumerable(DateTime limit)
            {
                Console.WriteLine("{0}Start of CreateEnumerable()", Padding);
                try
                {
                    for (int i = 0; i < 100; ++i)
                    {
                        if (DateTime.Now >= limit)
                        {
                            yield break;
                        }
                        else
                        {
                            Console.WriteLine("{0}About to yield {1}", Padding, i);
                            yield return i;
                            Console.WriteLine("{0}After yield", Padding);
                        }

                    }
                }
                finally
                {
                    System.Threading.Thread.Sleep(3000);
                    Console.WriteLine("Stopped!");
                }
                Console.WriteLine("{0}Yielding final value", Padding);
                yield return -1;
                Console.WriteLine("{0}End of CreateEnumerable()", Padding);
            }
            static void Main()
            {
                //IEnumerable<int> iterable = CreateEnumerable(DateTime.Now.AddMilliseconds(500));
                //IEnumerator<int> iter = iterable.GetEnumerator();
                //while(true)
                //{
                //    Console.WriteLine("Calling MoveNext()");
                //    bool result = iter.MoveNext();
                //    Console.WriteLine("...MoveNext, result={0}", result);
                //    if(!result)
                //    {
                //        break;
                //    }
                //    Console.WriteLine("Fetching Current\n...0Current result={0}", iter.Current);
                //}
                //foreach(int i in CreateEnumerable(DateTime.Now.AddMilliseconds(100)))
                //{
                //    //Console.WriteLine("Read:{0}", i);
                //    if(i > 10)//强制结束 Dispose会在finally调用
                //    {
                //        Console.WriteLine("Returning");
                //        break;
                //    }
                //    Console.WriteLine("Read:{0}", i);
                //}
                //
                IEnumerable<int> iterable2 = CreateEnumerable(DateTime.Now.AddMilliseconds(500));
                IEnumerator<int> iter2 = iterable2.GetEnumerator();
                iter2.MoveNext();
                iter2.MoveNext();//这一次没有执行finally
                //Console.ReadKey();
            }
        }
        //enumerate示例
        class EnumerateDemo
        {
            //
            public static IEnumerable<string> ReadLines(Func<TextReader> provider)
            {
                using (TextReader reader = provider())
                {
                    string line;
                    while ((line = reader.ReadLine()) !=null)
                    {
                        yield return line;
                    }
                }
            }
            //修改2：自定义编码
            public static IEnumerable<string> ReadLines(string filename, Encoding encoding)
            {
                return ReadLines(delegate
                {
                    return new StreamReader(filename, encoding);
                });
            }
            //修改3：不添加编码
            public static IEnumerable<string> ReadLines(string filename)
            {
                return ReadLines(delegate
                {
                    return new StreamReader(filename, Encoding.UTF8);
                });
            }
        }
        class Program
        {
            static void Main()
            {
                object[] numbers = new object[] { 1, 2, 3, 4 };
                Collection collection = new Collection(numbers, 2);
                foreach(object o in collection)
                {
                    Console.WriteLine(o);
                }
                //读取文件:但是不能更改读取的参数
                Console.WriteLine("不指定编码");
                foreach (string line in EnumerateDemo.ReadLines("lines.txt"))
                {
                    Console.WriteLine(line);
                }
                Console.WriteLine("指定编码");
                foreach (string line in EnumerateDemo.ReadLines("lines.txt",Encoding.Default))
                {
                    Console.WriteLine(line);
                }
                Console.ReadKey();
            }
        }
    }
    class CA6_Iterator
    {
    }
    namespace CA6_LINQ
    {
        //使用迭代器实现LINQ 的ehrere
        public class WhereImplByIteration
        {
            public static IEnumerable<T> Where<T>(IEnumerable<T> source, Predicate<T> predict)
            {
                if(source ==null || predict ==null)
                {
                    throw new ArgumentNullException();
                }
                return WhereImpl(source, predict);
            }
            private static IEnumerable<T> WhereImpl<T>(IEnumerable<T> source,Predicate<T> predict)
            {
                foreach(T item in source)
                {
                    if(predict(item))
                    {
                        yield return item;
                    }
                }
            }
            public static void Main()
            {
                IEnumerable<string> lines = IterationSample.EnumerateDemo.ReadLines(@"../../Program.cs");
                Predicate<string> predict = delegate (string s)
                {
                    return s.StartsWith("using");
                };
                foreach(string line in Where(lines,predict))
                {
                    Console.WriteLine(line);
                }
                Console.ReadKey();
            }
        }
        //
    }
    namespace CA7_ADVANCED_INCS2
    {
        //C#2 的高级特性
        //部分方法
        partial  class PartialMethodDemo
        {
            public PartialMethodDemo()
            {
                OnConstructStart();
                OnConstructEnd();
            }
            partial void OnConstructStart();
            partial void OnConstructEnd();
            static void Main()
            {
                PartialMethodDemo pd = new PartialMethodDemo();
                
            }
        }

    }
}
