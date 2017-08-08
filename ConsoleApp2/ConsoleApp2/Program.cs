#define DEBUG
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ma = Common.Pack;
using static System.Math;//使用静态声明
using System.Diagnostics;
//多线程
using System.Threading;

namespace Common
{
    class Pack
    {
        public static void func()
        {

        }
    }
}
namespace ConsoleApp2
{
    class Rectangle
    {
        //成员变量
        internal double length;
        double width;
        public void Acceptdetails()
        {
            length = 4.5;
            width = 3.5;
        }
        public double GetArea()
        {
            return length * width;
        }
        public void Display()
        {
            Console.WriteLine("Length:{0}\nWidth:{1}\nArea:{2}", length, width, GetArea());
        }
    }
    class Program
    {

        static void Main(string[] args)
        {
            //Rectangle r = new Rectangle();
            //r.Acceptdetails();
            //r.Display();
            //引用类型
            object obj;
            obj = 100;//装箱：值类型转换为对象类型。拆箱相反
            int nval = (int)obj;
            //动态类型
            dynamic d = 100;
            Console.WriteLine("{0}", System.Runtime.InteropServices.Marshal.SizeOf(d));//sizeof只能在不安全的上下文中使用。
            //字符串
            string str = @"
<html>
    <a href=""ccc\"">ddd</a>
</html>
";
            Console.WriteLine(str);
            //类型转换 https://docs.microsoft.com/zh-cn/dotnet/csharp/programming-guide/strings/
            double aa = 3.14;
            int bb = 52;
            string cc = "123j";
            int result;
            bool conv=int.
                TryParse(cc, out result);
            Console.WriteLine("{0}.{1}", conv, result);
            //变量
            double dd = 210f;
            double ff =2.3E-2;
            Console.WriteLine("{0}:{1}", dd, ff);
            //运算符
            unsafe//不安全的代码
            {
                int* gg = &bb;
                Console.WriteLine("{0}", (int)gg);
            }
            Console.ReadLine();
            //switch语句
            switch(bb)
            {
                case 0:
                case 1:
                    break;
            }
            //访问控制
            Rectangle rec=new Rectangle();
            double dddd=rec.length;
        }
    }
    class Function
    {
        public void exchange(ref int x,ref int y)
        {
            x = x + y;
            y = x - y;
            x = x - y;
        }
        //定义枚举
        enum Hour : int
        {
            HO = 2,
            SEC=HO
        }
        static void Main(string[] args)
        {
            //ref和out
            int x = 25, y = 30;
            Function f = new Function();
            Console.WriteLine("x:{0},y:{1}", x, y);
            f.exchange(ref x, ref y);
            Console.WriteLine("x:{0},y:{1}", x, y);
            //可空类型
            bool? bo=new bool?();
            Console.WriteLine($"可空的BOOL:{bo}");
            //checked语法
            short ss = 288;
            byte sb = unchecked((byte)ss);
            //short*short
            short s = 32761,s2=325;
            dynamic sps = s * s2;
            bool isi = sps is int;
            Console.WriteLine($"{(int)Hour.HO},{Hour.SEC}");
            //enumerate covert
            Hour fromint = (Hour)Enum.Parse(typeof(Hour), 1.ToString());
            Console.WriteLine($"{fromint}");
            Console.ReadKey();
        }
    }
    class Coll
    {
        static void print_jagged_array(int [][] aa)
        {
            for(int i=0;i<aa.Length;++i)
            {
                Console.WriteLine($"[{i}]");
                foreach(int j in aa[i])
                {
                    Console.WriteLine($"\t{j}");
                }
            }
        }
        //参数数组：params关键字(参数可变或者是一维数组)
        public static int sum(params int[] arr)
        {
            int sum=0;
            foreach(int s in arr)
            {
                sum += s;
            }
            return sum;
        }
        

