using Spectre.Console;
using System;
using System.Numerics;
using System.Text.RegularExpressions;


namespace gymschedver2
{
    public static class MainFit
    {
        public static void LoginBanner()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            string[] Lbanner =
            {
             "███████████████████████████████████████████████████████████████████████████████████████████████████████████",
             "█░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░█",
             "░░                                                                                                       ░░",
             "░░        ▄████████  ▄█      ███             ▄███████▄  ▄█   ████    ▐████▀    ▄████████  ▄█             ░░",
             "░░        ███    ███ ███  ▀█████████▄        ███    ███ ███    ███▌   ████▀    ███    ███ ███            ░░",
             "░░        ███    █▀  ███▌    ▀███▀▀██        ███    ███ ███▌    ███  ▐███      ███    █▀  ███            ░░",
             "░░       ▄███▄▄▄     ███▌     ███   ▀        ███    ███ ███▌    ▀███▄███▀     ▄███▄▄▄     ███            ░░",
             "░░       ▀███▀▀▀     ███▌     ███          ▀█████████▀  ███▌    ████▀██▄     ▀▀███▀▀▀     ███            ░░",
             "░░        ███        ███      ███            ███        ███    ▐███  ▀███      ███    █▄  ███            ░░",
             "░░        ███        ███      ███            ███        ███   ▄███     ███▄    ███    ███ ███▌    ▄      ░░",
             "░░        ███        █▀      ▄████▀         ▄████▀      █▀   ████       ███▄   ██████████ █████▄▄██      ░░",
             "░░                                                                                                       ░░",
             "░░                                 ██╗      ██████╗  ██████╗ ██╗███╗   ██╗                               ░░",
             "░░                                 ██║     ██╔═══██╗██╔════╝ ██║████╗  ██║                               ░░",
             "░░                                 ██║     ██║   ██║██║  ███╗██║██╔██╗ ██║                               ░░",
             "░░                                 ██║     ██║   ██║██║   ██║██║██║╚██╗██║                               ░░",
             "░░                                 ███████╗╚██████╔╝╚██████╔╝██║██║ ╚████║                               ░░",
             "░░                                 ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝╚═╝  ╚═══╝                               ░░",
             "█░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░█",
             "███████████████████████████████████████████████████████████████████████████████████████████████████████████"
            };

