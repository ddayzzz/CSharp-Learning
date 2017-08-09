using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//xml
using System.Xml.Linq;
//async
using static System.Console;
using System.Net.Http;
//for StructLayout
using System.Runtime.InteropServices;
using System.Collections;

namespace CSahrp_in_depth
{
    class Product
    {
        //public string Name { get; private set; }//自动实现的属性C#3
        //public decimal Price { get; private set; }
        //C#4
        private readonly string name;
        private readonly decimal? price;
        public string Name { get { return name; }  }
        public decimal? Price { get { return price; } }
        public Product(string name,decimal? price=null)
        {
            this.name = name;
            this.price = price;
        }
        Product() { }
        public static List<Product> GetProductsList()
        {
            return new List<Product>
            {
                new Product("book3",30),
                new Product("book2",300),
                //new Product{Name="book3",Price=12}//C#3集合初始值设定项
                new Product(name:"book1", price:25),//命名实参
                new Product(name:"unknown")//可空类型做默认的参数
            };
        }
    }
    class ProductNameComparer:IComparer<Product>
    {
        public int Compare(Product lhs,Product rhs)
        {
            return lhs.Name.CompareTo(rhs.Name);
        }
    }
    class AnonymousFunc
    {
        Func<int, int, string> func = (x,y) => (x * x).ToString();//Func相当于C++的function<T>?
        Func<string> func_dele = delegate (){ return "delegate"; };//仅限于没有参数
    }
    class Async
    {
        //异步操作 http://www.cnblogs.com/Cwj-XFH/p/5908562.html
        static void Wait() => Console.WriteLine("Waiting...");
        static void End() => Console.WriteLine("End...");
        static int Start()
        {
            Console.WriteLine("Start...");
            HttpClient client = new HttpClient();
            System.Threading.Thread.Sleep(5000);
            Wait();
            var res = client.GetStreamAsync("https://www.visualstudio.com/");
            string str = res.Result.ToString();
            return str.Length;
        }
        static void Main()
        {
            Start();
            End();
            Console.ReadKey();
        }
    }
    namespace Generics
    {
        namespace Static
        {
            public class Outer<T>
            {
                public class Inner<U, V>
                {
                    static Inner()
                    {
                        Console.WriteLine("Outer<{0}>.Inner<{1},{2}>", typeof(T).Name, typeof(U).Name, typeof(V).Name);
                    }
                    public static void DummyMethod() { }
                }
                public class Inner
                {
                    static Inner()
                    {
                        Console.WriteLine("Outer<{0}>.Inner", typeof(T).Name);
                    }
                    public static void DummyMethod() { }
                }
                
            }
            [StructLayout(LayoutKind.Sequential)]
            class ClassForSizeof:Object
            {

            }
            class CountingEnumerable:IEnumerable<int>
            {
                private int[] data;
                public IEnumerator<int> GetEnumerator()
                {
                    return new CountingEnumerator();
                }
                System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
                {
                    return GetEnumerator();
                }
            }
            class CountingEnumerator: IEnumerator<int>//继承泛型接口
            {
                int current = -1;
                public bool MoveNext()
                {
                    current++;
                    return current < 10;
                }
                public int Current { get { return current; } }//隐式实现了IEnumerator<int>的Current
                object IEnumerator.Current { get { return Current; } }

                //object IEnumerator.Current => throw new NotImplementedException();

