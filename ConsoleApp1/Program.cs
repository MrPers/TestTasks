using System;
using System.Collections;
using System.Collections.Generic;

namespace Calculator
{
    class Program
    {
        static void Main()
        {
            Context context = new Context();
            while (true)
            {
                Console.Write(">>>");
                string key = Console.ReadLine();
                switch (key)
                {
                    case "0":
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9": context.digit(key); break;
                    case "c":
                    case "C": context.clear(); break;
                    case "+":
                    case "-":
                    case "*":
                    case "/": context.opers(key); break;
                    case "=": context.equal(); break;
                    case "q": return;
                }
                Console.WriteLine(context.scren());
            }
        }
    }
}

