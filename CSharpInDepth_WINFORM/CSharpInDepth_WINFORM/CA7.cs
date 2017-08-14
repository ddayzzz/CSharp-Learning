//外部别的名：具有相同命名空间的类的外部名称:置于顶层
//for CA7_ADVANCED_EXTERNALextern alias FirstAlias;
//for CA7_ADVANCED_EXTERNAL extern alias SecondAlias;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//for CA7_ADVANCED_EXTERNAL using FD = FirstAlias::Demo;
namespace CSharpInDepth_WINFORM
{
    //namespace CA7_ADVANCED_INCS2
    //{
    //    partial class PartialMethodDemo
    //    {
    //        partial void OnConstructStart()
    //        {
    //            Console.WriteLine("Manual code");
    //        }

    //    }
    //}
    namespace CA7_ADVANCED_EXTERNAL
    {
        //外部别的名：具有相同命名空间的类的外部名称
        //class ExternalAlias
        //{
        //    static void Main()
        //    {
        //        Console.WriteLine(typeof(FD.Example));
        //        Console.WriteLine(typeof(SecondAlias::Demo.Example));
        //        Console.ReadKey();
        //    }
        //}
       
    }
    namespace CA7_ADVANCED_PRAGMA
    {
        public class FieldUsedOnlyByReflection
        {
            //pragma实现与C#编译器的实现有关
#pragma warning disable 0169//忽略错误警报CS0169
            int x;//CS0169
#pragma warning restore 0169
            static void Main()
            {

            }
        }


    }
}

