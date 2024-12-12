using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Mathix.Types
{
    /// <summary>
    /// Represents an Big Integer in a memory.
    /// The class is represented in RAM as byte-by-byte. Integer operations are also performed.
    /// </summary>
    public class BigInt
    {
        #region Fields

        /// <summary>
        /// Protected variable: The sign of Integer. Asked on the question: Is an Integer Negative?
        /// </summary>
        protected bool _isNegative;

        /// <summary>
        /// The sign of Integer. Asked on the question: Is an Integer Negative?
        /// </summary>
        public bool IsNegative
        {
            get
            {
                return _isNegative;
            }
            set
            {
                _isNegative = value;
            }
        }

        /// <summary>
        /// The Byte Representation of Integer is from the Low (the start index is 0) to the High (the end index is Lenght - 1).
        /// </summary>
        protected List<byte> _bytes;

        /// <summary>
        /// The Bytes of Integer part without sign.
        /// </summary>
        public byte[] Bytes
        {
            get
            {
                byte[] result = new byte[Length];
                if (Length > 0)
                    _bytes.CopyTo(result);
                return result;
            }
        }

        /// <summary>
        /// The Length of Integer is in bytes (byte count) without sign.
        /// </summary>
        public int Length
        {
            get
            {
                return _bytes.Count;
            }
        }

        #endregion

        #region Constructors

        public BigInt(byte[] bytes, bool isNegative)
        {
            _bytes = new List<byte>(bytes);
            this.DeleteHighZeroBytes();
            _isNegative = isNegative;
        }

        public BigInt(string bigIntStringHex)
        {
            if (bigIntStringHex != null && bigIntStringHex.Length > 0)
            {
                int i = 0;
                bool isNegative = false;

                if (bigIntStringHex[0] == '-')
                {
                    isNegative = true;
                    i++;
                }

                // Tests sting have all digits
                for (; i < bigIntStringHex.Length; i++)
                {
                    switch (bigIntStringHex[i])
                    {
                        case '0': break;
                        case '1': break;
                        case '2': break;
                        case '3': break;
                        case '4': break;
                        case '5': break;
                        case '6': break;
                        case '7': break;
                        case '8': break;
                        case '9': break;
                        case 'A': break;
                        case 'B': break;
                        case 'C': break;
                        case 'D': break;
                        case 'E': break;
                        case 'F': break;
                        default: throw new ArgumentException("The bigIntString must consist of hex-integers and have no more one negative char!");
                    }

                    // Convert hex-string in byte array
                }

                if (isNegative)
                {
                    i = 1;
                }
                else { i = 0; }

                _bytes = Enumerable.Range(i, bigIntStringHex.Length)
                                 .Where(x => x % 2 == 0)
                                 .Select(x => Convert.ToByte(bigIntStringHex.Substring(x, 2), 16))
                                 .ToList();
            }
            else { throw new ArgumentException("The bigIntStringHex is null or empty."); }
        }
        #endregion

        #region Object methods

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (GetType() != obj.GetType())
                return false;

            BigInt co = (BigInt)obj;

            return this == co;
        }

        public override int GetHashCode()
        {
            byte additionalElements = 0;
            int sizeOfInt = sizeof(int);
            int additionalBytes = _bytes.Count % sizeOfInt;
            if (additionalBytes > 0)
            {
                additionalElements = 1;
            }

            int[] result = new int[_bytes.Count / sizeOfInt + additionalElements];
            int index = 0;

            while (index < result.Length)
            {
                if (index < (result.Length - 1))
                {
                    for (int i = 0; i < sizeOfInt; i++)
                    {
                        result[index] |= _bytes[index + i];

                        if (i < (sizeOfInt - 1))
                        {
                            result[index] <<= 8;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < additionalBytes; i++)
                    {
                        result[index] |= _bytes[index + i];

                        if (i < (sizeOfInt - 1))
                        {
                            result[index] <<= 8;
                        }
                    }
                }
                ++index;
            }

            int hashCode = 0;

            for (int i = 0; i < result.Length; i++)
                hashCode ^= result[i];

            return hashCode;
        }

        #endregion

        #region Comparison Operators (x == y, x != y, x < y, x > y, x <= y, x >= y)

        public static bool operator ==(BigInt a, BigInt b)
        {

            if (a.Length != b.Length)
                return false;

            if (a.IsNegative != b.IsNegative)
                return false;

            bool result = true;

            int currentByte = a.Length - 1;
            while (result && currentByte >= 0)
            {
                result = a._bytes[currentByte] == b._bytes[currentByte];
                --currentByte;
            }

            return result;
        }

        public static bool operator !=(BigInt a, BigInt b)
        {
            return !(a == b);
        }

        public static bool operator <(BigInt a, BigInt b)
        {
            if (a.Length < b.Length)
                return true;
            if (a.Length > b.Length)
                return false;

            int firstDifferentByteIndexFromHigh = a.Length - 1;
            while (firstDifferentByteIndexFromHigh >= 0 && a._bytes[firstDifferentByteIndexFromHigh] == b._bytes[firstDifferentByteIndexFromHigh]) { --firstDifferentByteIndexFromHigh; }

            if (firstDifferentByteIndexFromHigh == -1)
                return false;
            else
                return a._bytes[firstDifferentByteIndexFromHigh] < b._bytes[firstDifferentByteIndexFromHigh];
        }

        public static bool operator >(BigInt a, BigInt b)
        {
            if (a.Length > b.Length)
                return true;
            if (a.Length < b.Length)
                return false;

            int firstDifferentByteIndexFromHigh = a.Length - 1;
            while (firstDifferentByteIndexFromHigh >= 0 && a._bytes[firstDifferentByteIndexFromHigh] == b._bytes[firstDifferentByteIndexFromHigh]) { --firstDifferentByteIndexFromHigh; }

            if (firstDifferentByteIndexFromHigh == -1)
                return false;
            else
                return a._bytes[firstDifferentByteIndexFromHigh] > b._bytes[firstDifferentByteIndexFromHigh];
        }

        public static bool operator <=(BigInt a, BigInt b)
        {
            if (a.Length < b.Length)
                return true;
            if (a.Length > b.Length)
                return false;

            int firstDifferentByteIndexFromHigh = a.Length - 1;
            while (firstDifferentByteIndexFromHigh >= 0 && a._bytes[firstDifferentByteIndexFromHigh] == b._bytes[firstDifferentByteIndexFromHigh] && firstDifferentByteIndexFromHigh >= 0) { --firstDifferentByteIndexFromHigh; }

            if (firstDifferentByteIndexFromHigh == -1)
                return true;
            else
                return a._bytes[firstDifferentByteIndexFromHigh] < b._bytes[firstDifferentByteIndexFromHigh];
        }

        public static bool operator >=(BigInt a, BigInt b)
        {
            if (a.Length > b.Length)
                return true;
            if (a.Length < b.Length)
                return false;

            int firstDifferentByteIndexFromHigh = a.Length - 1;
            while (firstDifferentByteIndexFromHigh >= 0 && a._bytes[firstDifferentByteIndexFromHigh] == b._bytes[firstDifferentByteIndexFromHigh]) { --firstDifferentByteIndexFromHigh; }

            if (firstDifferentByteIndexFromHigh == -1)
                return true;
            else
                return a._bytes[firstDifferentByteIndexFromHigh] > b._bytes[firstDifferentByteIndexFromHigh];
        }

        #endregion

        #region Delete High zero bytes

        protected void DeleteHighZeroBytes()
        {
            int highIndex = _bytes.Count - 1;

            while (highIndex > 0 && _bytes[highIndex] == 0)
            {
                --highIndex;
            }

            _bytes.RemoveRange(highIndex + 1, _bytes.Count - highIndex - 1);
        }

        #endregion

        #region Bitwise Operators (x & y, x | y, x ^ y, x << y, x >> y)

        public static BigInt operator &(BigInt a, BigInt b)
        {
            int minLength = a.Length > b.Length ? b.Length : a.Length;

            byte[] result = new byte[minLength];

            for (int i = 0; i < minLength; i++)
            {
                result[i] = (byte)(a._bytes[i] & b._bytes[i]);
            }

            BigInt resultUnlimited = new BigInt(result, a.IsNegative & b.IsNegative);
            resultUnlimited.DeleteHighZeroBytes();

            return resultUnlimited;
        }

        /// <summary>
        /// Getting min and max values from (a,b). Exception: if a==b then min = a, max = b. 
        /// </summary>
        /// <param name="a">First comparable element</param>
        /// <param name="b">Second comparable element</param>
        /// <param name="min">Minimum from elements</param>
        /// <param name="max">Maximum from elements</param>
        private static void GetMinMax(BigInt a, BigInt b, out BigInt min, out BigInt max)
        {
            if (a > b)
            {
                min = b; max = a;
            }
            else
            {
                min = a; max = b;
            }
        }

        public static BigInt operator |(BigInt a, BigInt b)
        {
            BigInt min, max;
            GetMinMax(a, b, out min, out max);

            byte[] result = new byte[max.Length];

            int copiedIndex = 0;
            for (int i = 0; i < min.Length; i++)
            {
                result[i] = (byte)(min._bytes[i] | max._bytes[i]);
                ++copiedIndex;
            }

            for (int i = copiedIndex; i < max.Length; i++)
            {
                result[i] = max._bytes[i];
            }

            BigInt resultUnlimited = new BigInt(result, a.IsNegative | b.IsNegative);
            resultUnlimited.DeleteHighZeroBytes();

            return resultUnlimited;
        }

        public static BigInt operator ^(BigInt a, BigInt b)
        {
            BigInt min, max;
            GetMinMax(a, b, out min, out max);

            byte[] result = new byte[max.Length];

            int copiedIndex = 0;
            for (int i = 0; i < min.Length; i++)
            {
                result[i] = (byte)(min._bytes[i] ^ max._bytes[i]);
                ++copiedIndex;
            }

            for (int i = copiedIndex; i < max.Length; i++)
            {
                result[i] = max._bytes[i];
            }

            BigInt resultUnlimited = new BigInt(result, a.IsNegative ^ b.IsNegative);
            resultUnlimited.DeleteHighZeroBytes();

            return resultUnlimited;
        }


        public static BigInt operator <<(BigInt a, int b)
        {
            if (b == 0)
            {
                return (BigInt)a.Clone();
            }

            if (a == Zero()) return (BigInt)a.Clone();

            if (b < 0)
                return a >> System.Math.Abs(b);

            byte[] result;

            int shiftInBytes = b / 8;
            int shift = b % 8;

            if (shift != 0)
            {
                int size = shift > 0 ? a.Length + shiftInBytes + 1 : a.Length + shiftInBytes;
                result = new byte[size];

                for (int i = 0; i < shiftInBytes; i++)
                {
                    result[i] = 0;
                }

                byte trans = 0;
                for (int i = shiftInBytes; i < size - 1; i++)
                {
                    result[i] = (byte)((a._bytes[i - shiftInBytes] << shift) ^ trans);
                    trans = (byte)(a._bytes[i - shiftInBytes] >> (8 - shift));
                }

                result[size - 1] = trans;
            }
            else
            {
                int size = shift > 0 ? a.Length + shiftInBytes + 1 : a.Length + shiftInBytes;
                result = new byte[size];

                for (int i = 0; i < shiftInBytes; i++)
                {
                    result[i] = 0;
                }

                for (int i = shiftInBytes; i < size; i++)
                {
                    result[i] = (byte) a._bytes[i - shiftInBytes];
                }
            }

            return new BigInt(result, a.IsNegative);
        }
        
        public static BigInt operator >>(BigInt a, int b)
        {
            if (b == 0) return (BigInt)a.Clone();

            if (a == Zero()) return (BigInt)a.Clone();

            if (b < 0) return a << System.Math.Abs(b);

            BigInt result;

            int shiftInBytes = b / 8;
            int shift = b % 8;

            int size = 1;
            if (a.Length > 1)
                size = a.Length - shiftInBytes;

            byte[] newResult = new byte[size];
            for (int i = shiftInBytes, j = 0; i < a.Length; i++, j++)
            {
                newResult[j] = a._bytes[i];
            }

            result = new BigInt(newResult, a.IsNegative);

            if (shift != 0)
            {
                for (int i = 1; i < result.Length; i++)
                {
                    result._bytes[i - 1] = (byte)((result._bytes[i - 1]) >> shift);
                    result._bytes[i - 1] = (byte)((result._bytes[i] << (8 - shift)) | result._bytes[i - 1]);
                }

                result._bytes[result.Length - 1] >>= shift;
            }

            result.DeleteHighZeroBytes();

            return result;
        }

        #endregion

        #region Unary Operators (+x, -x, !x, ~x, ++, --, true, false)

        public static BigInt operator +(BigInt x)
        {
            return (BigInt)x.Clone();
        }

        public static BigInt operator -(BigInt x)
        {
            BigInt result = new BigInt(x.Bytes, !x.IsNegative);
            return result;
        }

        public static bool operator true(BigInt x)
        {
            bool result = true;
            if (x.Length == 1 && x._bytes[0] == 0)
                result = false;
            return result;
        }

        public static bool operator false(BigInt x)
        {
            bool result = false;
            if (x.Length == 1 && x._bytes[0] == 0)
                result = true;
            return result;
        }

        public static bool operator !(BigInt x)
        {
            bool result = true;
            if (x)
                result = false;
            return result;
        }

        public static BigInt operator ~(BigInt x)
        {
            BigInt result = new BigInt(x.Bytes, !x.IsNegative);

            for (int index = 0; index < result.Length; index++)
            {
                result._bytes[index] = (byte)~result._bytes[index];
            }

            result.DeleteHighZeroBytes();

            return result;
        }


        public static BigInt operator ++(BigInt x)
        {
            BigInt result;

            if (x.IsNegative)
            {
                if (!x)
                    result = One();
                else
                {
                    result = BigInt.SubPositive(x, One());
                    result.IsNegative = true;
                }
            }
            else
            {
                if (!x)
                    result = One();
                else
                {
                    result = BigInt.AddPositive(x, One());
                    result.IsNegative = false;
                }
            }

            result.DeleteHighZeroBytes();

            return result;
        }

        public static BigInt operator --(BigInt x)
        {
            BigInt result;

            if (x.IsNegative)
            {
                if (!x)
                    result = MinusOne();
                else
                {
                    result = BigInt.AddPositive(x, One());
                    result.IsNegative = true;
                }
            }
            else
            {
                if (!x)
                    result = MinusOne();
                else
                {
                    result = BigInt.SubPositive(x, One());
                    result.IsNegative = false;
                }
            }

            result.DeleteHighZeroBytes();

            return result;
        }

        #endregion

        #region Maths Operations (x + y, x - y, x * y, x / y, x % y)

        private static BigInt AddPositive(BigInt summand, BigInt added)
        {
            int maskLenght = summand.Length > added.Length ? summand.Length : added.Length;
            maskLenght++;
            byte[] maskBytes = new byte[maskLenght];
            for (int i = 0; i < (maskLenght - 1); i++)
            {
                maskBytes[i] = 0xFF;
            }
            maskBytes[maskLenght - 1] = 0b0000_0001;

            BigInt carry = Zero(),
                myAdded = (BigInt)added.Clone(),
                mySummand = (BigInt)summand.Clone(),
                mask = new BigInt(maskBytes, false);

            while (myAdded.Length > 1 || (myAdded.Length == 1 && myAdded._bytes[0] != 0))
            {
                carry = (mySummand & myAdded);

                mySummand = mySummand ^ myAdded;

                myAdded = (carry << 1) & mask;
            }

            return mySummand;
        }

        private static BigInt SubPositive(BigInt minuend, BigInt subtrahend)
        {
            if (minuend < subtrahend)
                throw new ArgumentOutOfRangeException("Minuend must be higher or equal subtrahend");

            int maskLenght = minuend.Length;

            byte[] maskBytes = new byte[maskLenght];
            for (int i = 0; i < maskLenght; i++)
            {
                maskBytes[i] = 0xFF;
            }


            BigInt borrow = Zero(),
                myMinuend = (BigInt)minuend.Clone(),
                mySubtrahend = (BigInt)subtrahend.Clone(),
                mask = new BigInt(maskBytes, false);


            while (mySubtrahend.Length > 1 || (mySubtrahend.Length == 1 && mySubtrahend._bytes[0] != 0))
            {

                borrow = ((~myMinuend) & mySubtrahend);

                myMinuend = myMinuend ^ mySubtrahend;

                mySubtrahend = (borrow << 1) && mask;
            }

            return myMinuend;
        }

        public static BigInt operator +(BigInt a, BigInt b)
        {
            BigInt result;

            // Если у них разные знаки
            if (a.IsNegative ^ b.IsNegative)
            {
                // ... то делаем вычитание ...
                if (a._isNegative)
                {
                    if (a > b)
                    {
                        result = SubPositive(a, b);
                        result._isNegative = true;
                    }
                    else
                    {
                        result = SubPositive(b, a);
                        result._isNegative = false;
                    }
                }
                else
                {
                    if (b > a)
                    {
                        result = SubPositive(b, a);
                        result._isNegative = true;
                    }
                    else
                    {
                        result = SubPositive(a, b);
                        result._isNegative = false;
                    }
                }
            }
            else
            {
                // ... иначе сложение
                result = AddPositive(a, b);
                result._isNegative = a._isNegative;
            }

            return result;
        }

        public static BigInt operator -(BigInt a, BigInt b)
        {
            BigInt result;

            // Если у них разные знаки
            if (a.IsNegative ^ b.IsNegative)
            {
                // ... иначе сложение
                result = AddPositive(a, b);
                result._isNegative = a._isNegative;

            }
            else
            {
                // ... то делаем вычитание ...
                if (a._isNegative)
                {
                    if (a > b)
                    {
                        result = SubPositive(a, b);
                        result._isNegative = true;
                    }
                    else
                    {
                        result = SubPositive(b, a);
                        result._isNegative = false;
                    }
                }
                else
                {
                    if (b > a)
                    {
                        result = SubPositive(b, a);
                        result._isNegative = true;
                    }
                    else
                    {
                        result = SubPositive(a, b);
                        result._isNegative = false;
                    }
                }
            }

            return result;
        }

        public static BigInt operator *(BigInt a, BigInt b)
        {
            // Результат.
            BigInt result = Zero();

            BigInt mul1 = (BigInt)a.Clone(), mul2 = (BigInt)b.Clone();

            mul1.IsNegative = false;
            mul2.IsNegative = false;

            // Пока второй множитель не равен нулю.
            while (mul2 != Zero())
            {
                // Если установлен бит в очередном разряде...
                if ((mul2 & One()) == One())
                {
                    // сложить первый множитель с самим собою.
                    result = AddPositive(result, mul1);
                }

                // Очередной сдвиг первого множителя влево.
                mul1 <<= 1;

                // Подводим очередной разряд в начало для сверки с условием оператора if().
                mul2 >>= 1;
            }

            result.IsNegative = a.IsNegative ^ b.IsNegative;

            return result;
        }
        public static BigInt operator /(BigInt a, BigInt b)
        {
            if (a < b)
            {
                return Zero();
            }
            if (a == b)
            {
                return One();
            }

            if (b == Zero())
            {
                throw new DivideByZeroException();
            }

            // Результат.
            BigInt result = Zero();

            BigInt dividend = (BigInt)a.Clone(), divisor = (BigInt)b.Clone();

            dividend.IsNegative = false;
            divisor.IsNegative = false;

            int length = a.Length * 8 - 1;

            for (int i = length; i > -1; i--)
            {
                if ((divisor << i) <= dividend)
                {
                    dividend -= divisor << i;
                    result += One() << i;
                }
            }

            result.IsNegative = a.IsNegative ^ b.IsNegative;
            return result;
        }

        private static int GetLastOneBitPossition(BigInt a)
        {
            for (int i = a.Length * 8 - 1; i > -1; i--)
            {
                if (a >> i != Zero())
                {
                    return i - 1;
                }
            }

            return -1;
        }

        private static bool IsMinus(BigInt a, BigInt b)
        {
            if ((a && b) == b)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static BigInt operator %(BigInt a, BigInt b)
        {
            return Remainder(a, b);
        }

        private static BigInt Remainder(BigInt a, BigInt b)
        {
            if (a < b)
            {
                return (BigInt)a.Clone();
            }
            if (a == b)
            {
                return Zero();
            }

            if (b == Zero())
            {
                throw new DivideByZeroException();
            }

            // Результат.
            BigInt result = Zero();

            BigInt dividend = (BigInt)a.Clone(), divisor = (BigInt)b.Clone();

            dividend.IsNegative = false;
            divisor.IsNegative = false;

            int length = a.Length * 8 - 1;

            for (int i = length; i > -1; i--)
            {
                if ((divisor << i) <= dividend)
                {
                    dividend -= divisor << i;
                    result += One() << i;
                }
            }

            dividend.IsNegative = a.IsNegative ^ b.IsNegative;
            return dividend;
        }

        #endregion

        #region Math Functions

        public bool IsEven()
        {
            return (_bytes[0] & 1) == 0;
        }

        public bool IsOdd()
        {
            return (_bytes[0] & 1) == 1;
        }

        #endregion

        #region ToString Operations

        public override string ToString() => ToString(10);

        public string ToString(byte @base)
        {
            if (!(@base == 2 || @base == 8 || @base == 10 || @base == 16))
                throw new ArgumentOutOfRangeException("@base", "Avaliable values: 2, 8, 10, 16");

            string result = String.Empty;

            if (@base != 10)
            {
                if (@base == 2)
                {
                    for (int i = 0; i < _bytes.Count; i++)
                    {
                        result = Convert.ToString((int)(_bytes[i]) ^ 0b100000000, @base).Substring(1, 8) + result;
                    }
                    result = TrimLeftWithChar(result, '0');

                    if (result.Length == 0)
                        result = "0";
                }

                if (@base == 16)
                {
                    for (int i = 0; i < _bytes.Count; i++)
                    {
                        result = Convert.ToString(_bytes[i] ^ 0b100000000, @base).Substring(1, 2) + result;
                    }
                    result = TrimLeftWithChar(result, '0');

                    if (result.Length == 0)
                        result = "0";
                }

                if (@base == 8)
                {
                    BigInt forOctal = (BigInt)Clone();

                    while (forOctal.Length > 1)
                    {
                        result = Convert.ToString(forOctal._bytes[0] & 0b00000111, @base).Substring(0, 1) + result;

                        forOctal >>= 3;
                    }

                    byte lastByte = forOctal._bytes[0];

                    while (lastByte != 0)
                    {
                        while (lastByte != 0)
                        {
                            result = Convert.ToString(lastByte & 0b00000111, @base).Substring(0, 1) + result;

                            lastByte >>= 3;
                        }
                    }
                }
            }
            else
            {
                BigInt dec = (BigInt)Clone();
                dec.IsNegative = false;
                while (dec != Zero())
                {
                    result = (dec % Ten()).Bytes[0].ToString() + result;
                    dec = dec / Ten();
                }

                if (String.IsNullOrEmpty(result))
                {
                    result = "0";
                }
            }

            if (IsNegative)
            {
                result = "-" + result;
            }

            return result;
        }

        private static string TrimLeftWithChar(string input, char chr)
        {
            int count = 0;

            while (count < input.Length && input[count] == chr) count++;

            return input.Substring(count);
        }

        #endregion

        #region ICloneable Interface

        public object Clone()
        {
            BigInt result = new BigInt(this.Bytes, this.IsNegative);
            return result;
        }

        #endregion

        #region Constants

        public static BigInt One()
        {
            return new BigInt(new byte[1] { 1 }, false);
        }

        public static BigInt MinusOne()
        {
            return new BigInt(new byte[1] { 1 }, true);
        }

        public static BigInt Zero()
        {
            return new BigInt(new byte[1] { 0 }, false);
        }

        public static BigInt Ten()
        {
            return new BigInt(new byte[1] { 10 }, false);
        }

        #endregion

        #region Parse

        public static BigInt Parse(string str)
        {
            int startIndex = 0;

            bool isNeg = false;
            if (str[0] == '-')
            {
                isNeg = true;
                startIndex++;
            }

            BigInt result = BigInt.Zero();
            BigInt j = BigInt.One();
            for (int i = str.Length - 1; i >= startIndex; i--, j *= BigInt.Ten())
            {
                byte digit = byte.Parse(str[i].ToString());
                BigInt uiDigit = new BigInt(new byte[] { digit }, false);
                result += uiDigit * j;
            }

            result.IsNegative = isNeg;

            return result;
        }

        #endregion
    }
}
