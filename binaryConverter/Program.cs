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
            Number n = new Number(12, 10);
            Console.WriteLine(n);

            n = new Number(12, 2);
            Console.WriteLine(n);

            n = new Number(15, 16);
            Console.WriteLine(n);

            n = new Number(10, 16);
            Console.WriteLine(n);

            n = new Number(17, 16);
            Console.WriteLine(n);


            Console.WriteLine("doubles:");
            n = new Number(12.23, 10, 2);
            Console.WriteLine(n);

            n = new Number(12.23, 10, 1);
            Console.WriteLine(n);

            n = new Number(12.23, 10, 0);
            Console.WriteLine(n);

            n = new Number(12.23, 10, 4);
            Console.WriteLine(n);

            n = new Number(12.23, 2, 15);
            Console.WriteLine(n);

            n = new Number(15.23, 16, 4);
            Console.WriteLine(n);

            n = new Number(10.23, 16, 4);
            Console.WriteLine(n);

            n = new Number(17.23, 16, 4);
            Console.WriteLine(n);

            Console.Read();
        }
    }
}
