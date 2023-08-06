using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

public class Program
{
    public static async Task Main()
    {

        Console.ReadLine();
    }
}

//var ar = new string[] { "dfdf", "123", "456" };

//var a = ar[.. 0];
//var b = ar[2..^0];
//var c = ar[2^0];
//var g = ar[^0];

//___________________________

//MyClassI a = new A();
//MyClassI b = new B();
//List<Task> list = new List<Task>()
//{
//    a.Execute(),
//    b.Execute()
//};

////Task.WaitAll(list.ToArray());

//foreach (Task x in list)
//{
//    await x;
//}

interface MyClassI
{
    public async Task Execute() { }
}
class A : MyClassI
{
    public async Task Execute()
    {
        Console.WriteLine("Class A start executing");
        await Task.Delay(2000);
        Console.WriteLine("Class A finishedex ecuting");
    }
}
class B : MyClassI
{
    public async Task Execute()
    {
        Console.WriteLine("Class B start executing");
        await Task.Delay(3000);
        Console.WriteLine("Class B finishedex ecuting");
    }

}
