using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//基于文章：https://www.codeproject.com/Articles/152765/Task-Parallel-Library-1-of-n
namespace TPL_FromCodeProject
{
    class Program
    {

        private static string PrintTaskObjectState(object state)
        {
            Console.WriteLine(state.ToString());
            return "***WOWSERS***";
        }
        static void Main(string[] args)
        {
            //第一步：创建使用内联的action
            Task<List<int>> taskWithInlineAction = new Task<List<int>>(() =>
                  {
                      List<int> ints = new List<int>();
                      for (int i = 0; i < 1000; ++i)
                      {
                          ints.Add(i);
                      }
                      return ints;
                  });
            //第二步：创建一个任务，他用于调用返回一个stirng的方法
            Task<string> taskWithInActualMethodAndState =
                new Task<string>(new Func<object, string>(PrintTaskObjectState), "This is the task state, could be any object");
            //第三步：创建并运行一个使用了Task.Factory返回List<int>的任务
            Task<List<int>> taskWithFactoryAndState = Task.Factory.StartNew((stateObj) =>
            {
                List<int> ints = new List<int>();
                for (int i = 0; i < (int)stateObj; ++i)
                {
                    ints.Add(i);
                }
                return ints;
            }, 2000);
            taskWithInlineAction.Start();
            taskWithInActualMethodAndState.Start();

            //等待所有执行完毕
            Task.WaitAll(new Task[]
            {
                taskWithInlineAction,
                taskWithInActualMethodAndState,
                taskWithFactoryAndState
            });
            //打印结果
            var taskWithInLineActionResult = taskWithInlineAction.Result;
            Console.WriteLine(string.Format(
                "The task which called a method returned '{0}'",
                taskWithInlineAction.GetType(),
                taskWithInLineActionResult.Count));
            taskWithInlineAction.Dispose();

            //print results for taskWithInActualMethodAndState
            var taskWithInActualMethodResult = taskWithInActualMethodAndState.Result;
            Console.WriteLine(string.Format(
                "The task which called a Method returned '{0}'",
            taskWithInActualMethodResult.ToString()));
            taskWithInActualMethodAndState.Dispose();

            //print results for taskWithFactoryAndState
            var taskWithFactoryAndStateResult = taskWithFactoryAndState.Result;
            Console.WriteLine(string.Format(
                "The task with Task.Factory.StartNew<List<int>> " +
                "returned a Type of {0}, with {1} items",
                taskWithFactoryAndStateResult.GetType(),
                taskWithFactoryAndStateResult.Count));
            taskWithFactoryAndState.Dispose();

            Console.WriteLine("All done, press Enter to Quit");
            Console.ReadKey();
        }
    }
    class Cancellation
    {
        static void Main()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token1=tokenSource.Token;
            Task<List<string>> someTask = Task.Factory.StartNew<List<string>>((website) =>
              {
                  System.Net.WebClient wc = new System.Net.WebClient();
                  if (token1.IsCancellationRequested)
                  {
                      wc.Dispose();
                      throw new OperationCanceledException(token1);
                  }
                  else
                  {
                      string webContent = wc.DownloadString((string)website);
                      return webContent.Split(
                          new string[] { " ", ",", "，" },
                          Int16.MaxValue,
                          StringSplitOptions.None).ToList();
                  }
              }, "http://www.codeproject.com", token1);
            tokenSource.Cancel();
            try
            {
                foreach (string item in someTask.Result)
                {
                    Console.WriteLine(item);
                }
            }
            catch(AggregateException e)
            {
                foreach (Exception ex in e.InnerExceptions)
                {
                    Console.WriteLine(
                string.Format("Caught exception '{0}'", ex.Message));
                }
            }
            finally
            {
                someTask.Dispose();
            }
            Console.ReadKey();
        }
    }
    class WinFormMain
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DownloadDemo());
        }
    }
    //任务出错
    class TasksWithException
    {
        static void Main()
        {
            Task<int> taskWithException = Task.Factory.StartNew<int>((stateObj) =>
              {

                  for(int i=0;i<(int)stateObj;++i)
                  {
                      if (i > 1000)
                      {
                          throw new InvalidOperationException("stateobj > 1000");
                      }
                  }
                  return (int)stateObj;
              }, 1002);
            Task<int> taskWithoutException = Task.Factory.StartNew<int>((stateObj) =>
            {
                for (int i = 0; i < (int)stateObj; ++i)
                {
                    if (i > 1000)
                    {
                        throw new InvalidOperationException("stateobj > 1000");
                    }
                }
                return (int)stateObj;
            }, 999);
            //上面两个任务已经执行了
            //List<Task<int>> tasks = new List<Task<int>> { taskWithException, taskWithoutException };
            //Task.Factory.ContinueWhenAll(tasks.ToArray(), (results) =>
            // {
            //     try
            //     {
            //         foreach (var result in results)
            //         {
            //             Console.WriteLine("Result:{0}", result);
            //         }
            //     }
            //     catch(AggregateException e)
            //     {

            //     }
            // });
            taskWithException.ContinueWith((ant) =>
            {
                AggregateException ex = ant.Exception;
                Console.WriteLine("OOOOPS:Exception(s) occured");
                foreach (var e in ex.InnerExceptions)
                {
                    Console.WriteLine("\tMessage:{0}", e.Message);
                }
            }, TaskContinuationOptions.OnlyOnFaulted);
            Console.ReadKey();
        }
    }
    //任务传递
    class TasksPipeline
    {
        static void Main()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();//取消任务的信号

            CancellationToken token = tokenSource.Token;
            try
            {
                Task<List<int>> task1 = Task.Factory.StartNew((stateObj) =>
                {
                    List<int> ints = new List<int>();
                    for (int i = 0; i < (int)stateObj; ++i)
                    {
                        token.ThrowIfCancellationRequested();
                        ints.Add(i);
                        Thread.Sleep(800);
                    }
                    return ints;
                }, 3);
                task1.ContinueWith<List<int>>((ant) =>//延续任务的是一个新的Task
                {
                    if (ant.Status == TaskStatus.Faulted)
                    {
                        throw ant.Exception.InnerException;
                    }
                    Console.WriteLine("Parent task finished successfully");
                    List<int> parentList = ant.Result;
                    List<int> subs = new List<int>();
                    foreach (int restult in parentList)
                    {
                        Console.WriteLine("Sub computing:{0}", restult);
                        subs.Add(restult * restult);
                    }
                    return subs;
                }, TaskContinuationOptions.OnlyOnRanToCompletion).ContinueWith((ant) =>
                {
                    foreach (var item in ant.Result)
                    {
                        Console.WriteLine("Sub result:{0}", item);
                    }
                }, TaskContinuationOptions.OnlyOnRanToCompletion);

                Task cancelthread = Task.Factory.StartNew((time) =>
                {
                    Thread.Sleep((int)time);
                    Console.WriteLine("Cancel");
                    tokenSource.Cancel();
                }, 2000);

                task1.Wait();//不要结束try中的线程。等待任务运行完成
            }
            catch(AggregateException ex)
            {
                Console.WriteLine("Caught error(s):1");
                foreach(var e in ex.InnerExceptions)
                {
                    Console.WriteLine("\t{0}", e.Message);
                }
            }
            Console.ReadKey();
        }
    }
}
