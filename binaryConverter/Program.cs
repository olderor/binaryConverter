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

            Number n = new Number("-3.123", NumeralSystem.Decimal, 100);
            Number n2 = new Number("-2.321", NumeralSystem.Decimal, 100);
            Console.Write(n);
            Console.Write(" (" + n.ConvertToDecimal() + ")");
            Console.Write(" - ");
            Console.Write(n2);
            Console.Write(" (" + n2.ConvertToDecimal() + ")");
            Console.Write(" = ");
            n += n2;
            Console.Write(n);
            Console.WriteLine(" (" + n.ConvertToDecimal() + ")");
            n.Floor(7);
            Console.Write(n);
            Console.WriteLine(" (" + n.ConvertToDecimal() + ")");
            n.Floor();
            Console.Write(n);
            Console.WriteLine(" (" + n.ConvertToDecimal() + ")");
            Console.Read();
        }
    }
}
