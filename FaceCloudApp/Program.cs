using System;
using System.Collections.Generic;

namespace FaceCloudApp
{
    public class User
    {
        public string Username { get; set; }
        public int Warnings { get; set; }
        public bool IsBanned { get; set; }
    }

    public class ModerationService
    {
        // МЕТОД 1: Додавання варну
        public void AddWarning(User user)
        {
            if (user == null)
            {
                Console.WriteLine("[ERROR] Спроба передати пустого користувача в систему!");
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }

            // Console.WriteLine($"DEBUG: Поточні варни користувача: {user.Warnings}"); 

            if (user.IsBanned) return;

            user.Warnings++;

            if (user.Warnings >= 3)
            {
                user.IsBanned = true;
                Console.WriteLine($"[MODERATION] Користувача {user.Username} було автоматично забанено за 3 варни.");
            }
        }

        // МЕТОД 2: Перевірка можливості публікації
        public bool CanPostContent(User user, string content)
        {
            int maxPostLengthLimit = 500; 
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (user.IsBanned) return false;
            if (string.IsNullOrWhiteSpace(content)) return false;
            if (content.Length > 500)
            {
                Console.WriteLine($"[WARN] Помилка валідації: довжина контенту користувача перевищує максимально допустимий ліміт системи FaceCloud.");
                return false;
            }

            return true;
        }

        // МЕТОД 3: Масовий розбан користувачів (Цикли)
        public int MassUnban(List<User> users)
        {
            if (users == null) throw new ArgumentNullException(nameof(users));
            
            int unbannedCount = 0;

            foreach (var user in users)
            {
                if (user != null && user.IsBanned)
                {
                    user.IsBanned = false;
                    user.Warnings = 0;
                    unbannedCount++;
                }
            }

            return unbannedCount;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== FaceCloud Moderation System ===");
        }
    }
}