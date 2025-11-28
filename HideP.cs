using System;
using System.Security.Cryptography;
using System.Text;

namespace gymschedver2
{
    public static class HIdeP
    {
        public static void PassHash(string password, out string passwordHash, out string passwordSalted)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalted = Convert.ToBase64String(hmac.Key);
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                passwordHash = Convert.ToBase64String(hash);
            }
        }

        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            var hashBytes = Convert.FromBase64String(storedHash);

            using (var hmac = new HMACSHA512(Convert.FromBase64String(storedSalt)))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                var computedHashString = Convert.ToBase64String(computedHash);
                return computedHashString == storedHash;
            }
        }

        public static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key != ConsoleKey.Backspace && keyInfo.Key != ConsoleKey.Enter)
                {
                    password += keyInfo.KeyChar;
                    Console.Write("*");
                }
                else if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password[0..^1];
                    Console.Write("\b \b");
                }
            } while (keyInfo.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return password;
        }
    }
}