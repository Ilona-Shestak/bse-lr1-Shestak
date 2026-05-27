using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Hello World!");
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