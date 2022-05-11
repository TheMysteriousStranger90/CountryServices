﻿using System;
using System.Collections.Generic;

namespace CountryServices.Tests.Comparers
{
    /// <summary>
    /// Present equality comparer by value of LocalCurrency objects.
    /// </summary>
    public class CountryInfoComparer : IEqualityComparer<CountryInfo>
    {
        /// <summary>
        /// Compare two LocalCurrency objects.
        /// </summary>
        /// <param name="x">First parameter.</param>
        /// <param name="y">Second parameter.</param>
        /// <returns>true if two LocalCurrency object equals by value; false otherwise.</returns>
        public bool Equals(CountryInfo? x, CountryInfo? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null))
            {
                return false;
            }

            if (ReferenceEquals(y, null))
            {
                return false;
            }

            if (x.GetType() != y.GetType())
            {
                return false;
            }

            return x.Name == y.Name && x.CapitalName == y.CapitalName;
        }

        /// <summary>
        /// Calculate hash code of object.
        /// </summary>
        /// <param name="obj">Source object</param>
        /// <returns></returns>
        public int GetHashCode(CountryInfo obj)
        {
            return HashCode.Combine(obj.Name, obj.CapitalName);
        }
    }
}
