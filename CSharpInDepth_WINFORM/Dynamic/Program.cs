using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
namespace Dynamic
{
    class First
    {
        static void Main()
        {
            dynamic items = new List<string> { "Naive", "Angry", "Simple" };
            dynamic items2 = new List<int> { 1, 2, 3 };
            dynamic valueToAdd = 2;
            foreach (dynamic item in items2)
            {
                //string result = item + valueToAdd;
                Console.WriteLine(item + valueToAdd);
                //Console.WriteLine(result);
            }
            Console.ReadKey();
        }
    }
    namespace COM
    {
        class Excel
        {
            static void Main()
            {
                var app = new Application { Visible = true };
                app.Workbooks.Add();
                //Worksheet worksheet = app.ActiveSheet;
                //Range start = worksheet.Cells[1, 1];
                //Range end = worksheet.Cells[1, 20];
                //worksheet.Range[start, end].Value = Enumerable.Range(1, 20).ToArray();
                //代码14-7 使用dynamic
                dynamic worksheet = app.ActiveSheet;
                dynamic start = worksheet.Cells[1, 1];
                dynamic end = worksheet.Cells[1, 20];
                worksheet.Range[start, end].Value = Enumerable.Range(1, 20).ToArray();
                Console.ReadKey();
            }
        }

    }
    namespace CA14_Dynamic
    {
        class DynamicDeduce
        {
            private static bool AddConditionalImpl<T>(IList<T> list, T item)
            {
                if (list.Count < 10)
                {
                    //小于10个元素添加新的项目
                    list.Add(item);
                    return true;
                }
                return false;
            }
            private static bool AddConditionally(dynamic list, dynamic item)
            {
                return AddConditionalImpl(list, item);
            }

            public static void Main()
            {
                object list = new List<string> { "X", "Y" };
                object item = "Z";
                AddConditionally(list, item);
                Console.ReadKey();
            }
        }
        static class DynamicType
        {
            public static T? DynamicSum<T>(this IEnumerable<T?> source)
                where T : struct
            {
                T total = default(T);//值类型。如果是nullable的话，机会返回默认值而不是一个nullable对象
                foreach (T? item in source)
                {
                    if (item != null)
                    {
                        dynamic value = item.Value;
                        total = (T)(value + total);
                    }
                }
                return total;
            }
            public static T DynamicSum<T>(this IEnumerable<T> source)
            {
                dynamic total = default(T);
                foreach (T element in source)
                {
                    total = (T)(total + element);//值类型的+返回int，而如果没有强制转换的话，不存在从byte到int的转换
                }
                return total;
            }
            public static dynamic VeryDynamicSum(this object obj)
            {
                dynamic dynamicSource = obj;
                return DynamicSum(dynamicSource);
            }
            static void Main()
            {
                byte[] bytes = new byte[] { 1, 2, 3 };//传递的是值类型
                var times = new List<TimeSpan>
                {
                    TimeSpan.FromHours(2),TimeSpan.FromMinutes(25),TimeSpan.FromSeconds(45)
                };
                var tt = new List<int?>
                {
                    null,null,null
                };
                Console.WriteLine(tt.DynamicSum());//迭代器的值不会是null
                Console.WriteLine(tt.VeryDynamicSum());
                Console.WriteLine(bytes.DynamicSum());
                Console.WriteLine(times.DynamicSum());
                Console.ReadKey();
            }
        }
        class DuckType
        {
            static void PrintCount<T>(IEnumerable<T> collection)
            {
                dynamic d = collection;
                int count = d.Count;
                Console.WriteLine(count);
            }
            static void PrintCount<T>(ICollection<T> collection)
            {
                Console.WriteLine(collection.Count);
            }
            static void Main()
            {
                PrintCount(new HashSet<int> { 1, 2, 5 });
                PrintCount(new int[] { 1, 2, 5 });//隐式转换为ICollection
                List<dynamic> dynamicList = new List<dynamic> { 2.5, 3.3, 3.3 };//string不能转换为int
                List<int> intList = dynamicList.Select(x => Convert.ToInt32(x * x)).Cast<int>().ToList();//静态类型始终为dynamic。CLR将之视为类似于object类型。如果不Cast<int>则无法将类型“System.Collections.Generic.List<dynamic>”隐式转换为“System.Collections.Generic.List<int>”
                //List<int> intList = dynamicList.Select(x => x*x).Cast<int>().ToList();如果dynamicList包含double类型或者是不能隐式转换的类型，那么就会出错。
                //编译器总是在编译期或者是编译之前推出类型为静态的类型 ref http://csharpindepth.com/Articles/Chapter14/DynamicGotchas.aspx
                Console.ReadKey();
            }
        }
        //继承的体系下的动态重载决策
        class Base
        {
            public void Execute(object x) { }
        }
        class Derived : Base
        {
            public void Execute(dynamic x) { }
        }

