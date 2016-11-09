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



            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();


            Console.Write(n);
            Console.WriteLine(" (" + n.ConvertToDecimal() + ")");
            Number n3 = new Number(n);
            n2 = n - n3 + n3 + n - n - n;
            Console.Write(n2);
            Console.WriteLine(" (" + n2.ConvertToDecimal() + ")");


            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();



            n = new Number("127.74", NumeralSystem.Decimal, 2);
            n2 = new Number("255.123412412", NumeralSystem.Decimal, 7);
            Console.Write(n);
            Console.Write(" (" + n.ConvertToDecimal() + ")");
            Console.Write(" / ");
            Console.Write(n2);
            Console.Write(" (" + n2.ConvertToDecimal() + ")");
            Console.Write(" = ");
            n3 = n / n2;
            Console.Write(n3);
            Console.Write(" (" + n3.ConvertToDecimal() + ")");
            Console.Read();

        }
    }
}
