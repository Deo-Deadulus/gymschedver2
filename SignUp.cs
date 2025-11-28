using Spectre.Console;
using System;
namespace gymschedver2

{
    public static class SignUp
    {
        private static void SUBanner()
        {

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            string[] SUbanner =
            {
                "██████████████████████████████████████████████████████████████████████████████████████████",
                "██                                                                                      ██",
                "██      ███████╗██╗ ██████╗ ███╗   ██╗██╗███╗   ██╗ ██████╗       ██╗   ██╗██████╗      ██",
                "██      ██╔════╝██║██╔════╝ ████╗  ██║██║████╗  ██║██╔════╝       ██║   ██║██╔══██╗     ██",
                "██      ███████╗██║██║  ███╗██╔██╗ ██║██║██╔██╗ ██║██║  ███╗      ██║   ██║██████╔╝     ██",
                "██      ╚════██║██║██║   ██║██║╚██╗██║██║██║╚██╗██║██║   ██║      ██║   ██║██╔═══╝      ██",
                "██      ███████║██║╚██████╔╝██║ ╚████║██║██║ ╚████║╚██████╔╝      ╚██████╔╝██║          ██",
                "██      ╚══════╝╚═╝ ╚═════╝ ╚═╝  ╚═══╝╚═╝╚═╝  ╚═══╝ ╚═════╝        ╚═════╝ ╚═╝          ██",
                "██                                                                                      ██",
                "██████████████████████████████████████████████████████████████████████████████████████████"
            };

            int SUwidth = Console.WindowWidth;
            foreach (string SUline in SUbanner)
            {
                int SUpad = (SUwidth - SUline.Length) / 2;
                Console.WriteLine(new string(' ', Math.Max(SUpad, 0)) + SUline);
            }

            string hLine = new string('-', 120);
            int linePad = (Console.WindowWidth - hLine.Length) / 2;
            Console.WriteLine(new string(' ', Math.Max(linePad, 0)) + hLine);
        }
        public static void SignUpUser(List<UserAuth> users)
        {
            SUBanner();
            string padding = new string(' ', 20);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(padding + "Enter your name: ");
            string name = Console.ReadLine();

            string username;
            while (true)
            {
                Console.Write(padding + "Sign up your username: ");
                username = Console.ReadLine();
                if (users.Any(u => !string.IsNullOrEmpty(u?.Username) && u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine(padding + "Username already taken. Please choose another one.");
                }
                else
                {
                    break;
                }
            }
            string password;
            while (true)
            {
                Console.Write(padding + "Sign up password: ");
                password = HIdeP.ReadPassword();
                Console.Write(padding + "Confirm password: ");
                string confirmPassword = HIdeP.ReadPassword();
                if (password != confirmPassword)
                {
                    Console.WriteLine(padding + "Passwords do not match. Please try again.");
                }
                else
                {
                    break;
                }
            }
            HIdeP.PassHash(password, out string passwordHash, out string passwordSalted);

            var newUser = new UserAuth(name, username)
            {
                PasswordHash = passwordHash,
                PasswordSalted = passwordSalted
            };

            FitnessCategory(newUser);

            users.Add(newUser);
            Console.WriteLine();
            
            Console.WriteLine(padding + padding + " \t\tSign up complete! Press any key to continue...");
            Console.ReadKey();
        }
        public static void FitnessCategory(UserAuth user)
        {
            Console.Clear();
            SUBanner();

            void WriteCentered(string text)
            {
                int windowWidth = Console.WindowWidth;
                int padding = Math.Max(0, (windowWidth - text.Length) / 2);
                Console.WriteLine(new string(' ', padding) + text);
            }

            string GetCenteredPrompt(string text)
            {
                int windowWidth = Console.WindowWidth;
                int padding = Math.Max(0, (windowWidth - text.Length) / 2);
                return new string(' ', padding) + text;
            }

            WriteCentered("--- FITNESS PROFILE ---");
            Console.WriteLine();

            user.HeightCm = GetValidDouble(GetCenteredPrompt("Enter your height in cm: "));
            user.WeightKg = GetValidDouble(GetCenteredPrompt("Enter your weight in kg: "));
            user.Age = GetValidInt(GetCenteredPrompt("Enter your age: "));

            string genderPrompt = "Enter your gender (M/F): ";
            Console.Write(GetCenteredPrompt(genderPrompt));
            user.Gender = Console.ReadLine().Trim();

            BMIPfCalc.CalculateProfile(user);

            Console.WriteLine();
            WriteCentered($"BMI Score: {user.BmiScore:F1}");
            WriteCentered($"Category: {user.Category}");
            WriteCentered($"Recommended Workout Intensity: {user.WorkoutIntensity}");
        }

        public static double GetValidDouble(string prompt)
        {
            double value;
            while (true)
            {
                Console.Write(prompt);
                if (double.TryParse(Console.ReadLine(), out value) && value > 0)
                {
                    return value;
                }
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\t\t\t\t  Invalid input. Please enter a valid positive number.");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
        }

        public static int GetValidInt(string prompt)
        {
            int value;
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out value) && value > 0)
                {
                    return value;
                }
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\t\t\t\t  Invalid input. Please enter a valid positive integer.");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
        }
    }
}
