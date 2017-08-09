using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Point
using System.Drawing;
using System.Collections;

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

    class Var
    {
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
