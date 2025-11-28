using Spectre.Console;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace gymschedver2
{
   
    internal class Program
    {
        public static List<WorkoutSplit> allWorkoutSplits = new List<WorkoutSplit>();
        public static List<Exercise> allExercises = new List<Exercise>();
        private static List<UserAuth> users = new List<UserAuth>();
        private const string UserFilePath = "users.json";

        static void Main(string[] args)
        {
            users = LoadUsers();

            var allSplitsJson = File.ReadAllText("allSPLITS.json");
            allWorkoutSplits = JsonSerializer.Deserialize<List<WorkoutSplit>>(allSplitsJson);

            allExercises.AddRange(LoadJsonData<Exercise>("arms.json"));
            allExercises.AddRange(LoadJsonData<Exercise>("back.json"));
            allExercises.AddRange(LoadJsonData<Exercise>("chest.json"));
            allExercises.AddRange(LoadJsonData<Exercise>("legs.json"));
            allExercises.AddRange(LoadJsonData<Exercise>("shoulders.json"));
            allExercises.AddRange(LoadJsonData<Exercise>("core.json"));
            while (true)
            {
                introMenu();
            }
        }

        public static void TitleBanner()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            string[] TBbanner =
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
                "█░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░█",
                "█████████████████████████████████████████████████████████████████████████████████████████████████████████"
            };

            int TBwidth = Console.WindowWidth;
            foreach (string TBline in TBbanner)
            {
                int TBpad = (TBwidth - TBline.Length) / 2 + 1;
                Console.WriteLine(new string(' ', Math.Max(TBpad, 0)) + TBline);
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            string message = "Optimize | Build | Maintain";
            int msgPad = (TBwidth - message.Length) / 2;
            Console.WriteLine("\n" + new string(' ', Math.Max(msgPad, 0)) + message);

            string hLine = new string('-', 120);
            int linePad = (Console.WindowWidth - hLine.Length) / 2;
            Console.WriteLine(new string(' ', Math.Max(linePad, 0)) + hLine);
        }
        public static void ErrTitleBanner()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine();
            string[] TBbanner =
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
                "█░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░█",
                "█████████████████████████████████████████████████████████████████████████████████████████████████████████"
            };

            int TBwidth = Console.WindowWidth;
            foreach (string TBline in TBbanner)
            {
                int TBpad = (TBwidth - TBline.Length) / 2 + 1;
                Console.WriteLine(new string(' ', Math.Max(TBpad, 0)) + TBline);
            }
            string hLine = new string('-', 120);
            int linePad = (Console.WindowWidth - hLine.Length) / 2;
            Console.WriteLine(new string(' ', Math.Max(linePad, 0)) + hLine);
        }

        public static void Introbar()
        {
            Console.SetCursorPosition(0, 13);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Yellow;

            string plug = " Github: Deo-Deadulus";
            string time = DateTime.Now.ToString("D") + " ";

            int width = Console.WindowWidth;
            int spaces = width - plug.Length - time.Length;
            if (spaces < 0) spaces = 0;

            Console.Write(plug + new string(' ', spaces) + time);

            Console.ResetColor();
        }


        public static List<T> LoadJsonData<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Warning: File not found {filePath}.");
                return new List<T>();
            }
            try
            {
                string jsonString = File.ReadAllText(filePath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var list = JsonSerializer.Deserialize<List<T>>(jsonString, options) ?? new List<T>();
                var cleaned = list.Where(item => item != null).ToList();


                if (typeof(T) == typeof(WorkoutSplit))
                {
                    foreach (var obj in cleaned.Cast<WorkoutSplit>())
                    {
                        if (obj.Days == null) obj.Days = new List<WorkoutDay>();
                        foreach (var d in obj.Days)
                        {
                            if (d.Exercises == null) d.Exercises = new List<PrescribedExercise>();
                        }
                    }
                }
                else if (typeof(T) == typeof(WorkoutDay))
                {
                    foreach (var obj in cleaned.Cast<WorkoutDay>())
                    {
                        if (obj.Exercises == null) obj.Exercises = new List<PrescribedExercise>();
                    }
                }

                return cleaned;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error loading {filePath}: {e.Message}.");
                return new List<T>();
            }
        }

        public static int intro(string[] options)
        {
            Console.CursorVisible = false;
            int selectedIndex = 0;
            ConsoleKey key;

            int startTop = 18;

            Console.Clear();
            TitleBanner();
            Introbar();

            do
            {
                for (int i = 0; i < options.Length; i++)
                {
                    string currentOption = options[i];
                    string textToPrint;

                    int left = (Console.WindowWidth - (currentOption.Length + 6)) / 2 + 3;
                    Console.SetCursorPosition(left, startTop + i);

                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        textToPrint = $"{currentOption}";
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.BackgroundColor = ConsoleColor.Black;
                        textToPrint = $"{currentOption}";
                    }
                    Console.WriteLine(textToPrint);
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
                        Console.CursorVisible = false;
                        return -1;
                }

            } while (key != ConsoleKey.Enter);

            Console.CursorVisible = true;
            return selectedIndex;
        }

        private static void introMenu()
        {
            string[] introMenuActions =
            {
              "  Sign Up  ",
              "  Log In   ",
              "Save & Exit"
            };

            bool isIntroMenuActive = true;
            while (isIntroMenuActive)
            {
                int selectedIndex = intro(introMenuActions);

                switch (selectedIndex)
                {
                    case 0:
                        Console.Clear();
                        SignUp.SignUpUser(users);
                        SaveUsers(users);
                        break;
                    case 1:
                        Console.Clear();
                        MainFit.LoginUser(users);
                        break;
                    case 2:
                        Console.Clear();
                        SaveUsers(users);
                        EndBanner.End();
                        isIntroMenuActive = false;
                        Environment.Exit(0);
                        break;
                }
            }

        }
        public static void SaveUsers(List<UserAuth> users)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(users, options);
                File.WriteAllText(UserFilePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving users: {ex.Message}");
            }
        }
        public static List<UserAuth> LoadUsers()
        {
            if (!File.Exists(UserFilePath))
            {
                return new List<UserAuth>();
            }
            try
            {
                string jsonString = File.ReadAllText(UserFilePath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var list = JsonSerializer.Deserialize<List<UserAuth>>(jsonString, options) ?? new List<UserAuth>();


                var cleaned = list.Where(u => u != null).ToList();
                foreach (var u in cleaned)
                {
                    if (u.Schedule == null) u.Schedule = new List<ScheduledWorkout>();
                    u.Username ??= string.Empty;
                    u.Name ??= string.Empty;
                    u.Category ??= string.Empty;
                    u.WorkoutIntensity ??= string.Empty;
                    u.Gender ??= string.Empty;
                }

                return cleaned;
            }
            catch (Exception eL)
            {
                Console.WriteLine($"Error loading users: {eL.Message}. Starting fresh.");
                Console.ReadKey();
                return new List<UserAuth>();
            }
        }
    }
    public static class EndBanner
    {
        public static void End()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            string[] Ender =
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
                "█░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░█",
                "█████████████████████████████████████████████████████████████████████████████████████████████████████████"
            };

            
            End(Ender, ConsoleColor.Yellow, 2000, 15);

            
            Environment.Exit(0);
        }
        public static void EndDis()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            string[] Ender =
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
                "█░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░█",
                "█████████████████████████████████████████████████████████████████████████████████████████████████████████"
            };


            End(Ender, ConsoleColor.Yellow, 2000, 15);
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
        public static void End(string[] bannerLines, ConsoleColor bannerColor, int displayTimeMs, double deleteSpeedMs)
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = bannerColor;

            
            int startTop = Console.CursorTop;
            int Endwidth = Console.WindowWidth;

            var linePositions = new List<(int Left, int Top, int Length)>();

            
            for (int i = 0; i < bannerLines.Length; i++)
            {
                string Endline = bannerLines[i];
                int Endpad = (Endwidth - Endline.Length) / 2 + 1;
                int left = Math.Max(Endpad, 0);
                int top = startTop + i;

                linePositions.Add((left, top, Endline.Length));

                Console.SetCursorPosition(left, top);
                Console.Write(Endline);
            }

            Thread.Sleep(displayTimeMs);

            foreach (var line in linePositions.AsEnumerable().Reverse())
            {
                Console.SetCursorPosition(line.Left, line.Top);
                Console.Write(new string(' ', line.Length));

                if (deleteSpeedMs > 0)
                {
                    Thread.Sleep((int)deleteSpeedMs);
                }
            }

            Console.SetCursorPosition(0, startTop); 
           
            
        }
    }

}

