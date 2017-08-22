using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter13
{
    using System.Threading;
    class Multithreading
    {
        
    }
    class MoniterSample
    {
        private int n = 1;//生产者和消费者共同处理的数据
        private int max = 10000;
        private object monitor = new object();
        public void Produce()
        {
            lock(monitor)
            {
                for(;n <=max;++n)
                {
                    Console.WriteLine("妈妈：第" + n.ToString() + "块蛋糕做好了");
                    Monitor.Pulse(monitor);
                    Monitor.Wait(monitor);
                }
            }
        }
        public void Consume()
        {
            lock(monitor)
            {
                while(true)
                {
                    Monitor.Pulse(monitor);
                    Monitor.Wait(monitor, 1000);
                    Console.WriteLine("孩子：开始吃第" + n.ToString() + "块蛋糕");
                }
            }
        }
        static void Main()
        {
            MoniterSample obj = new MoniterSample();
            Thread tProduce = new Thread(new ThreadStart(obj.Produce));
            Thread tConsume = new Thread(new ThreadStart(obj.Consume));
            tProduce.Start();
            tConsume.Start();
            Console.ReadLine();
        }
    }
    class EventWaitTest
    {
        private string name;//顾客姓名
        private static ManualResetEvent eventWait = new ManualResetEvent(false);
        private static ManualResetEvent eventOver = new ManualResetEvent(false);
        public EventWaitTest(string name)
        {
            this.name = name;
        }
        public static void Product()
        {
            Console.WriteLine("服务员：厨师在做菜。请稍等");
            Thread.Sleep(2000);
            Console.WriteLine("服务员：做好了");
            eventWait.Set();
            while(true)
            {
                if(eventOver.WaitOne(1000,false))
                {
                    Console.WriteLine("服务员：两位请买单");
                    eventOver.Reset();//通知结账的那位
                }
            }
        }
        public void Consume()
        {
            while(true)
            {
                if(eventWait.WaitOne(1000,false))
                {
                    Console.WriteLine(this.name + ":开始吃宫爆鸡丁");
                    Thread.Sleep(2000);
                    Console.WriteLine(this.name + ":宫爆鸡丁吃完了");
                    eventWait.Reset();
                    eventOver.Set();
                    break;
                }
                else
                {
                    Console.WriteLine(this.name + ":等上菜无聊，玩会儿手机");
                }
            }
        }
    }
    class Program
    {
        static void Main()
        {
            EventWaitTest zhangsan = new EventWaitTest("张三");
            EventWaitTest lisi = new EventWaitTest("李四");
            Thread t1 = new Thread(new ThreadStart(zhangsan.Consume));
            Thread t2 = new Thread(new ThreadStart(lisi.Consume));
            Thread t3 = new Thread(new ThreadStart(EventWaitTest.Product));
            t1.Start();
            t2.Start();
            t3.Start();
            Console.ReadKey();
        }
    }
    namespace LockSample
    {
        class Person
        {
            public int RestLife { get; set; }
            //念句子的人名
            public string Name { get; set; }
            public Person(int restlife,string name)
            {
                this.Name = name;
                this.RestLife = restlife;
                Console.WriteLine("{0}还有：{1}s的生命",Name,RestLife);
            }
        }
        class Xuming
        {
            //ref http://www.cnblogs.com/lxblog/archive/2013/03/07/2947182.html
            //jzm的名言
            public string FamousSaying { get; set; }
            //减少的生命s
            public int XumingValue { get; set; }
            public Xuming(string famoussaying,int xumingvalue)
            {
                this.FamousSaying = famoussaying;
                this.XumingValue = xumingvalue;
            }
            //念诗或者句子
            public void Saying(Object person)
            {
                Person p = person as Person;
                while(p.RestLife > 0)
                {
                    Monitor.Enter(person);
                    Console.WriteLine("{0}念了“{1}”", p.Name, this.FamousSaying);
                    if(p.RestLife > XumingValue)
                    {
                        p.RestLife -= XumingValue;
                    }
                    else
                    {
                        p.RestLife = 0;
                    }
                    Console.WriteLine("{0}剩下{1}s", p.Name, p.RestLife);
                }
                
                Thread.Sleep(500);
                Monitor.Exit(person);
            }
            //弹夏威夷吉他
            public void PlayHawiiGuitar(Object person)
            {
                Person p = person as Person;
                Monitor.Enter(p);
                Console.WriteLine("{0}开始弹夏威夷吉他", p.Name);
                while(p.RestLife > 0)
                {
                    Monitor.Wait(person,1000);//也必须要等待1s 不然也会死锁。不过正确的情况是在续命之前检查剩余的寿命
                    Console.WriteLine("{0}获得续命的许可", p.Name);
                    if(p.RestLife > 0)
                    {
                        Console.WriteLine("{0}【弹夏威夷吉他】花式续命",p.Name);
                        p.RestLife = (p.RestLife >= this.XumingValue) ? p.RestLife -= this.XumingValue : 0;
                        Console.WriteLine("{0}的剩余生命{1}s", p.Name, p.RestLife);
                    }
                    else
                    {
                        Console.Write("{0}已经死了", p.Name);
                        break;
                    }
                    Thread.Sleep(500);
                    Monitor.Pulse(person);
                }
                Monitor.Exit(person);
            }
            //怒斥
            public void Angry(Object person)
            {
                Person p = person as Person;
                Monitor.Enter(p);//进入操控对象
                Console.WriteLine("{0}开始怒斥", p.Name);
                while (p.RestLife > 0)
                {
                    Monitor.Pulse(person);//通知其他的线程。我已经在操控对象了
                    if (Monitor.Wait(person,1000))//如果1s之后不能够释锁就继续运行（返回false）。如果获得锁，则返回true
                    {
                        Console.WriteLine("{0}获得续命的许可", p.Name);
                        if (p.RestLife > 0)
                        {
                            
                            Console.WriteLine("{0}【怒斥】花式续命",p.Name);
                            p.RestLife = (p.RestLife >= this.XumingValue) ? p.RestLife -= this.XumingValue : 0;
                            Console.WriteLine("{0}的剩余生命{1}s", p.Name, p.RestLife);
                        }
                        else
                        {
                            Console.Write("{0}已经死了", p.Name);
                            //break;
                        }
                        Thread.Sleep(500);
                    }
                }
                Monitor.Exit(person);//释放对象的排他锁
            }

        }
        class Program
        {
            static void Main()
            {
                Xuming x1 = new Xuming("苟利国家生死以，岂因祸福避趋之", 3);
                Xuming x2 = new Xuming("你们啊，naive!", 5);
                Person p1 = new Person(120, "Shu");
                Thread t1 = new Thread(new ParameterizedThreadStart(x1.PlayHawiiGuitar));
                t1.Start(p1);
                Thread t2 = new Thread(new ParameterizedThreadStart(x2.Angry));
                t2.Start(p1);
                t1.Join();
                t2.Join();
                Console.ReadKey();
            }
        }
    }
}
