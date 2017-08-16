using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth_WINFORM
{
    using CSharpInDepth_WINFORM.CA11_LINQ_To_Objects.SkeetySoft;
    using SampleData = CSharpInDepth_WINFORM.CA11_3_SampleData;
    namespace CA11_LINQ_To_Objects
    {
        //定义数据成员
        namespace SkeetySoft
        {
            public enum Status { Created, Accepted, Fixed, Reopened, Closed };
            public enum Severity { Trivial, Minor, Major, Showstopper };
            public enum UserType { Customer, Developer, Tester, Manager };
            public class NotificationSubscription
            {
                /// <summary>
                /// Project for which this subscriber is notified
                /// </summary>
                public Project Project { get; set; }

                /// <summary>
                /// The address to send the notification to
                /// </summary>
                public string EmailAddress { get; set; }
            }
            public class User
            {
                public string Name { get; set; }
                public UserType UserType { get; set; }

                public User(string name, UserType userType)
                {
                    Name = name;
                    UserType = userType;
                }

                public override string ToString()
                {
                    return string.Format("User: {0} ({1})", Name, UserType);
                }
            }
            public static class StaticCounter
            {
                static int next = 1;

                public static int Next()
                {
                    return next++;
                }
            }
            public class Project
            {
                public string Name { get; set; }

                public override string ToString()
                {
                    return string.Format("Project: {0}", Name);
                }
            }
            public class Defect
            {
                public Project Project { get; set; }
                /// <summary>
                /// Which user is this defect currently assigned to? Should not be null until the status is Closed.
                /// </summary>
                public User AssignedTo { get; set; }
                public string Summary { get; set; }
                public Severity Severity { get; set; }
                public Status Status { get; set; }
                public DateTime Created { get; set; }
                public DateTime LastModified { get; set; }
                public User CreatedBy { get; set; }
                public int ID { get; private set; }

                public Defect()
                {
                    ID = StaticCounter.Next();
                }

                public override string ToString()
                {
                    return string.Format("{0,2}: {1}\r\n    ({2:d}-{3:d}, {4}/{5}, {6} -> {7})",
                        ID, Summary, Created, LastModified, Severity, Status, CreatedBy.Name,
                        AssignedTo == null ? "n/a" : AssignedTo.Name);
                }
            }
        }
        public static class Extensions
        {
            public static Dummy<T> Where<T>(this Dummy<T> dummy, Func<T, bool> predicate)
            {
                Console.WriteLine("Where called");
                return dummy;
            }
            public class Dummy<T>
            {
                public Dummy<U> Select<U>(Func<T, U> selector)
                {
                    Console.WriteLine("Select called");
                    return new Dummy<U>();
                }
            }
            public class TranslationExample
            {
                static void Main()
                {
                    var source = new Dummy<string>();
                    var query = from dummy in source
                                where dummy.ToString() == "Ignored"
                                select "Anything";
                    Console.ReadKey();
                }
            }
        }
        namespace Code_11_5
        {
            class Program
            {
                static void Main()
                {
                    ArrayList list = new ArrayList { "First", "Second", "Third" };
                    IEnumerable<string> strings = list.Cast<string>();
                    foreach (string item in strings)
                    {
                        Console.WriteLine(item);
                    }
                    list = new ArrayList { 1, "not", 2.3 };
                    IEnumerable<double> ints = list.OfType<double>();
                    foreach (double item in ints)
                    {
                        Console.WriteLine(item);
                    }
                    //
                    List<short> shortlist = new List<short> { 2, 3, 4 };
                    //IEnumerable<int> ints2 = shortlist.Cast<int>();//不允许这张非一次性地转换。需要使用select
                    //foreach(int n in ints2)
                    //{
                    //    Console.WriteLine(n);
                    //}
                    var ints2 = from item in shortlist
                                select Convert.ToInt32(item);
                    foreach (int n in ints2)
                    {
                        Console.WriteLine(n);
                    }
                    Console.ReadKey();
                }
            }
        }
        namespace Sort_Filter
        {

            class Program
            {
                static void Main()
                {
                    User tim = SampleData.Users.TesterTim;
                    var query = from defect in SampleData.AllDefects
                                where defect.Status != Status.Closed
                                where defect.AssignedTo == tim
                                orderby defect.Severity descending, defect.LastModified ascending//第一个关键字相同结果的两个项按照日期升序
                                select defect;
                    foreach (var defect in query)
                    {
                        Console.WriteLine($"{defect.Severity}:{defect.Summary}({defect.LastModified})");
                    }
                    Console.ReadKey();
                }

            }
            class Code11
            {
                static void Main()
                {
                    //var query = from user in SampleData.AllUsers
                    //            orderby user.Name.Length
                    //            select user.Name;
                    //foreach(var name in query)
                    //{
                    //    Console.WriteLine($"{name.Length}:{name}");
                    //}
                    //使用let避免两次访问Length属性
                    var query = from user in SampleData.AllUsers
                                let length = user.Name.Length
                                orderby length
                                select new { Name = user.Name, Length = length };
                    foreach (var entry in query)
                    {
                        Console.WriteLine($"{entry.Length}:{entry.Name}");
                    }
                    Console.ReadKey();
                }
            }
        }
        namespace Join
        {
            using System.IO;
            
            class Code_11_12
            {
                static void Main()
                {
                    var query = from defect in SampleData.AllDefects
                                join subscription in SampleData.AllSubscriptions
                                on defect.Project equals subscription.Project
                                select new { defect.Summary, subscription.EmailAddress };
                    foreach (var entry in query)
                    {
                        Console.WriteLine($"{entry.EmailAddress}: {entry.Summary}");
                    }
                    Console.ReadKey();
                }
            }
            class Code_11_13
            {
                static void Main()
                {
                    //使用join into分组连接
                    var query = from defect in SampleData.AllDefects
                                join subscription in SampleData.AllSubscriptions
                                    on defect.Project
                                     equals subscription.Project
                                     into groupedSubscriptions
                                select new { Defect = defect, Subscriptions = groupedSubscriptions };
                    foreach (var entry in query)
                    {
                        Console.WriteLine(entry.Defect.Summary);
                        foreach (var subscription in entry.Subscriptions)
                        {
                            Console.WriteLine("\t{0}", subscription.EmailAddress);
                        }
                    }
                    Console.ReadKey();
                }
            }
            class Code_11_14
            {
                static void Main()
                {
                    var dates = new DateTimeRange(SampleData.Start, SampleData.End);
                    var query = from date in dates
                                join defect in SampleData.AllDefects
                                on date equals defect.Created.Date
                                into joined
                                select new { Date = date, Count = joined.Count() };
                    foreach (var grouped in query)
                    {
                        Console.WriteLine("{0:d}:{1}", grouped.Date, grouped.Count);
                    }
                    Console.ReadKey();
                }
            }
            //交叉连接和其他
            class Code_11_15
            {
                static void Main()
                {
                    //var query = from user in SampleData.AllUsers
                    //            from project in SampleData.AllProjects
                    //            select new { User = user, Project = project };
                    //foreach (var pair in query)
                    //{
                    //    Console.WriteLine("{0}/{1}", pair.User.Name, pair.Project.Name);
                    //}
                    //右方依赖于左方序列
                    //var query = from left in Enumerable.Range(1, 4)
                    //            from right in Enumerable.Range(11, left)
                    //            select new { Left = left, Right = right };
                    //foreach(var pair in query)
                    //{
                    //    Console.WriteLine($"Left={pair.Left};Right={pair.Right}");
                    //}
                    //SelectMany交叉连接的应用
                    //处理大量的日志。我改为CS文件吧
                    string csharpDirectory = @"..\..";
                    var query = from file in Directory.GetFiles(csharpDirectory, "*.cs", SearchOption.AllDirectories)
                                from line in IterationSample.EnumerateDemo.ReadLines(file)
                                where line.StartsWith("using")//line是筛选的每一条using
                                group line by file;
                    foreach(var file in query)
                    {
                        Console.WriteLine(file.Key);
                        //System.Threading.Thread.Sleep(5000);
                        foreach(var use in file)
                        {
                            Console.WriteLine("\t{0}", use);
                        }
                    }


                    Console.ReadKey();
                }
            }
        }
        //分组……延续
        namespace GropyBy
        {
            class Code_11_1x
            {
                static void Main()
                {
                    //var query = from defect in SampleData.AllDefects
                    //            where defect.AssignedTo != null
                    //            group defect by defect.AssignedTo;//分别是Defect User
                    //foreach(var entry in query)
                    //{
                    //    Console.WriteLine(entry.Key.Name);
                    //    foreach(var defect in entry)
                    //    {
                    //        Console.WriteLine("\t({0}) {1}", defect.Severity, defect.Summary);
                    //    }
                    //}
                    //代码11-18：投影保留概要信息
                    //var query = from defect in SampleData.AllDefects
                    //            where defect.AssignedTo != null
                    //            group defect.Summary by defect.AssignedTo;
                    //foreach(var entry in query)
                    //{
                    //    Console.WriteLine(entry.Key.Name);
                    //    foreach(var summay in entry)
                    //    {
                    //        Console.WriteLine($"\t{summay}");
                    //    }
                    //}
                    //代码11-19 另一个投影
                    var query = from defect in SampleData.AllDefects
                                where defect.AssignedTo != null
                                group defect by defect.AssignedTo into grouped//添加到第二组
                                select new { Assignee = grouped.Key, Count = grouped.Count() } into result//代码11-20
                                orderby result.Count descending
                                select result;
                    //foreach(var entry in query)
                    //{
                    //    Console.WriteLine($"{entry.Assignee} have/has {entry.Count} bug(s) to fix");
                    //}
                    foreach(var entry in query)
                    {
                        Console.WriteLine($"{entry.Assignee.Name}:{entry.Count}");
                    }
                    Console.ReadKey();
                }
            }
        }
    }
}
