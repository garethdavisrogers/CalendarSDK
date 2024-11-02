using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

public class CalendarModel
{
    public List<List<CalendarDay>> GenerateWeeks(int year, int month)
    {
        List<List<CalendarDay>> weeks = new List<List<CalendarDay>>();
        List<CalendarDay> days = new List<CalendarDay>();

        // Get the first and last day of the current month
        DateTime firstDayOfMonth = new DateTime(year, month, 1);
        DateTime lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

        // Find the start day, which is the last Sunday before the first day of the month
        DateTime startDay = firstDayOfMonth;
        while (startDay.DayOfWeek != DayOfWeek.Sunday)
        {
            startDay = startDay.AddDays(-1);
        }

        // Calculate the end day, which is the first Saturday after the last day of the month
        DateTime endDay = lastDayOfMonth;
        while (endDay.DayOfWeek != DayOfWeek.Saturday)
        {
            endDay = endDay.AddDays(1);
        }

        // Populate days from startDay to endDay
        DateTime currentDay = startDay;
        while (currentDay <= endDay)
        {
            days.Add(new CalendarDay { Date = currentDay });

            // When a full week is reached, add it to weeks
            if (days.Count == 7)
            {
                weeks.Add(days);
                days = new List<CalendarDay>();
            }

            currentDay = currentDay.AddDays(1);
        }

        // Ensure a 5-6 week display format
        while (weeks.Count < 6)
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


    public class CalendarDay
    {
        public DateTime Date { get; set; }
    }

}
