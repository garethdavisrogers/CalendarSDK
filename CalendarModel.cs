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

        // Method to move to the previous day
        public void PreviousDay()
        {
            Today = Today.AddDays(-1);
            UpdateWeeksIfMonthChanged();
        }

        // Method to move to the next day
        public void NextDay()
        {
            Today = Today.AddDays(1);
            UpdateWeeksIfMonthChanged();
        }

        // Method to move to the previous week
        public void PreviousWeek()
        {
            Today = Today.AddDays(-7);
            UpdateWeeksIfMonthChanged();
        }

        // Method to move to the next week
        public void NextWeek()
        {
            Today = Today.AddDays(7);
            UpdateWeeksIfMonthChanged();
        }

        // Method to move to the previous month
        public void PreviousMonth()
        {
            Today = Today.AddMonths(-1);
            Weeks = GenerateWeeks(Today.Year, Today.Month);
        }

        // Method to move to the next month
        public void NextMonth()
        {
            Today = Today.AddMonths(1);
            Weeks = GenerateWeeks(Today.Year, Today.Month);
        }

        // Generate weeks for any given month and year
        public List<List<CalendarDay>> GenerateWeeks(int year, int month)
        {
            var weeks = new List<List<CalendarDay>>();
            var days = new List<CalendarDay>();
            DateTime firstDayOfMonth = new DateTime(year, month, 1);
            DateTime startDay = GetStartingSunday(firstDayOfMonth);

            DateTime currentDay = startDay;
            int daysInMonth = DateTime.DaysInMonth(year, month);

            while (weeks.Count < 6 && (currentDay.Month == month || currentDay < firstDayOfMonth.AddMonths(1)))
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

        // Helper method to update weeks if the month has changed
        private void UpdateWeeksIfMonthChanged()
        {
            if (Today.Month != Weeks[2][0].Date.Month) // Check a middle week day to detect month change
            {
                Weeks = GenerateWeeks(Today.Year, Today.Month);
            }
        }
    }

    public class CalendarDay
    {
        public DateTime Date { get; set; }
    }
}
