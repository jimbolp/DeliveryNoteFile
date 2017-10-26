using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelNoteItems
{
    public static class Parse
    {
        /// <summary>
        /// This method is trying to convert DateID (20171020) to DateTime (20.10.2017) and return the result. Returns null if it fails.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime? ConvertToDateTime(string str)
        {
            str = str.Trim();
            if (str.Length != 8)
                return null;

            DateTime date = new DateTime();
            try
            {
                if (DateTime.TryParseExact(str, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                    return date;
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Returns true if "J" and false if "N". Else returns null.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool? StringToBool(string s)
        {
            switch (s.Trim())
            {
                case "J":
                    return true;
                case "N":
                    return false;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Returns true if '1' and false if '0'. Else returns null.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static bool? IntToBool(int i)
        {
            switch (i)
            {
                case 1:
                    return true;
                case 0:
                    return false;
                default:
                    return null;
            }
        }
    }
}
