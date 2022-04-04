using AspitPlanner.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AspitPlanner.Helpers
{
    public class Util
    {
        public static DateTime getDateTime()
        {
            DateTime d = DateTime.Now;

            d = new DateTime(d.Year, d.Month, d.Day);

            return d;
        }
        
        public static bool validerFredagLigeUge(DateTime time)
        {
            if(time.DayOfWeek == DayOfWeek.Friday && getWeek(time) % 2 == 0)
            {
                return true;
            }
            return false;
        }
        public static int getWeek(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static System.Collections.ObjectModel.Collection<ResourceDictionary> MakePalette(List<ChartValue> list)
        {
            System.Collections.ObjectModel.Collection<ResourceDictionary> palette = new System.Collections.ObjectModel.Collection<ResourceDictionary>();
            foreach (ChartValue item in list)
            {
                ResourceDictionary rd = new ResourceDictionary();
                Style style = new Style(typeof(Control));
                SolidColorBrush brush = null;
                switch (item.ID)
                {
                    case 1: brush = new SolidColorBrush(Colors.LightBlue); break;
                    case 2: brush = new SolidColorBrush(Colors.DarkOrange); break;
                    case 3: brush = new SolidColorBrush(Colors.Red); break;
                    case 4: brush = new SolidColorBrush(Colors.Green); break;
                    case 5: brush = new SolidColorBrush(Colors.LightGreen); break;
                    case 6: brush = new SolidColorBrush(Colors.DarkKhaki); break;
                    case 7: brush = new SolidColorBrush(Colors.Yellow); break;
                    case 9: brush = new SolidColorBrush(Colors.OrangeRed); break;
                    default: brush = new SolidColorBrush(Colors.Black); break;
                }
                style.Setters.Add(new Setter() { Property = Control.BackgroundProperty, Value = brush });
                rd.Add("DataPointStyle", style);
                palette.Add(rd);
            }
            return palette;
        }

        /// <summary>
        /// Will determine if day is past or future.
        /// If future - ensure date is not a weekend, holiday or other day where student has a day off.
        /// </summary>
        /// <returns></returns>
        public static bool ValidateIsSchoolday(DateTime date)
        {
            //Should never create presents in the past.
            //if (date < DateTime.Now)
            //{
            //    Debug.WriteLine("Day was in the past - do not create");
            //    return false;
            //}
            //Pædagogisk dag.
            if (Util.validerFredagLigeUge(date))
            {
                return false;
            }
            //Weekend 
            if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday)
            {
                return false;
            }

            switch (GetIso8601WeekOfYear(date))
            {
                //Check up on vacation weeks

                //Autumn
                case 7:
                //Winter
                case 42:

                //Summer
                case 25:
                case 26:
                case 27:
                case 28:
                case 29:
                case 30:

                //jul
                case 53:
                case 54:
                    return false;

                default:
                    break;
            }

            //A little flawed since all other days can be holiday, easter etc.
            return true;

        }

        private static bool IsDayOff() 
        {
            //Påskedage, grundlovsdag mm.
            return false;
        }
        private static int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }
}
