using System.Dynamic;
using System.Linq.Expressions;
using System;
using IronPython.Hosting;

public class Dyn : DynamicObject
{
    private readonly string text;

    public Dyn(string text)
    {
        this.text = text;
    }

    public override string ToString()
    {
        return this.text;
    }

    public override bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object result)
    {
        result = new Dyn(this + " " + binder.Operation + " " + arg);
        return true;
    }

    public override bool TryUnaryOperation(UnaryOperationBinder binder, out object result)
    {
        switch (binder.Operation)
        {
            case ExpressionType.IsFalse:
            case ExpressionType.IsTrue:
                result = false;
                return true;
        }

        return base.TryUnaryOperation(binder, out result);
    }
}

class Program
{
static void Main(string[] args)
{
dynamic a = new Dyn("a");
dynamic b = new Dyn("b");
dynamic c = new Dyn("c");

var correct = a && b || c;

var engine = Python.CreateEngine();
var scope = engine.CreateScope();
scope.SetVariable("a", a);
scope.SetVariable("b", b);
scope.SetVariable("c", c);
var incorrect = engine.Execute("a and b or c", scope);

Console.WriteLine("Correct: " + correct);
Console.WriteLine("Incorrect: " + incorrect);

}
}