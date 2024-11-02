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

            // First day of the current month
            DateTime firstDayOfMonth = new DateTime(year, month, 1);

            // Start from the last Sunday before the first day of the month
            DateTime startDay = firstDayOfMonth;
            while (startDay.DayOfWeek != DayOfWeek.Sunday)
            {
                startDay = startDay.AddDays(-1);
            }

            // End on the first Saturday after the last day of the month
            DateTime lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            DateTime endDay = lastDayOfMonth;
            while (endDay.DayOfWeek != DayOfWeek.Saturday)
            {
                endDay = endDay.AddDays(1);
            }

            // Populate the calendar days from startDay to endDay
            DateTime currentDay = startDay;
            while (currentDay <= endDay)
            {
                days.Add(new CalendarDay { Date = currentDay });

                // When a week is filled, add it to the weeks and start a new one
                if (days.Count == 7)
                {
                    weeks.Add(days);
                    days = new List<CalendarDay>();
                }

                currentDay = currentDay.AddDays(1);
            }

            // Ensure we have a minimum of 4 weeks and a maximum of 6 weeks
            while (weeks.Count < 4)
            {
                weeks.Add(new List<CalendarDay>(Enumerable.Repeat(new CalendarDay { Date = DateTime.MinValue }, 7)));
            }
            while (weeks.Count < 6 && (weeks.Count == 4 || weeks.Count == 5))
            {
                days = new List<CalendarDay>();
                for (int i = 0; i < 7; i++)
                {
                    days.Add(new CalendarDay { Date = currentDay });
                    currentDay = currentDay.AddDays(1);
                }
                weeks.Add(days);
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
