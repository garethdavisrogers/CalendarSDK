using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CalendarSDK
{
    public class CalendarModel
    {
        public DateTime Today { get; private set; }
        public List<List<CalendarDay>> Weeks { get; private set; }
        public List<string> DaysOfWeek { get; private set; }

        public CalendarModel()
        {
            Today = DateTime.Today;
            DaysOfWeek = new List<string>(CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames);
            Weeks = GenerateWeeks(Today.Year, Today.Month);
        }

        public List<List<CalendarDay>> GenerateWeeks(int year, int month)
        {
            var weeks = new List<List<CalendarDay>>();
            var days = new List<CalendarDay>();

            DateTime firstDayOfMonth = new DateTime(year, month, 1);

            // Find the start of the calendar on the Sunday before the first day of the month
            DateTime startDay = firstDayOfMonth;
            while (startDay.DayOfWeek != DayOfWeek.Sunday)
            {
                startDay = startDay.AddDays(-1);
            }

            // Find the end of the calendar on the Saturday after the last day of the month
            DateTime lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            DateTime endDay = lastDayOfMonth;
            while (endDay.DayOfWeek != DayOfWeek.Saturday)
            {
                endDay = endDay.AddDays(1);
            }

            // Populate the days from startDay to endDay
            DateTime currentDay = startDay;
            while (currentDay <= endDay || weeks.Count < 6)
            {
                days.Add(new CalendarDay { Date = currentDay });

                // Once we have 7 days, add the week to the list of weeks and start a new week
                if (days.Count == 7)
                {
                    weeks.Add(days);
                    days = new List<CalendarDay>();
                }

                currentDay = currentDay.AddDays(1);

                // Stop if we have exactly 6 weeks (42 days), even if we’ve reached the end
                if (weeks.Count == 6) break;
            }

            return weeks;
        }

        public void PreviousMonth()
        {
            Today = Today.AddMonths(-1);
            Weeks = GenerateWeeks(Today.Year, Today.Month);
        }

        public void NextMonth()
        {
            Today = Today.AddMonths(1);
            Weeks = GenerateWeeks(Today.Year, Today.Month);
        }
    }

    public class CalendarDay
    {
        public DateTime Date { get; set; }
    }
}
