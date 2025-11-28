using System;
using System.Collections.Generic;
using System.Linq;

namespace gymschedver2
{
    public class CalenSelector
    {
        private static HashSet<DateTime> _selectedDates = new HashSet<DateTime>();

        public static HashSet<DateTime> SelectDates(int requiredDateCount, string splitName)
        {
           
            _selectedDates.Clear();
            Console.CursorVisible = false;
            DateTime displayDate = DateTime.Today;
            DateTime cursorDate = DateTime.Today;

            Console.Clear();
            Program.TitleBanner();
            while (true)
            {
                Console.SetCursorPosition(5,16);
                DisplayCalendar(displayDate, cursorDate);
                DisplayInstructionsAndSelection(requiredDateCount, splitName);
                
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        cursorDate = cursorDate.AddDays(-7);
                        break;
                    case ConsoleKey.DownArrow:
                        cursorDate = cursorDate.AddDays(7);
                        break;
                    case ConsoleKey.LeftArrow:
                        cursorDate = cursorDate.AddDays(-1);
                        break;
                    case ConsoleKey.RightArrow:
                        cursorDate = cursorDate.AddDays(1);
                        break;

                    case ConsoleKey.PageUp:
                    case ConsoleKey.P:
                        displayDate = displayDate.AddMonths(-1);
                        int day = cursorDate.Day > DateTime.DaysInMonth(displayDate.Year, displayDate.Month)
                            ? DateTime.DaysInMonth(displayDate.Year, displayDate.Month)
                            : cursorDate.Day;
                        cursorDate = new DateTime(displayDate.Year, displayDate.Month, day);
                        break;
                    case ConsoleKey.PageDown:
                    case ConsoleKey.N:
                        displayDate = displayDate.AddMonths(1);
                        int dayN = cursorDate.Day > DateTime.DaysInMonth(displayDate.Year, displayDate.Month)
                            ? DateTime.DaysInMonth(displayDate.Year, displayDate.Month)
                            : cursorDate.Day;
                        cursorDate = new DateTime(displayDate.Year, displayDate.Month, dayN);
                        break;

                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                        ToggleDateSelection(cursorDate, requiredDateCount);
                        break;

                    case ConsoleKey.C:
                        if (_selectedDates.Count == requiredDateCount)
                        {
                            Console.CursorVisible = true;
                            Console.WriteLine("\nDates confirmed.".PadRight(Console.WindowWidth));
                            return _selectedDates;
                        }
                        break;

                    case ConsoleKey.Q:
                    case ConsoleKey.Escape:
                        Console.CursorVisible = true;
                        Console.WriteLine("\nCalendar selection cancelled.".PadRight(Console.WindowWidth));
                        return new HashSet<DateTime>();
                }

                if (cursorDate.Year != displayDate.Year || cursorDate.Month != displayDate.Month)
                {
                    displayDate = new DateTime(cursorDate.Year, cursorDate.Month, 1);
                }
            }
        }

