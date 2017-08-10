using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Point
using System.Drawing;
using System.Collections;
//PartialComparer
using System.ComponentModel;

namespace CSahrp_in_depth
{
    public interface IShape
    {
        double Area { get; }
    }
    public interface IDrawing
    {
        IEnumerable<IShape> Shapes { get; }//子类型必须实现这个属性
    }
    public class Circle:IShape
    {
        private readonly double radius;
        public Circle(double radius)
        {
            this.radius = radius;
        }
        public double Area { get { return Math.PI * radius * radius; } }
        public double Radius { get { return radius; } }
    }
    public class Rectangle : IShape
    {
        private readonly Size size;
        public Rectangle(Size size)
        {
            this.size = size;
        }
        public double Area { get { return size.Height * size.Width; } }
    }
    public class ModrianDrawing:IDrawing
    {
        public IEnumerable<IShape> Shapes { get { throw new NotImplementedException(); } }

    }
    public class SeuratDrawing:IDrawing
    {
        public IEnumerable<IShape> Shapes { get { throw new NotImplementedException(); } }
    }
    //圆形的比较器
    public class CircleComparer:IComparer<Circle>
    {
        public int Compare(Circle lhs,Circle rhs)
        {
            return (int)(lhs.Area - rhs.Area);
        }
    }
    public class ComparisonHelper<TBase,TDerived>:IComparer<TDerived>
        where TDerived:TBase
    {
        private readonly IComparer<TBase> comparer;
        public ComparisonHelper(IComparer<TBase> comparer)
        {
            this.comparer = comparer;
        }
        public int Compare(TDerived x,TDerived y)
        {
            return comparer.Compare(x, y);
        }
    }
    //通用面积比较类
    public sealed class AreaComparer : IComparer<IShape>
    {
        public int Compare(IShape x, IShape y)
        {
            return x.Area.CompareTo(y.Area);
        }
    }
    //委托的协变
    public class First { }
    public class Second : First { }
    public delegate First SampleDelegate(Second a);
    public delegate R SampleGenericDelegate<A, R>(A a);//
    public delegate T SampleGenericDelegate<out T>();//out支持返回类型协变 in支持参数逆变 https://docs.microsoft.com/zh-cn/dotnet/csharp/programming-guide/concepts/covariance-contravariance/variance-in-delegates
    //类型限制
    class Var
    {
        //可变性
        public static First ASecondRFirst(Second first) { return new First(); }
        public static Second ASecondRSecond(Second second) { return new Second(); }
        public static First AFirstRFirst(First first){ return new First(); }
        public static Second AFirstRSecond(First first){ return new Second(); }
        static void Main()
        {
            //IComparer<Circle> areaComparer = new CircleComparer();
            IComparer<IShape> areaComparer = new AreaComparer();
            ComparisonHelper<IShape, Circle> areaComp = new ComparisonHelper<IShape, Circle>(areaComparer);
            List<Circle> circles = new List<Circle>();
            circles.Add(new Circle(2));
            circles.Add(new Circle(20));
            circles.Add(new Circle(12));
            circles.Add(new Circle(14));
            circles.Sort(areaComp);
            foreach(Circle c in circles)
            {
                Console.WriteLine($"Circle radius:{c.Radius}");
            }
            //委托的变体
            SampleDelegate dNonGeneric = ASecondRFirst;
            SampleDelegate dNonGenericConversion = AFirstRSecond;
            SampleGenericDelegate<Second, First> dGeneric = ASecondRFirst;
            SampleGenericDelegate<Second, First> dGenericConversion = AFirstRSecond;
            SampleGenericDelegate<string> hahah = () => "";
            SampleGenericDelegate<object> obj = hahah;
            Console.ReadKey();
        }
    }
}
//研究可空类
namespace CSahrp_in_depth
{
    class DisplayNullableMembers
    {
        static void Display(Nullable<int> x)
        {
            Console.WriteLine("HasValue:{0}", x.HasValue);
            if(x.HasValue)
            {
                Console.WriteLine("Value:{0}", x.Value);
                Console.WriteLine("Explicit conversion:{0}", (int)x);//nullable拆箱
            }
            Console.WriteLine("GetValueOrDefault():{0}", x.GetValueOrDefault());
            Console.WriteLine("GetValueOrDefault(10):{0}", x.GetValueOrDefault(10));
            Console.WriteLine("ToString():\"{0}\"", x.ToString());
            Console.WriteLine("GetHashCode():{0}", x.GetHashCode());

        }
        static void Main()
        {
            Nullable<int> x = 5;
            //x = new Nullable<int>(5);
            Console.WriteLine("Instance with value:");
            Display(x);
            x = new Nullable<int>();
            Console.WriteLine("Instance without value:");
            //Display(x);
            bool dd = null == (object)x;//x拆箱的引用为null
            Console.ReadKey();
        }
    }
    class Person:IEquatable<Person>
    {
        DateTime birth;
        DateTime? death;
        string name;
        public TimeSpan Age
        {
            get
            {
                #region          
                /*
                if(death == null)
                {
                    return DateTime.Now - birth;
                }
                else
                {
                    return death.Value - birth;//拆包
                }
                */
                #endregion
                DateTime lastAlive = death ?? DateTime.Now;
                return lastAlive - birth;
            }
        }
        public Person(string name,DateTime birth,DateTime? death)
        {
            this.name = name;
            this.birth = birth;
            this.death = death;
        }
        
