using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//io
using System.IO;
using System.Net;

namespace CSharpInDepth_WINFORM
{
    namespace CA10_ExpandedMethod
    {
        namespace StringTool1
        {
            public static class Program
            {
                public static void Display(this string str)
                {
                    Console.WriteLine(str);
                }
            }
        }
        namespace StringTool2
        {
            public static class Program
            {
                public static void Display(this string str)
                {
                    Console.WriteLine(str);
                }
            }
        }
        namespace OverlappingExpandedMethod
        {
            using StringTool1;
            using StringTool2;
            class Prgoram
            {
                static void Main()
                {
                    string hello = "hello";
                    StringTool2.Program.Display(hello);
                }
            }
        }
        public static class StreamUtil
        {
            const int BufferSize = 8192;
            public static void CopyTo(this Stream input,Stream output)
            {
                byte[] buffer = new byte[BufferSize];
                int read;
                while((read = input.Read(buffer,0,buffer.Length)) > 0)
                {
                    output.Write(buffer, 0, read);//返回读取的位置

                }
            }
            public static byte[] ReadFully(this Stream input)
            {
                using (MemoryStream tempStream = new MemoryStream())
                {
                    CopyTo(input, tempStream);
                    return tempStream.ToArray();
                }
            }
        }
        
        public class Program
        {
            static void Main()
            {
                //将web响应写入磁盘
                //WebRequest request=WebRequest.Create("http://manning.com");
                //using (WebResponse response = request.GetResponse())
                //using (Stream responseStream = response.GetResponseStream())
                //using (FileStream output = File.Create("response.dat"))
                //{
                //    StreamUtil.Copy(responseStream, output);
                //}
                //使用扩展方法调用：
                WebRequest request = WebRequest.Create("http://manning.com");
                using (WebResponse response = request.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                using (FileStream output = File.Create("response.dat"))
                {
                    //responseStream.CopyTo(output);//没有使用扩展方法
                    StreamUtil.CopyTo(responseStream, output);
                }
                Console.ReadKey();
            }
        }
    }
}
namespace CSharpInDepth_WINFORM
{
    namespace CA10_2_NullCall
    {
        public static class NullUtl
        {
            public static bool IsNull(this object x)
            {
                return x == null;
            }
            public class Test
            {
                static void Main()
                {
                    object y = null;
                    Console.WriteLine(y.IsNull());
                    y = new object();
                    Console.WriteLine(y.IsNull());
                    Console.ReadKey();
                }
            }
        }
    }
    namespace CA10_3_LambdaWhere
    {
        class Program
        {
            static void Main()
            {
                //var collections = Enumerable.Range(0, 10).Where(x => x % 2 != 0)
                //    .Reverse();
                //foreach(var element in collections)
                //{
                //    Console.WriteLine(element);
                //}
                //使用where扩展方法
                //foreach (string line in CSharpInDepth_WINFORM.IterationSample.EnumerateDemo.ReadLines("../../Program.cs")
                //    .Where(line => line.StartsWith("using")))
                //{
                //    Console.WriteLine(line);
                //}
                //使用Select
                var collection = Enumerable.Range(0, 10)
                    .Where(x => x % 2 != 0)
                    .Reverse()
                    .Select(x => new { Original = x, SquareRoot = Math.Sqrt(x) });
                foreach(var item in collection)
                {
                    Console.WriteLine($"sqrt({item.Original})={item.SquareRoot}");
                }
                Console.ReadKey();

            }
        }
        //过滤操作+扩展方法
        public static class ExpandedWhere
        {
            public static IEnumerable<T> Where<T>(this IEnumerable<T> source,Func<T,bool> predicate)
            {
                if(source == null || predicate ==null)
                {
                    throw new ArgumentNullException();
                }
                return WhereImpl(source, predicate);
            }
            //延迟求值：懒惰求值
            private static IEnumerable<T> WhereImpl<T>(IEnumerable<T> source,Func<T,bool> predicate)
            {
                foreach(T item in source)
                {
                    if(predicate(item))
                    {
                        yield return item;
                    }
                }
            }
            //Select
            public static IEnumerable<TOutput> Select<TInput,TOutput>(this IEnumerable<TInput> source,Func<TInput,TOutput> converter)
            {
                if(source ==null || converter == null)
                {
                    throw new ArgumentNullException();
                }
                return SelectImpl(source, converter);
            }
            private static IEnumerable<TOutput> SelectImpl<TInput, TOutput>(IEnumerable<TInput> source, Func<TInput, TOutput> converter)
            {
                foreach(TInput item in source)
                {
                    yield return converter(item);
                }
            }
            //封装一个平方根类型
            public class SquareRootType
            {
                public int Original { get; private set; }
                public double SquareRoot { get; private set; }
                public SquareRootType(int original,double squareRoot)
                {
                    Original = original;
                    SquareRoot = squareRoot;
                }
            }
            
        }
    }
}
