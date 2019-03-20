using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePonti.BLL.Common
{
    public static class Extensions
    {

        #region DateTime Format

        public static string ToCustomDateString(this DateTime Date)
        {
            return Date.ToString("MM/dd/yy");
        }
        public static string ToCustomDateString(this DateTime? Date)
        {
            if (!Date.HasValue) { return ""; }
            return Date.Value.ToCustomDateString();
        }

        public static string ToCustomTimeString(this DateTime Date, bool IncludeSeconds = false)
        {
            string format = "hh:mm tt";
            if (IncludeSeconds)
            {
                format = "hh:mm:ss tt";
            }

            return Date.ToString(format);
        }
        public static string ToCustomTimeString(this DateTime? Date)
        {
            if (!Date.HasValue) { return ""; }
            return Date.Value.ToCustomTimeString();
        }

        public static string ToCustomDateTimeString(this DateTime Date, bool IncludeSeconds = false)
        {
            return Date.ToCustomDateString() + " " + Date.ToCustomTimeString(IncludeSeconds);
        }
        public static string ToCustomDateTimeString(this DateTime? Date)
        {
            if (!Date.HasValue) { return ""; }
            return Date.Value.ToCustomDateTimeString();
        }

        #endregion

        #region DateTime Format for DatePicker
        //yyyy-MM-dd hh:mm tt
        public static string ToCustomDateStringForPicker(this DateTime Date)
        {
            return Date.ToString("yyyy-MM-dd");
        }
        public static string ToCustomDateStringForPicker(this DateTime? Date)
        {
            if (!Date.HasValue) { return ""; }
            return Date.Value.ToCustomDateStringForPicker();
        }

        public static string ToCustomTimeStringForPicker(this DateTime Date, bool IncludeSeconds = false)
        {
            return Date.ToCustomTimeString(IncludeSeconds);
        }
        public static string ToCustomTimeStringForPicker(this DateTime? Date)
        {
            return Date.ToCustomTimeString();
        }

        public static string ToCustomDateTimeStringForPicker(this DateTime Date, bool IncludeSeconds = false)
        {
            return Date.ToCustomDateStringForPicker() + " " + Date.ToCustomTimeStringForPicker(IncludeSeconds);
        }
        public static string ToDateTimeStringForPicker(this DateTime? Date)
        {
            if (!Date.HasValue) { return ""; }
            return Date.Value.ToCustomDateTimeStringForPicker();
        }

        #endregion

        #region Decimal

        public static string ToFormattedString(this decimal? Value, int DecimalPlaces = 2)
        {
            if (!Value.HasValue)
            {
                return null;
            }

            string format = "0." + "".PadRight(DecimalPlaces, '0');
            return Value.Value.ToString(format);
        }

        #endregion

    }
}