            int Lwidth = Console.WindowWidth;
            foreach (string Lline in Lbanner)
            {
                int Lpad = (Lwidth - Lline.Length) / 2 + 1;
                Console.WriteLine(new string(' ', Math.Max(Lpad, 0)) + Lline);
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            string message = "Optimize | Build | Maintain";
            int msgPad = (Lwidth - message.Length) / 2;
            Console.WriteLine("\n" + new string(' ', Math.Max(msgPad, 0)) + message);

            string hLine = new string('-', 120);
            int linePad = (Console.WindowWidth - hLine.Length) / 2;
            Console.WriteLine(new string(' ', Math.Max(linePad, 0)) + hLine);
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
        public static void LoginUser(List<UserAuth> users)
        {
            Console.Clear();
            AnsiConsole.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Black;
            LoginBanner();
            string padding = new string(' ', 20);
            Console.Write(padding + "Enter your username: ");
            string username = Console.ReadLine();
            if (username.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                AdminMenu.AdminLogin(users);
                return;
            }
            Console.Write(padding + "Enter your password: ");
            string password = HIdeP.ReadPassword();
            Console.CursorVisible = false;
            Console.WriteLine();
            Bar(padding);

            UserAuth user = users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (user != null && HIdeP.VerifyPassword(password, user.PasswordHash, user.PasswordSalted))
            {
                Console.Clear();
                Program.TitleBanner();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(padding + "Login successful!");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(padding + $"Welcome back, {user.Name}!");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine(padding + $"Your fitness category is: {user.Category}");
                Console.WriteLine(padding + "Press any key to continue...");
                Console.ReadKey();

                MainMenu(user, users);
            }
            else
            {
                Console.Clear();
                Program.ErrTitleBanner();
                Console.SetCursorPosition(20, 15);
                string errpad = new string(' ', 48);
                Console.ForegroundColor = ConsoleColor.DarkRed;

                Console.WriteLine("\n" + errpad + "Invalid username or password.");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(errpad + "Press any key to Return");
                Console.ReadKey();
            }
        }

        private static void Bar(string padding)
        {
            Console.CursorVisible = false;

            string[] stages = { "Verifying credentials", "Checking database", "Loading profile", "Authentication complete" };
            int barWidth = 40;
            int delayPerStage = 400;

            for (int stage = 0; stage < stages.Length; stage++)
            {
                int percentage = (int)(((double)(stage + 1) / stages.Length) * 100);
                int progress = (int)(((double)(stage + 1) / stages.Length) * barWidth);

                Console.SetCursorPosition(0, Console.CursorTop);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(padding + $"{stages[stage],-30} ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("[");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(new string('█', progress));

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(new string('░', barWidth - progress));

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"] {percentage}%");

                Thread.Sleep(delayPerStage);
            }

            Console.WriteLine();
            Console.CursorVisible = false;
            Console.ResetColor();
        }

        public static void MainMenu(UserAuth user, List<UserAuth> users)
        {
            string[] MainMenuActions =
            {
                "Profile Settings",
                "Generate Workout Plan",
                "View My Schedule",
                "Track Progress",
                "Log Out & Exit to Intro Menu",
            };

            bool isMainMenuActive = true;
            while (isMainMenuActive)
            {
                Console.Clear();
                AnsiConsole.Clear();
                Program.TitleBanner();
                int selectedIndex = Program.intro(MainMenuActions);

                switch (selectedIndex)
                {
                    case 0:
                        ProfileSettings(user, users);
                        break;
                    case 1:
                        GenerateWorkoutPlan(user, users);
                        break;
                    case 2:
                        ViewSchedule(user, users);
                        break;
                    case 3:
                        TrackProgress(user);
                        break;
                    case 4:
                        Logout();
                        isMainMenuActive = false;

                        break;
                }
            }
        }

        public static void ProfileSettings(UserAuth user, List<UserAuth> users)
        {

            string[] ProfileActions =
             {
               "View Profile",
               "Update Fitness Data",
               "Back to Main Menu",
             };

            bool isProfileMenuActive = true;
            while (isProfileMenuActive)
            {
                int selectedProfileIndex = Program.intro(ProfileActions);

                switch (selectedProfileIndex)
                {
                    case 0:
                        ViewProfile(user);
                        break;
                    case 1:
                        UpdateFitnessData(user, users);
                        break;
                    case 2:
                        isProfileMenuActive = false;
                        break;
                }
            }
        }


       
        public static void ViewProfile(UserAuth user)
        {
            Console.CursorVisible = false;
            Console.Clear();
            Program.TitleBanner();

            AnsiConsole.Write(new Rule("[yellow]USER PROFILE[/]").RuleStyle("grey").Centered());

            var table = new Table().Centered().Border(TableBorder.Rounded).ShowRowSeparators();
            table.AddColumn("[yellow]BioData[/]");
            table.AddColumn("Info.");

            var profileData = new[]
            {
        ("Name", user.Name),
        ("Username", user.Username),
        ("Height", $"{user.HeightCm} cm"),
        ("Weight", $"{user.WeightKg} kg"),
        ("Age", user.Age.ToString()),
        ("Gender", user.Gender),
        ("BMI Score", user.BmiScore.ToString("F1")),
        ("Category", user.Category),
        ("Rec. Intensity", user.WorkoutIntensity)
    };

            foreach (var (label, value) in profileData)
            {
                table.AddRow($"[yellow]{label}[/]", value);
            }

            AnsiConsole.Write(table);
            AnsiConsole.WriteLine();

            AnsiConsole.Write(
                new Markup("[cornsilk1]Press [[ESC]] key to return...[/]")
                .LeftJustified()
            );

            // Clean loop to wait for ESC
            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Escape) break;
            }

            Console.CursorVisible = true;
        }
        public static void UpdateFitnessData(UserAuth user, List<UserAuth> users)
        {
            bool isEditing = true;

            while (isEditing)
            {
                Console.Clear();
                Program.TitleBanner(); //

                AnsiConsole.Write(new Rule("[yellow]UPDATE FITNESS PROFILE[/]").RuleStyle("yellow"));
                AnsiConsole.WriteLine();

                
                var infoTable = new Table()
                    .Border(TableBorder.Rounded)
                    .BorderColor(Color.Yellow)
                    .Width(100)
                    .Centered();

                infoTable.AddColumn("[yellow]Measurements[/]");
                infoTable.AddColumn(new TableColumn("[yellow]Current Value[/]").Centered());
                infoTable.AddColumn(new TableColumn("[yellow]Status[/]").Centered());

                infoTable.AddRow("Height", $"{user.HeightCm} cm", "[grey]Editable[/]");
                infoTable.AddRow("Weight", $"{user.WeightKg} kg", "[grey]Editable[/]");
                infoTable.AddRow("Current BMI", $"{user.BmiScore:F1}", $"[green1]{user.Category}[/]");

                AnsiConsole.Write(infoTable);
                AnsiConsole.WriteLine();

                string[] options = {
            "Edit Height",
            "Edit Weight",
            "Save & Exit",
            "Cancel & Return"
        };

                int selectedIndex = 0;
                int menuStartRow = Console.CursorTop; 
                bool selectionMade = false;
                Console.CursorVisible = false;

                // Custom Loop for Menu
                while (!selectionMade)
                {
                    Console.SetCursorPosition(0, menuStartRow);

                    for (int i = 0; i < options.Length; i++)
                    {
                        string currentOption = options[i];

                        
                        int windowWidth = Console.WindowWidth;
                        int padding = Math.Max(0, (windowWidth - currentOption.Length) / 2);
                        string centeredText = new string(' ', padding) + currentOption;

                        if (i == selectedIndex)
                        {
                            
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.WriteLine(centeredText.PadRight(windowWidth));
                        }
                        else
                        {
                            
                            Console.ForegroundColor = ConsoleColor.Yellow; 
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.WriteLine(centeredText.PadRight(windowWidth));
                        }
                    }
                    Console.ResetColor();

                    
                    var key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            selectedIndex = (selectedIndex == 0) ? options.Length - 1 : selectedIndex - 1;
                            break;
                        case ConsoleKey.DownArrow:
                            selectedIndex = (selectedIndex == options.Length - 1) ? 0 : selectedIndex + 1;
                            break;
                        case ConsoleKey.Enter:
                            selectionMade = true;
                            break;
                        case ConsoleKey.Escape:
                            selectedIndex = -1;
                            return;
                           
                    }
                }

                
                string choice = options[selectedIndex];

                switch (choice)
                {
                    case "Edit Height":
                        user.HeightCm = AnsiConsole.Prompt(
                            new TextPrompt<double>("Enter new [cyan]Height[/] (cm):")
                                .PromptStyle("yellow")
                                .Validate(h => h > 50 && h < 300 ? ValidationResult.Success() : ValidationResult.Error("[red]Invalid height[/]")));

                        BMIPfCalc.CalculateProfile(user);
                        break;

                    case "Edit Weight":
                        user.WeightKg = AnsiConsole.Prompt(
                            new TextPrompt<double>("Enter new [green]Weight[/] (kg):")
                                .PromptStyle("yellow")
                                .Validate(w => w > 20 && w < 500 ? ValidationResult.Success() : ValidationResult.Error("[red]Invalid weight[/]")));

                        BMIPfCalc.CalculateProfile(user);
                        break;

                    case "Save & Exit":
                        isEditing = false;
                        break;

                    case "Cancel & Return":
                        return;
                }
            }

          
            AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .Start("Saving profile...", ctx =>
                {
                    BMIPfCalc.CalculateProfile(user); 
                    Program.SaveUsers(users); //
                    Thread.Sleep(1000);
                });

           
            Console.Clear();
            Program.TitleBanner();

