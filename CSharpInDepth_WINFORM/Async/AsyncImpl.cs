using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;

namespace Async
{
    class AsyncImpl
    {
        static async Task<int> SumCharactersAsync(IEnumerable<char> text)
        {
            int total = 0;
            foreach (char ch in text)
            {
                int unicode = ch;
                await Task.Delay(unicode);
                total += unicode;
            }
            await Task.Yield();
            return total;
        }
    }
    //CA15.5.3 模拟状态机
    class StateMchineImpl
    {
        [CompilerGenerated]
        private struct DemoStateMachine:IAsyncStateMachine
        {
            public IEnumerable<char> text;
            public IEnumerator<char> iterator;
            public char ch;
            public int total;
            public int unicode;
            private TaskAwaiter taskAwaiter;
            private YieldAwaitable.YieldAwaiter yieldAwaiter;
            public int state;
            public AsyncTaskMethodBuilder<int> builder;
            private object stack;

            void IAsyncStateMachine.MoveNext()
            {

            }
            [DebuggerHidden]
            void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
            {
                builder.SetStateMachine(stateMachine);
            }
        }
        static void Main()
        {
            Task task = DemonstrateStacks();
            task.Wait();
        }

        static async Task DemonstrateStacks()
        {
            int y = 10;
            Task<int> z = Task.FromResult(10);
            var x = y * await z;

            Task<int> task = Task.FromResult(20);
            Console.WriteLine("{0} {1}", x, await task);
        }
    }
    //CA15 测试configureawait
    namespace TestConfigureAwait
    {
        class TestClass
        {
            public int ID { get; private set; }
            public TestClass(int id)
            {
                ID = id;
            }
        }
        class Program
        {
            static async Task DisplayID(TestClass testClass)
            {
                Console.WriteLine(testClass.ID);
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
            static void Main()
            {
                TestClass tc = new TestClass(20);
                var task = DisplayID(tc);
                task.ConfigureAwait(continueOnCapturedContext: true);
                task.Wait();
                Console.ReadKey();
            }
        }
    }
    //CA15.6.2 组合异步操作
    static class CA15_6_2
    {
        //处理异常
        public static AggregatedExceptionAwaitable WithAggregatedExceptions(this Task task)
        {
            return new AggregatedExceptionAwaitable(task);
        }
        public struct AggregatedExceptionAwaitable
        {
            private readonly Task task;

            internal AggregatedExceptionAwaitable(Task task)
            {
                this.task = task;
            }

            public AggregatedExceptionAwaiter GetAwaiter()
            {
                return new AggregatedExceptionAwaiter(task);
            }
        }
        public struct AggregatedExceptionAwaiter : ICriticalNotifyCompletion
        {
            private readonly Task task;

            internal AggregatedExceptionAwaiter(Task task)
            {
                this.task = task;
            }

            // Delegate most members to the task's awaiter
            public bool IsCompleted { get { return task.GetAwaiter().IsCompleted; } }

            public void UnsafeOnCompleted(Action continuation)
            {
                task.GetAwaiter().UnsafeOnCompleted(continuation);
                
            }

            public void OnCompleted(Action continuation)
            {
                task.GetAwaiter().OnCompleted(continuation);
            }

            public void GetResult()
            {
                // This will throw AggregateException directly on failure,
                // unlike task.GetAwaiter().GetResult()
                task.Wait();
            }
        }
        static async Task MainAsync()
        {
            var urls = new List<string>
            {
                "https://www.baidu.com",
                
                "https://www.google.com.hk"
            };
            var tasks = urls.Select(async url =>
            {
                using (var client = new HttpClient())
                {
                    if (url == "throw")
                    {
                        Console.WriteLine("Faild to read from \"throw\"");
                        throw new ArgumentException("throw sample");
                    }
                    else
                    {
                        return await client.GetStringAsync(url);
                    }
                }
            }).ToList();
            //string[] results = await Task.WhenAll(tasks);//只要抛出一个错误就会终止其他的异步操作
            //foreach (string sz in results)
            //{
            //    Console.WriteLine("HTML:{0}\n\n\n", sz);
            //}

            //只会等待第一个成功的请求
            //Task<string> successReq = await Task.WhenAny(tasks);
            //Console.WriteLine(successReq.Result);

            //捕获错误
            try
            {
                
                await Task.WhenAll(tasks).WithAggregatedExceptions();
                foreach(Task<string> task in tasks)
                {
                    Console.WriteLine(task.Status);
                    
                }
            }
            catch(AggregateException e)
            {
                Console.WriteLine("Caught:{0}, exceptions:{1}", e.InnerExceptions.Count,
                    string.Join(", ",
                    e.InnerExceptions.Select(x => x.Message)));
            }
        }
        //代码15-12 将未完成的异步操作附加延续操作
        public static IEnumerable<Task<T>> InCompletionOrder<T>(this IEnumerable<Task<T>> source)
        {
            var inputs = source.ToList();
            var boxes = inputs.Select(x => new TaskCompletionSource<T>()).ToList();

            int currentIndex = -1;
            foreach(var task in inputs)
            {
                task.ContinueWith(completed =>
                {
                    var nextBox = boxes[Interlocked.Increment(ref currentIndex)];
                    PropagateResult(completed, nextBox);
                }, TaskContinuationOptions.ExecuteSynchronously/*确保能够在开始后续操作，任务已经执行*/);
            }
            return boxes.Select(box => box.Task);
        }
        //ref:https://github.com/jskeet/DemoCode/blob/master/AsyncIntro/Code/Testing.MsTest/MagicOrdering.cs
        /// <summary> 
        /// Propagates the status of the given task (which must be completed) to a task completion source 
        /// (which should not be). 
        /// </summary> 
        private static void PropagateResult<T>(Task<T> completedTask,
            TaskCompletionSource<T> completionSource)
        {
            switch (completedTask.Status)
            {
                case TaskStatus.Canceled:
                    completionSource.TrySetCanceled();
                    break;
                case TaskStatus.Faulted:
                    completionSource.TrySetException(completedTask.Exception.InnerExceptions);
                    break;
                case TaskStatus.RanToCompletion:
                    completionSource.TrySetResult(completedTask.Result);
                    break;
                default:
                    throw new ArgumentException("Task was not completed");
            }
        }
        static async Task<int> ShowPageLengthsAsync(params string[] urls)
        {
            var tasks = urls.Select(async url =>
            {
                using (HttpClient client = new HttpClient())
                {
                    return await client.GetStringAsync(url);
                }
            }).ToList();
            int total = 0;
            foreach(var task in tasks.InCompletionOrder())
            {
                string page = await task;
                Console.WriteLine("Got page length {0}", page.Length);
                total += page.Length;
            }
            return total;
        }
        static void Main()
        {
            
            //MainAsync();
            //使用diamagnetic15-12
            var getLengthTasks = ShowPageLengthsAsync("https://www.google.com.hk",
                "https://www.baidu.com",
                "http://ddayzzz.win","http://www.ccc");
            try
            {
                int totalSize = getLengthTasks.Result;
                Console.WriteLine("Total page size:{0}", totalSize);
            }
            catch(AggregateException e)
            {
                Console.WriteLine("Caught:{0}", e.InnerExceptions.Count);
                foreach(var err in e.InnerExceptions)
                {
                    Console.WriteLine("\tError message:{0}", err.Message);
                }
            }
            
           
            Console.ReadKey();
        }
    }
    
}
