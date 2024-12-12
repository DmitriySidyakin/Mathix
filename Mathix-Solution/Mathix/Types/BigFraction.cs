using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mathix.Types
{
    public class BigFraction
    {
        public BigInt IntegerPartOfNumber { get; set; }

        public BigInt Fraction { get; set; }
        public BigFraction() { IntegerPartOfNumber = new BigInt(new byte[] { 0 }, false); Fraction = new BigInt(new byte[] { 0 }, false); }

        #region Parse

        public static BigFraction Parse(string str)
        {
            int startIndex = 0;

            bool isNeg = false;
            if (str[0] == '-')
            {
                isNeg = true;
                startIndex++;
            }

            // Get the dot position
            int dotPosition = str.IndexOf('.', startIndex);


        }

        #endregion
    }
}
