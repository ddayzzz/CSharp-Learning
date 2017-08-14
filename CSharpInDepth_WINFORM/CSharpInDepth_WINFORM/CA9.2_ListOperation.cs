using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth_WINFORM
{
    namespace CA9_Film
    {
        class Film
        {
            public string Name { get; set; }
            public int Year { get; set; }
        }
        class Program
        {
            static void Main()
            {
                var films = new List<Film>
                {
                    new Film{Name="Warwolf 2",Year=2017},
                    new Film{Name="Wonders of solar system",Year=2013},
                    new Film{Year=2016,Name="The Chinese New Year"}
                };
                Action<Film> print = film => Console.WriteLine($"Name={film.Name},Year={film.Year}");
                films.ForEach(print);
                films.FindAll(film => film.Year >= 2016).ForEach(print);
                films.Sort((f1, f2) => f1.Year.CompareTo(f2.Year) );
                films.ForEach(print);
                Console.ReadKey();
            }
        }
    }
}
