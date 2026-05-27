using System;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("=== FaceCloude ===");

        string username = "Ilona_Shestak";
        string bio = "Студентка ПЗПІ";
        int warnings = 0;

        Console.WriteLine("Користувач: " + username);
        Console.WriteLine("Про себе: " + bio);

        // Створення простого поста
        string postAuthor = username;
        string postContent = "Перший пост.";

        Console.WriteLine("\n--- Стрічка новин ---");
        Console.WriteLine(postAuthor + " написав: " + postContent);
    }
    // Функція для додавання варну користувачу. Якщо варнів більше 3, то користувач отримує бан.
    static void AddWarning(User user)
    {
        user.Warnings++;
        if (user.Warnings > 3)
        {
            BanUser(user);
        }
    }
}