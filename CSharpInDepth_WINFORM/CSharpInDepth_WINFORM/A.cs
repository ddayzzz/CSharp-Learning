using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth_WINFORM
{
    //附录A
    class A
    {
        //关于延迟和立即求值：https://stackoverflow.com/questions/2515796/deferred-execution-and-eager-evaluation
        static int Computation(int i) { return i; }
        //延迟但是热请求值
        static IEnumerable<int> GetComputation_Deferred_Eager(int maxIndex)
        {
            var result = new int[maxIndex];
            for (int i = 0; i < maxIndex; i++)
            {
                result[i] = Computation(i);
            }
            foreach (var value in result)
            {
                yield return value;
            }
        }
        static void Main()
        {
            //var words = new List<string>
            //{
            //    "one","two","zero"
            //};
            //var n=words.SelectMany((w, i) => Enumerable.Repeat(w, i));
            //foreach(var item in n)
            //{
            //    Console.WriteLine(item);
            //}
            //var d=words.ToDictionary(word => word[0]);
            //int j=2;
            var de=GetComputation_Deferred_Eager(20);
            foreach(var item in de)
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }
    }
}