        class DynamicInCompile
        {
            static void Execute(string x)
            {
                Console.WriteLine("string overload");
            }
            static void Execute(dynamic x)
            {
                Console.WriteLine("dynamic overload");
            }
            //代码14-21 混合静态和动态的泛型判断
            static void Execute<T>(T first, T second, string other) where T : struct
            {

            }
            static void Method(Action<string> action, string value)
            {
                action(value);
            }
            static void Main()
            {
                //动态的调用重载
                //dynamic text = "text";
                //Execute(text);
                //dynamic number = 10;
                //Execute(number);
                //
                //dynamic d = 0;
                //string x = "text";
                //var array = new[] { d, x };
                //Console.ReadKey();
                //查看IL
                //string text = "text to cut";
                //dynamic startIndex = 2;
                //string substring = text.Substring(startIndex);
                //类层次结构
                //Base receiver = new Derived();
                //dynamic d = "text";
                //receiver.Execute(d);
                //代码14-21 混合静态和动态的泛型判断
                //dynamic guid = Guid.NewGuid();
                //Execute(10, 0, guid);
                //代码14-22
                //dynamic size = 5;
                //var numbers = Enumerable.Range(10, 10);
                //bool jh = numbers is IQueryable;
                //var error = numbers.Take(size);//出现错误。不能动态处理扩展方法。
                //var workaround1 = numbers.Take((int)size);
                //var workaround2 = Enumerable.Take(numbers, size);//使用扩展方法的方法调用的形式
                //代码14-23 动态类型和lambda表达式：必须使用委托的类型
                //dynamic badMethodGroup = Console.WriteLine;//需要使用委托
                //dynamic goodMethodGroup = (Action<string>)Console.WriteLine;
                ////dynamic badLambda = y => y + 1;//这个lambda也非委托
                //dynamic goodLambda = (Func<int, int>)(y => y + 1);
                //dynamic veryDynamic = (Func<dynamic, dynamic>)(d => d.Do());
                dynamic text = "error";
                Method((Action<string>)(x => Console.WriteLine(x)), text); //lambda必须保存为委托类型
                //代码14-24 查询动态元素的集合
                var list = new List<dynamic> { 50, 5m, 5d };//d m是双精度浮点、decimal类型的字面值后缀
                var query = from number in list
                            where number > 4
                            select (number / 20) * 10;
                foreach (var item in query)
                {
                    Console.WriteLine(item);
                }
                Type type = typeof(object);
                Console.ReadKey();
            }
        }
    }
    namespace CA14_5_1_DynamicImpl
    {
        using System.Dynamic;
        using System.Xml.Linq;
        class ExpandoObjectDemo
        {
            public static dynamic CreateDynamicXml(XElement element)
            {
                dynamic expando = new ExpandoObject();
                expando.XElement = element;
                expando.ToXml = (Func<string>)element.ToString;
                IDictionary<string, object> dictionary = expando;
                foreach (XElement subElement in element.Elements())
                {
                    dynamic subNode = CreateDynamicXml(subElement);
                    string name = subElement.Name.LocalName;
                    string listName = name + "List";//如果包含
                    if (dictionary.ContainsKey(name))
                    {
                        ((List<dynamic>)dictionary[listName]).Add(subNode);

                    }
                    else
                    {
                        dictionary[name] = subNode;
                        dictionary[listName] = new List<dynamic> { subNode };
                    }
                }
                return expando;
            }
            public static void XMLRead()
            {
                XDocument doc = XDocument.Load("books.xml");
                dynamic root = CreateDynamicXml(doc.Root);
                Console.WriteLine(root.book.author.ToXml());
                Console.WriteLine(root.bookList[2].except.XElement.Value);

            }
            static void Main()
            {
                //dynamic expando = new ExpandoObject();
                //IDictionary<string, object> dictionary = expando;
                //expando.First = "value set dynamically";
                //Console.WriteLine(dictionary["First"]);
                //dictionary["Second"] = "value set with dictionary";
                //Console.WriteLine(expando.Second);
                //代码14-25
                //dynamic expando = new ExpandoObject();
                //expando.AddOne=(Func<int, int>)(x => x + 1);//添加函数AddOne
                //Console.Write(expando.AddOne(10));
                XMLRead();
                Console.ReadKey();
            }
        }
        namespace DynamicObjectDemo
        {
            public class DynamicXElement : DynamicObject
            {
                private readonly XElement element;
                private DynamicXElement(XElement element)
                {
                    this.element = element;
                }
                public static dynamic CreateInstance(XElement element)
                {
                    return new DynamicXElement(element);
                }
                //
                public override string ToString()
                {
                    return element.ToString();
                }
                public XElement XElement
                {
                    get { return element; }
                }
                public XAttribute this[XName name]
                {
                    get { return element.Attribute(name); }
                }
                public dynamic this[int index]
                {
                    get
                    {
                        XElement parent = element.Parent;
                        if (parent == null)//是否是根节点
                        {
                            if (index != 0)
                            {
                                throw new ArgumentOutOfRangeException();
                            }
                            return this;
                        }
                        XElement sibling = parent.Elements(element.Name).ElementAt(index);//如果不存在多个Name相同的元素返回null
                        return element == sibling ? this :
                            new DynamicXElement(sibling);
                    }
                }
                //Try类型方法
                //获取属性的值
                public override bool TryGetMember(GetMemberBinder binder, out object result)//binder类似于this引用。动态的
                {
                    Console.WriteLine("获取成员");
                    XElement subElement = element.Element(binder.Name);
                    if (subElement != null)
                    {
                        result = new DynamicXElement(subElement);
                        return true;
                    }
                    return base.TryGetMember(binder, out result);
                }
                //获取类的成员名称类似于python中dir()
                public override IEnumerable<string> GetDynamicMemberNames()
                {
                    return element.Elements().Select(x => x.Name.LocalName)
                        .Distinct()
                        .OrderBy(x => x);
                }
            }
            class Program
            {
                static void Main()
                {
                    XDocument doc = XDocument.Load("books.xml");
                    dynamic root = DynamicXElement.CreateInstance(doc.Root);
                    Console.WriteLine(root.book[2]["name"]);
                    Console.WriteLine(root.book[1].author[1]);
                    Console.WriteLine(root.book);
                    Console.WriteLine("显示类的成员名：");
                    foreach (string item in root.GetDynamicMemberNames())
                    {
                        Console.WriteLine(item);
                    }
                    Console.ReadKey();
                }
            }
        }
        namespace IDynamicMetaObjectProviderDemo
        {
            using System.Linq.Expressions;
            using System.Reflection;
            public sealed class Rumpelstiltskin : IDynamicMetaObjectProvider
            {
                private readonly string name;
                public Rumpelstiltskin(string name)
                {
                    this.name = name;
                }