        static void Main()
        {
            string[] str = { "sb", "250" };
            for(int i =0;i<str.Length;++i)
            {
                Console.WriteLine($"{str[i]}");
            }
            //等价
            foreach(string s in str)
            {
                //s是迭代变量
                Console.WriteLine($"{s}");
            }
            //多维数组
            double[,,] peoplehealth = { { { 1.0,2.5 },{ 3.0, 4.0 } } };//维度必须等长
            foreach(double s in peoplehealth)
            {
                Console.WriteLine($"{s}");
            }
            //数组的数组（维度可以不相同）
            int[][] scores = new int[2][] { new int[] { 92, 93, 94 }, new int[] { 89, 88 } };
            print_jagged_array(scores);
            Console.Write($"参数数组：{sum(25,25)}\n{sum(scores[0])}");
            //string
            string[] s2 = new string[] { "Hello", "World" };
            string str_j = String.Join("\n", s2);
            //SortedList
            SortedList<int,string> ss=new SortedList<int, string>();
            ss.Add(25,"John");
            ss.Add(1, "Tom");
            ICollection key = (ICollection)ss.Keys;
            foreach(int n in key)
            {
                Console.WriteLine($"{n}->{ss[n]}");
            }
            Console.ReadKey();
        }
    }
    class Structe
    {
        //与C++不同：struct
        struct Book
        {
            private int numer;
            public int getValue()
            {
                return numer;
            }
            public void set(int n)
            {
                numer = n;
            }
        }
        //C#的OOP。interface支持多重继承
        class Shape
        {
            protected int width;
            protected int height;
            //构造函数
            public Shape(int w,int h)
            {
                width = w;
                height = h;
            }
        }
        public interface PaintCost
        {
            void OutputCost();//不能定义接口的成员。纯虚函数？

        }
        class Rectangle:Shape,PaintCost//如果需要访问Shape中的成员需要使用对象
        {
            public Rectangle(int w,int h):base(w,h)
            { }
            public void OutputArea()
            {
                Console.WriteLine($"{width * height}");
            }
            public void OutputCost()
            {
                Console.WriteLine($"{width * height * 7}");
            }
        }
        
        static void Main()
        {
            Book s=new Book();
            //s.set(5);
            int h=s.getValue();
            Rectangle rec = new Rectangle(25, 2);
            rec.OutputArea();
            rec.OutputCost();
            Console.ReadKey();
        }
    }
    class Poly
    {
        abstract class APeople
        {
            protected int id;
            public APeople(int _id)
            {
                id = _id;
            }
            //public abstract int weight();//抽象方法在派生类中实现。抽象类与接口的区别 https://stackoverflow.com/questions/747517/interfaces-vs-abstract-classes
            public virtual void ID()
            {
                Console.WriteLine(id);
            }
            public static bool operator ==(APeople lhs, APeople rhs)
            {
                Console.WriteLine("== CALLED");
                return lhs.id == rhs.id;
            }
            public static bool operator !=(APeople lhs, APeople rhs)
            {
                return lhs.id != rhs.id;
            }
        }
        class Student:APeople
        {
            public Student(int d):base(d)
            { }
            public override void ID()
            {

                Console.WriteLine("SS ID:{0}",id);
            }
            
        }
        class Worker : APeople
        {
            public Worker(int d):base(d)
            {
      
            }
            public override void ID()
            {

                Console.WriteLine("WORKER ID:{0}",id);
            }
        }
        static void Main()
        {
            APeople ap = new Worker(252);
            APeople ap2 = new Worker(252);
            APeople ap3 = new Worker(25);
            Student ss = new Student(2);
            ap.ID();
            ss.ID();
            bool r_ap1 = ap == ap2;
            bool r_ap2 = ap == ap3;
            Console.ReadKey();
        }
    }
    class Using
    {
        
