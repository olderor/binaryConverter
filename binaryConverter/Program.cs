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

        static private Number ReadNumber(string index)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Enter information about the " + index + " number.");

                    Console.Write("Enter value: ");
                    string value1 = Console.ReadLine();

                    Console.Write("Enter numeral system (2, 8, 10, 16): ");
                    string system1Str = Console.ReadLine();
                    int system1 = Convert.ToInt32(system1Str);

                    Console.Write("Enter precision (in binary number): ");
                    string precisionStr = Console.ReadLine();
                    int precision = Convert.ToInt32(precisionStr);

                    return new Number(value1, (NumeralSystem)system1, precision);
                }
                catch
                {
                    Console.WriteLine("Failed to create such number. Please, check that your input is correct and try again.");
                }
            }
        }

        static void RunCalculator()
        {
            Console.WriteLine("Welcome.");

            while (true)
            {
                Number n1 = ReadNumber("first");
                if (n1 == null)
                    continue;
                Number n2 = ReadNumber("second");
                if (n2 == null)
                    continue;


                Number result = null;

                while (true)
                {
                    Console.Write("Enter operation between those numbers (+, -, *, /): ");
                    string operation = Console.ReadLine();
                    switch (operation)
                    {
                        case "+":
                            result = n1 + n2;
                            break;
                        case "-":
                            result = n1 - n2;
                            break;
                        case "*":
                            result = n1 * n2;
                            break;
                        case "/":
                            result = n1 / n2;
                            break;
                        default:
                            Console.WriteLine("Wrong operation, try again.");
                            break;
                    }
                    if (result == null)
                        continue;
                    break;
                }

                try
                {
                    Console.Write("Enter numeral system (2, 8, 10, 16): ");
                    string system1Str = Console.ReadLine();
                    int system1 = Convert.ToInt32(system1Str);
                    Console.WriteLine();
                    Console.WriteLine("Result is: " + result.ConvertTo((NumeralSystem)system1));
                }
                catch
                {
                    Console.WriteLine("Wrong numeral system. Please, try again.");
                }
            }
        }

        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            
            RunCalculator();
        }
    }
}
