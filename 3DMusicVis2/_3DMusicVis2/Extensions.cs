#region License

// Copyright (c) 2015, Vira
// All rights reserved.
// Solution: 3DMusicVis2
// Project: _3DMusicVis2
// Filename: Extensions.cs
// Date - created: 2015.08.31 - 10:42
// Date - current: 2016.05.22 - 12:52

#endregion

namespace _3DMusicVis2
{
    public static class Extensions
    {
        public static int Clamp(this int number, int smalestPossible, int biggestPossible)
        {
            if (number < biggestPossible && number > smalestPossible)
            {
                return number;
            }
            if (number > smalestPossible)
            {
                return biggestPossible;
            }
            return smalestPossible;
        }
    }
}