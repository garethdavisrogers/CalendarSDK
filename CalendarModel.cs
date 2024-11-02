using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

public class CalendarModel
{
    public List<List<CalendarDay>> GenerateWeeks(int year, int month)
    {
        var weeks = new List<List<CalendarDay>>();
        var days = new List<CalendarDay>();

        // 1. Define the first and last days of the month
        DateTime firstDayOfMonth = new DateTime(year, month, 1);
        DateTime lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

        // 2. Determine the starting display day by finding the previous Sunday
        DateTime startDisplayDay = firstDayOfMonth;
        while (startDisplayDay.DayOfWeek != DayOfWeek.Sunday)
        {
            startDisplayDay = startDisplayDay.AddDays(-1);
        }

        // 3. Populate the display starting from `startDisplayDay` for 42 days (6 weeks * 7 days)
        DateTime currentDay = startDisplayDay;
        for (int i = 0; i < 42; i++)
        {
            days.Add(new CalendarDay { Date = currentDay });
            currentDay = currentDay.AddDays(1);

            // Group into weeks of 7 days
            if (days.Count == 7)
            {
                weeks.Add(new List<CalendarDay>(days));
                days.Clear();
            }
        }

        return weeks;
    }

    public class CalendarDay
    {
        public DateTime Date { get; set; }
    }

}
