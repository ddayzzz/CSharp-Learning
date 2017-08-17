using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Linq.Expressions;
using System.Collections;

namespace CA12_LINQ_TO_SQL
{
    //class CA12_LINQ_TO_SQL
    //{
    //    static void Main()
    //    {
    //        using (var context = new DefectModelDataContext())
    //        {
    //            context.Log = Console.Out;
    //            var query = from user in context.Users
    //                        let length = user.Name.Length
    //                        orderby length
    //                        select new { Name = user.Name, Length = length };
    //            foreach(var entry in query)
    //            {
    //                Console.WriteLine("{0}:{1}", entry.Length, entry.Name);
    //            }
    //            Console.ReadKey();
    //        }
    //    }
    //}
    namespace Query
    {
        class Code_12_2
        {
            class FakeQuery<T> : IQueryable<T>
            {
                public Expression Expression { get; private set; }
                public IQueryProvider Provider { get; private set; }
                public Type ElementType { get; private set; }
                //
                internal FakeQuery(IQueryProvider provider,Expression expression)
                {
                    Expression = expression;
                    Provider = provider;
                    ElementType = typeof(T);
                }
                internal FakeQuery():this(new FakeQueryProvider(),null)
                {
                    Expression = Expression.Constant(this);
                }
                public IEnumerator<T> GetEnumerator()
                {
                    Logger.Log(this, Expression);
                    return Enumerable.Empty<T>().GetEnumerator();
                }
                public override string ToString()
                {
                    return "FakeQuery";
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    Logger.Log(this, Expression);
                    return Enumerable.Empty<T>().GetEnumerator();
                }
            }
            class FakeQueryProvider:IQueryProvider
            {
                public IQueryable<T> CreateQuery<T>(Expression expression)
                {
                    Logger.Log(this, expression);
                    return new FakeQuery<T>(this, expression);
                }
                public IQueryable CreateQuery(Expression expression)
                {
                    Type queryYype = typeof(FakeQuery<>).MakeGenericType(expression.Type);
                    object[] constructorArgs = new object[] { this, expression };
                    return (IQueryable)Activator.CreateInstance(queryYype, constructorArgs);
                }
                public T Execute<T>(Expression expression)
                {
                    Logger.Log(this, expression);
                    return default(T);
                }
                public object Execute(Expression expression)
                {
                    Logger.Log(this, expression);
                    return null;
                }
            }
            static void Main()
            {
                //模拟查询Fake
                //var query = from x in new FakeQuery<string>()
                //            where x.StartsWith("abc")
                //            select x.Length;
                //foreach(int i in query)
                //{
                //    ;
                //}
                //
                var query = from x in new FakeQuery<string>()
                            where x.StartsWith("abc")
                            select x.Length;
                double mean = query.Average();
                

                Console.ReadKey();
            }
        }
    }
}
