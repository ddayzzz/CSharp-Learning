using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
namespace SQLAccess
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectString = @"Server=.\DESKTOP-QBVP6A5;Database=CSharp_SQL;User ID=sa;Password=@wangshu1715$";
            System.Security.SecureString secureStr = new System.Security.SecureString();
            secureStr.AppendChar('@');
            secureStr.AppendChar('w');
            secureStr.AppendChar('a');
            secureStr.AppendChar('n');
            secureStr.AppendChar('g');
            secureStr.AppendChar('s');
            secureStr.AppendChar('h');
            secureStr.AppendChar('u');
            secureStr.AppendChar('1');
            secureStr.AppendChar('7');
            secureStr.AppendChar('1');
            secureStr.AppendChar('5');
            secureStr.AppendChar('$');
            SqlConnection conn = new SqlConnection(connectString);
            conn.Open();
            Console.ReadKey();
        }
    }
}