        public bool Equals(Person p)
        {
            bool nameAndBirth = (name == p.name) && (birth == p.birth);
            if(death.HasValue)
            {
                return nameAndBirth && p.death.HasValue;
            }
            else
            {
                return nameAndBirth && !p.death.HasValue;
            }
        }
        static void Main()
        {
            Person turing = new Person("Alan Turing", new DateTime(1912, 6, 23), new DateTime(1954, 6, 7));
            Person turing2 = new Person("Alan Turing", new DateTime(1912, 6, 23), new DateTime(1954, 6, 7));
            Person knuth = new Person("Donald Knuth", new DateTime(1938, 6, 7), null);
            Person jiang = new Person("Jiang zemin", new DateTime(1926, 8, 17), null);
            Console.WriteLine("{0}", knuth.Equals(jiang));
            Console.WriteLine("{0}", turing.Equals(turing2));
            //int是值类型，永远不会是null。除非是int?
            int Int = 5;
            if(Int ==null)//CS0472
            {

            }
            Console.ReadKey();
        }
    }
    public static class NullableDemo
    {
        public static int? TryParse(string value)
        {
            int ret;
            if (int.TryParse(value, out ret))
                return ret;
            else
                return null;
        }
        public static class PartialComparer//部分比较辅助类
        {
            public static int? Compare<T>(T first,T second)
            {
                return Compare(Comparer<T>.Default, first, second);
            }
            public static int? Compare<T>(IComparer<T> comparer,T first,T second)
            {
                int ret = comparer.Compare(first, second);
                return ret == 0 ? new int?() : ret;
            }
            public static int? ReferenceCompare<T>(T first,T second)
                where T:class
            {
                return first == second ? 0
                    : first == null ? -1
                    : second == null ? 1
                    : new int?();//条件运算符是右结合的
            }
        }
        public static void Main()
        {

        }
    }
    namespace ProductionCompare
    {
        class Product
        {
            private string name;
            private int price;
            public string Name { get { return name; } }
            public int Price { get { return price; } }
            public Product(string name, int price)
            {
                this.name = name;
                this.price = price;
            }
        }
        //比较器：实现Product的比较方法
        class ProductComparer:IComparer<Product>,IEqualityComparer<Product>//IComparable<Product>适用于当前类的对象与另一个相同对象或不同对象进行比较
        {
            
            //实现比较
            public int Compare(Product lhs,Product rhs)
            {
                return NullableDemo.PartialComparer.ReferenceCompare(lhs, rhs) ??//在比较对象：是否引用了同一个对象
                    NullableDemo.PartialComparer.Compare(lhs.Name, rhs.Name) ??//再比较名字
                    NullableDemo.PartialComparer.Compare(lhs.Price, rhs.Price) ??//先比较价格
                    0;
            }
            public bool Equals(Product lhs,Product rhs)
            {
                //??的优先级高于&&
                return NullableDemo.PartialComparer.ReferenceEquals(lhs, rhs) &&
                    lhs.Name == rhs.Name &&
                    lhs.Price == rhs.Price;
            }
            public int GetHashCode(Product p)
            {
                throw new NotImplementedException();
            }
            public static void Main()
            {
                IComparer<Product> comp = new ProductComparer();
                Product book = new Product(name: "C++ Primer", price: 100);
                Product book2 = new Product(name: "C++ Primer", price: 10);
                Product pc = new Product(name: "Apple PC", price: 10000);
                Product hdd = new Product(name: "Seagate 1TB", price: 100);
                Product bbc = new Product(name: "BBC", price: 5);
                Product[] products = new Product[] { book,book2, pc, hdd, bbc };
                Array.Sort(products,comp);
                foreach(Product p in products)
                {
                    Console.WriteLine($"{p.Name}:{p.Price}");
                }
                Product bbc2 = new Product(name: "BBC", price: 5);
                Console.WriteLine(comp.Compare(bbc2,bbc));//引用对象地址不同，comp.Compare方法返回比较的属性相同
                Product none = null;
                Console.WriteLine(comp.Compare(none, bbc)==0);
                Console.ReadKey();
            }
        }
    }
}