            var resultGrid = new Grid().Width(120);
            resultGrid.AddColumn(new GridColumn().Centered());
            Console.CursorVisible = false;
            var successPanel = new Panel(
                Align.Center(
                    new Markup($"[bold green1]Profile Updated Successfully![/]\n\n" +
                               $"New BMI Score: [bold yellow1]{user.BmiScore:F1}[/]\n" +
                               $"New Category:  [bold cyan]{user.Category}[/]\n" +
                               $"Intensity Rec: [bold white]{user.WorkoutIntensity}[/]")
                ))
                .Header("[yellow]RESULTS[/]")
                .Border(BoxBorder.Heavy)
                .BorderColor(Color.Green1)
                .Padding(2, 1, 2, 1)
                .Expand();

            resultGrid.AddRow(successPanel);

            AnsiConsole.Write(Align.Center(resultGrid));
            AnsiConsole.WriteLine();

            var footer = "Press any key to return...";
            var pad = (120 - footer.Length) / 2;
            AnsiConsole.MarkupLine($"[grey]{new string(' ', Math.Max(0, pad))}{footer}[/]");
            Console.ReadKey();
        }

        public static void Logout()
        {
            Console.Clear();
            AnsiConsole.Clear();
        }

        public static void GenerateWorkoutPlan(UserAuth user, List<UserAuth> users)
        {
            string[] workoutPlanOptions =
            {
                "Get Split Recommendation",
                "View All Workout Splits",
                "Search Exercises",
                "Mark Workout as Complete",
                "Clear Workout Plan",
                "Save and Exit"
            };

            bool isRunning = true;
            while (isRunning)
            {
                int choiceIndex = GWorkoutPlan(workoutPlanOptions, "--- GENERATE WORKOUT PLAN ---");
                if (choiceIndex == -1) return;

                Console.Clear();
                AnsiConsole.Clear();
                Console.CursorVisible = false;
                switch (choiceIndex)
                {
                    case 0:
                        SplitRecommendation(user, users);
                        break;
                    case 1:
                        ViewAllSplits(user);
                        break;
                    case 2:
                        SearchExercises(user);
                        break;
                    case 3:
                        MarkWorkoutComplete(user, users);
                        break;
                    case 4:
                        ClearWorkoutPlan(user, users);
                        break;
                    case 5:
                        isRunning = false;
                        Console.WriteLine("Returning to main menu...");
                        break;
                    default:
                        Console.WriteLine("\t\tInvalid choice, please try again.");
                        break;
                }

                if (isRunning)
                {
                    Console.CursorVisible = false;
                    Console.WriteLine();
                    Console.WriteLine("\t\t\t\tEnter a key to return....");
                    Console.ReadKey();

                }
            }
        }

