using Spectre.Console;
using Spectre.Console.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace gymschedver2
{
    public static class AdminMenu
    {
        private static bool isAdminLoggedIn = false;
        private const string AdminFilePath = "admin.json";

        public static void AdminBanner()
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Yellow;
            string[] ADbanner =
            {
                "█████████████████████████████████████████████████████████████████████████████████████████████████████████",
                "█░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░█",
                "░░                                                                                                     ░░",
                "░░        ▄████████  ▄█      ███             ▄███████▄  ▄█   ████    ▐████▀    ▄████████  ▄█           ░░",
                "░░        ███    ███ ███  ▀█████████▄        ███    ███ ███    ███▌   ████▀    ███    ███ ███          ░░",
                "░░        ███    █▀  ███▌    ▀███▀▀██        ███    ███ ███▌    ███  ▐███      ███    █▀  ███          ░░",
                "░░       ▄███▄▄▄     ███▌     ███   ▀        ███    ███ ███▌    ▀███▄███▀     ▄███▄▄▄     ███          ░░",
                "░░       ▀███▀▀▀     ███▌     ███          ▀█████████▀  ███▌    ████▀██▄     ▀▀███▀▀▀     ███          ░░",
                "░░        ███        ███      ███            ███        ███    ▐███  ▀███      ███    █▄  ███          ░░",
                "░░        ███        ███      ███            ███        ███   ▄███     ███▄    ███    ███ ███▌    ▄    ░░",
                "░░        ███        █▀      ▄████▀         ▄████▀      █▀   ████       ███▄   ██████████ █████▄▄██    ░░",
                "░░                                                                                                     ░░",
                "░░                                  █████╗ ██████╗ ███╗   ███╗██╗███╗   ██╗                            ░░",
                "░░                                 ██╔══██╗██╔══██╗████╗ ████║██║████╗  ██║                            ░░",
                "░░                                 ███████║██║  ██║██╔████╔██║██║██╔██╗ ██║                            ░░",
                "░░                                 ██╔══██║██║  ██║██║╚██╔╝██║██║██║╚██╗██║                            ░░",
                "░░                                 ██║  ██║██████╔╝██║ ╚═╝ ██║██║██║ ╚████║                            ░░",
                "░░                                 ╚═╝  ╚═╝╚═════╝ ╚═╝     ╚═╝╚═╝╚═╝  ╚═══╝                            ░░",
                "█░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░█",
                "█████████████████████████████████████████████████████████████████████████████████████████████████████████"
            };

            int ADwidth = Console.WindowWidth;
            foreach (string ADline in ADbanner)
            {
                int ADpad = (ADwidth - ADline.Length) / 2 + 1;
                Console.WriteLine(new string(' ', Math.Max(ADpad, 0)) + ADline);
            }
            string hLine = new string('-', 120);
            int linePad = (Console.WindowWidth - hLine.Length) / 2;
            Console.WriteLine(new string(' ', Math.Max(linePad, 0)) + hLine);
        }

        public static void AdminLogin(List<UserAuth> users)
        {
            Console.Clear();
            AdminBanner();

            Console.CursorVisible = false;
            if (!File.Exists(AdminFilePath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                WriteCentered("admin.json file not found.");
                Console.ResetColor();
                System.Threading.Thread.Sleep(3000);
                return;
            }

            AdminAuth adminConfig;
            try
            {
                string jsonString = File.ReadAllText(AdminFilePath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                adminConfig = JsonSerializer.Deserialize<AdminAuth>(jsonString, options);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                WriteCentered("Error reading admin config file.");
                WriteCentered(ex.Message);
                Console.ResetColor();
                System.Threading.Thread.Sleep(3000);
                return;
            }
            string adpadding = new string(' ', 20);
            Console.Write(adpadding + "Enter admin password: ");
            string password = HIdeP.ReadPassword();
            Console.CursorVisible = false;
            if (password == adminConfig.Password)
            {
                isAdminLoggedIn = true;
                AdminDashboard(users);
                isAdminLoggedIn = true;
            }
            else
            {
                Console.CursorVisible = false;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                WriteCentered("   Invalid admin password.");
                Console.ResetColor();
                System.Threading.Thread.Sleep(2000);
            }
        }

        public static void AdminDashboard(List<UserAuth> users)
        {
            Console.CursorVisible = false;
            string[] adminMenuOptions =
            {
                "View All Users",
                "View User Details",
                "Delete User Account",
                "View System Statistics",
                "Manage Exercises",
                "Manage Workout Split",
                "View User Activity Logs",
                "Logout"
            };

            bool isAdminActive = true;
            while (isAdminActive)
            {
                int selectedIndex = Program.intro(adminMenuOptions);

                switch (selectedIndex)
                {
                    case 0:
                        ViewAllUsers(users);
                        break;
                    case 1:
                        ViewUserDetails(users);
                        break;
                    case 2:
                        DeleteUserAccount(users);
                        break;
                    case 3:
                        ViewSystemStatistics(users);
                        break;
                    case 4:
                        ManageExercise();
                        break;
                    case 5:
                        ManageWorkoutSplits();
                        break;
                    case 6:
                        ViewActivityLogs(users);
                        break;
                    case 7:
                        isAdminActive = false;
                        Console.Clear();
                        EndBanner.EndDis();
                        var table = new Table()
                            .Width(50)
                            .Border(TableBorder.DoubleEdge)
                            .BorderColor(Color.MediumSpringGreen);
                        table.AddColumn("[mediumspringgreen]Admin Logged Out[/]");
                        var paddedTable = new Padder(table).Padding(0, 10, 0, 0);


                        AnsiConsole.Write(Align.Center(paddedTable, VerticalAlignment.Middle));

                        System.Threading.Thread.Sleep(1000);

                        break;
                }
            }
        }


        private static void WriteCentered(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                Console.WriteLine();
                return;
            }
            int leftPadding = (Console.WindowWidth - text.Length) / 2;
            leftPadding = Math.Max(0, leftPadding);
            Console.SetCursorPosition(leftPadding, Console.CursorTop);
            Console.WriteLine(text);
        }

        private static void WriteCenteredPrompt(string text)
        {
            int leftPadding = (Console.WindowWidth - text.Length) / 2;
            leftPadding = Math.Max(0, leftPadding);
            Console.SetCursorPosition(leftPadding, Console.CursorTop);
            Console.Write(text);
        }

        private static void ViewAllUsers(List<UserAuth> users)
        {
            Console.Clear();
            AnsiConsole.Clear();
            Console.CursorVisible = false;
            Program.TitleBanner();


            Console.ForegroundColor = ConsoleColor.Yellow;
            WriteCentered("--- ALL USERS ---");
            Console.WriteLine();

            if (users.Count == 0)
            {
                WriteCentered("No users registered yet.");
            }
            else
            {

                var grid = new Grid();
                grid.AddColumn();
                grid.AddColumn();
                grid.Columns[1].Width = 50;
                grid.Columns[1].NoWrap = true;


                var table = new Table().Expand().Border(TableBorder.Rounded).BorderColor(Color.Yellow);
                table.AddColumn("[yellow]Username[/]");
                table.AddColumn("[yellow]Name[/]");
                table.AddColumn("[yellow]Category[/]");
                table.AddColumn("[yellow]Workouts[/]");

                foreach (var user in users.OrderBy(u => u.Username))
                {
                    table.AddRow(
                        user.Username,
                        user.Name,
                        user.Category,
                        user.Schedule.Count.ToString()
                    );
                }


                var categoryData = users
                    .GroupBy(u => u.Category)
                    .Select(g => new { Label = g.Key, Value = (double)g.Count() })
                    .ToList();


                var barChart = new BarChart()
                    .Label("[aqua bold]Users by Category[/]")
                    .CenterLabel()
                    .Width(46)
                    .AddItems(categoryData, (item) => new BarChartItem(item.Label, item.Value, Color.Yellow));

                int totalUsers = users.Count;
                int activeUsers = users.Count(u => u.Schedule.Any());
                int inactiveUsers = totalUsers - activeUsers;
                int totalWorkouts = users.Sum(u => u.Schedule.Count);


                var statsGrid = new Grid();
                statsGrid.AddColumn();
                statsGrid.AddRow($"[bold]Total Users:[/] {totalUsers}");
                statsGrid.AddRow($"[bold green]Active Users:[/] {activeUsers}");
                statsGrid.AddRow($"[bold red1]Inactive Users:[/] {inactiveUsers}");
                statsGrid.AddRow($"[bold yellow]Total Scheduled:[/] {totalWorkouts}");

                var rightSideGrid = new Grid();
                rightSideGrid.AddColumn();

                rightSideGrid.AddRow(
                   new Panel(Align.Center(barChart, VerticalAlignment.Middle))
                       .Header("BMI Bar Graph")
                       .Expand()
               );

                rightSideGrid.AddRow(
                    new Panel(Align.Center(statsGrid, VerticalAlignment.Middle))
                        .Header("Summary of Statistics")
                        .Expand()
                );

                grid.AddRow(
                    new Panel(table)
                        .Header("Users")
                        .Expand(),
                    rightSideGrid
                );

                AnsiConsole.Write(Align.Center(grid));
            }

            Console.CursorVisible = false;
            Console.WriteLine("\n");
            WriteCentered("Press any key to return to the admin menu...");
            Console.ReadKey();

        }

        private static void ViewUserDetails(List<UserAuth> users)
        {
            Console.Clear();
            AnsiConsole.Clear();
            Console.CursorVisible = false;
            Program.TitleBanner();


            AnsiConsole.Write(new Rule("[yellow]VIEW USER DETAILS[/]").RuleStyle("yellow"));
            AnsiConsole.WriteLine();

            if (users.Count == 0)
            {

                int w = AnsiConsole.Profile.Width;
                AnsiConsole.MarkupLine($"[yellow]{"No users to view.".PadLeft((w + 17) / 2)}[/]");
                Console.ReadKey();
                return;
            }


            const int colNameWidth = -20;
            const int colUserWidth = -20;
            string rowPattern = $" {{0,{colNameWidth}}} | {{1,{colUserWidth}}} ";


            string Center(string text)
            {
                int width = AnsiConsole.Profile.Width;
                int padding = Math.Max(0, (width - text.Length) / 2);
                return text.PadLeft(text.Length + padding);
            }


            string headerContent = string.Format(rowPattern, "NAME", "USERNAME");


            AnsiConsole.MarkupLine(Center($"[bold yellow]\t\t{headerContent}[/]"));



            var prompt = new SelectionPrompt<UserAuth?>()
                .Title(Center("[grey]Select a user to view details:[/]"))
                .PageSize(15)
                .HighlightStyle(new Style(foreground: Color.Black, background: Color.Yellow))
                .MoreChoicesText(Center("[grey](Scroll for more)[/]"));


            prompt.UseConverter(user =>
            {
                if (user == null) return Center("[red]Back[/]");
                string rowText = string.Format(rowPattern, user.Name, user.Username);
                return Center(rowText);
            });


            prompt.AddChoices(users.Cast<UserAuth?>());
            prompt.AddChoice(null);


            var selectedUser = AnsiConsole.Prompt(prompt);

            if (selectedUser != null)
            {
                Console.Clear();
                Program.TitleBanner();
                Console.CursorVisible = false;

                var mainGrid = new Grid();
                mainGrid.AddColumn();
                mainGrid.AddColumn();


                var profileGrid = new Grid();
                profileGrid.AddColumn(new GridColumn().NoWrap().PadRight(2));
                profileGrid.AddColumn();

                profileGrid.AddRow("[bold yellow]Username:[/]", selectedUser.Username);
                profileGrid.AddRow("[bold yellow]Name:[/]", selectedUser.Name);
                profileGrid.AddRow("[bold yellow]Age:[/]", selectedUser.Age.ToString());
                profileGrid.AddRow("[bold yellow]Gender:[/]", selectedUser.Gender);
                profileGrid.AddRow("[bold yellow]Height:[/]", $"{selectedUser.HeightCm} cm");
                profileGrid.AddRow("[bold yellow]Weight:[/]", $"{selectedUser.WeightKg} kg");
                profileGrid.AddRow("[bold yellow]BMI:[/]", $"{selectedUser.BmiScore:F1}");
                profileGrid.AddRow("[bold yellow]Category:[/]", selectedUser.Category);
                profileGrid.AddRow("[bold yellow]Intensity:[/]", selectedUser.WorkoutIntensity);

                var profilePanel = new Panel(profileGrid)
                    .Header($"[cyan]{selectedUser.Name}[/]")
                    .Border(BoxBorder.Rounded)
                    .Expand();


                var activityGrid = new Grid();
                activityGrid.AddColumn();


                int completed = selectedUser.Schedule.Count(w => w.IsCompleted);
                int incomplete = selectedUser.Schedule.Count(w => w.Date < DateTime.Now && !w.IsCompleted);
                int pending = selectedUser.Schedule.Count - (completed + incomplete);

                if (selectedUser.Schedule.Any())
                {
                    var chart = new BreakdownChart()
                        .Width(50)
                        .AddItem("Completed", completed, Color.Green)
                        .AddItem("Pending", pending, Color.Orange1)
                        .AddItem("Incomplete", incomplete, Color.Red1);

                    activityGrid.AddRow(new Panel(chart).Header("Progress").Border(BoxBorder.None));
                }
                else
                {
                    activityGrid.AddRow(new Markup("[grey]No workout history available.[/]"));
                }
                if (selectedUser.Schedule.Any())
                {
                    var scheduleTable = new Table().Border(TableBorder.Simple).Expand();
                    scheduleTable.AddColumn("[green]Date[/]");
                    scheduleTable.AddColumn("[green]Workout[/]");

                    foreach (var workout in selectedUser.Schedule.OrderBy(w => w.Date).Take(5))
                    {
                        scheduleTable.AddRow(
                            workout.Date.ToString("yyyy-MM-dd"),
                            workout.WorkoutDayName
                        );
                    }
                    activityGrid.AddRow(new Panel(scheduleTable).Header("Upcoming Workouts").Border(BoxBorder.None));
                }

                var rightPanel = new Panel(activityGrid)
                    .Header("Activity Overview")
                    .Border(BoxBorder.Rounded)
                    .Expand();

                mainGrid.AddRow(profilePanel, rightPanel);
                AnsiConsole.Write(Align.Center(mainGrid));
                Console.WriteLine();
                int footerPadding = (AnsiConsole.Profile.Width - 28) / 2;
                AnsiConsole.MarkupLine($"[grey]{"Press any key to return...".PadLeft(28 + footerPadding)}[/]");
                Console.ReadKey();
            }
        }

        private static void DeleteUserAccount(List<UserAuth> users)
        {
            Console.Clear();
            AnsiConsole.Clear();
            Console.CursorVisible = false;
            Program.TitleBanner();


            AnsiConsole.Write(new Rule("[yellow]DELETE USER ACCOUNT[/]").RuleStyle("yellow"));
            AnsiConsole.WriteLine();

            if (users.Count == 0)
            {
                AnsiConsole.MarkupLine("[centered yellow]No users to delete.[/]");
                Console.ReadKey();
                return;
            }


            var userMap = new Dictionary<string, UserAuth>();
            var prompt = new SelectionPrompt<string>()
                .Title("[yellow]Select a user to [bold red1]DELETE[/]:[/]")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more users)[/]")
                .HighlightStyle(new Style(foreground: Color.White, background: Color.Red1));


            int contentWidth = 41;
            int padding = (AnsiConsole.Profile.Width - contentWidth) / 2;
            string CenterString(string text)
            {
                int totalWidth = text.Length + Math.Max(0, padding);
                return text.PadLeft(totalWidth);
            }

            string headerText = string.Format("{0,-25} {1,-25}", "Name", "Username");
            prompt.Title += $"\n  [bold red1]{CenterString(headerText)}[/]";

            foreach (var user in users)
            {
                string rawData = $"{user.Name,-25} {user.Username,-25}";
                string display = CenterString(rawData);

                if (!userMap.ContainsKey(display))
                {
                    userMap[display] = user;
                    prompt.AddChoice(display);
                }
            }

            string backOption = CenterString("[yellow]Back[/]");
            prompt.AddChoice(backOption);

            var selection = AnsiConsole.Prompt(prompt);


            if (selection != backOption && userMap.ContainsKey(selection))
            {
                var userToDelete = userMap[selection];


                Console.Clear();
                Program.TitleBanner();

                var warningGrid = new Grid();
                warningGrid.AddColumn(new GridColumn().Centered());

                warningGrid.AddRow($"[bold red1]WARNING: About to delete user '{userToDelete.Username}'[/]");
                warningGrid.AddRow("[red1]Cannot be undone.[/]");
                warningGrid.AddRow($"[red1]({userToDelete.Name})[/]");
                warningGrid.AddRow("");

                var warningPanel = new Panel(warningGrid)
                    .Border(BoxBorder.Heavy)
                    .BorderColor(Color.Red1)
                    .Padding(2, 1, 2, 1)
                    .Expand();

                AnsiConsole.Write(new Padder(warningPanel).Padding(4, 1));


                var inputPassword = AnsiConsole.Prompt(
                    new TextPrompt<string>("Enter [bold yellow]Admin Password[/] to proceed:")
                        .Secret()
                        .PromptStyle("red1")
                );
                AnsiConsole.WriteLine();


                string correctPassword = "";
                if (File.Exists(AdminFilePath))
                {
                    try
                    {
                        string jsonString = File.ReadAllText(AdminFilePath);
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        var adminConfig = JsonSerializer.Deserialize<AdminAuth>(jsonString, options);
                        correctPassword = adminConfig.Password;
                    }
                    catch
                    {
                        AnsiConsole.MarkupLine("[bold red1]System error: Could not verify admin credentials.[/]");
                        System.Threading.Thread.Sleep(2000);
                        return;
                    }
                }

                if (inputPassword != correctPassword)
                {
                    AnsiConsole.MarkupLine("[bold red1]Incorrect Password. Deletion Cancelled.[/]");
                    System.Threading.Thread.Sleep(2000);
                    return;
                }


                var confirm = AnsiConsole.Prompt(
                    new TextPrompt<string>("Type [bold red1]DELETE[/] to confirm:")
                        .InvalidChoiceMessage("[red1]You must type DELETE to confirm[/]")
                        .ValidationErrorMessage("[red1]Invalid input[/]")
                        .AllowEmpty()
                );

                if (confirm.Equals("DELETE", StringComparison.OrdinalIgnoreCase))
                {
                    users.Remove(userToDelete);
                    Program.SaveUsers(users);

                    AnsiConsole.WriteLine();
                    var successPanel = new Panel(Align.Center(new Markup("[aqua]User account deleted successfully.[/]")))
                        .Border(BoxBorder.Square)
                        .BorderColor(Color.Aqua)
                        .Expand();
                    AnsiConsole.Write(new Padder(successPanel).Padding(4, 0));
                }
                else
                {
                    AnsiConsole.MarkupLine("\n[yellow]Deletion cancelled.[/]");
                }
            }

        }

        private static void ViewSystemStatistics(List<UserAuth> users)
        {
            Console.Clear();
            AnsiConsole.Clear();
            Console.CursorVisible = false;
            Program.TitleBanner();

            AnsiConsole.Write(new Rule("[cyan]SYSTEM STATISTICS[/]").RuleStyle("cyan"));
            AnsiConsole.WriteLine();

            var grid = new Grid();
            grid.AddColumn(new GridColumn().Width(50));
            grid.AddColumn(new GridColumn().Width(50));


            var statsTable = new Table().Border(TableBorder.Rounded).BorderColor(Color.Yellow).Expand();
            statsTable.AddColumn("[bold yellow]USERS[/]");
            statsTable.AddColumn(new TableColumn("[yellow]Value[/]").RightAligned());

            statsTable.AddRow("Total Registered Users", users.Count.ToString());

            int totalScheduled = users.Sum(u => u.Schedule.Count);
            int totalCompleted = users.Sum(u => u.Schedule.Count(w => w.IsCompleted));
            statsTable.AddRow("Total Workouts Scheduled", totalScheduled.ToString());
            statsTable.AddRow("Total Workouts Completed", totalCompleted.ToString());

            if (totalScheduled > 0)
            {
                double completionRate = (double)totalCompleted / totalScheduled * 100;
                statsTable.AddRow("Overall Completion Rate", $"{completionRate:F1}%");
            }

            var activeUsers = users.Count(u => u.Schedule.Any());
            statsTable.AddRow("Active Users", activeUsers.ToString());
            statsTable.AddRow("Inactive Users", (users.Count - activeUsers).ToString());


            var categories = users.GroupBy(u => u.Category)
                                  .Select(g => new { Category = g.Key, Count = g.Count() })
                                  .OrderByDescending(x => x.Count)
                                  .ToList();

            var catChart = new BarChart()
                .Label("[aqua]Users by Fitness Category[/]")
                .CenterLabel()
                .Width(60)
                .AddItems(categories, (item) => new BarChartItem(item.Category, item.Count, Color.LightGoldenrod2));


            grid.AddRow(
                new Panel(statsTable).Header("System Grid").Expand(),
                new Panel(Align.Center(catChart, VerticalAlignment.Middle)).Header("BMI Chart").Expand()
            );

            AnsiConsole.Write(Align.Center(grid));

            Console.WriteLine();
            WriteCentered("Press any key to return...");
            Console.ReadKey();
        }

        private static WorkoutSplit SelectSplit(string prompt)
        {
            Console.Clear();
            Console.CursorVisible = false;
            if (!Program.allWorkoutSplits.Any())
            {
                Program.TitleBanner();
                WriteCentered($"--- {prompt} ---");
                Console.WriteLine();
                WriteCentered("No workout splits available.");
                System.Threading.Thread.Sleep(1500);
                return null;
            }

            var splitNames = Program.allWorkoutSplits.Select(s => s.WorkoutName).ToList();
            splitNames.Add("Cancel");

            int selectedIndex = Program.intro(splitNames.ToArray());

            if (selectedIndex < 0 || selectedIndex == splitNames.Count - 1)
            {
                return null;
            }
            return Program.allWorkoutSplits[selectedIndex];
        }
        private static void SaveAllSplits()
        {
            Console.CursorVisible = false;
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true, PropertyNameCaseInsensitive = true };
                string jsonString = JsonSerializer.Serialize(Program.allWorkoutSplits, options);
                File.WriteAllText("allSPLITS.json", jsonString);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                WriteCentered($"Error saving splits: {ex.Message}");
                Console.ResetColor();
                WriteCentered("Press any key to continue...");
                Console.ReadKey();
            }
        }
        private static Exercise SelectExercise(string prompt)
        {
            Console.CursorVisible = false;
            string term = "";

            List<Exercise> sortedResults = Program.allExercises.OrderBy(e => e.ExName).ToList();
            bool selectionMade = false;

            while (!selectionMade)
            {
                Console.Clear();
                Program.TitleBanner();

                AnsiConsole.Write(new Rule($"[yellow]{prompt}[/]").RuleStyle("yellow"));
                AnsiConsole.Write(Align.Left(new Markup($"\t\t\t\t   Search: [white]{term}[/][blink dim]_[/]")));
                AnsiConsole.WriteLine();

                if (!string.IsNullOrEmpty(term))
                {
                    string pattern = Regex.Escape(term);
                    Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                    var results = Program.allExercises.Where(e =>
                        regex.IsMatch(e.ExName) ||
                        regex.IsMatch(e.TargetBodyPart) ||
                        regex.IsMatch(e.ExerciseType)
                    ).ToList();
                    sortedResults = results.OrderBy(e => e.TargetBodyPart).ThenBy(e => e.ExName).ToList();
                }
                else
                {
                    sortedResults = Program.allExercises.OrderBy(e => e.ExName).ToList();
                }

                if (!sortedResults.Any())
                {
                    AnsiConsole.Write(Align.Center(new Markup("[red1]No exercises found.[/]")));
                    AnsiConsole.WriteLine();
                }
                else
                {
                    var table = new Table().Centered().Border(TableBorder.Rounded).Width(120);
                    table.AddColumn("[yellow]Exercise Name[/]");
                    table.AddColumn("[yellow]Target Body Part[/]");

                    foreach (var e in sortedResults.Take(10))
                    {
                        table.AddRow(e.ExName, e.TargetBodyPart);
                    }
                    if (sortedResults.Count > 10)
                    {
                        table.Caption($"[grey]...and {sortedResults.Count - 10} more.[/]");
                    }
                    AnsiConsole.Write(table);
                }

                AnsiConsole.Write(Align.Center(new Markup("\n[grey](Press [yellow]Enter[/] to select from results, [yellow]Esc[/] to cancel)[/]")));
                AnsiConsole.WriteLine();

                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        if (sortedResults.Any()) selectionMade = true;
                        break;
                    case ConsoleKey.Escape:
                        return null;
                    case ConsoleKey.Backspace:
                        if (term.Length > 0) term = term.Substring(0, term.Length - 1);
                        break;
                    default:
                        if (!char.IsControl(key.KeyChar)) term += key.KeyChar;
                        break;
                }
            }
            AnsiConsole.Clear();
            Program.TitleBanner();
            if (!sortedResults.Any()) return null;

            int maxNameWidth = sortedResults.Max(e => e.ExName.Length) + 2;
            string indent = new string(' ', 120);

            return AnsiConsole.Prompt(
                new SelectionPrompt<Exercise>()
                    .Title($"{indent}[yellow]{prompt}[/] ({sortedResults.Count} results)\n" +
                           $"{indent}[grey]{"Exercise Name".PadRight(maxNameWidth)}Target Body Part[/]")
                    .PageSize(15)
                    .MoreChoicesText($"{indent}[grey](Move up/down for more)[/]")
                    .AddChoices(sortedResults)
                    .UseConverter(e => $"{indent}{e.ExName.PadRight(maxNameWidth)}[cyan]{e.TargetBodyPart}[/]")
                    .HighlightStyle(new Style(foreground: Color.Black, background: Color.Yellow))
            );
        }


        private static void AddNewSplit()
        {
            Console.Clear();
            Program.TitleBanner();
            Console.CursorVisible = false;
            WriteCentered("--- ADD NEW SPLIT ---");
            Console.WriteLine();

            WriteCenteredPrompt("Enter new workout name: ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                WriteCentered("Name cannot be empty. Cancelled.");
                System.Threading.Thread.Sleep(1500);
                return;
            }
            if (Program.allWorkoutSplits.Any(s => s.WorkoutName.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                WriteCentered("A split with this name already exists. Cancelled.");
                System.Threading.Thread.Sleep(1500);
                return;
            }
            Console.WriteLine();
            WriteCenteredPrompt("Enter workout description: ");
            string description = Console.ReadLine();

            var newSplit = new WorkoutSplit
            {
                WorkoutName = name,
                WorkoutDescription = description,
                Days = new List<WorkoutDay>()
            };

            Program.allWorkoutSplits.Add(newSplit);
            ManageSplitDays(newSplit, true);
            SaveAllSplits();

            Console.ForegroundColor = ConsoleColor.Green;
            WriteCentered("New split added and saved!");
            Console.ResetColor();
            System.Threading.Thread.Sleep(1500);
        }

        private static void EditSplit()
        {
            Console.CursorVisible = false;
            WorkoutSplit splitToEdit = SelectSplit("Select Split to Edit");
            if (splitToEdit == null) return;

            bool isEditingSplit = true;
            while (isEditingSplit)
            {
                var options = new[] {
                    $"Edit Name: {splitToEdit.WorkoutName}",
                    $"Edit Description: {splitToEdit.WorkoutDescription}",
                    $"Manage Days ({splitToEdit.Days.Count})",
                    "Save and Back"
                };
                int choice = Program.intro(options);

                switch (choice)
                {
                    case 0:
                        Console.Clear();
                        Program.TitleBanner();
                        WriteCentered("--- EDIT SPLIT NAME ---");
                        Console.WriteLine();
                        WriteCenteredPrompt("Enter new name: ");
                        string newName = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newName) && !Program.allWorkoutSplits.Any(s => s.WorkoutName.Equals(newName, StringComparison.OrdinalIgnoreCase)))
                        {
                            splitToEdit.WorkoutName = newName;
                        }
                        else
                        {
                            WriteCentered("Name is invalid or already taken.");
                            System.Threading.Thread.Sleep(1500);
                        }
                        break;
                    case 1:
                        Console.Clear();
                        Program.TitleBanner();
                        WriteCentered("--- EDIT SPLIT DESCRIPTION ---");
                        Console.WriteLine();
                        WriteCenteredPrompt("Enter new description: ");
                        splitToEdit.WorkoutDescription = Console.ReadLine();
                        break;
                    case 2:
                        ManageSplitDays(splitToEdit, false);
                        break;
                    case 3:
                        SaveAllSplits();
                        WriteCentered("Changes saved.");
                        System.Threading.Thread.Sleep(1000);
                        isEditingSplit = false;
                        break;
                }
            }
        }

        private static void DeleteSplit()
        {
            Console.CursorVisible = false;
            WorkoutSplit splitToDelete = SelectSplit("Select Split to Delete");
            if (splitToDelete == null) return;
            Console.CursorVisible = false;
            Console.Clear();
            Program.TitleBanner();
            Console.ForegroundColor = ConsoleColor.Red;
            WriteCentered($"WARNING: About to delete split '{splitToDelete.WorkoutName}'");
            WriteCentered("This action cannot be undone!");
            Console.WriteLine();
            WriteCenteredPrompt("Type 'DELETE' to confirm: ");
            Console.ResetColor();

            string confirmation = Console.ReadLine();
            Console.WriteLine();

            if (confirmation == "DELETE")
            {
                Program.allWorkoutSplits.Remove(splitToDelete);
                SaveAllSplits();
                Console.ForegroundColor = ConsoleColor.Green;
                WriteCentered("Split deleted successfully.");
                Console.ResetColor();
            }
            else
            {
                WriteCentered("Deletion cancelled.");
            }
            System.Threading.Thread.Sleep(1500);
        }

        private static void ManageSplitDays(WorkoutSplit split, bool isNewSplit)
        {
            bool isManagingDays = true;
            while (isManagingDays)
            {
                Console.Clear();
                Console.WriteLine($"--- MANAGING DAYS for {split.WorkoutName} ---");
                Console.WriteLine("Current Days:");
                if (!split.Days.Any())
                {
                    Console.WriteLine("  (No days added yet)");
                }
                for (int i = 0; i < split.Days.Count; i++)
                {
                    Console.WriteLine($"  {i + 1}. {split.Days[i].DayName} ({split.Days[i].Exercises.Count} exercises)");
                }

                var options = new List<string> { "Add New Day", "Edit Day's Exercises", "Delete Day" };
                options.Add(isNewSplit ? "Finish Adding Days" : "Back to Split Editor");

                int choice = Program.intro(options.ToArray());

                switch (choice)
                {
                    case 0:
                        Console.Clear();
                        Program.TitleBanner();
                        WriteCentered("--- ADD NEW DAY ---");
                        Console.WriteLine();
                        WriteCenteredPrompt("Enter new day name (e.g., 'Day 1: Chest' or 'Rest Day'): ");
                        string dayName = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(dayName))
                        {
                            var newDay = new WorkoutDay { DayName = dayName, Exercises = new List<PrescribedExercise>() };
                            split.Days.Add(newDay);
                            WriteCentered("Day added. Now add exercises for this day.");
                            System.Threading.Thread.Sleep(1000);
                            ManageDayExercises(newDay);
                        }
                        break;
                    case 1:
                        if (!split.Days.Any())
                        {
                            WriteCentered("No days to edit.");
                            System.Threading.Thread.Sleep(1000);
                            break;
                        }
                        var dayOptions = split.Days.Select(d => d.DayName).ToList();
                        dayOptions.Add("Cancel");
                        int dayIndex = Program.intro(dayOptions.ToArray());
                        if (dayIndex < split.Days.Count)
                        {
                            ManageDayExercises(split.Days[dayIndex]);
                        }
                        break;
                    case 2:
                        if (!split.Days.Any())
                        {
                            WriteCentered("No days to delete.");
                            System.Threading.Thread.Sleep(1000);
                            break;
                        }
                        var dayOptionsDel = split.Days.Select(d => d.DayName).ToList();
                        dayOptionsDel.Add("Cancel");
                        int dayIndexDel = Program.intro(dayOptionsDel.ToArray());
                        if (dayIndexDel < split.Days.Count)
                        {
                            split.Days.RemoveAt(dayIndexDel);
                            WriteCentered("Day deleted.");
                            System.Threading.Thread.Sleep(1000);
                        }
                        break;
                    case 3:
                        isManagingDays = false;
                        break;
                }
            }
        }

        private static void ManageDayExercises(WorkoutDay day)
        {
            Console.CursorVisible = false;
            bool isManagingExercises = true;
            while (isManagingExercises)
            {
                Console.Clear();
                Console.WriteLine($"--- MANAGING EXERCISES for {day.DayName} ---");
                Console.WriteLine("Current Exercises:");
                if (!day.Exercises.Any())
                {
                    Console.WriteLine("  (No exercises added yet)");
                }
                for (int i = 0; i < day.Exercises.Count; i++)
                {
                    var ex = day.Exercises[i];
                    Console.WriteLine($"  {i + 1}. {ex.Exercise} ({ex.Sets} sets of {ex.Reps})");
                }

                var options = new[] { "Add Exercise", "Delete Exercise", "Back to Day List" };
                int choice = Program.intro(options);

                switch (choice)
                {
                    case 0:
                        Exercise exToAdd = SelectExercise("Select Exercise to Add");
                        if (exToAdd != null)
                        {
                            Console.Clear();
                            Program.TitleBanner();
                            WriteCentered($"--- ADDING {exToAdd.ExName} ---");
                            Console.WriteLine();
                            WriteCenteredPrompt("Enter sets: ");
                            int sets = 0;
                            while (!int.TryParse(Console.ReadLine(), out sets) || sets <= 0)
                            {
                                WriteCenteredPrompt("Please enter a valid positive number for sets: ");
                            }
                            Console.WriteLine();
                            WriteCenteredPrompt("Enter reps (e.g., '8-12' or '5'): ");
                            string reps = Console.ReadLine();

                            day.Exercises.Add(new PrescribedExercise
                            {
                                Exercise = exToAdd.ExName,
                                Sets = sets,
                                Reps = reps
                            });
                            WriteCentered("Exercise added.");
                            System.Threading.Thread.Sleep(1000);
                        }
                        break;
                    case 1:
                        if (!day.Exercises.Any())
                        {
                            WriteCentered("No exercises to delete.");
                            System.Threading.Thread.Sleep(1000);
                            break;
                        }
                        var exOptions = day.Exercises.Select(e => $"{e.Exercise} ({e.Sets}x{e.Reps})").ToList();
                        exOptions.Add("Cancel");
                        int exIndex = Program.intro(exOptions.ToArray());
                        if (exIndex < day.Exercises.Count)
                        {
                            day.Exercises.RemoveAt(exIndex);
                            WriteCentered("Exercise removed.");
                            System.Threading.Thread.Sleep(1000);
                        }
                        break;
                    case 2:
                        isManagingExercises = false;
                        break;
                }
            }
        }

        private static void ViewAllSplits()
        {
            while (true)
            {
                Console.Clear();
                AnsiConsole.Clear();
                Program.TitleBanner(); 
                Console.CursorVisible = false;
                if (!Program.allWorkoutSplits.Any())
                {
                    Console.WriteLine("No workoutsplits found");
                    Console.ReadKey();
                    return;
                }
                List<string> menuOptions = Program.allWorkoutSplits
                    .Select(s => $"{s.WorkoutName} ({s.Days.Count} Days)")
                    .ToList();

                menuOptions.Add("Back");

                string subHeader = "--- SELECT A SPLIT TO VIEW WORKOUT SCHEDULES ---";
                int headerX = (Console.WindowWidth - subHeader.Length) / 2;
                Console.SetCursorPosition(headerX, Console.CursorTop + 1);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(subHeader);
                Console.ResetColor();
                int startY = Console.CursorTop + 2;
                int selectedIndex = selects(menuOptions.ToArray(), startY);

                if (selectedIndex == -1 || selectedIndex == menuOptions.Count - 1)
                {
                    return;
                }

                var selectedSplit = Program.allWorkoutSplits[selectedIndex];

                Console.Clear();
                AnsiConsole.Clear();
                Program.TitleBanner();

                var mainGrid = new Grid();
                mainGrid.AddColumn(new GridColumn().Centered());

                var descPanel = new Panel(new Markup($"[grey]{selectedSplit.WorkoutDescription}[/]"))
                    .Header($"[bold yellow]{selectedSplit.WorkoutName}[/]")
                    .Border(BoxBorder.Rounded)
                    .BorderColor(Color.Yellow);
                descPanel.Width = 80;

                var table = new Table()
                    .Border(TableBorder.Simple)
                    .BorderColor(Color.DarkSlateGray1)
                    .Width(80);

                table.AddColumn(new TableColumn("[lightgreen]Day[/]").NoWrap());
                table.AddColumn("[cyan]Exercise[/]");
                table.AddColumn(new TableColumn("[yellow]Sets[/]").Centered());
                table.AddColumn(new TableColumn("[yellow]Reps[/]").Centered());

                foreach (var day in selectedSplit.Days)
                {
                    if (day.Exercises.Any())
                    {
                        bool isFirst = true;
                        foreach (var ex in day.Exercises)
                        {
                            table.AddRow(
                                isFirst ? $"[bold white]{day.DayName}[/]" : "",
                                ex.Exercise,
                                ex.Sets.ToString(),
                                ex.Reps
                            );
                            isFirst = false;
                        }
                    }
                    else
                    {
                        table.AddRow($"[bold white]{day.DayName}[/]", "[grey]Rest / Active Recovery[/]", "-", "-");
                    }
                    table.AddEmptyRow();
                }

                var contentPanel = new Panel(table)
                    .Header("Workout Schedule")
                    .Border(BoxBorder.Rounded)
                    .BorderColor(Color.Cyan1);
                contentPanel.Width = 80;

                mainGrid.AddRow(descPanel);
                mainGrid.AddRow(contentPanel);
                AnsiConsole.Write(Align.Center(mainGrid));

                Console.WriteLine();
                string footer = "Press any key to return to the list...";
                int footerPad = (Console.WindowWidth - footer.Length) / 2;
                Console.WriteLine(new string(' ', Math.Max(0, footerPad)) + footer);
                Console.ReadKey();
            }
        }
        private static void ManageWorkoutSplits()
        {
            Console.CursorVisible = false;
            string[] splitOptions =
            {
                "View All Splits",
                "Add New Split",
                "Edit Split",
                "Delete Split",
                "Back"
            };

            bool isManagingSplits = true;
            while (isManagingSplits)
            {
                int choice = Program.intro(splitOptions);

                switch (choice)
                {
                    case 0:
                        ViewAllSplits();
                        break;
                    case 1:
                        AddNewSplit();
                        break;
                    case 2:
                        EditSplit();
                        break;
                    case 3:
                        DeleteSplit();
                        break;
                    case 4:
                        isManagingSplits = false;
                        break;
                }
            }
        }

        private static void ViewActivityLogs(List<UserAuth> users)
        {
            Console.Clear();
            AnsiConsole.Clear();
            Console.CursorVisible = false;
            Program.TitleBanner();


            AnsiConsole.Write(new Rule("[cyan]ACTIVITY LOGS[/]").RuleStyle("cyan"));
            AnsiConsole.WriteLine();

            if (users.Count == 0)
            {
                AnsiConsole.MarkupLine("[centered yellow]No user activity recorded.[/]");
                Console.ReadKey();
                return;
            }


            var mainGrid = new Grid();
            mainGrid.AddColumn();
            mainGrid.AddColumn();
            mainGrid.Columns[1].Width = 45;
            mainGrid.Columns[1].NoWrap = true;


            var sortedUsers = users
                .OrderByDescending(u => u.Schedule.Count(w => w.IsCompleted))
                .ThenByDescending(u => u.Schedule.OrderByDescending(w => w.Date).FirstOrDefault()?.Date)
                .ToList();

            var table = new Table().Expand().Border(TableBorder.Rounded).BorderColor(Color.Cyan);
            table.AddColumn("[yellow]Username[/]");
            table.AddColumn("[yellow]Last Action[/]");
            table.AddColumn(new TableColumn("[cyan]Completed[/]").Centered());
            table.AddColumn(new TableColumn("[lightgreen]Status[/]").Centered());

            foreach (var user in sortedUsers)
            {
                var lastWorkout = user.Schedule.OrderByDescending(w => w.Date).FirstOrDefault();
                string lastDate = lastWorkout?.Date.ToString("yyyy-MM-dd") ?? "[honeydew2]N/A[/]";

                int completedCount = user.Schedule.Count(w => w.IsCompleted);
                string completedDisplay = completedCount > 0 ? $"[lightgreen]{completedCount}[/]" : "[grey]0[/]";


                bool isActive = lastWorkout != null && lastWorkout.Date >= DateTime.Now.AddDays(-7);
                string status = isActive ? "[lightgreen bold]Active[/]" : "[honeydew2]Idle[/]";

                table.AddRow(
                    $"[bold white]{user.Username}[/]",
                    lastDate,
                    completedDisplay,
                    status
                );
            }


            int totcomp = users.Sum(u => u.Schedule.Count(w => w.IsCompleted));
            int totincomp = users.Sum(u => u.Schedule.Count(w => w.Date < DateTime.Now && !w.IsCompleted));
            int totpending = users.Sum(u => u.Schedule.Count(w => !w.IsCompleted));

            var topUser = sortedUsers.FirstOrDefault();
            string topPerformer = (topUser != null && totcomp > 0) ? topUser.Username : "None";


            var progressChart = new BreakdownChart()
                .Width(40)
                .AddItem("Done", totcomp, Color.LightGreen)
                .AddItem("Pending", totpending, Color.OrangeRed1)
                .AddItem("Incomplete", totincomp, Color.Red1)
                .Compact();

            var statsTextGrid = new Grid();
            statsTextGrid.AddColumn();
            statsTextGrid.AddRow($"[bold]Total Workouts Logged:[/] {totcomp + totpending}");
            statsTextGrid.AddRow($"[bold lightgreen]Total Completed:[/] {totcomp}");
            statsTextGrid.AddRow($"[bold gold1]Total Pending:[/] {totpending}");
            statsTextGrid.AddRow($"[bold red1]Total Incomplete:[/] {totincomp}");
            statsTextGrid.AddRow("");
            statsTextGrid.AddRow($"[bold cyan]Top Performer:[/] {topPerformer}");


            var rightSideGrid = new Grid();
            rightSideGrid.AddColumn();

            rightSideGrid.AddRow(
                new Panel(Align.Center(progressChart, VerticalAlignment.Middle))
                    .Header("Global Completion")
                    .Expand()
            );

            rightSideGrid.AddRow(
                new Panel(Align.Center(statsTextGrid, VerticalAlignment.Middle))
                    .Header("Highlights")
                    .Expand()
            );


            mainGrid.AddRow(
                new Panel(table)
                    .Header("User Logs")
                    .Expand(),
                rightSideGrid
            );


            AnsiConsole.Write(Align.Center(mainGrid));

            Console.WriteLine("\n");
            int footerPadding = (Console.WindowWidth - 30) / 2;
            AnsiConsole.MarkupLine($"[grey]{"Press any key to return...".PadLeft(30 + footerPadding)}[/]");
            Console.ReadKey();
        }
        private static int selects(string[] options, int startTop)
        {
            Console.CursorVisible = false;
            int selectedIndex = 0;
            ConsoleKey key;

            do
            {
                for (int i = 0; i < options.Length; i++)
                {
                    string currentOption = options[i];

                    int left = (Console.WindowWidth - currentOption.Length) / 2;

                    Console.SetCursorPosition(left, startTop + i);

                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Yellow;
                    }
                    else
                    {
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.BackgroundColor = ConsoleColor.Black;
                    }

                    Console.WriteLine(currentOption);
                }
                Console.ResetColor();

                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = (selectedIndex == 0) ? options.Length - 1 : selectedIndex - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = (selectedIndex == options.Length - 1) ? 0 : selectedIndex + 1;
                        break;
                    case ConsoleKey.Escape:
                        Console.CursorVisible = true;
                        return -1;
                }

            } while (key != ConsoleKey.Enter);

            Console.CursorVisible = true;
            return selectedIndex;
        }
        private static void ManageExercise()
        {
            string[] options = 
            {
                "Search Exercises",
                "Add New Exercise",
                "Delete Exercise",
                "Back to Admin Menu"
            };

            bool isManaging = true;
            while (isManaging)
            {
                Console.Clear();
                Program.TitleBanner();
                string subHeader = "--- MANAGE EXERCISES ---";
                int headerX = (Console.WindowWidth - subHeader.Length) / 2;

                Console.SetCursorPosition(headerX, Console.CursorTop + 1);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(subHeader);
                Console.ResetColor();
                
                int startY = Console.CursorTop + 2;
                int selectedIndex = selects(options, startY);

                switch (selectedIndex)
                {
                    case 0:
                        AdSearchExercises();
                        break;
                    case 1:
                        AdNewExercise();
                        break;
                    case 2:
                        DelExercise();
                        break;
                    case 3:
                        isManaging = false;
                        break;
                    case -1: 
                        isManaging = false;
                        break;
                }
            }
        }
        private static void AdSearchExercises()
        {
            Console.CursorVisible = false;
            string term = "";


            List<Exercise> sortedResults = Program.allExercises
                .OrderBy(e => e.ExName)
                .ToList();

            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Program.TitleBanner();


                AnsiConsole.Write(
                Align.Left
                (
                    new Markup($"\t\t\t\t   Search: [white]{term}[/][blink dim]_[/]")
                ));
                AnsiConsole.WriteLine();


                if (!string.IsNullOrEmpty(term))
                {

                    string pattern = Regex.Escape(term);
                    Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

                    var results = Program.allExercises.Where(e =>
                        regex.IsMatch(e.ExName) ||
                        regex.IsMatch(e.TargetBodyPart) ||
                        regex.IsMatch(e.ExerciseType)
                    ).ToList();

                    sortedResults = results
                        .OrderBy(e => e.TargetBodyPart)
                        .ThenBy(e => e.ExName)
                        .ToList();
                }
                else
                {
                    sortedResults = Program.allExercises.OrderBy(e => e.ExName).ToList();
                }

                if (!sortedResults.Any())
                {
                    AnsiConsole.Write(
                    Align.Center
                    (
                    new Markup("[red1]No exercises found.[/]")
                    ));
                    AnsiConsole.WriteLine();
                }
                else
                {
                    var table = new Table()
                        .Centered()
                        .Border(TableBorder.Rounded)
                        .Width(120);

                    table.AddColumn("[yellow]Exercise Name[/]");
                    table.AddColumn("[yellow]Target Body Part[/]");


                    foreach (var e in sortedResults.Take(10))
                    {
                        table.AddRow(e.ExName, e.TargetBodyPart);
                    }

                    if (sortedResults.Count > 10)
                    {
                        table.Caption($"[grey]...and {sortedResults.Count - 10} more.[/]");
                    }
                    AnsiConsole.Write(table);
                }
                AnsiConsole.Write(
                Align.Center
                (
                   new Markup("\n[grey](Press [yellow]Enter[/] to select from results, [yellow]Esc[/] to go back)[/]")
                  ));
                AnsiConsole.WriteLine();


                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.Enter:

                        if (sortedResults.Any())
                        {
                            exit = true;
                        }
                        break;
                    case ConsoleKey.Escape:
                        return;
                    case ConsoleKey.Backspace:
                        if (term.Length > 0)
                            term = term.Substring(0, term.Length - 1);
                        break;
                    default:
                        if (!char.IsControl(key.KeyChar))
                            term += key.KeyChar;
                        break;
                }
            }


            AnsiConsole.Clear();
            Program.TitleBanner();


            if (!sortedResults.Any()) return;


            int maxNameWidth = sortedResults.Max(e => e.ExName.Length) + 2;


            var selectedExercise = AnsiConsole.Prompt(
                new SelectionPrompt<Exercise>()
                    .Title(
                        $"[yellow]SELECT EXERCISE[/] ({sortedResults.Count} found)\n" +
                        $"[grey]{"Exercise Name".PadRight(maxNameWidth)}Target Body Part[/]"
                     )
                    .PageSize(20)
                    .MoreChoicesText("[grey](Move up/down for more)[/]")
                    .AddChoices(sortedResults)
                    .UseConverter(e =>
                    {

                        string nameCol = e.ExName.PadRight(maxNameWidth);
                        return $"{nameCol}[cyan]{e.TargetBodyPart}[/]";
                    })
                    .HighlightStyle(new Style(foreground: Color.Black, background: Color.Yellow))
            );


            AnsiConsole.Clear();
            Program.TitleBanner();

            AnsiConsole.Write(new Rule($"[yellow]{selectedExercise.ExName}[/]")
                .LeftJustified());
            Console.ResetColor();
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Panel(selectedExercise.ExDescription)
                .Header("Description")
                .Border(BoxBorder.Square)
                .Expand());


            AnsiConsole.Write(
            Align.Center
            (
                new Markup("[grey]Press any key to continue...[/]")
            ));
            AnsiConsole.WriteLine();
            Console.ReadKey(true);
        }
        private static void AdNewExercise()
        {
           
            Console.CursorVisible = false;
            string term = "";
            bool nameConfirmed = false;

            while (!nameConfirmed)
            {
                Console.Clear();
                Program.TitleBanner(); 
                AnsiConsole.Write(new Rule("[cyan]ADD NEW EXERCISE[/]").RuleStyle("cyan"));

                
                AnsiConsole.Write(Align.Left(new Markup($"\t\t\t\t   Enter Name: [white]{term}[/][blink dim]_[/]")));
                AnsiConsole.WriteLine();

                
                var similarExercises = new List<Exercise>();

                if (!string.IsNullOrWhiteSpace(term))
                {
                    string pattern = Regex.Escape(term);
                    Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

                    similarExercises = Program.allExercises.Where(e =>
                        regex.IsMatch(e.ExName) ||
                        regex.IsMatch(e.TargetBodyPart)
                    ).Take(8).ToList(); 
                }

                if (similarExercises.Any())
                {
                    AnsiConsole.WriteLine();
                    AnsiConsole.Write(Align.Center(new Markup($"[cyan]Found {similarExercises.Count} similar match(es):[/]")));

                    var table = new Table().Centered().Border(TableBorder.Rounded).BorderColor(Color.Cyan).Width(120);
                    table.AddColumn("[yellow]Existing Name[/]");
                    table.AddColumn("[yellow]Body Part[/]");
                    table.AddColumn("[yellow]Type[/]");

                    foreach (var match in similarExercises)
                    {
                        table.AddRow(match.ExName, match.TargetBodyPart, match.ExerciseType);
                    }
                    AnsiConsole.Write(table);
                }
                else if (!string.IsNullOrWhiteSpace(term))
                {
                    AnsiConsole.WriteLine();
                    AnsiConsole.Write(Align.Center(new Markup("[bold cyan]You may add.[/]")));
                }

                
                AnsiConsole.WriteLine();
                AnsiConsole.Write(Align.Center(new Markup("[grey](Type name. Press [yellow]Enter[/] to proceed, [yellow]Esc[/] to cancel)[/]")));

                
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        if (!string.IsNullOrWhiteSpace(term)) nameConfirmed = true;
                        break;
                    case ConsoleKey.Escape:
                        return; // Exit method
                    case ConsoleKey.Backspace:
                        if (term.Length > 0) term = term.Substring(0, term.Length - 1);
                        break;
                    default:
                        if (!char.IsControl(key.KeyChar)) term += key.KeyChar;
                        break;
                }
            }

            string exName = term.Trim();

            if (Program.allExercises.Any(e => e.ExName.Contains(exName, StringComparison.OrdinalIgnoreCase)))
            {
                if (!AnsiConsole.Confirm($"Similar exercises exist. Do you really want to create '[bold cyan]{exName}[/]'?"))
                {
                    return;
                }
            }

            Console.Clear();
            Program.TitleBanner();
            AnsiConsole.Write(new Rule($"[lime]ADDING: {exName.ToUpper()}[/]").RuleStyle("lime"));
            Console.WriteLine();

            var bodyPart = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select [green]Target Body Part[/] (or Cancel):")
                    .PageSize(10)
                    .AddChoices("Arms", "Back", "Chest", "Core", "Legs")
                    
            );

            

            string targetFile = bodyPart.ToLower() + ".json";

            string exDesc = AnsiConsole.Ask<string>("Enter [green]Description[/]:");

            string exType = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select [lime]Exercise Type[/]:")
                    .AddChoices("Strength", "Hypertrophy", "Cardio", "Flexibility")
            );

            var newExercise = new Exercise
            {
                ExName = exName,
                ExDescription = exDesc,
                TargetBodyPart = bodyPart,
                ExerciseType = exType
            };

            try
            {
                // Update Memory
                Program.allExercises.Add(newExercise); //

                // Update File
                List<Exercise> specificList = Program.LoadJsonData<Exercise>(targetFile); //
                specificList.Add(newExercise);

                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(specificList, options);
                File.WriteAllText(targetFile, jsonString);

                AnsiConsole.MarkupLine($"\n[green]Success![/] Added [bold white]{exName}[/] to [cyan]{targetFile}[/].");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error saving exercise: {ex.Message}[/]");
            }

            System.Threading.Thread.Sleep(2000);
        }
        private static void DelExercise()
        {
           
            Console.CursorVisible = false;
            string term = "";

            List<Exercise> sortedResults = Program.allExercises.OrderBy(e => e.ExName).ToList();
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                AnsiConsole.Clear();
                Program.TitleBanner();
                AnsiConsole.Write(new Rule("[red1]DELETE EXERCISE[/]").RuleStyle("red1"));

                AnsiConsole.Write(Align.Left(new Markup($"\t\t\t\t   Search to Delete: [red1]{term}[/][blink dim]_[/]")));
                AnsiConsole.WriteLine();

                if (!string.IsNullOrEmpty(term))
                {
                    string pattern = Regex.Escape(term);
                    Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

                    var results = Program.allExercises.Where(e =>
                        regex.IsMatch(e.ExName) ||
                        regex.IsMatch(e.TargetBodyPart) ||
                        regex.IsMatch(e.ExerciseType)
                    ).ToList();

                    sortedResults = results.OrderBy(e => e.TargetBodyPart).ThenBy(e => e.ExName).ToList();
                }
                else
                {
                    sortedResults = Program.allExercises.OrderBy(e => e.ExName).ToList();
                }

                if (!sortedResults.Any())
                {
                    AnsiConsole.Write(Align.Center(new Markup("[red1]No exercises found.[/]")));
                    AnsiConsole.WriteLine();
                }
                else
                {
                    var table = new Table().Centered().Border(TableBorder.Rounded).Width(120).BorderColor(Color.Red1);
                    table.AddColumn("[red1]Exercise Name[/]");
                    table.AddColumn("[red1]Target Body Part[/]");

                    foreach (var e in sortedResults.Take(10))
                    {
                        table.AddRow(e.ExName, e.TargetBodyPart);
                    }

                    if (sortedResults.Count > 10)
                    {
                        table.Caption($"[grey]...and {sortedResults.Count - 10} more.[/]");
                    }
                    AnsiConsole.Write(table);
                }
                AnsiConsole.Write(Align.Center(new Markup("\n[grey](Press [yellow]Enter[/] to select for deletion, [yellow]Esc[/] to go back)[/]")));
                AnsiConsole.WriteLine();

                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        if (sortedResults.Any()) exit = true;
                        break;
                    case ConsoleKey.Escape:
                        return;
                    case ConsoleKey.Backspace:
                        if (term.Length > 0) term = term.Substring(0, term.Length - 1);
                        break;
                    default:
                        if (!char.IsControl(key.KeyChar)) term += key.KeyChar;
                        break;
                }
            }

            AnsiConsole.Clear();
            Program.TitleBanner();

            if (!sortedResults.Any()) return;

            int maxNameWidth = sortedResults.Max(e => e.ExName.Length) + 2;

            var selectedExercise = AnsiConsole.Prompt(
                new SelectionPrompt<Exercise>()
                    .Title($"[red]SELECT EXERCISE TO DELETE[/] ({sortedResults.Count} found)\n" +
                           $"[grey]{"Exercise Name".PadRight(maxNameWidth)}Target Body Part[/]")
                    .PageSize(20)
                    .MoreChoicesText("[grey](Move up/down for more)[/]")
                    .AddChoices(sortedResults)
                    .UseConverter(e =>
                    {
                        string nameCol = e.ExName.PadRight(maxNameWidth);
                        return $"{nameCol}[cyan]{e.TargetBodyPart}[/]";
                    })
                    .HighlightStyle(new Style(foreground: Color.White, background: Color.Red)) 
            );


            
            AnsiConsole.Clear();
            Program.TitleBanner();
            AnsiConsole.Write(new Rule("[red bold]CONFIRM DELETION[/]").RuleStyle("red"));
            AnsiConsole.WriteLine();

            var warningGrid = new Grid();
            warningGrid.AddColumn(new GridColumn().Centered());
            warningGrid.AddRow($"[bold white]You are about to delete:[/]");
            warningGrid.AddRow($"[bold yellow]{selectedExercise.ExName}[/] ([grey]{selectedExercise.TargetBodyPart}[/])");
            warningGrid.AddRow("");
            warningGrid.AddRow("[red]This action will remove it from the database permanently.[/]");

            var panel = new Panel(warningGrid)
                .Border(BoxBorder.Rounded)
                .BorderColor(Color.Red);
            AnsiConsole.Write(Align.Center(panel));
            AnsiConsole.WriteLine();

            
            var passwordInput = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter [bold red]Admin Password[/] to confirm:")
                    .Secret()
                    .PromptStyle("red")
            );

           
            bool isAuthenticated = false;
            try
            {
                if (File.Exists("admin.json"))
                {
                    string jsonString = File.ReadAllText("admin.json");
                    var adminConfig = JsonSerializer.Deserialize<AdminAuth>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (adminConfig != null && passwordInput == adminConfig.Password)
                    {
                        isAuthenticated = true;
                    }
                }
            }
            catch
            {
               new Exception("Error reading admin configuration.");
            }

            if (!isAuthenticated)
            {
                AnsiConsole.MarkupLine("\n[bold red]Incorrect Password. Deletion cancelled.[/]");
                System.Threading.Thread.Sleep(2000);
                return;
            }

            AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .Start("Deleting exercise...", ctx =>
                {
                    try
                    {
                        
                        Program.allExercises.Remove(selectedExercise);

                        
                        string targetFile = selectedExercise.TargetBodyPart.ToLower() + ".json"; 

                        if (File.Exists(targetFile))
                        {
                            
                            List<Exercise> fileList = Program.LoadJsonData<Exercise>(targetFile);

                            var itemInFile = fileList.FirstOrDefault(e => e.ExName.Equals(selectedExercise.ExName, StringComparison.OrdinalIgnoreCase));
                            if (itemInFile != null)
                            {
                                fileList.Remove(itemInFile);

                                var options = new JsonSerializerOptions { WriteIndented = true };
                                string newJson = JsonSerializer.Serialize(fileList, options);
                                File.WriteAllText(targetFile, newJson);
                            }
                        }

                        System.Threading.Thread.Sleep(1000);
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.MarkupLine($"[red]Error during file operation: {ex.Message}[/]");
                    }
                });

            AnsiConsole.MarkupLine($"\n[green]Successfully deleted '{selectedExercise.ExName}'.[/]");
            System.Threading.Thread.Sleep(2000);
        }
    }
}