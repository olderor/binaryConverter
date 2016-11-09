﻿using System;
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

            if (number[0] == '-')
            {
                ++i;
                negative = true;
            }

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

            if (negative)
            {
                Invert();
                Inc();
                removeLeadingZeros(ref digits);
                removeLeadingZeros(ref fractionalDigits);
            }
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
                List<bool> part = convertToBinary(c - '0', system);
                digits.AddRange(part);
            }
            removeLeadingZeros(ref digits);
            if (digits.Count == 0)
                digits.Add(false);
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
            removeLeadingZeros(ref fractionalDigits);
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

        public string ConvertToDecimal()
        {
            double number = 0;
            double pow = 1;
            for (int i = 0; i < digits.Count; ++i)
            {
                number += (digits[i] ? 1 : 0) * pow;
                pow *= 2;
            }
            pow = 1;
            for (int i = 0; i < fractionalDigits.Count; ++i)
            {
                pow /= 2;
                number += (fractionalDigits[i] ? 1 : 0) * pow;
            }
            return number.ToString();
        }

        public string ConvertTo(NumeralSystem system)
        {
            string result = "";
            
            if (system == NumeralSystem.Binary) return ToString();

            int partCount = system == NumeralSystem.Octal ? 3 : 4;

            int digit = 0;
            int pow = 1;
            for (int i = 0; i < digits.Count; )
            {
                digit = 0;
                pow = 1;
                for (int j = 0; j < partCount; ++i, ++j)
                {
                    if (i < digits.Count)
                    {
                        digit += (digits[i] ? 1 : 0) * pow;
                    }
                    pow *= 2;
                }
                result = digitToChar(digit) + result;
            }

            if (fractionalDigits.Count == 0) return result;

            result = result + ".";
            digit = 0;
            int maxPow = 1;
            for (int i = 1; i < partCount; ++i)
            {
                maxPow *= 2;
            }

            for (int i = 0; i < fractionalDigits.Count;)
            {
                digit = 0;
                pow = maxPow;
                for (int j = 0; j < partCount && i < fractionalDigits.Count; ++i, ++j)
                {
                    digit += (fractionalDigits[i] ? 1 : 0) * pow;
                    pow /= 2;
                }
                result = result + digitToChar(digit);
            }
            return result;
        }

        private char digitToChar(int digit)
        {
            char c;
            if (digit < 10)
            {
                c = (char)digit;
                c += '0';
                return c;
            }
            digit -= 10;
            c = (char)digit;
            c += 'A';
            return c;
        }

        private List<bool> convertToBinary(int digit, NumeralSystem system)
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

        public void Invert()
        {
            for (int i = 0; i < digits.Count; ++i)
            {
                digits[i] = !digits[i];
            }
            for (int i = 0; i < fractionalDigits.Count; ++i)
            {
                fractionalDigits[i] = !fractionalDigits[i];
            }
        }

        public void Inc()
        {
            
            string number = "";
            if (fractionalDigits.Count == 0)
                number = "1";
            else
            {
                number = "0.";
                for (int i = 0; i < fractionalDigits.Count - 1; ++i)
                    number += "0";
                number += "1";
            }
            Add(new Number(number, NumeralSystem.Binary, number.Length - 2));
        }

        public void Add(Number n)
        {
            int length1 = fractionalDigits.Count;
            int length2 = n.fractionalDigits.Count;
            int minLength = Math.Min(length1, length2);
            int maxLength = length1 + length2 - minLength;

            int i = 0;
            for (i = length1; i <= maxLength + 1; ++i)
                fractionalDigits.Add(false);

            i = maxLength;
            int rest = 0;
            for (; i >= 0; --i)
            {
                rest += (fractionalDigits[i] ? 1 : 0);
                if (i < length2) rest += (n.fractionalDigits[i] ? 1 : 0);

                fractionalDigits[i] = rest % 2 == 1;
                rest /= 2;
            }
            removeLeadingZeros(ref fractionalDigits);
            addIntegerPart(n, rest);
        }

        private void addIntegerPart(Number n, int rest = 0)
        {
            int length1 = digits.Count;
            int length2 = n.digits.Count;
            int minLength = Math.Min(length1, length2);
            int maxLength = length1 + length2 - minLength;

            int i = 0;
            for (i = length1; i <= maxLength + 1; ++i)
                digits.Add(false);

            i = 0;
            for (; i < length2; ++i)
            {
                rest += (digits[i] ? 1 : 0) + (n.digits[i] ? 1 : 0);
                digits[i] = rest % 2 == 1;
                rest /= 2;
            }
            if (rest == 1 && negative)
            {
                negative = false;
            }
            else
            {
                while (rest != 0)
                {
                    rest += (digits[i] ? 1 : 0);
                    digits[i] = rest % 2 == 1;
                    rest /= 2;
                    ++i;
                }
            }
            removeLeadingZeros(ref digits);
            if (digits.Count == 0)
                digits.Add(false);
        }

        private void removeLeadingZeros(ref List<bool> list)
        {
            while (list.Count != 0 && !list[list.Count - 1])
            {
                list.RemoveAt(list.Count - 1);
            }
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
            string result = negative ? "-" : "";
            for (int i = digits.Count - 1; i >= 0; --i)
            {
                result += digits[i] ? '1' : '0';
            }

            if (fractionalDigits.Count == 0) return result;

            result += ".";
            for (int i = 0; i < fractionalDigits.Count; ++i)
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
