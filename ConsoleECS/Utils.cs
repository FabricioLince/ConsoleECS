using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleECS
{
    public static class EnumerableUtils
    {
        public static bool Any<T>(this IEnumerable<T> enumerable, Predicate<T> match)
        {
            foreach (var item in enumerable)
            {
                if (match(item)) return true;
            }
            return false;
        }

        public static S Reduce<T, S>(this IEnumerable<T> enumerable, S start, Func<S, T, S> reduceFunction)
        {
            foreach (var item in enumerable)
            {
                start = reduceFunction(start, item);
            }
            return start;
        }

        public static string ToString<T>(this IEnumerable<T> enumerable, string separator = ", ", Func<T, string> toStringFunction = null)
        {
            string rt = "";
            foreach (var item in enumerable)
            {
                if (toStringFunction == null)
                    rt += item + separator;
                else
                    rt += toStringFunction(item) + separator;
            }
            if (rt.Length > separator.Length)
                rt = rt.Substring(0, rt.Length - separator.Length);

            return rt;
        }
        public static string ToString<T>(this IEnumerable<T> enumerable, Func<T, string> toStringFunction = null, string separator = ", ")
        {
            return ToString(enumerable, separator, toStringFunction);
        }
    }

    public static class StringUtils
    {
        public static string SeparateWords(this string str, string separator = " ")
        {
            if (string.IsNullOrEmpty(str)) return str;

            string rt = "" + str[0];
            for (int i = 1; i < str.Length; i++)
            {
                if (char.IsUpper(str[i]))
                {
                    rt += separator;
                }
                rt += str[i];
            }
            return rt;
        }
        public static string ReplaceSpecialCharacters(this string str)
        {
            str = str.ToLower();
            str = str.Replace('ç', 'c');
            str = str.Replace('á', 'a');
            str = str.Replace('â', 'a');
            str = str.Replace('ã', 'a');
            str = str.Replace('é', 'e');
            str = str.Replace('ê', 'e');
            str = str.Replace('í', 'i');
            str = str.Replace('ó', 'o');
            str = str.Replace('õ', 'o');
            str = str.Replace('ô', 'o');
            str = str.Replace('ú', 'u');

            return str;
        }

        public static List<string> Test(this string input)
        {
            var rt = new List<string>();

            for (int i = 0; i < input.Length; i++)
            {
                for (char c = '0'; c <= '9'; c++)
                {
                    var array = input.ToCharArray();
                    array[i] = c;
                    var str = new string(array);
                    if (str.Equals(input)) continue;
                    rt.Add(str);
                }
            }

            return rt;
        }
    }
    /*
    public static class SizePointUtils
    {
        public static Point IndexToPoint(this Size size, int index)
        {
            return new Point(index % size.Width, index / size.Width);
        }
        public static int PointToIndex(this Size size, int x, int y)
        {
            return x + (y * size.Width);
        }
        public static int PointToIndex(this Size size, Point point)
        {
            return size.PointToIndex(point.X, point.Y);
        }
    }
    */

    public interface IIndexable<TKey, TValue>
    {
        TValue this[TKey key] { get; set; }
    }
}