        static void Main()
        {

            var pi= PI;//System.Math.PI
                       //using 语句块 https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/using-statement   

  
#if (DEBUG)
            Console.WriteLine("debug");
#elif (!DEBUG)
            Console.WriteLine("no debug");
#endif
            Console.ReadKey();
        }
    }
    class Exception
    {
        class DefinedExpception:ApplicationException
        {
            public DefinedExpception(string message):base(message)
            {
                Console.WriteLine("DefinedException");
            }
        }
        static void Main()
        {
            try
            {
                try
                {

                    throw new DefinedExpception("naive");
                }
                catch(DefinedExpception e)
                {
                    Console.WriteLine("1");
                    throw e;
                }
            }
            catch(DefinedExpception e)
            {
                Console.WriteLine("2");
                throw e;
            }
            finally
            {
                Console.ReadKey();
            }
        }
    }
    class FileIO
    {
        static void Main()
        {
            //FileStream fs = new FileStream("1.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
            //for(int i=0;i<20;++i)
            //{
            //    fs.WriteByte((byte)(i + 1));
            //}
            //fs.Position = 0;
            //for(int i=0;i<20;++i)
            //{
            //    Console.Write((int)fs.ReadByte());
            //}
            //fs.Close();
            //文本文件读写
            //using (StreamReader str = new StreamReader("poem.txt", Encoding.Default))
            //{
            //    while (!str.EndOfStream)
            //    {
            //        Console.WriteLine(str.ReadLine());
            //    }
            //}
            using (BinaryWriter bi = new BinaryWriter(new FileStream("binary.txt", FileMode.OpenOrCreate)))
            {
                string name="shu";
                int id=25;
                double height = 180.5;
                bi.Write(name);
                bi.Write(id);
                bi.Write(height);
            }
            //读取
            using (BinaryReader br = new BinaryReader(new FileStream("binary.txt", FileMode.Open, FileAccess.Read)))
            {
                Console.Write("NAME:{0}\nID:{1}\nHEIGHT:{2}", br.ReadString(), br.ReadInt32(), br.ReadDouble());
            }
            Console.ReadKey();
        }
        
    }
}
namespace AdvancedFeature
{
    namespace Ref
    {
        [AttributeUsage(AttributeTargets.All)]
        public class HelpAttribute:System.Attribute
        {
            public readonly string url;
            private string topic;
            public string Topic
            {
                get
                {
                    return topic;
                }
                set
                {
                    topic = value;
                }
            }
            public HelpAttribute(string url)//url是定位参数
            {
                this.url = url;
            }
        }
        [HelpAttribute("Info on myclass")]
        class MyClass
        {

        }
        class Program
        {
            static void Main()
            {
                System.Reflection.MemberInfo info = typeof(MyClass);
                object[] attr = info.GetCustomAttributes(true);
                for(int i=0;i<attr.Length;++i)
                {
                    System.Console.WriteLine(attr[i]);
                }
                Console.ReadKey();
            }
        }
    }
    class Attr
    {
        [Conditional("DEBUG")]
        public static void Message(string msg)
        {
            Console.WriteLine(msg);
        }
        //“过时的”标记
        [Obsolete("DON'T USE THIS METHOD",true)]
        public static void OLD()
        {

        }
    }
    class Test
    {
        static void func1()
        {
            Attr.Message("IN FUNC1");
            func2();
        }
        static void func2()
        {
            Attr.Message("IN FUNC2");
        }
        //自定义特性
        //1.声明自定义特性
        [AttributeUsage(AttributeTargets.Class |
            AttributeTargets.Constructor |
            AttributeTargets.Field |
            AttributeTargets.Method |
            AttributeTargets.Property,
            AllowMultiple =true)]
        public class DeBugInfo:System.Attribute
        {
            private int bugNo;
            private string message;
            public DeBugInfo(int bug,string msg)
            {
                bugNo = bug;
                message = msg;
            }
            public int BugNo
            {
                get
                {
                    return bugNo;
                }
            }
            public string Message
            {
                get
                {
                    return message;
                }
            }
        }
        //3.应用特性
        [DeBugInfo(1,"CANNOT CALCULATE AREA")]
        [DeBugInfo(2, "RECOMMEND TO USE DOUBLE")]
        class Rectangle
        {
            protected int width;
            protected int height;
            public Rectangle(int w,int h)
            {
                width = w;
                height = h;
            }
        }

