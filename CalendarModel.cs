using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

public class CalendarModel
{
    public DateTime Today { get; private set; }
    public DateTime CurrentViewDate { get; private set; }  // Tracks the date displayed in the calendar view

    public List<List<CalendarDay>> Weeks { get; private set; }
    public List<string> DaysOfWeek { get; private set; }

    public List<CalendarDay> CurrentWeek => Weeks.FirstOrDefault(week => week.Any(day => day.Date.Date == Today.Date)) ?? new List<CalendarDay>();

    public CalendarDay CurrentDay => Weeks.SelectMany(week => week).FirstOrDefault(day => day.Date.Date == Today.Date) ?? new CalendarDay
    {
        Date = Today
    };

    public CalendarModel()
    {
        Today = DateTime.Today;
        CurrentViewDate = Today;  // Initialize with today's month and year
        DaysOfWeek = new List<string>(CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames);
        Weeks = GenerateWeeks(CurrentViewDate.Year, CurrentViewDate.Month);
    }

    public void PreviousMonth()
    {
        CurrentViewDate = new DateTime(CurrentViewDate.Year, CurrentViewDate.Month, 1).AddMonths(-1); // Move to the first of the previous month
        Weeks = GenerateWeeks(CurrentViewDate.Year, CurrentViewDate.Month);
    }

    public void NextMonth()
    {
        CurrentViewDate = new DateTime(CurrentViewDate.Year, CurrentViewDate.Month, 1).AddMonths(1); // Move to the first of the next month
        Weeks = GenerateWeeks(CurrentViewDate.Year, CurrentViewDate.Month);
    }

    public List<List<CalendarDay>> GenerateWeeks(int year, int month)
    {
        var weeks = new List<List<CalendarDay>>();
        var days = new List<CalendarDay>();

        DateTime firstDayOfMonth = new DateTime(year, month, 1);
        DateTime lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));

        // Start from the last Sunday before the first day of the month
        DateTime startDay = firstDayOfMonth;
        while (startDay.DayOfWeek != DayOfWeek.Sunday)
        {
            startDay = startDay.AddDays(-1);
        }

        // Populate days from startDay to cover the month and reach at least the next Saturday
        DateTime currentDay = startDay;
        while (weeks.Count < 6 || currentDay <= lastDayOfMonth)
        {
            days.Add(new CalendarDay { Date = currentDay });

            if (days.Count == 7) // Complete the week
            {
                weeks.Add(days);
                days = new List<CalendarDay>();
            }

            currentDay = currentDay.AddDays(1);
        }

        return weeks;
    }

    public class CalendarDay
    {
        public DateTime Date { get; set; }
    }

}
