﻿using System;

namespace PizzaStore.Extensions
{
    public static class IntegerExtensions
    {
        public static int RoundOff(this int i)
        {
            return ((int)Math.Round(i / 10.0)) * 10;
        }
    }
}