        //索引器
        class IndexTest
        {
            private int[] datas;
            public readonly int size;
            public IndexTest(int size)
            {

                this.size = size;
                datas = new int[size];
                for (int i = 0; i < size; ++i)
                    datas[i] = 0;
            }
            public int this[int index]
            {
                get
                {
                    if (index >= 0 && index < size)
                        return datas[index];
                    else
                        throw (new IndexOutOfRangeException("OUT OF RANGE1"));
                }
                set
                {
                    if (index >= 0 && index < size)
                        datas[index] = value;
                    else
                        throw (new IndexOutOfRangeException("OUT OF RANGE1"));
                }
            }
            public int this[string index]
            {
                get
                {
                    int i = Convert.ToInt32(index);
                    if (i >= 0 && i < size)
                        return datas[i];
                    else
                        throw (new IndexOutOfRangeException("OUT OF RANGE1"));
                }
            }
  
        }
        static void Main()
        {
            Attr.Message("IN MAIN");
            func1();
            //obsolete
            //Attr.OLD();
            //便利Rectangle的属性
            Rectangle rect = new Rectangle(10,25);
            Type type = typeof(Rectangle);
            foreach(Object attrs in type.GetCustomAttributes(false))
            {
                DeBugInfo dbi = (DeBugInfo)attrs;
                if(dbi !=null)
                {
                    Console.WriteLine("BugNo:{0}, Message:{1}", dbi.BugNo, dbi.Message);
                }
            }
            //索引
            IndexTest idt = new IndexTest(5);
            for (int i = 0; i <+ 5; ++i)
                idt[i] = i * 10 + 1;
            for (int i = 0; i < idt.size; ++i)
                Console.WriteLine(idt[i]);
            Console.WriteLine($"STRING:{idt["1"]}");
            Console.ReadKey();
        }
    }
    class Delegate
    {
        //委托：类似于安全版本的函数指针 :b https://docs.microsoft.com/zh-cn/dotnet/csharp/programming-guide/delegates/
        delegate T NumericOperator<T>(T a, T b);
        class TestDelegate
        {
            static int num = 10;
            public static int Add(int l,int r)
            {
                Console.WriteLine("ADD");
                num = l + r;
                return num;
            }
            public static int Subtract(int l, int r)
            {
                Console.WriteLine("SUBTRACT");
                num = l - r;
                return num;
            }
            public static void Main()
            {
                NumericOperator<int> nn1 = new NumericOperator<int>(Add);
                NumericOperator<int> nn2 = new NumericOperator<int>(Subtract);
                nn1(25, 25);
                Console.WriteLine($"{num}");
                nn2(25, 100);
                Console.WriteLine($"{num}");
                //多播
                NumericOperator<int> nn3;
                nn3 = nn1;
                nn3 += nn2;
                nn3(100, 20);
                Console.WriteLine($"{num}");
                Console.ReadKey();
            }
        }

    }

