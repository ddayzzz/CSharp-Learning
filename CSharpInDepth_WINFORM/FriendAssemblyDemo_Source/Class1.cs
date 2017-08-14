using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("CSharpInDepth_WINFORM")]//在CSharpInDepth_WINFORM可见
public class Source
{
    internal static void InternalMethod() { }
    public static void PublicMethid() { }
}