                public DynamicMetaObject GetMetaObject(Expression expression)
                {
                    return new MetaRumpelstiltskin(expression, this);
                }

                private object RespondToWrongGuess(string guess)
                {
                    Console.WriteLine("No, I'm not {0}! (I'm {1}.)",
                        guess, name);
                    return false;
                }

                private object RespondToRightGuess()
                {
                    Console.WriteLine("Curses! Foiled again!");
                    return true;
                }

                private class MetaRumpelstiltskin : DynamicMetaObject
                {
                    private static readonly MethodInfo RightGuessMethod =
                        typeof(Rumpelstiltskin).GetMethod("RespondToRightGuess",
                        BindingFlags.Instance | BindingFlags.NonPublic);

                    private static readonly MethodInfo WrongGuessMethod =
                        typeof(Rumpelstiltskin).GetMethod("RespondToWrongGuess",
                        BindingFlags.Instance | BindingFlags.NonPublic);

                    internal MetaRumpelstiltskin
                        (Expression expression, Rumpelstiltskin creator)
                        : base(expression, BindingRestrictions.Empty, creator)
                    {
                    }

                    public override DynamicMetaObject BindInvokeMember
                        (InvokeMemberBinder binder, DynamicMetaObject[] args)
                    {
                        Rumpelstiltskin targetObject = (Rumpelstiltskin)base.Value;
                        Expression self = Expression.Convert(base.Expression,
                            typeof(Rumpelstiltskin));

                        Expression targetBehavior;
                        if (binder.Name == targetObject.name)
                        {
                            targetBehavior = Expression.Call(self, RightGuessMethod);
                        }
                        else
                        {
                            targetBehavior = Expression.Call(self, WrongGuessMethod,
                                Expression.Constant(binder.Name));
                        }

                        var restrictions = BindingRestrictions.GetInstanceRestriction
                            (self, targetObject);
                        return new DynamicMetaObject(targetBehavior, restrictions);
                    }
                }
            }
            class Test
            {
                static void Main()
                {
                    dynamic x = new Rumpelstiltskin("Hermione");
                    x.Harry();
                    x.Ron();
                    x.Herminone();
                    Console.WriteLine("第二轮测试");
                    x = new Rumpelstiltskin("Haha");
                    x.Liu();
                    x.Haha(25);
                    Console.ReadKey();
                }
            }
        }

    }
}
