using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.WebService
{
    public static class Extentions
    {
        private const string DateTimeFormat = "{0}-{1}-{2}T{3}:{4}:{5}Z";


        public static dynamic IgnoreEmptyPropertyEx<T>(this T fixMe)
        {
            var t = fixMe.GetType();
            var returnClass = new ExpandoObject() as IDictionary<string, object>;
            foreach (var pr in t.GetProperties())
            {
                var val = pr.GetValue(fixMe);
                if (val is string && string.IsNullOrWhiteSpace(val.ToString()))
                {
                }
                else if (val == null)
                {
                }
                else
                {
                    returnClass.Add(pr.Name, val);
                }
            }
            return returnClass;
        }
        //String
        public static string TruncateEx(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }


        /// <summary> /// To the iso8601 date. /// </summary>
        ///The date.
        /// <returns></returns>
        public static string ToIso8601DateEx(this DateTime date)
        {
            return string.Format(
                DateTimeFormat,
                date.Year,
                PadLeft(date.Month),
                PadLeft(date.Day),
                PadLeft(date.Hour),
                PadLeft(date.Minute),
                PadLeft(date.Second));
        }

        /// <summary> /// Froms the iso8601 date. /// </summary>
        ///The date.
        /// <returns></returns>
        public static DateTime FromIso8601DateEx(this string date)
        {
            return DateTime.ParseExact(date.Replace("T", " "), "u", CultureInfo.InvariantCulture);
        }

        public static string FromEpochToTimeEx(this string timeEpoch)
        {
            var ans = "";
            var cleanTime = timeEpoch.Replace("P", " ").Replace("T", " ").Replace("H", ":").Replace("M", "");

            ans = cleanTime;
            return ans;

        }
        private static string PadLeft(int number)
        {
            if (number > 10)
            {
                return string.Format("0{0}", number);
            }

            return number.ToString(CultureInfo.InvariantCulture);
        }



        // Arrays 
        public static T[] SubArrayEx<T>(this T[] sourceArray, int sourceArrayIndex, int length = 0)
        {
            T[] destinationArray = new T[0];
            do
            {
                var maxLength = 0;
                if (sourceArrayIndex > sourceArray.Length)
                {
                    break;
                }

                if (length == 0 || (length + sourceArrayIndex - 1 >= sourceArray.Length))
                {
                    maxLength = sourceArray.Length - sourceArrayIndex;
                }
                else
                {
                    maxLength = length;
                }

                destinationArray = new T[maxLength];
                Array.Copy(sourceArray, sourceArrayIndex, destinationArray, 0, maxLength);

            } while (false);
            return destinationArray;
        }


    }


}
