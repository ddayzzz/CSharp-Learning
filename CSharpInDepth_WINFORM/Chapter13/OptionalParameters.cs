using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using Microsoft.Office.Interop.Word;
namespace Chapter13
{
    class ClassWithIndexr
    {
        private List<string> data = new List<string>();
        public ClassWithIndexr()
        {
            data = new List<string>
            {
                "hello","naive","young"
            };
        }
        public string this[int index]
        {
            get { return data[index]; }
            
        }
        public int this[string NamedIndex]
        {
            get
            {
                int beg = 0;
                for(;beg <data.Count;++beg)
                {
                    if(data[beg] == NamedIndex)
                    {
                        return beg;
                    }
                }
                return -1;
            }
        }
    }
    class OptionalParameters
    {
        static void AppendTimestamp(string filename,
            string message,
            Encoding encoding=null,
            DateTime? timestamp=null)
        {
            Encoding realEncoding = encoding ?? Encoding.UTF8;
            DateTime realTimestamp = timestamp ?? DateTime.Now;
            using (TextWriter writer = new StreamWriter(filename, true, realEncoding))
            {
                writer.WriteLine("{0:s}:{1}", realTimestamp, message);
            }
        }
        
        static void Main(string[] args)
        {
            //AppendTimestamp("utf8.txt", "First");
            //AppendTimestamp("ASCII.txt", "ASCII", null);
            //AppendTimestamp("utf8.txt", "Second message", Encoding.UTF8, new DateTime(2030, 1, 1));
            //命名实参
            //MessageBox.Show(caption: "Hello", text: "Welcome!");

            Console.ReadKey();
        }
    }
    class COMOperation
    {
        static void ShowInfo(SynonymInfo info)
        {
            Console.WriteLine("{0} has {1} meanings", info.Word, info.MeaningCount);
        }
        static void Main()
        {
            //命名索引
            //ClassWithIndexr cl = new ClassWithIndexr();
            //int h = cl[NamedIndex: "naive"];
            //Console.WriteLine(h);
            //word
            //Application app = new Application { Visible = true };
            //app.Documents.Add();
            //Document doc= app.ActiveDocument;
            //Paragraph para = doc.Paragraphs.Add();
            //para.Range.Text = "Thank godness for C#4";
            //object filename = "D:\\demo.doc";//必须指定完整的路径
            //object format = WdSaveFormat.wdFormatDocument;
            //doc.SaveAs2(FileName: ref filename, FileFormat: ref format);
            //doc.Close();
            //app.Application.Quit();
            //使用命名索引器展示同义词数量
            Application app = new Application { Visible = true };
            object missing = Type.Missing;
            ShowInfo(app.SynonymInfo["nice", WdLanguageID.wdEnglishUS]);
            ShowInfo(app.SynonymInfo[Word: "features"]);
            Console.ReadKey();
        }
    }
    namespace CovariantInInterface
    {
        using Chapter13.Serialization;//引用一些使用多态的类
        class Covariant
        {
            //支持逆变的比较类
            class NameComparer:IComparer<Person>
            {
                public int Compare(Person p,Person q)
                {
                    return p.Name.CompareTo(q.Name);
                }
            }
            static void Print(Person p)
            {
                Console.WriteLine("Name={0}, Sex={1}", p.Name, p.Sex);
            }
            //out用于普通函数
            static void Func_Out(string original, out int value)
            {
                value = int.Parse(original);
            }
            //多播委托与不可变性
            static void MultiBroadcasting()
            {
                Func<string> stringFunc = () => { Console.WriteLine("stringfunc");return ""; };
                Func<object> defensiveCopy = new Func<object>(stringFunc);//动态类型
                Func<object> objectFunc = () => { Console.WriteLine("objectFunc"); return new object(); };
                Func<object> combined = objectFunc + defensiveCopy;
                combined();
            }
            static void Main()
            {
                //List<Person> person = new List<Person>
                //{
                //    new Student("Shu",Sex.Male,20),
                //    new Worker("Kang",Sex.Male,8),
                //    new Student("Alice",Sex.Female,5)
                //};
                //IComparer<Person> comp = new NameComparer();
                //person.Sort(comp);
                //foreach(var s in person)
                //{
                //    Print(s);
                //}
                //func、action
                //Func<Student> studentFactory = () => new Student("shu", Sex.Male,20);
                //Func<Person> personFactory = studentFactory;

                //Action<Person> personPrint = person => Console.WriteLine($"Name={person.Name}, Sex={person.Sex}");
                //Action<Student> studentPrint = personPrint;

                //studentPrint(studentFactory());
                //personPrint(personFactory());
                //多播委托与不可变性
                MultiBroadcasting();
                Console.ReadKey();
            }
        }
    }
}