        static void DisplayCalendar(DateTime dateForMonth, DateTime highlightedDate)
        {
            DateTime startDate = _selectedDates.FirstOrDefault();
            bool weekIsSelected = startDate != default(DateTime);

            int calendarWidth = 28;
            int screenWidth = Console.WindowWidth;
            int leftPadVal = (screenWidth - calendarWidth) / 2;
            string indent = new string(' ', Math.Max(0, leftPadVal));
            string monthName = $"{dateForMonth:MMMM yyyy}";
            int monthLabelPad = (calendarWidth - monthName.Length) / 2;

            Console.WriteLine();
            Console.WriteLine(indent + new string(' ', Math.Max(0, monthLabelPad)) + monthName);
            Console.WriteLine(indent + "Su  Mo  Tu  We  Th  Fr  Sa");

            DateTime firstDayOfMonth = new DateTime(dateForMonth.Year, dateForMonth.Month, 1);
            int dayOfWeek = (int)firstDayOfMonth.DayOfWeek;
            int daysInMonth = DateTime.DaysInMonth(dateForMonth.Year, dateForMonth.Month);

            int day = 1;
            for (int row = 0; row < 6; row++)
            {
                Console.Write(indent);
                for (int col = 0; col < 7; col++)
                {
                    if (row == 0 && col < dayOfWeek || day > daysInMonth)
                    {
                        Console.Write("    ");
                    }
                    else
                    {
                        DateTime currentDate = new DateTime(dateForMonth.Year, dateForMonth.Month, day);
                        bool isInWeekRange = false;

                        if (weekIsSelected)
                        {
                            isInWeekRange = currentDate.Date >= startDate.Date && currentDate.Date < startDate.Date.AddDays(7);
                        }

                        if (currentDate.Date == highlightedDate.Date)
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                        else if (currentDate.Date == startDate.Date)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkCyan;
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else if (isInWeekRange)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else if (currentDate.Date == DateTime.Today.Date)
                        {
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }

                        Console.Write($"{day,2}  ");
                        Console.ResetColor();
                        day++;
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static void ToggleDateSelection(DateTime dateToToggle, int maxSelectedDays)
        {
            if (_selectedDates.Contains(dateToToggle.Date))
            {
                _selectedDates.Remove(dateToToggle.Date);
            }
            else
            {
                _selectedDates.Clear();
                _selectedDates.Add(dateToToggle.Date);
            }
        }

        static void DisplayInstructionsAndSelection(int maxSelectedDays, string splitName)
{
    void WriteCentered(string text)
    {
        int winWidth = Console.WindowWidth;
        int pad = Math.Max(0, (winWidth - text.Length) / 2);
        Console.WriteLine(new string(' ', pad) + text);
    }

    WriteCentered($"--- Select {maxSelectedDays} start date for '{splitName}' ---");
    WriteCentered("Arrow Keys: Navigate | Enter/Space: Select/Deselect Day");
    WriteCentered("N/P: Next/Prev Month | C: Confirm Selection | Q/Esc: Cancel");
    WriteCentered("---------------------------------------------------------");

    string selectionLine;
    if (_selectedDates.Any())
    {
        DateTime startDate = _selectedDates.First();
        selectionLine = $"Selected Week: {startDate:yyyy-MM-dd} to {startDate.AddDays(6):yyyy-MM-dd}";
    }
    else
    {
        selectionLine = $"Selected (0/{maxSelectedDays}): None";
    }
    WriteCentered(selectionLine);

    if (_selectedDates.Count == maxSelectedDays)
    {
        int winWidth = Console.WindowWidth;
        string msg = "Selection complete. Press 'C' to confirm or 'Q' to cancel.";
        int pad = Math.Max(0, (winWidth - msg.Length) / 2);
        
        Console.Write(new string(' ', pad));
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(msg);
        Console.ResetColor();
    }
}

        public static void DisplayScheduledMonth(DateTime monthToShow, List<DateTime> datesToHighlight)
        {
            int calendarWidth = 28;
            int screenWidth = Console.WindowWidth;
            int leftPadVal = (screenWidth - calendarWidth) / 2;
            string indent = new string(' ', Math.Max(0, leftPadVal));

            string monthName = $"{monthToShow:MMMM yyyy}";
            int monthLabelPad = (calendarWidth - monthName.Length) / 2;

            Console.WriteLine();
            Console.WriteLine(indent + new string(' ', Math.Max(0, monthLabelPad)) + monthName);

            Console.WriteLine(indent + "Su  Mo  Tu  We  Th  Fr  Sa");

            DateTime firstDayOfMonth = new DateTime(monthToShow.Year, monthToShow.Month, 1);
            int dayOfWeek = (int)firstDayOfMonth.DayOfWeek;
            int daysInMonth = DateTime.DaysInMonth(monthToShow.Year, monthToShow.Month);

            var scheduledDatesSet = new HashSet<DateTime>(datesToHighlight.Select(d => d.Date));

            int day = 1;
            for (int row = 0; row < 6; row++)
            {
                Console.Write(indent);

                for (int col = 0; col < 7; col++)
                {
                    if (row == 0 && col < dayOfWeek || day > daysInMonth)
                    {
                        Console.Write("    ");
                    }
                    else
                    {
                        DateTime currentDate = new DateTime(monthToShow.Year, monthToShow.Month, day);

                        if (scheduledDatesSet.Contains(currentDate.Date))
                        {
                            Console.BackgroundColor = ConsoleColor.DarkCyan;
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else if (currentDate.Date == DateTime.Today.Date)
                        {
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }

                        Console.Write($"{day,2}  ");
                        Console.ResetColor();
                        day++;
                    }
                }
                Console.WriteLine();
                if (day > daysInMonth) break;
            }
            Console.WriteLine();
        }
    }
}