using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace binaryConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("integers:");

            Number n = new Number("123", NumeralSystem.Decimal);
            Console.WriteLine(n);
            n = new Number("0.12", NumeralSystem.Octal, 10);
            Console.WriteLine(n);
            n = new Number("123.125", NumeralSystem.Hexadecimal, 10);
            Console.WriteLine(n);
            n = new Number("1010", NumeralSystem.Binary);
            Console.WriteLine(n);

            Console.Read();
        }
    }
}
