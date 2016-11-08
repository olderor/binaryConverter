using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace binaryConverter
{

    enum NumeralSystem
    {
        Binary = 2,
        Octal = 8,
        Decimal = 10,
        Hexadecimal = 16
    };

    class Number
    {
        public Number(string number, NumeralSystem system, int precision = 0)
        {
            string integerPart = "";
            double fractionalPart = 0;
            int i = 0;
            for (; i < number.Length; ++i)
            {
                if (number[i] == '.') break;
                integerPart += number[i];
            }
            ++i;

            double pow = 1.0 / (int)system;
            for (; i < number.Length; ++i)
            {
                int c = number[i];
                if (c >= 'A')
                {
                    c = c - 'A' + 10;
                }
                else
                {
                    c = c - '0';
                }
                fractionalPart += c * pow;
                pow /= (int)system;
            }

            setInteger(integerPart, system);
            setFractional(fractionalPart, precision);
        }

        private void setInteger(string number, NumeralSystem system)
        {
            if (system == NumeralSystem.Decimal)
            {
                digits = convertDecimalToBinary(Convert.ToInt32(number));
            }
            for (int i = number.Length - 1; i >= 0; --i)
            {
                char c = number[i];
                List<bool> part = convert(c - '0', system);
                digits.AddRange(part);
            }
        }

        private void setFractional(double number, int precision)
        {
            for (int i = 0; i < precision; ++i)
            {
                number *= 2;
                if (number >= 1)
                {
                    fractionalDigits.Add(true);
                    number -= 1;
                }
                else
                {
                    fractionalDigits.Add(false);
                }
            }
            fractionalDigits.Reverse();
            checkZero();
        }

        private List<bool> convertDecimalToBinary(int number)
        {
            List<bool> result = new List<bool>();
            while (number > 0)
            {
                result.Add(number % 2 == 1);
                number /= 2;
            }
            return result;
        }

        private List<bool> convert(int digit, NumeralSystem system)
        {
            List<bool> result = new List<bool>();
            switch (system)
            {
                case NumeralSystem.Binary:
                    result.Add(digit == 1);
                    return result;
                case NumeralSystem.Octal:
                    result = convertDecimalToBinary(digit);
                    while (result.Count < 3)
                    {
                        result.Add(false);
                    }
                    return result;
                case NumeralSystem.Hexadecimal:
                    int dig = digit;
                    if (dig > 9) dig = dig + '0' - 'A' + 10;
                    result = convertDecimalToBinary(dig);
                    while (result.Count < 4)
                    {
                        result.Add(false);
                    }
                    return result;
            }
            return result;
        }

        public bool isPositive()
        {
            return !negative && !zero;
        }

        public bool isZero()
        {
            return zero;
        }

        public bool isNegative()
        {
            return negative;
        }


        public static string digitToString(int digit, int numeralSystem)
        {
            if (digit < 10)
            {
                return digit.ToString();
            }
            digit -= 10;
            char c = (char)digit;
            c += 'A';
            return "" + c;
        }

        public override string ToString()
        {
            string result = "";
            for (int i = digits.Count - 1; i >= 0; --i)
            {
                result += digits[i] ? '1' : '0';
            }

            if (fractionalDigits.Count == 0) return result;

            result += ".";
            for (int i = fractionalDigits.Count - 1; i >= 0; --i)
            {
                result += fractionalDigits[i] ? '1' : '0';
            }
            return result;
        }

        private bool negative = false;
        private bool zero
        {
            get
            {
                return checkZero();
            }
        }
        private int precision
        {
            get
            {
                return fractionalDigits.Count;
            }
        }

        private List<bool> digits = new List<bool>();
        private List<bool> fractionalDigits = new List<bool>();

        private bool checkZero()
        {
            for (int i = 0; i < digits.Count; ++i)
            {
                if (digits[i])
                {
                    return false;
                }
            }
            for (int i = 0; i < fractionalDigits.Count; ++i)
            {
                if (fractionalDigits[i])
                {
                    return false;
                }
            }
            return true;
        }
    }

    /*
    class Number2
    {

        public Number(int number, byte numeralSystem)
        {
            precision = 0;
            setComponents(number, 0, numeralSystem, 0);
        }

        public Number(double number, byte numeralSystem, int precision)
        {
            this.precision = precision;
            setDouble(number, numeralSystem, precision);
        }

        private void setComponents(int number, int fractionalPart, byte numeralSystem, int precision)
        {
            digits = new List<byte>();
            fractionalDigits = new List<byte>();
            this.precision = precision;
            setIntegerPart(number, numeralSystem);
            setFractionalPart(fractionalPart, numeralSystem, precision);
        }

        private void setDouble(double number, byte numeralSystem, int precision)
        {
            // Dividing number into two parts: integer and francional.
            int integerPart = (int)number;
            number -= integerPart;
            int fractionalPart = 0;
            for (int i = 0; i < precision; ++i)
            {
                fractionalPart *= numeralSystem;
                number *= numeralSystem;
                fractionalPart += (int)number % numeralSystem;
                number -= (int)number;
            }

            setComponents(integerPart, fractionalPart, numeralSystem, precision);
        }

        private void setIntegerPart(int number, byte numeralSystem)
        {
            this.NumeralSystem = numeralSystem;
            if (number < 0)
            {
                negative = true;
                number = -number;
            }

            while (number > 0)
            {
                digits.Add((byte)(number % numeralSystem));
                number /= numeralSystem;
            }

            checkZero();
        }

        private void setFractionalPart(int fractionalPart, byte numeralSystem, int precision)
        {
            for (int i = 0; i < precision; ++i)
            {
                fractionalDigits.Add((byte)(fractionalPart % numeralSystem));
                fractionalPart /= numeralSystem;
            }

            checkZero();
        }


        public bool isPositive()
        {
            return !negative && !zero;
        }

        public bool isZero()
        {
            return zero;
        }

        public bool isNegative()
        {
            return !negative;
        }


        public static string digitToString(int digit, int numeralSystem)
        {
            if (digit < 10)
            {
                return digit.ToString();
            }
            digit -= 10;
            char c = (char)digit;
            c += 'A';
            return "" + c;
        }

        public override string ToString()
        {
            string result = "";
            for (int i = digits.Count - 1; i >= 0; --i)
            {
                result += digitToString(digits[i], NumeralSystem);
            }

            if (fractionalDigits.Count == 0) return result;

            result += ".";
            for (int i = fractionalDigits.Count - 1; i >= 0; --i)
            {
                result += digitToString(fractionalDigits[i], NumeralSystem);
            }
            return result;
        }

        public void ConvertTo(byte numeralSystem)
        {
            int part = 0;
            List<byte> integers = new List<byte>();
            List<byte> fractionals = new List<byte>();

            for (int i = digits.Count - 1; i >= 0; --i)
            {
                part = part * NumeralSystem + digits[i];
                int rest = part % numeralSystem;
                part = part / numeralSystem;
                if (!(rest == 0 && digits.Count == 0))
                {
                    integers.Add((byte)rest);
                }
            }
            digits = integers;
            NumeralSystem = numeralSystem;
        }

        public void ConvertToDecimal()
        {
            //if (NumeralSystem == 10) return;

            double newNumber = 0;
            double numPower = 1;

            for (int i = 0; i < digits.Count; ++i)
            {
                newNumber += numPower * digits[i];
                numPower *= NumeralSystem;
            }
            numPower = 1;
            for (int i = fractionalDigits.Count - 1; i >= 0; --i)
            {
                numPower /= NumeralSystem;
                newNumber += numPower * fractionalDigits[i];
            }

            setDouble(newNumber, 10, precision);
        }


        public byte NumeralSystem;

        private bool negative = false;
        private bool zero = true;
        private int precision;

        private List<byte> digits = new List<byte>();
        private List<byte> fractionalDigits = new List<byte>();

        private void checkZero()
        {
            zero = true;
            for (int i = 0; i < digits.Count; ++i)
            {
                if (digits[i] != 0)
                {
                    zero = false;
                    return;
                }
            }
            for (int i = 0; i < fractionalDigits.Count; ++i)
            {
                if (digits[i] != 0)
                {
                    zero = false;
                    return;
                }
            }
        }

    }*/
}
