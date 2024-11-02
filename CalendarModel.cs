using System;
using System.Collections.Generic;
using System.Globalization;

public class CalendarModel
{
    public DateTime Today { get; private set; } = DateTime.Today;
    public List<List<CalendarDay>> Weeks { get; private set; }

    // Property for the first day of the current month
    public DateTime FirstDayOfCurrentMonth => new DateTime(Today.Year, Today.Month, 1);

    // Property for the last day of the current month
    public DateTime LastDayOfCurrentMonth => new DateTime(Today.Year, Today.Month, DateTime.DaysInMonth(Today.Year, Today.Month));

    // Property for the closest previous or same Sunday relative to the first day of the current month
    public DateTime ClosestSunday
    {
        get
        {
            DateTime closestSunday = FirstDayOfCurrentMonth;
            while (closestSunday.DayOfWeek != DayOfWeek.Sunday)
            {
                closestSunday = closestSunday.AddDays(-1);
            }
            return closestSunday;
        }
    }

    public CalendarModel(int year, int month)
    {
        Today = new DateTime(year, month, 1);
        Weeks = GenerateWeeks(year, month);
    }

    public List<List<CalendarDay>> GenerateWeeks(int year, int month)
    {
        var weeks = new List<List<CalendarDay>>();
        var days = new List<CalendarDay>();

        // Start with the closest previous or same Sunday
        DateTime currentDay = ClosestSunday;

        // Generate days and weeks until we reach the end of the month view
        DateTime lastDayOfMonth = LastDayOfCurrentMonth;
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

        // Fill the last week if it's not complete
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
