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
                //
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