        private static int GWorkoutPlan(string[] options, string title)
        {
            Console.Clear();
            AnsiConsole.Clear();
            Program.TitleBanner();
            Console.CursorVisible = false;
            int selectWP = 0;
            ConsoleKey key;

            int titleTop = 17;
            int menuTop = 19;


            Console.ForegroundColor = ConsoleColor.Yellow;
            int titleLeft = (Console.WindowWidth - title.Length) / 2;
            Console.SetCursorPosition(Math.Max(0, titleLeft), titleTop);
            Console.WriteLine(title);
            Console.ResetColor();

            do
            {
                for (int i = 0; i < options.Length; i++)
                {
                    int left = (Console.WindowWidth - (options[i].Length + 6)) / 2;
                    Console.SetCursorPosition(Math.Max(0, left), menuTop + i);

                    if (i == selectWP)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"  {options[i]}  ");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.WriteLine($"  {options[i]}  ");
                    }
                }
                Console.ResetColor();

                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectWP = (selectWP == 0) ? options.Length - 1 : selectWP - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectWP = (selectWP == options.Length - 1) ? 0 : selectWP + 1;
                        break;
                    case ConsoleKey.Escape:
                        Console.CursorVisible = true;
                        return -1;
                }
            } while (key != ConsoleKey.Enter);

            Console.CursorVisible = false;
            return selectWP;
        }

        private static void SplitRecommendation(UserAuth user, List<UserAuth> users)
        {
            Console.Clear();
            Program.TitleBanner();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("   --- GET SPLIT RECOMMENDATION ---");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("   Split training divides workouts into separate days to focus");
            Console.WriteLine("   on specific muscle groups.");
            Console.WriteLine();
            Console.WriteLine($"   Your profile recommends: {user.WorkoutIntensity}");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();


            Console.WriteLine("   Press any key to select your intensity...");
            Console.ReadKey();

            string[] intensityOptions = 
            {
             "Easy (Beginner)",
            "Medium (Intermediate)",
            "High (Advanced)",
            "Cancel"
            };

            
            int intensityIndex = GWorkoutPlan(intensityOptions, "SELECT DESIRED INTENSITY");

            if (intensityIndex == -1 || intensityIndex == 3) 
            {
                return;
            }

           
            string targetKeyword = intensityIndex switch
            {
                0 => "beginner",
                1 => "intermediate",
                2 => "advanced",
                _ => "beginner"
            };

            
            var matchingSplits = Program.allWorkoutSplits
                .Where(s => s.WorkoutName.Contains(targetKeyword, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!matchingSplits.Any())
            {
                Console.Clear();
                Program.TitleBanner();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n   No splits found containing the keyword '{targetKeyword}'.");
                Console.ResetColor();
                return;
            }

            
            var splitOptions = matchingSplits.Select(s => s.WorkoutName).ToList();
            splitOptions.Add("Back");

            int splitIndex = GWorkoutPlan(splitOptions.ToArray(), $"--- {targetKeyword.ToUpper()} SPLITS FOUND ---");

            if (splitIndex == -1 || splitIndex == splitOptions.Count - 1) 
            {
                return;
            }

            WorkoutSplit selectedSplit = matchingSplits[splitIndex];

            if (selectedSplit == null || !selectedSplit.Days.Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n   Error: This split has no days defined and cannot be scheduled.");
                Console.ResetColor();
                Console.ReadKey();
                return;
            }

            string prompt = $"{selectedSplit.WorkoutName}'";
            HashSet<DateTime> selectedDates = CalenSelector.SelectDates(1, prompt); //

            if (selectedDates.Count == 0)
            {
                Console.WriteLine("\n   Scheduling cancelled.");
                Thread.Sleep(1000);
                return;
            }

            DateTime startDate = selectedDates.First();

            Console.Clear();
            Program.TitleBanner();
            Console.WriteLine("   Generating schedule...".PadLeft(40));

            for (int i = 0; i < selectedSplit.Days.Count; i++)
            {
                WorkoutDay currentDay = selectedSplit.Days[i];
                DateTime scheduleDate = startDate.AddDays(i);

                user.Schedule.Add(new ScheduledWorkout
                {
                    Date = scheduleDate,
                    SplitName = selectedSplit.WorkoutName,
                    WorkoutDayName = currentDay.DayName,
                    IsCompleted = false
                });
            }

            Program.SaveUsers(users); 
            Thread.Sleep(1000);

            
            Console.Clear();
            Program.TitleBanner();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n\t\t\t\t\t   Successfully Scheduled!");
            Console.ResetColor();
            Console.WriteLine($"\t\t\t\t\t  Split:    {selectedSplit.WorkoutName}");
            Console.WriteLine($"\t\t\t\t\t   Duration: {selectedSplit.Days.Count} Days");
            Console.WriteLine($"\t\t\t\t\t   Start:    {startDate:yyyy-MM-dd}");
            Console.WriteLine();
            Console.WriteLine("\t\t\t\t\t   Check 'View My Schedule' to see your new plan.");
            Console.WriteLine("\t\t\t\t\t   Press any key to return...");
            Console.ReadKey();
        }

        private static void ViewSchedule(UserAuth user, List<UserAuth> users)
        {
            bool isManaging = true;
            int currentPage = 0;
            const int pageSize = 10;

            while (isManaging)
            {
                Console.Clear();
                Program.TitleBanner();

                var allWorkouts = user.Schedule.OrderBy(w => w.Date).ToList();
                int totalWorkouts = allWorkouts.Count;
                int totalPages = (int)Math.Ceiling((double)totalWorkouts / pageSize);

                if (currentPage < 0) currentPage = 0;
                if (currentPage >= totalPages && totalPages > 0) currentPage = totalPages - 1;

                var pageData = allWorkouts.Skip(currentPage * pageSize).Take(pageSize);

                AnsiConsole.Write(new Rule("[yellow]CURRENT MONTH[/]").RuleStyle("grey"));
                var scheduledDates = user.Schedule.Select(w => w.Date.Date).ToList();
                CalenSelector.DisplayScheduledMonth(DateTime.Today, scheduledDates);

                AnsiConsole.Write(new Rule("[yellow]FULL SCHEDULE[/]").RuleStyle("grey"));

                if (!allWorkouts.Any())
                {
                    AnsiConsole.MarkupLine("\n[grey]No scheduled workouts found.[/]\n");
                }
                else
                {
                    var table = new Table().Border(TableBorder.Rounded).Expand();
                    table.AddColumn("Date");
                    table.AddColumn("Day Name");
                    table.AddColumn("Split");
                    table.AddColumn("Status");

                    foreach (var workout in pageData)
                    {
                        string status;
                        string dateColor = "white";

                        if (workout.IsCompleted)
                        {
                            status = "[cyan]Completed[/]";
                            dateColor = "lime";
                        }
                        else if (workout.Date.Date < DateTime.Today)
                        {
                            status = "[red1]Incomplete[/]";
                            dateColor = "red1";
                        }
                        else
                        {
                            status = "[yellow]Pending[/]";
                            dateColor = "white";
                        }

                        table.AddRow(
                            $"[{dateColor}]{workout.Date:yyyy-MM-dd}[/]",
                            workout.WorkoutDayName,
                            $"[grey]{workout.SplitName}[/]",
                            status
                        );
                    }

                    table.Caption($"[grey]Page {currentPage + 1} of {Math.Max(1, totalPages)} (Total: {totalWorkouts})[/]");
                    AnsiConsole.Write(table);
                }

                var navGrid = new Grid().Expand();
                navGrid.AddColumn(new GridColumn().Centered());

                string navText = "[yellow1][[N]][/] Next Page  |  [yellow1][[P]][/] Prev Page  |  [green1][[Enter]][/] Open Options  |  [red1][[Esc]][/] Back";
                navGrid.AddRow(new Panel(new Markup(navText)).Border(BoxBorder.None));

                AnsiConsole.Write(navGrid);

                var keyInfo = Console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.N:
                        if (currentPage < totalPages - 1) currentPage++;
                        break;

                    case ConsoleKey.P:
                        if (currentPage > 0) currentPage--;
                        break;

                    case ConsoleKey.Escape:
                        isManaging = false;
                        break;

                    case ConsoleKey.Enter:
                        bool keepMenuOpen = OpenScheduleMenu(user, users);
                        if (!keepMenuOpen) isManaging = false;
                        break;
                }
            }
        }

        private static bool OpenScheduleMenu(UserAuth user, List<UserAuth> users)
        {
            Console.Clear();
            Program.TitleBanner();
            AnsiConsole.Write(new Rule("[yellow]SCHEDULE ACTIONS[/]").RuleStyle("yellow"));
            Console.WriteLine();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an action:")
                    .HighlightStyle(new Style(foreground: Color.Black, background: Color.Yellow))
                    .AddChoices(new[] {
                "Add a New Workout",
                "View Exercises for a Pending Workout",
                "Back to Schedule View"
                    })
            );

            switch (choice)
            {
                case "Add a New Workout":
                    AddWorkoutManual(user, users);
                    return true;

                case "View Exercises for a Pending Workout":
                    ViewPendingWorkoutExercises(user);
                    return true;

                case "Back to Schedule View":
                    return true;
            }

            return true;
        }


        private static void AddWorkoutManual(UserAuth user, List<UserAuth> users)
        {
            AnsiConsole.MarkupLine("[yellow1]Add New Workout[/]");

            string dateStr = AnsiConsole.Ask<string>("Enter date ([green]yyyy-MM-dd[/]):");
            if (!DateTime.TryParse(dateStr, out DateTime date))
            {
                AnsiConsole.MarkupLine("[red1]Invalid date format.[/]");
                Thread.Sleep(1000);
                return;
            }

            if (!Program.allWorkoutSplits.Any())
            {
                AnsiConsole.MarkupLine("[red1]No splits available.[/]");
                return;
            }

           
            var selectedSplit = AnsiConsole.Prompt(
                new SelectionPrompt<WorkoutSplit>()
                    .Title("Select a [yellow1]Split[/]:")
                    .PageSize(10)
                    .AddChoices(Program.allWorkoutSplits)
                    .UseConverter(s => s.WorkoutName)
            );

            var selectedDay = AnsiConsole.Prompt(
                new SelectionPrompt<WorkoutDay>()
                    .Title($"Select a [yellow1]Day[/]:")
                    .PageSize(10)
                    .AddChoices(selectedSplit.Days)
                    .UseConverter(d => d.DayName)
            );

            user.Schedule.Add(new ScheduledWorkout
            {
                Date = date,
                SplitName = selectedSplit.WorkoutName,
                WorkoutDayName = selectedDay.DayName,
                IsCompleted = false
            });

            Program.SaveUsers(users);
            AnsiConsole.MarkupLine($"[lime]Scheduled {selectedDay.DayName} for {date:yyyy-MM-dd}![/]");
            Thread.Sleep(1500);
        }

        private static void ViewAllSplits(UserAuth user)
        {
            while (true)
            {
                Console.Clear();
                Program.TitleBanner(); 
                Console.CursorVisible = false;

                
                if (!Program.allWorkoutSplits.Any())
                {
                    AnsiConsole.MarkupLine("[center yellow]No workout splits found.[/]");
                    Console.ReadKey();
                    return;
                }

                
                List<string> menuOptions = Program.allWorkoutSplits
                    .Select(s => $"{s.WorkoutName} ({s.Days.Count} Days)")
                    .ToList();

                menuOptions.Add("Back");

                
                string subHeader = "--- SELECT A SPLIT TO VIEW DETAILS ---";
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

        private static void SearchExercises(UserAuth user)
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

        private static void MarkWorkoutComplete(UserAuth user, List<UserAuth> users)
        {
            Console.Clear();
            Program.TitleBanner(); 
            AnsiConsole.Write(new Rule("[yellow1]UPDATE WORKOUT STATUS[/]").RuleStyle("yellow"));


            var allWorkouts = user.Schedule.OrderBy(w => w.Date).ToList();

            if (!allWorkouts.Any())
            {
                AnsiConsole.MarkupLine("[red1]No scheduled workouts found.[/]");
                Console.ReadKey();
                return;
            }

            

            var selectedWorkout = AnsiConsole.Prompt(
                new SelectionPrompt<ScheduledWorkout>()
                    .Title("[white]Select a workout to change its status:[/]")
                    .PageSize(15)
                    .HighlightStyle(new Style(foreground: Color.Black, background: Color.Yellow))
                    .MoreChoicesText("[grey](Move up and down for more)[/]")
                    .AddChoices(allWorkouts)
                    .UseConverter(w =>
                    {
                        
                        if (w.IsCompleted)
                        {
                            
                            return $"[cyan]|  COMPLETE  |  {w.Date:yyyy-MM-dd} - {w.WorkoutDayName} ({w.SplitName})[/]";
                        }
                        else if (w.Date.Date < DateTime.Now.Date)
                        {
                            
                            return $"[red1]| INCOMPLETE | {w.Date:yyyy-MM-dd} - {w.WorkoutDayName} ({w.SplitName})[/]";
                        }
                        else
                        {
                           
                            return $"[yellow1]|  PENDING   |   {w.Date:yyyy-MM-dd} - {w.WorkoutDayName} ({w.SplitName})[/]";
                        }
                    })
            );
            
            
            

            
            AnsiConsole.WriteLine();
            if (selectedWorkout.IsCompleted)
            {

                selectedWorkout.IsCompleted = false;
                AnsiConsole.Write(
                Align.Center(
                new Markup($"[yellow1]Status reverted to PENDING for {selectedWorkout.WorkoutDayName}.[/]")
                ));
                AnsiConsole.WriteLine();
            }
            else
            {

                selectedWorkout.IsCompleted = true;
                AnsiConsole.Write(
                Align.Center(
                new Markup($"[cyan]Success! Marked {selectedWorkout.WorkoutDayName} as COMPLETED.[/]")
                ));
                AnsiConsole.WriteLine();

            }

           
            Program.SaveUsers(users); 

            AnsiConsole.WriteLine();
           
        }

        public static void ViewPendingWorkoutExercises(UserAuth user)
        {
            Console.Clear();
            Program.TitleBanner();

            
            var pending = user.Schedule
                .Where(w => !w.IsCompleted)
                .OrderBy(w => w.Date)
                .ToList();

            if (!pending.Any())
            {
                AnsiConsole.MarkupLine("[yellow]No pending workouts to view.[/]");
                Console.ReadKey();
                return;
            }

           
            AnsiConsole.Write(new Rule("[yellow]CURRENT MONTH[/]").RuleStyle("grey"));
            var scheduledDates = user.Schedule.Select(w => w.Date.Date).ToList();
            CalenSelector.DisplayScheduledMonth(DateTime.Today, scheduledDates);

            
            var backOption = new ScheduledWorkout { SplitName = "BACK_OPTION" };

           
            var menuOptions = new List<ScheduledWorkout>();
            menuOptions.AddRange(pending);
            menuOptions.Add(backOption);


            var selectedWorkout = AnsiConsole.Prompt(
                new SelectionPrompt<ScheduledWorkout>()

                    .Title("[yellow]\t\t\t--- SELECT PENDING WORKOUT ---[/]")
                    .PageSize(10)
                    .HighlightStyle(new Style(foreground: Color.Black, background: Color.Yellow))
                    .AddChoices(menuOptions)
                    .UseConverter(w =>
                    {
                        // Display logic
                        if (w.SplitName == "BACK_OPTION") return "[grey]Back to Main Menu[/]";
                        return $"{w.Date:yyyy-MM-dd}: {w.WorkoutDayName} ({w.SplitName})";
                    })
            ); 

          
            if (selectedWorkout.SplitName == "BACK_OPTION")
            {
                return;
            }

            var split = Program.allWorkoutSplits.FirstOrDefault(s =>
                s.WorkoutName.Equals(selectedWorkout.SplitName, StringComparison.OrdinalIgnoreCase));

            if (split == null)
            {
                AnsiConsole.MarkupLine("[red1]Error: Could not find the details for that split.[/]");
            }
            else
            {
                var day = split.Days.FirstOrDefault(d =>
                    d.DayName.Equals(selectedWorkout.WorkoutDayName, StringComparison.OrdinalIgnoreCase));

                Console.Clear();
                Program.TitleBanner();

                if (day == null)
                {
                    AnsiConsole.MarkupLine($"[red]--- Could not find specific day '{selectedWorkout.WorkoutDayName}' ---[/]");
                }
                else
                {
                    AnsiConsole.Write(new Rule($"[yellow]EXERCISES FOR {selectedWorkout.Date:yyyy-MM-dd}[/]").RuleStyle("yellow"));
                    Console.WriteLine();
                    DisplayWorkoutDay(day, "");
                }
            }

            Console.WriteLine();
            AnsiConsole.MarkupLine("[grey]Press any key to return...[/]");
            Console.ReadKey();
        }

        private static void DisplayWorkoutDay(WorkoutDay day, string indent = "")
        {
            if (!day.Exercises.Any())
            {
                AnsiConsole.MarkupLine($"\n[italic grey]{indent}(Rest Day - No exercises)[/]\n");
                return;
            }

            var table = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.Cyan1)
                .Expand();

            table.AddColumn(new TableColumn("[cyan]Exercise[/]").NoWrap());
            table.AddColumn(new TableColumn("[yellow]Sets x Reps[/]").Centered());
            table.AddColumn(new TableColumn("[green]Target[/]").Centered());
            table.AddColumn(new TableColumn("Description"));

            foreach (var prescribedEx in day.Exercises)
            {
                var exerciseDetails = Program.allExercises.FirstOrDefault(e =>
                    e.ExName.Equals(prescribedEx.Exercise, StringComparison.OrdinalIgnoreCase) ||
                    e.ExName.StartsWith(prescribedEx.Exercise, StringComparison.OrdinalIgnoreCase)
                );

                string description = exerciseDetails != null
                    ? exerciseDetails.ExDescription
                    : "[italic grey]Details not found[/]";
                string target = exerciseDetails != null
                    ? exerciseDetails.TargetBodyPart
                    : "-";

                table.AddRow(
                    $"[bold white]{prescribedEx.Exercise}[/]",
                    $"{prescribedEx.Sets} x {prescribedEx.Reps}",
                    target,
                    $"[grey]{description}[/]"
                );
            }

            AnsiConsole.Write(table);
        }

        public static void TrackProgress(UserAuth user)
        {
            Console.Clear();
            Program.TitleBanner();
            Console.CursorVisible = false;

            int totalWorkouts = user.Schedule.Count;
            int completedReal = user.Schedule.Count(w => w.IsCompleted);
            int pendingReal = totalWorkouts - completedReal;
            int incompleteReal = user.Schedule.Count(w => w.Date < DateTime.Now.Date && !w.IsCompleted);
            var mainLayout = new Grid().Width(100);
            mainLayout.AddColumn();


            var bioGrid = new Grid();
            bioGrid.AddColumn(new GridColumn().NoWrap().PadRight(2));
            bioGrid.AddColumn();
            bioGrid.AddRow("[yellow]Weight:[/]", $"{user.WeightKg} kg");
            bioGrid.AddRow("[yellow]BMI Score:[/]", $"{user.BmiScore:F1}");
            bioGrid.AddRow("[yellow]Category:[/]", $"[bold cyan]{user.Category}[/]");

            var bioPanel = new Panel(bioGrid)
                .Header(" [bold yellow]Body Stats[/] ", Justify.Center)
                .Border(BoxBorder.Rounded)
                .Expand();


            AnsiConsole.Live(mainLayout)
                .AutoClear(false)
                .Overflow(VerticalOverflow.Ellipsis)
                .Start(ctx =>
                {

                    int step = Math.Max(1, completedReal / 20);

                    for (int i = 0; i <= completedReal; i += (totalWorkouts > 50 ? 2 : 1))
                    {

                        int visualCompleted = (i > completedReal) ? completedReal : i;
                        int visualPending = totalWorkouts - visualCompleted;
                        int visualIncomplete = (i < completedReal) ? completedReal : i;
                        double visualRate = totalWorkouts > 0 ? ((double)visualCompleted / totalWorkouts) * 100 : 0;




                        var breakdown = new BreakdownChart()
                            .Width(60)
                            .AddItem("Completed", visualCompleted, Color.Lime)
                            .AddItem("Pending", visualPending, Color.Yellow1)
                            .AddItem("Incomplete", visualIncomplete, Color.Red1);


                        var statsGrid = new Grid();
                        statsGrid.AddColumn();
                        statsGrid.AddRow($"[bold]Total Scheduled:[/] {totalWorkouts}");
                        statsGrid.AddRow($"[bold green]Completed:[/]     {visualCompleted}");
                        statsGrid.AddRow($"[bold grey]Remaining:[/]     {visualPending}");
                        statsGrid.AddRow($"[bold yellow]Completion Rate:[/] {visualRate:F0}%");
                        statsGrid.AddRow(new Rule());
                        statsGrid.AddRow(breakdown);

                        var statsPanel = new Panel(statsGrid)
                            .Header(" [bold yellow]Workout Performance[/] ", Justify.Center)
                            .Border(BoxBorder.Rounded)
                            .Expand();


                        mainLayout = new Grid().Width(100);
                        mainLayout.AddColumn(new GridColumn().Width(40));
                        mainLayout.AddColumn();
                        mainLayout.AddRow(bioPanel, statsPanel);

                        
                        ctx.UpdateTarget(Align.Center(mainLayout));


                        if (completedReal < 5) Thread.Sleep(150);
                        else Thread.Sleep(30);
                    }
                });

            Console.CursorVisible = false;
            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Rule("[yellow]RECENTLY COMPLETED[/]").RuleStyle("grey"));

            if (completedReal > 0)
            {
                var historyTable = new Table().Border(TableBorder.Simple).BorderColor(Color.DarkSlateGray1);
                historyTable.AddColumn("[green]Date[/]");
                historyTable.AddColumn("[cyan]Split Name[/]");
                historyTable.AddColumn("[yellow]Day[/]");


                var recentHistory = user.Schedule
                    .Where(w => w.IsCompleted)
                    .OrderByDescending(w => w.Date)
                    .Take(10);

                foreach (var w in recentHistory)
                {
                    historyTable.AddRow(
                        w.Date.ToString("yyyy-MM-dd"),
                        w.SplitName ?? "-",
                        w.WorkoutDayName
                    );
                }

                AnsiConsole.Write(Align.Center(historyTable));

                if (completedReal > 10)
                {
                    AnsiConsole.MarkupLine($"[grey](...and {completedReal - 10} older records)[/]");
                }
            }
            else
            {
                AnsiConsole.Write(
                Align.Center(
                 new Markup("\n[grey]No completed workouts recorded yet. Go hit the gym![/]")
                 ));
                AnsiConsole.WriteLine();
            }

            Console.WriteLine();
            AnsiConsole.Write(
            Align.Center(
            new Markup("\n[grey]Press any key to return to the main menu...[/]")
            ));
            AnsiConsole.WriteLine();
            Console.ReadKey();
            Console.CursorVisible = false;
        }

        public static void ClearWorkoutPlan(UserAuth user, List<UserAuth> users)
        {
            Console.Clear();
            AnsiConsole.Clear();
            Console.CursorVisible = false;
            Program.TitleBanner();


            AnsiConsole.Write(new Rule("[yellow]CLEAR WORKOUT PLAN[/]").RuleStyle("yellow"));
            AnsiConsole.WriteLine();


            var warningGrid = new Grid();
            warningGrid.AddColumn(new GridColumn().Centered());

            warningGrid.AddRow($"[bold red1]WARNING: About to clear schedule for '{user.Username}'[/]");
            warningGrid.AddRow("[red1]This action cannot be undone.[/]");
            warningGrid.AddRow("[red1]All scheduled workouts and history will be lost.[/]");
            warningGrid.AddRow("");

            var warningPanel = new Panel(warningGrid)
                .Border(BoxBorder.Heavy)
                .BorderColor(Color.Red1)
                .Padding(2, 1, 2, 1)
                .Expand();

            AnsiConsole.Write(new Padder(warningPanel).Padding(4, 1));

            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("[yellow]Do you wish to proceed?[/]")
            .HighlightStyle(new Style(foreground: Color.Black, background: Color.Red1))
            .AddChoices("Proceed", "Cancel")
    );

            if (choice == "Cancel")
            {
                return;
            }
            var inputPassword = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter [bold yellow]Password[/] to proceed:")
                    .Secret()
                    .PromptStyle("red1")
                    .AllowEmpty()
            );
            AnsiConsole.WriteLine();


            if (!HIdeP.VerifyPassword(inputPassword, user.PasswordHash, user.PasswordSalted))
            {
                AnsiConsole.MarkupLine("[bold red1]Incorrect Password. Operation Cancelled.[/]");
                System.Threading.Thread.Sleep(2000);
                return;
            }


            var confirm = AnsiConsole.Prompt(
                new TextPrompt<string>("Type [bold red1]CLEAR[/] to confirm:")
                    .InvalidChoiceMessage("[red1]You must type CLEAR to confirm[/]")
                    .ValidationErrorMessage("[red1]Invalid input[/]")
                    .AllowEmpty()
            );

           
            if (confirm.Equals("CLEAR", StringComparison.OrdinalIgnoreCase))
            {
                user.Schedule.Clear();
                Program.SaveUsers(users);

                AnsiConsole.WriteLine();

                
                var successPanel = new Panel(Align.Center(new Markup("[aqua]Workout plan cleared successfully.[/]")))
                    .Border(BoxBorder.Square)
                    .BorderColor(Color.Aqua)
                    .Expand();

                AnsiConsole.Write(new Padder(successPanel).Padding(4, 0));

            }
            else
            {
                AnsiConsole.MarkupLine("\n[yellow]Operation cancelled.[/]");
                System.Threading.Thread.Sleep(1500);
            }
        }

    }
}