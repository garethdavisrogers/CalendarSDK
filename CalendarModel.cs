using System;
using System.Collections.Generic;
using System.Globalization;

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

            // Find the first and last day of the month
            DateTime firstDayOfMonth = new DateTime(year, month, 1);
            DateTime lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

            // Calculate the starting day (the last Sunday before or on the first day of the month)
            DateTime startDay = firstDayOfMonth;
            while (startDay.DayOfWeek != DayOfWeek.Sunday)
            {
                startDay = startDay.AddDays(-1);
            }

            // Start from the beginning of the grid, fill weeks until we've covered all days in the month
            DateTime currentDay = startDay;
            while (weeks.Count < 6) // Ensure a maximum of 6 weeks
            {
                days.Add(new CalendarDay { Date = currentDay });

                if (days.Count == 7) // Complete the current week
                {
                    weeks.Add(days);
                    days = new List<CalendarDay>();
                }

                // Stop adding weeks once we have captured all days up to the end of the month, ensuring a minimum of 4 weeks
                if (currentDay >= lastDayOfMonth && weeks.Count >= 4)
                {
                    break;
                }

                // Move to the next day
                currentDay = currentDay.AddDays(1);
            }

            // Add an extra week if needed to reach 6 weeks in total for visual consistency
            while (weeks.Count < 6)
            {
                days.Add(new CalendarDay { Date = currentDay });
                if (days.Count == 7)
                {
                    weeks.Add(days);
                    days = new List<CalendarDay>();
                }
                currentDay = currentDay.AddDays(1);
            }

            return weeks;
        }
    }

    public class CalendarDay
    {
        public DateTime Date { get; set; }
    }
}
