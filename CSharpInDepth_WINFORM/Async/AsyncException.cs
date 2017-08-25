using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace Async
{
    //class AsyncException
    //{
    //    public static AggregateExceptionAwaitable WithAggregatedExceptions(this Task task)
    //    {
    //        return new AggregateExceptionAwaitable(task);
    //    }
    //    public AggregateExceptionAwaiter GetAwaiter
    //}
    class AsyncExceptionIO
    {
        //code 15-4
        static async Task MainAsync()
        {
            Task<string> task = ReadFileAsync("df");
            try
            {
                string text = await task;
                Console.WriteLine("File contents:{0}", text);
            }
            catch(System.IO.IOException e)
            {
                Console.WriteLine("Caught IOException:{0}", e.Message);
            }
        }
        static async Task<string> ReadFileAsync(string filename)
        {
            using (var reader = File.OpenText(filename))
            {
                return await reader.ReadToEndAsync();
            }
        }
        //code15-7
        static async Task ThrowCancellationException()
        {
            throw new OperationCanceledException();
        }
        //code 15-8
        static async Task DelayFor30Seconds(CancellationToken token)
        {
            try
            {
                Console.WriteLine("Waiting for 30 seconds");
                await Task.Delay(TimeSpan.FromSeconds(30), token);
            }
            catch(OperationCanceledException e)
            {
                Console.WriteLine("Caught :{0}", e.Message);
                throw e;//重新抛出
            }

        }
        //code 15-10
        static Task<int> ComputeLengthAsync(string text)
        {
            if(text == null)
            {
                throw new ArgumentNullException("text");
            }
            Func<Task<int>> func = async () =>
            {
                await Task.Delay(500);
                return text.Length;
            };
            return func();
        }
        static void Main()
        {
            //MainAsync();
            //代码15-8
            //var source = new CancellationTokenSource();
            //var task = DelayFor30Seconds(source.Token);//30s延迟任务
            //source.CancelAfter(TimeSpan.FromSeconds(1));//1s后取消执行
            //Console.WriteLine("Initial status: {0}", task.Status);
            //try
            //{
            //    task.Wait();// 等于task.Result
            //}
            //catch(AggregateException e)
            //{
            //    Console.WriteLine("Caught {0}", e.InnerExceptions[0]);
            //}
            //Console.WriteLine("Final status: {0}", task.Status);
            //code15-9
            Func<int, Task<int>> function = async x =>
            {
                Console.WriteLine("Starting...x={0}", x);
                await Task.Delay(x * 1000);
                Console.WriteLine("Finished...x={0}", x);
                return x * 2;
            };
            Task<int> first = function(5);
            Task<int> second = function(3);
            Console.WriteLine("First result : {0}", first.Result);
            Console.WriteLine("Second result : {0}", second.Result);
            Console.ReadKey();
        }
    }
}
