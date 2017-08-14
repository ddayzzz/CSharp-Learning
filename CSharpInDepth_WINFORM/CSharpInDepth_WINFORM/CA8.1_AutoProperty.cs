using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth_WINFORM
{
    namespace AutoProperty
    {
        public class Location
        {
            public string Country { get; set; }
            public string Town { get; set; }
        }
        public class Person
        {
            public int Age { get;set; }
            public string Name { get; set; }
            public bool IsAdult { get; set; }

            List<Person> friends=new List<Person>();
            public List<Person> Friends { get { return friends; } }

            Location home=new Location();
            public Location Home { get { return home; } }
            public Person()
            {
                
            }
            public Person(string name)
            {
               
                this.Name = name;
            }
            static void Main()
            {
                Person tom1 = new Person("Tom") { Age = 9 };
                Person tom2 = new Person("Tom")
                {
                    Age = 52,
                    home = new Location { Country = "United Kingdom", Town = "Paddington" }
                };
                //等价的&使用集合初始化列表
                Person tom3 = new Person("Tom")
                {
                    Age = 52,
                    home = { Country = "America", Town = "Ohio" },
                    Friends =
                    {
                        new Person
                        {
                            Name = "Shu",
                            Age = 19,
                            Friends =
                            {
                                new Person("FEF")
                            }
                        },
                        new Person("Wang"){Age=55 }
                    }
                };
                //计算年龄
                var family = new List<Person>
                {
                    new Person{Name="Jhon",Age=9},
                    new Person{Name="Alice",Age=10},
                    new Person{Name="Tom",Age=50}
                };
                var converted = family.ConvertAll(delegate (Person person) { return new { person.Name, person.Age,IsAudlt = (person.Age >= 18) }; });
                foreach(var member in converted)
                {
                    Console.WriteLine("Member:{0}, IsAdult:{1}", member.Name, member.IsAudlt);
                }
                
                Console.ReadKey();
            }
        }
    }
    namespace AnonymousArray
    {
        class Program
        {
            static void Main()
            {
                var arr = new[] { (object)new System.IO.MemoryStream(), new System.IO.StringReader("geg") };//这连个实例都可以转换到object和Idispose,需要指明
            }
        }
    }
}
