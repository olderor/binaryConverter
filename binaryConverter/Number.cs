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
        public static Number Zero()
        {
            return new Number("0", NumeralSystem.Binary);
        }

        public static Number One()
        {
            return new Number("1", NumeralSystem.Binary);
        }

        public Number(Number n)
        {
            Copy(n);
        }

        public void Copy(Number n)
        {
            digits = new List<bool>();
            fractionalDigits = new List<bool>();
            negative = n.negative;
            for (int i = 0; i < n.digits.Count; ++i)
            {
                digits.Add(n.digits[i]);
            }
            for (int i = 0; i < n.fractionalDigits.Count; ++i)
            {
                fractionalDigits.Add(n.fractionalDigits[i]);
            }
        }

        public Number(string number, NumeralSystem system, int precision = 0)
        {
            string integerPart = "";
            double fractionalPart = 0;
            int i = 0;
            bool neg = false;
            if (number[0] == '-')
            {
                ++i;
                neg = true;
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

            if (neg)
            {
                Number n2 = Number.Zero();
                n2.Sub(this);
                Copy(n2);
            }
            negative = neg;
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
            if (negative)
                digits.Add(true);
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
            if (negative)
                fractionalDigits.Add(true);
            removeLeadingZeros(ref fractionalDigits);
            checkZero();
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
            string result = negative ? "-" : "";
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
            return result + number.ToString();
        }

        public string ConvertTo(NumeralSystem system)
        {
            string result = "";

            if (system == NumeralSystem.Binary) return ToString();

            int partCount = system == NumeralSystem.Octal ? 3 : 4;

            int digit = 0;
            int pow = 1;
            for (int i = 0; i < digits.Count;)
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
            result = (negative ? "-" : "") + result;
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

        private static bool isSmallerAbs(Number n1, Number n2)
        {
            if (n1.digits.Count != n2.digits.Count)
            {
                return n1.digits.Count < n2.digits.Count;
            }
            for (int i = n1.digits.Count - 1; i >= 0; --i)
            {
                if (!n1.digits[i] && n2.digits[i])
                    return true;
                if (n1.digits[i] && !n2.digits[i])
                    return false;
            }

            for (int i = 0; ; ++i)
            {
                if (i == n2.fractionalDigits.Count)
                {
                    return false;
                }
                if (i == n1.fractionalDigits.Count)
                {
                    return true;
                }

                if (!n1.fractionalDigits[i] && n2.fractionalDigits[i])
                    return true;
                if (n1.fractionalDigits[i] && !n2.fractionalDigits[i])
                    return false;
            }
        }

        private static bool isBiggerAbs(Number n1, Number n2)
        {
            if (n1.digits.Count != n2.digits.Count)
            {
                return n1.digits.Count > n2.digits.Count;
            }
            for (int i = n1.digits.Count - 1; i >= 0; --i)
            {
                if (!n1.digits[i] && n2.digits[i])
                    return false;
                if (n1.digits[i] && !n2.digits[i])
                    return true;
            }

            for (int i = 0; ; ++i)
            {
                if (i == n1.fractionalDigits.Count)
                {
                    return false;
                }
                if (i == n2.fractionalDigits.Count)
                {
                    return true;
                }

                if (!n1.fractionalDigits[i] && n2.fractionalDigits[i])
                    return false;
                if (n1.fractionalDigits[i] && !n2.fractionalDigits[i])
                    return true;
            }
        }

        public static bool operator <(Number n1, Number n2)
        {
            if (n1.negative && !n2.negative) return true;
            if (!n1.negative && n2.negative) return false;

            bool result = isSmallerAbs(n1, n2);
            if (n1.negative)
            {
                result = !result;
            }
            return result;
        }

        public static bool operator >(Number n1, Number n2)
        {
            if (n1.negative && !n2.negative) return false;
            if (!n1.negative && n2.negative) return true;

            bool result = isBiggerAbs(n1, n2);
            if (n1.negative)
            {
                result = !result;
            }
            return result;
        }






        public static Number operator +(Number n1, Number n2)
        {
            Number n = new Number(n1);
            n.Add(n2);
            return n;
        }

        public static Number operator -(Number n1, Number n2)
        {
            Number n = new Number(n1);
            n.Sub(n2);
            return n;
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

        public void Increase()
        {
            Add(Number.One());
        }
        public void IncreaseLastBit()
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

        public void Decrease()
        {
            Sub(Number.One());
        }
        public void DecreaseLastBit()
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
            Sub(new Number(number, NumeralSystem.Binary, number.Length - 2));
        }

        public void Add(Number n)
        {
            if (negative == n.negative)
            {
                add(n);
                return;
            }
            Number n2 = new Number(n);
            if (n.negative)
            {
                n2.negative = false;
                sub(n2);
                return;
            }
            negative = false;
            n2.sub(this);
            Copy(n2);
        }

        public void Sub(Number n)
        {
            if (negative == !n.negative)
            {
                add(n);
                return;
            }
            Number n2 = new Number(n);
            if (n.negative)
            {
                n2.negative = false;
                this.negative = false;
                sub(this);
                Copy(n2);
                return;
            }
            sub(n);
        }


        public void Floor(int digits = 0)
        {
            bool needDecrease = false;
            if (negative && digits < fractionalDigits.Count)
            {
                needDecrease = true;
            }
            List<bool> newFractionalDigits = new List<bool>();
            for (int i = 0; i < digits && i < fractionalDigits.Count; ++i)
            {
                newFractionalDigits.Add(fractionalDigits[i]);
            }
            fractionalDigits = newFractionalDigits;
            removeLeadingZeros(ref fractionalDigits);
            if (needDecrease)
            {
                DecreaseLastBit();
            }
        }

        public void Mul(Number n)
        {
            Number result = Number.Zero();
        }

        private void add(Number n)
        {
            int length1 = fractionalDigits.Count;
            int length2 = n.fractionalDigits.Count;
            int minLength = Math.Min(length1, length2);
            int maxLength = length1 + length2 - minLength;

            int i = 0;
            for (i = length1; i < maxLength; ++i)
                fractionalDigits.Add(false);

            i = maxLength - 1;
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
        private void sub(Number n)
        {
            if (n > this)
            {
                Number n2 = new Number(n);
                n2.Sub(this);
                Copy(n2);
                negative = true;
                return;
            }
            int length1 = fractionalDigits.Count;
            int length2 = n.fractionalDigits.Count;
            int minLength = Math.Min(length1, length2);
            int maxLength = length1 + length2 - minLength;

            int i = 0;
            for (i = length1; i < maxLength; ++i)
                fractionalDigits.Add(false);

            i = length2 - 1;
            bool flag = false;
            for (; i >= 0; --i)
            {
                if (fractionalDigits[i])
                {
                    if (n.fractionalDigits[i])
                    {
                        fractionalDigits[i] = false;
                    }
                }
                else
                {
                    if (n.fractionalDigits[i])
                    {
                        int index = findFree(ref fractionalDigits, i - 1, false);
                        if (index < 0)
                        {
                            flag = true;
                        }
                        else
                        {
                            fractionalDigits[index] = false;
                        }
                        fractionalDigits[i] = true;
                    }
                }
            }

            removeLeadingZeros(ref fractionalDigits);
            subIntegerPart(n);
            if (flag)
            {
                Sub(Number.One());
            }
        }

        private int findFree(ref List<bool> list, int index, bool increase)
        {
            if (increase && index == list.Count || !increase && index == -1)
                return index;
            if (list[index])
            {
                list[index] = false;
                return index;
            }
            list[index] = true;
            if (increase)
            {
                return findFree(ref list, index + 1, increase);
            }
            return findFree(ref list, index - 1, increase);
        }

        private void subIntegerPart(Number n)
        {
            int length1 = digits.Count;
            int length2 = n.digits.Count;
            int minLength = Math.Min(length1, length2);
            int maxLength = length1 + length2 - minLength;

            int i = 0;
            for (i = length1; i < maxLength; ++i)
                digits.Add(false);

            i = 0;
            bool flag = false;
            for (; i < length2; ++i)
            {
                if (flag)
                {
                    digits[i] = n.digits[i];
                    continue;
                }
                if (digits[i])
                {
                    if (n.digits[i])
                    {
                        digits[i] = false;
                    }
                }
                else
                {
                    if (n.digits[i])
                    {
                        int index = findFree(ref digits, i + 1, true);
                        if (index >= maxLength)
                        {
                            flag = true;
                        }
                        else
                        {
                            digits[index] = false;
                        }
                        digits[i] = true;
                    }
                }
            }

            if (flag)
            {
                negative = true;
            }

            removeLeadingZeros(ref digits);
            if (digits.Count == 0)
                digits.Add(false);
        }

        private void addIntegerPart(Number n, int rest = 0)
        {
            int length1 = digits.Count;
            int length2 = n.digits.Count;
            int minLength = Math.Min(length1, length2);
            int maxLength = length1 + length2 - minLength;

            int i = 0;
            for (i = length1; i <= maxLength; ++i)
                digits.Add(false);

            i = 0;
            for (; i < length2; ++i)
            {
                rest += (digits[i] ? 1 : 0) + (n.digits[i] ? 1 : 0);
                digits[i] = rest % 2 == 1;
                rest /= 2;
            }

            while (rest != 0)
            {
                rest += (digits[i] ? 1 : 0);
                digits[i] = rest % 2 == 1;
                rest /= 2;
                ++i;
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
}