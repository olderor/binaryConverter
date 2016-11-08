using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace binaryConverter
{
    class Number
    {

        public Number(int number, byte numeralSystem)
        {
            setIntegerPart(number, numeralSystem);
        }

        public Number(double number, byte numeralSystem, int precision)
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

        private void setComponents(int number, int fractionalPart, byte numeralSystem, int precision)
        {
            setIntegerPart(number, numeralSystem);
            setFractionalPart(fractionalPart, numeralSystem, precision);
        }

        private void setIntegerPart(int number, byte numeralSystem)
        {
            this.numeralSystem = numeralSystem;
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
                result += digitToString(digits[i], numeralSystem);
            }

            if (fractionalDigits.Count == 0) return result;

            result += ".";
            for (int i = fractionalDigits.Count - 1; i >= 0; --i)
            {
                result += digitToString(fractionalDigits[i], numeralSystem);
            }
            return result;
        }

        private byte numeralSystem;

        private bool negative = false;
        private bool zero = true;

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

    }
}
