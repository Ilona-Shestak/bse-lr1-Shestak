using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Дозволяємо тестовому проєкту FaceCloudTests бачити та змінювати internal-властивості для Arrange-фази тестів
[assembly: InternalsVisibleTo("FaceCloudTests")]

namespace FaceCloudApp
{
    public class User
    {
        // Додано ініціалізацію за замовчуванням, щоб прибрати warning CS8618
        public string Username { get; set; } = string.Empty;
        
        // Змінено з private set на internal set для сумісності з тестами
        public int Warnings { get; internal set; }
        public bool IsBanned { get; internal set; }

        public void IncrementWarnings()
        {
            Warnings++;
        }

        public void SetBanned(bool banned)
        {
            IsBanned = banned;
        }

        public void ResetWarnings()
        {
            Warnings = 0;
        }
    }

    public class ModerationService
    {
        private const int MaxWarningsBeforeBan = 3;
        private const int MaxPostLength = 500;

        // МЕТОД 1: Додавання варну
        public void AddWarning(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }

            if (user.IsBanned) return;

            user.IncrementWarnings();

            if (user.Warnings >= MaxWarningsBeforeBan)
            {
                user.SetBanned(true);
                Console.WriteLine($"[MODERATION] Користувача {user.Username} було автоматично забанено за {MaxWarningsBeforeBan} варни.");
            }
        }

        // МЕТОД 2: Перевірка можливості публікації
        public bool CanPostContent(User user, string content)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (user.IsBanned) return false;
            if (string.IsNullOrWhiteSpace(content)) return false;
            
            if (content.Length > MaxPostLength)
            {
                Console.WriteLine($"[WARN] Помилка валідації: довжина контенту користувача перевищує максимально допустимий ліміт системи FaceCloud.");
                return false;
            }

            return true;
        }

        // МЕТОД 3: Масовий розбан користувачів
        public int MassUnban(IEnumerable<User> users)
        {
            if (users == null) throw new ArgumentNullException(nameof(users));
            
            int unbannedCount = 0;

            foreach (var user in users)
            {
                if (user != null && user.IsBanned)
                {
                    user.SetBanned(false);
                    user.ResetWarnings();
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