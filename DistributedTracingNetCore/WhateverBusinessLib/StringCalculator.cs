using System;

namespace WhateverBusinessLib
{
    public class StringCalculator
    {
        private Random rnd = new Random();
        public string ToUpper(StringVal stringVal)
        {
            if (rnd.Next(1, 10) % 4 == 0)
            {
                throw new Exception("some-random-exception-for-upper-opt");
            }

            return stringVal.Val.ToUpper();
        }

        public string SubString(StringVal stringVal)
        {
            if (rnd.Next(1, 10) % 4 == 0)
            {
                throw new Exception("some-random-exception-for-substring-opt");
            }

            return stringVal.Val.Substring(stringVal.StartIndex,stringVal.Length);
        }
    }
}
