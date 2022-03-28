using System;
using System.Collections.Generic;
using System.Linq;

namespace Networking
{
    public static class HelperMethods
    {
        public static IEnumerable<Enum> GetSelectedFlags(this Enum @enum)
        {
            return Enum.GetValues(@enum.GetType()).Cast<Enum>().Where(v => !Equals((int)(object)v, 0) && @enum.HasFlag(v));
        }
    }
}
