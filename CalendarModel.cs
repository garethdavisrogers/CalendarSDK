using System;
using System.Collections.Generic;
using System.Globalization;

public class CalendarModel
{
    public DateTime Today { get; private set; } = DateTime.Today;
    public List<List<CalendarDay>> Weeks { get; private set; }

    public CalendarModel(int year, int month)
    {
        Weeks = GenerateWeeks(year, month);
    }

    public List<List<CalendarDay>> GenerateWeeks(int year, int month)
    {
        var weeks = new List<List<CalendarDay>>();
        var days = new List<CalendarDay>();

        // Start with the first day of the month
        DateTime firstDayOfMonth = new DateTime(year, month, 1);

        // Shift back to the previous Sunday if needed
        DateTime currentDay = firstDayOfMonth;
        while (currentDay.DayOfWeek != DayOfWeek.Sunday)
        {
            currentDay = currentDay.AddDays(-1);
        }

        // Generate days and weeks until we reach the end of the month view
        DateTime lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
        while (currentDay <= lastDayOfMonth || days.Count > 0)
        {
            days.Add(new CalendarDay { Date = currentDay });

            // When we have a full week, add it to weeks and reset days
            if (days.Count == 7)
            {
                weeks.Add(new List<CalendarDay>(days));
                days.Clear();
            }

            currentDay = currentDay.AddDays(1);
        }

        // If there are remaining days after the last week, fill up to 7 days
        while (days.Count > 0 && days.Count < 7)
        {
            days.Add(new CalendarDay { Date = currentDay });
            currentDay = currentDay.AddDays(1);
        }

        if (days.Count == 7)
        {
            weeks.Add(days);
        }

        return weeks;
    }

    public class CalendarDay
    {
        public DateTime Date { get; set; }
    }
}