                public void Reset()
                {
                    current = -1;
                }
                public void Dispose() { }
            }
            class Reflection
            {
                public static void PrintType<T>()
                {
                    Console.WriteLine(typeof(T));
                }
            }
            class Program
            {
                static void Main()
                {
                    Console.WriteLine(Marshal.SizeOf(new ClassForSizeof()));
                    System.Collections.ArrayList alb = new System.Collections.ArrayList();
                    alb.Add((byte)25);
                    alb.Add((byte)25);
                    alb.Add((byte)25);
                    Outer<int>.Inner<string, DateTime>.DummyMethod();
                    Outer<string>.Inner<int, int>.DummyMethod();
                    Outer<string>.Inner<int, int>.DummyMethod();//静态构造函数只执行了一次
                    Outer<string>.Inner.DummyMethod();
                    Outer<string>.Inner.DummyMethod();
                    //迭代 http://blog.csdn.net/yangbindxj/article/details/11964343 和 https://msdn.microsoft.com/zh-cn/library/78dfe2yb(v=vs.110).aspx
                    CountingEnumerable counter = new CountingEnumerable();
                    foreach(int v in counter)
                    {
                        Console.WriteLine(v);
                    }
                    //typeof
                    Console.WriteLine($"{typeof(List<>)}:{typeof(List<>).ContainsGenericParameters}");
                    Console.WriteLine($"{typeof(Dictionary<,>)}:{typeof(Dictionary<,>).ContainsGenericParameters}");
                    Console.WriteLine($"{typeof(Dictionary<int,string>)}:{typeof(Dictionary<int,string>).ContainsGenericParameters}");//:False密封的已构造类型
                    string listTypeName = "System.Collections.Generic.List`1";
                    Type defByName = Type.GetType(listTypeName);
                    Type closedByName = Type.GetType(listTypeName + "[System.String]");
                    Type closedByMethod = defByName.MakeGenericType(typeof(string));//填充类型参数，返回一个已构造的类型
                    Type closedByTypeof = typeof(List<string>);
                    Console.WriteLine(closedByMethod == closedByName);
                    Console.WriteLine(closedByName == closedByTypeof);
                    Type defByTypeof = typeof(List<>);
                    Type defByMethod = closedByName.GetGenericTypeDefinition();
                    Console.WriteLine(defByMethod == defByName);
                    Console.WriteLine(defByName == defByTypeof);
                    //反射调用
                    Type type = typeof(Reflection);
                    var definition=type.GetMethod("PrintType");
                    var cons = definition.MakeGenericMethod(typeof(string));
                    cons.Invoke(null, null);
                    Console.ReadKey();
                }
            }
        }
        namespace Equal
        {
            class MyClass
            {
                public string Name { get; set; }
                public override bool Equals(object obj)
                {
                    if (obj == null)
                        return this == null;
                    if (!(obj is MyClass))
                        return false;
                    MyClass w = obj as MyClass;//拆箱
                    return this.Name == w.Name;
                }
                public override int GetHashCode()
                {
                    return this.Name.GetHashCode();
                }
                public static bool operator==(MyClass w1,MyClass w2)
                {
                    return w1.Equals(w2);
                }
                public static bool operator !=(MyClass w1, MyClass w2)
                {
                    return !w1.Equals(w2);
                }
                public MyClass(string n) { this.Name = n; }
            }
            public sealed class Pair<T1,T2>:IEquatable<Pair<T1,T2>>
            {
                private static readonly IEqualityComparer<T1> firstComparer = EqualityComparer<T1>.Default;
                private static readonly IEqualityComparer<T2> secondComparer = EqualityComparer<T2>.Default;
                private readonly T1 first;
                private readonly T2 second;
                public Pair(T1 first,T2 second)
                {
                    this.first = first;
                    this.second = second;
                }
                public T1 First { get { return first; } }
                public T2 Second { get { return second; } }
                public bool Equals(Pair<T1,T2> other)
                {
                    return firstComparer.Equals(this.first, other.first) &&
                        secondComparer.Equals(this.second, other.second);
                }
                public override bool Equals(object obj)
                {
                    return Equals(obj as Pair<T1, T2>);

                }
                public override int GetHashCode()
                {
                    return firstComparer.GetHashCode(first) * 37 + secondComparer.GetHashCode(second);
                }
                public static bool operator==(Pair<T1, T2> lhs, Pair<T1, T2> rhs)
                {
                    Console.WriteLine("operator==");
                    return lhs.Equals(rhs);
                }
                public static bool operator!=(Pair<T1, T2> lhs, Pair<T1, T2> rhs)
                {
                    return !lhs.Equals(rhs);
                }

            }
            //非泛型辅助类 https://docs.microsoft.com/zh-cn/dotnet/csharp/programming-guide/classes-and-structs/static-classes-and-static-class-members
            public static class Pair
            {
                public static Pair<T1, T2> Of<T1, T2>(T1 first, T2 second)
                {
                    return new Pair<T1, T2>(first, second);
                }
            }
            class Program
            {
                public static void Main()
                {
                    //实现1
                    MyClass m1 = new MyClass("Shu");
                    MyClass m2 = new MyClass("Shu");
                    Console.WriteLine(m1 == m2);
                    Console.WriteLine(m1.Equals(m2));
                    Console.WriteLine(object.ReferenceEquals(m1, m2));
                    //实现2
                    Pair<string, int> pair = Pair.Of("Shu", 1604);
                    Pair<string, int> pair2 = new Pair<string, int>("Shu", 1604);
                    Console.WriteLine(pair.Equals(pair2));
                    Console.WriteLine(pair == pair2);
                    Console.ReadKey();
                }
                
            }
        }
        namespace Constrain
        {
            struct RefSample<T> where T:struct
            {

            }
            class CreatAnInstance<t> where t:class
            {

            }
            class BaseClass:Object
            {

            }
            class DerivedClass:BaseClass
            {

            }
            //类型推导
            class TypeDeduce
            {
                public static void Change<T>(T cls)
                {

                }
                //类型判断
                static bool AreRefEqual<T>(T first,T second)
                    where T:class
                {
                    return first == second;
                }
                public static int CompareToDefault<T>(T value)
                    where T:IComparable<T>
                {
                    return value.CompareTo(default(T));
                }
                public static void Main()
                {
                    Console.WriteLine(CompareToDefault<string>("NAIVE"));//引用类型默认值为null
                    Console.WriteLine(CompareToDefault<int>(1));
                    Console.WriteLine(CompareToDefault(DateTime.MinValue));
                    //重载对象的判断 可以去看看：http://www.cnblogs.com/yang_sy/p/3582946.html
                    string name = "Jon";
                    string i1 = "My name is " + name;
                    string i2 = "My name is " + name;
                    string i3 = null;
                    Console.WriteLine(i1 == i2);//True
                    Console.WriteLine(AreRefEqual(i1, i3));//False
                    Console.ReadKey();
                }
            }
            //类型转换
            class StreamReader<T> where T: BaseClass,
                                          new()
            {

            }
            class Program
            {
                static void Main()
                {

                    int j = new int();//值类型。哟i一个默认的无参数的构造函数。这个与int j;等价
                    RefSample<int> re=new RefSample<int>();
                    CreatAnInstance<string> re2 = new CreatAnInstance<string>();//不接受单个字符串的构造函数
                    StreamReader<DerivedClass> dd = new StreamReader<DerivedClass>();
                    //System.String h = new String("D");//string不存在默认的构造函数
                    //StreamReader<string> dd2 = new StreamReader<string>();//new()是失败的
                    TypeDeduce.Change<object>(251);//派生类向基类的转换
                    //TypeDeduce.Change<int>((object)25);//禁止反向转换
                }
            }
        }
        class Program
        {
            static Dictionary<string, int> CountWords(string text)
            {
                Dictionary<string, int> frequencies = new Dictionary<string, int>();
                string[] words = System.Text.RegularExpressions.Regex.Split(text, @"\W+");
                foreach (string word in words)
                {
                    if (frequencies.ContainsKey(word))
                    {
                        frequencies[word]++;
                    }
                    else
                    {
                        frequencies[word] = 1;
                    }
                }
                return frequencies;
            }
            static void Main()
            {

                string text = "Do you love me?" +
                    "Do you like reading book?";
                Dictionary<string, int> fre = CountWords(text);
                foreach (KeyValuePair<string, int> entry in fre)
                {
                    Console.WriteLine($"{entry.Key}：{entry.Value}");
                }
                int[,] xx = new int[,] { { 1, 2, 3 }, { 3, 4, 5 } };
                var set = new SortedSet<int> { 1, 5, 12, 20, 25 };
                var view = set.GetViewBetween(10, 20);
                view.Add(14);//view和set之间仍然有关联
                Console.WriteLine(set.Count);
                foreach (int value in view)
                {
                    Console.WriteLine(value);
                }
                Console.ReadKey();
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<Product> d=Product.GetProductsList();
            //排序
            ProductNameComparer pcmp = new ProductNameComparer();
            //d.Sort(pcmp.Compare);
            //d.Sort(delegate (Product x, Product y) { return x.Name.CompareTo(y.Name); });//委托
            //扩展方法进行排序
            //foreach(Product product in d.OrderBy(p=>p.Name))
            //{
            //    Console.WriteLine(product.Name);
            //}
            //查询集合
            //1 条件和操作分开
            //Predicate<Product> test = delegate (Product x) { return x.Price > 100; };
            //List<Product> matches = d.FindAll(test);
            //Action<Product> print = delegate(Product x) { Console.WriteLine($"{x.Name}:{x.Price}"); };
            //matches.ForEach(print);
            //2.LINQ版本
            //var filtered = from p in d
            //               where p.Price > 10
            //               select p;
            //foreach(Product x in filtered)
            //{
            //    Console.WriteLine($"{x.Name}:{x.Price}");
            //}
            //3.LINQ对XML处理 https://msdn.microsoft.com/en-us/library/system.xml.linq(v=vs.110).aspx
            XDocument doc = XDocument.Load("data.xml");
            var dff = doc.Descendants("Products");
            foreach(var i in dff)
            {
                i.Attribute("Price");
            }
            var filtered = from p in doc.Descendants("Product")
                           join s in doc.Descendants("Supplier")
                            on (int)p.Attribute("SupplierID")
                            equals (int)s.Attribute("SupplierID")
                           where (decimal)p.Attribute("Price") > 10
                           orderby (string)s.Attribute("Name"),
                                   (string)p.Attribute("Name")
                           select new
                           {
                               SupplierName = (string)s.Attribute("Name"),
                               ProductName = (string)p.Attribute("Name")
                           };
            foreach(var sp in filtered)
            {
                Console.WriteLine($"Supplier={sp.SupplierName},Product={sp.ProductName}");//显示价格>10的产品，并显示出供货商的信息
            }
            object[] oarray=new object[5];
            oarray[0] = 1;
            Console.ReadKey();
        }
    }

}