    //C# 事件
    class Event
    {
        class Shape
        {
            private int width;
            private int height;
            private DelegateShapeEvent eventHandler;
            public Shape(int w,int h, DelegateShapeEvent eventh)
            {
                width = w;
                height = h;
                eventHandler = eventh;
            }
            public int Width
            {
                get
                {
                    return width;
                }
                set
                {
                    width = value;
                    if(eventHandler !=null)
                    {
                        eventHandler.LogProcess(this);
                    }
                }
            }
            public int Height
            {
                get
                {
                    return height;
                }
                set
                {
                    height = value;
                    if (eventHandler != null)
                    {
                        eventHandler.LogProcess(this);
                    }
                }
            }

        }
        //事件发布器
        class DelegateShapeEvent
        {
            public delegate void ShapeChangeHandler(string message);
            //基于上面的委托定义事件：ShapeChangeToLog
            public event ShapeChangeHandler ShapeChangeToLog;
            public void LogProcess(Shape ss)
            {
                OnShapeChange($"ShapeChanged:\n\tWidth:{ss.Width}\n\tHeight:{ss.Height}");
            }
            //处理程序：调用绑定的委托
            protected void OnShapeChange(string message)
            {
                if(ShapeChangeToLog !=null)
                {
                    ShapeChangeToLog(message);
                }
            }
        }
        //日志写入
        class ShapeChangeLogger
        {
            FileStream fs;
            StreamWriter sw;
            public ShapeChangeLogger(string filename)
            {
                fs = new FileStream(filename, FileMode.OpenOrCreate | FileMode.Append, FileAccess.Write);
                sw = new StreamWriter(fs);
            }
            public void Logger(string info)
            {
                sw.WriteLine(info);
            }
            public void Close()
            {
                sw.Close();
                fs.Close();
            }
        }
        //写入到屏幕输出
        public class RecordShapeChangeInfo
        {
            public static void Logger(string info)
            {
                Console.WriteLine(info);
            }
        }
        static void Main()
        {
            ShapeChangeLogger filelog = new ShapeChangeLogger("shapeChange.txt");
            DelegateShapeEvent shapechangeevent = new DelegateShapeEvent();
            shapechangeevent.ShapeChangeToLog += new DelegateShapeEvent.ShapeChangeHandler(RecordShapeChangeInfo.Logger);//输出到控制台
            shapechangeevent.ShapeChangeToLog += new DelegateShapeEvent.ShapeChangeHandler(filelog.Logger);//记录到文件
            Shape shape1 = new Shape(100, 20, shapechangeevent);
            shape1.Width = 30;
            shape1.Height = 200;
            Console.ReadKey();
            filelog.Close();
        }
    }

    //泛型
    class Generics
    {
        static void Swap<T>(ref T l,ref T r)
        {
            T temp;
            temp = l;
            l = r;
            r = temp;
        }
        static void Main()
        {
            int a = 2, b = 3;
            Swap(ref a, ref b);
            Console.ReadKey();
        }
    }
    //不安全的代码
    class Unsafe
    {
        public unsafe static void Main()
        {
            int[] list = { 10, 20, 30 };
                fixed (int* ptr = list) //fixed关键字可以固定变量的地址（C# 有垃圾回收机制所以变量的地址可能不固定）
                                    //int* ptr = list; int*和int[]是不同类型。前者可以修改地址后者不可以
                for (int i=0;i<3;++i)
                {
                    Console.WriteLine($"Address:{(int)(ptr + i)}. Data:{*(ptr + i)}");
                }
            int* ptr2 = stackalloc int[2];//stackalloc在堆栈上分配内存，分配的内存不受内存管理器的影响
            
            ptr2[1] = 5;
            Console.ReadKey();
            
        }
    }
    //匿名方法
    class AnonymousMethod
    {
        delegate void NumberChanger(int n);
        private static int num;
        static void Add(int p)
        {
            num += p;
        }
        static void Main()
        {
            num = 25;
            //NumberChanger n = new NumberChanger(Add);
            NumberChanger n2 = new NumberChanger((int x)=> { num += x; });//lambda https://docs.microsoft.com/zh-cn/dotnet/csharp/programming-guide/statements-expressions-operators/lambda-expressions#statement-lambdas
            n2(25);
            int j=5;
        }
    }
    //主线程
    class ThreadApp
    {
        public static void Call()
        {
            try
            {
                Console.WriteLine("child thread starts");
                int sleepfor = 5000;
                Console.WriteLine("Child thread paused for {0} seconds", sleepfor / 1000);
                Thread.Sleep(sleepfor);
                Console.WriteLine("Child thraed resume");
            }
            catch (ThreadAbortException e)
            {
                Console.WriteLine("Thread abort");
            }
            
        }
        static void Main()
        {
            ThreadStart childref = new ThreadStart(Call);
            Console.WriteLine("main thread starts");
            Thread childth = new Thread(childref);
            childth.Start();
            Console.WriteLine("main thread continue");
            Thread.Sleep(2000);
            childth.Abort();
            Console.ReadKey();
        }
    }
}
