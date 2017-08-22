using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
namespace Chapter13
{
    //序列化
    namespace Serialization
    {
        enum Sex { Male,Female};
        [Serializable]
        class Person
        {
            public string Name { get; private set; }
            public Sex Sex { get; private set; }
            public Person(string name,Sex sex)
            {
                Name = name;
                Sex = sex;
            }
        }
        [Serializable]//基类可序列化，子类不一定继承这个特性
        class Student:Person
        {
            public int ID { get; private set; }
            public Student(string name,Sex sex,int id):base(name,sex)
            {
                ID = id;
            }
        }
        [Serializable]//基类可序列化，子类不一定继承这个特性
        class Worker : Person
        {
            public int Salary { get; private set; }
            public Worker(string name, Sex sex, int salary) : base(name, sex)
            {
                Salary = salary;
            }
        }
        class Binary//二进制的可序列化
        {
            static void Main()
            {
                Person pp = new Person("Fang", Sex.Male);
                Person ps = new Student("Shu", Sex.Female, 20);
                Student ss = new Student("Hu", Sex.Female, 222);
                Person pw = new Worker("KK", Sex.Female, 50);
                FileStream fs = new FileStream("ser.dat",FileMode.OpenOrCreate);
                FileStream fsw = new FileStream("ser_w.dat", FileMode.OpenOrCreate);
                BinaryFormatter b = new BinaryFormatter();
                b.Serialize(fs, ps);
                b.Serialize(fsw, pw);
                fs.Close();
                fsw.Close();
                //反序列化
                FileStream rd = new FileStream("ser.dat", FileMode.Open, FileAccess.Read);
                BinaryFormatter rb = new BinaryFormatter();
                Person r_pp = rb.Deserialize(rd) as Person;//序列化的对象是Student，所以r_pp的动态类型是Student
                Console.WriteLine(r_pp);
                //worker
                FileStream rdw = new FileStream("ser_w.dat", FileMode.Open, FileAccess.Read);
                Person r_pw = rb.Deserialize(rdw) as Person;//Worker->Student不存在转换的关系返回null。如果是Person，从Worker->Person逆变存在，动态的类型为Worker
                rd.Close();
                rdw.Close();

            }
        }
    }
}
