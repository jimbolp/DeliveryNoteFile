using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelNoteItems
{
    public static class DateID
    {
        public static DateTime? Convert(string dateid)
        {
            dateid = dateid.Trim();
            if (dateid.Length != 8)
                return null;

            DateTime date = new DateTime();
            //string dateFromDateID = "";
            try
            {
                //dateFromDateID = dateid.Substring(6, 2) + "." + dateid.Substring(4, 2) + "." + dateid.Substring(0, 4);
                if (DateTime.TryParseExact(dateid, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                    return date;
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
