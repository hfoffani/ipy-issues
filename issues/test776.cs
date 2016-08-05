using System;
using System.Threading;
using IronPython.Hosting;
using IronPython.Runtime;
using IronPython.Compiler;
using System.Collections.Generic;
using Microsoft.Scripting.Hosting;

namespace IPyTest
{

class Program
{
    static void Main(string[] args)
    {
        int cont = 2000;
        while (cont-- > 0)
        {
            var ipy = new IPy();
            try
            {
                // Set the below boolean to "false" to run without a memory leak
                // Set it to "true" to run with a memory leak.
                ipy.run(true);
            }
            catch { }
        }
        System.Console.ReadLine();
    }
}


class IPy
{
    private string scriptWithoutLeak = "import random; random.randint(1,10)";
    private string scriptWithLeak = @"
from System.Collections.Generics import List 
list = Listint 
list.AddRange(range(1, 1000000)) 
raise Exception(), 'error'
";

    public IPy()
    {
    }

    public void run(bool withLeak)
    {
        //set up script environment
        Dictionary<String, Object> options = new Dictionary<string, object>();
        options["LightweightScopes"] = true;
        ScriptEngine engine = Python.CreateEngine(options);
        PythonCompilerOptions pco = (PythonCompilerOptions) engine.GetCompilerOptions();
        pco.Module &= ~ModuleOptions.Optimized;
        engine.SetSearchPaths(new string[]{
            @"C:\Program Files\IronPython 2.7\Lib"
        });
        ScriptRuntime runtime = engine.Runtime;
        ScriptScope scope = runtime.CreateScope();
        var source = engine.CreateScriptSourceFromString(
            withLeak ? scriptWithLeak : scriptWithoutLeak
        );
        var comped = source.Compile();
        comped.Execute(scope);
        runtime.Shutdown();
    }
}
}
