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

            Number n = new Number("0.7245", NumeralSystem.Decimal, 110);
            Console.WriteLine(n);
            Console.WriteLine(n.ConvertToDecimal());
            Console.WriteLine(n.ConvertTo(NumeralSystem.Binary));
            Console.WriteLine(n.ConvertTo(NumeralSystem.Octal));
            Console.WriteLine(n.ConvertTo(NumeralSystem.Hexadecimal));

            Console.Read();
        }
    }
}
