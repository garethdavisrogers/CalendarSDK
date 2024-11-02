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

        // Property to retrieve the current week based on today's date
        public List<CalendarDay> CurrentWeek => Weeks.FirstOrDefault(week => week.Any(day => day.Date.Date == Today.Date)) ?? new List<CalendarDay>();

        // Property to retrieve the current day as a CalendarDay object
        public CalendarDay CurrentDay => Weeks.SelectMany(week => week).FirstOrDefault(day => day.Date.Date == Today.Date) ?? new CalendarDay { Date = Today };

        // Initialize the model with today's date and generate current month's weeks
        public CalendarModel()
        {
            Today = DateTime.Today;
            DaysOfWeek = new List<string>(CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames);
            Weeks = GenerateWeeks(Today.Year, Today.Month);
        }

        // Generate weeks for any given month and year, ensuring 6 rows for a complete month view
        public List<List<CalendarDay>> GenerateWeeks(int year, int month)
        {
            var weeks = new List<List<CalendarDay>>();
            var days = new List<CalendarDay>();
            DateTime firstDayOfMonth = new DateTime(year, month, 1);
            DateTime startDay = GetStartingSunday(firstDayOfMonth);

            DateTime currentDay = startDay;

            // Generate up to 6 weeks (42 days) to cover any layout
            for (int i = 0; i < 42; i++)
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

        private DateTime GetStartingSunday(DateTime firstDayOfMonth)
        {
            DateTime startDay = firstDayOfMonth;
            while (startDay.DayOfWeek != DayOfWeek.Sunday)
            {
                startDay = startDay.AddDays(-1);
            }
            return startDay;
        }
    }

    public class CalendarDay
    {
        public DateTime Date { get; set; }
    }
}
