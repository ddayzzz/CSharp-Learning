using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
namespace CA12_3_LINQ_TO_XML
{
    using CSharpInDepth_WINFORM.CA11_LINQ_To_Objects.SkeetySoft;
    using SampleData = CSharpInDepth_WINFORM.CA11_3_SampleData;
    class Program
    {
        static void Main(string[] args)
        {
            //var users = new XElement("users", SampleData.AllUsers.Select(user => new XElement("user",
            //     new XAttribute("name", user.Name),
            //     new XAttribute("type", user.UserType))));
            //Console.WriteLine(users);
            //创建文本节点元素
            //var developers = new XElement("developers",
            //    from user in SampleData.AllUsers
            //    where user.UserType == UserType.Developer
            //    select new XElement("developer", user.Name));
            //Console.WriteLine(developers);
            //添加组合的XML元素
            //var projects = new XElement("projects",
            //    from project in SampleData.AllProjects
            //    select new XElement("project",
            //    new XAttribute("name", project.Name)));
            //var users = new XElement("users",
            //    from user in SampleData.AllUsers
            //    select new XElement("user",
            //    new XAttribute("name", user.Name),
            //    new XAttribute("type", user.UserType)));
            //XElement root = new XElement("defect-system", projects, users);
            //Console.WriteLine(root);
            //显示特定的XML结构
            XElement root = XmlExamples.XmlSampleData.GetElement();
            var query = root.Element("users").Elements().Select(user => new
            {
                Name = (string)user.Attribute("name"),
                UserType = (string)user.Attribute("type")
            });
            foreach (var user in query)
            {
                Console.WriteLine("{0}:{1}", user.Name, user.UserType);
            }
            //合并查询运算符
            var query2 =from project in root.Elements()
                        from subscription in project.Elements("subscription")
                        select subscription;
            Console.WriteLine("subscription:");
            foreach(var subscription in query2)
            {
                Console.WriteLine(subscription);
            }
            var subs = root.Element("projects").Elements("").Elements("subscription");
            Console.ReadKey();
        }
    }
}
