using System;
using System.Collections.Generic;
using System.IO;
using IronPython.Hosting;
using IronPython.Runtime;

namespace ConsoleApplication1 {

class Program {
    static void Main(string[] args) {
        var engine = Python.CreateEngine(new Dictionary<string, object>() { { "Debug", true }, {"ExceptionDetails",true} });
        engine.Runtime.LoadAssembly(typeof(System.Linq.Enumerable).Assembly);
        engine.Runtime.LoadAssembly(typeof(System.String).Assembly);
        engine.Runtime.LoadAssembly(typeof(System.Diagnostics.Process).Assembly);
        engine.Runtime.LoadAssembly(typeof(DLRCachedCode).Assembly);

        try {
            engine.Execute(@"
import sys
from os import path
def f(*args):
    return f
sys.settrace(f)
path.abspath('Z:\\ipy\\issues\\LibPy.dll')
def g(): pass
g()
");
        } catch(Exception e) {
            Console.WriteLine(e);
        }
        Console.ReadLine();
    }
}
}