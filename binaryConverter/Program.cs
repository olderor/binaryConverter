using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace binaryConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");

            Console.WriteLine("integers:");

            Number n = new Number("245", NumeralSystem.Decimal);
            Number n2 = new Number("-245", NumeralSystem.Decimal);
            Console.Write(n);
            Console.Write(" + ");
            Console.Write(n2);
            Console.Write(" = ");
            n.Add(n2);
            Console.WriteLine(n);
            Console.Read();
        }
    }
}
