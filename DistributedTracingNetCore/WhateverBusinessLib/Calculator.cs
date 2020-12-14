using System;

namespace WhateverBusinessLib
{
    public class Calculator
    {
        private Random rnd = new Random();
        public int Sum(Val values)
        {
            if (rnd.Next(1,10) % 4 == 0)
            {
                throw new Exception("some-random-exception-for-sum-opt");
            }

            return values.Val1 + values.Val2;
        }

        public int Substract(Val values)
        {
            if (rnd.Next(1, 10) % 3 == 0)
            {
                throw new Exception("some-random-exception-for-sub-opt");
            }

            return values.Val1 - values.Val2;
        }
    }
}
