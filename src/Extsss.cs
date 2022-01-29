using System;
using System.Collections.Generic;

namespace JudgePracticeMod
{
    public static class Extsss
    {
        public static List<TKey> ToList<TKey, TValue>(this Dictionary<TKey, TValue>.KeyCollection Keys)
        {
            var list = new List<TKey>();
            foreach(TKey tk in Keys)
            {
                list.Add(tk);
            }
            return list;
        }
        public static List<TValue> ToList<TKey, TValue>(this Dictionary<TKey, TValue>.ValueCollection Values)
        {
            var list = new List<TValue>();
            foreach (TValue tk in Values)
            {
                list.Add(tk);
            }
            return list;
        }
        public static double ToDouble(this object obj)
        {
            return Convert.ToDouble(obj);
        }
        public static int ToInt(this object obj)
        {
            return Convert.ToInt32(obj);
        }
        public static bool SetBoolAfter(this ref bool b, bool setTo)
        {
            var c = b;
            b = setTo;
            return c;
        }
    }
}
