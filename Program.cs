using WtLab2Korbut.Models;
using WtLab2Korbut.Services;

namespace WtLab2Korbut;

class Program
{
    static async Task Main()
    {
        Console.WriteLine("=====================================================");
        Console.WriteLine("  Банковская система — ЛР №2 (вариант 4)");
        Console.WriteLine("=====================================================");

        IItemService<Account> service = new AccountService();
        Seed(service);

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\n--- МЕНЮ ---");
            Console.WriteLine("1. Показать все счета");
            Console.WriteLine("2. Найти счёт по Id (TryGetValue)");
            Console.WriteLine("3. Пополнить счёт");
            Console.WriteLine("4. Снять средства");
            Console.WriteLine("5. Статистика (LINQ)");
            Console.WriteLine("6. Асинхронная загрузка счетов");
            Console.WriteLine("7. Параллельные задачи + отмена");
            Console.WriteLine("0. Выход");
            Console.Write("Выбор: ");

            switch (Console.ReadLine())
            {
                case "1": ShowAll(service); break;
                case "2": FindById(service); break;
                case "3": Deposit(service); break;
                case "4": Withdraw(service); break;
                case "5": Statistics(service); break;
                case "6": await LoadAsync(service); break;
                case "7": await ParallelAsync(); break;
                case "0": exit = true; break;
                default: Console.WriteLine("Неверный пункт меню"); break;
            }
        }
        Console.WriteLine("Работа завершена.");
    }

    static void Seed(IItemService<Account> service)
    {
        var client1 = new BankClient(1, "Моргенштерн И.И.", "MP1234567");
        var client2 = new BankClient(2, "Искусственный Интелект А.С.", "MP7654321");
        var client3 = new BankClient(3, "Кишиба Рохан", "MP1112223");

        client1.PrintInfo();

        var a1 = new SavingsAccount(1, "BY01-0001", 1200m) { Owner = client1 };
        var a2 = new CheckingAccount(2, "BY02-0002", 450m, 500m) { Owner = client2 };
        var a3 = new DepositAccount(3, "BY03-0003", 7000m, DateTime.Now.AddMonths(6)) { Owner = client3 };
        var a4 = new SavingsAccount(4, "BY01-0004", 300m) { Owner = client2 };
        var a5 = new CheckingAccount(5, "BY02-0005", 2500m) { Owner = client1 };

        Account[] demo = { a1, a2, a3, a4, a5 };
        foreach (var acc in demo) service.Add(acc);

        a1.Deposit(200m, "премия");
    }

    static void ShowAll(IItemService<Account> service)
    {
        Console.WriteLine("\n--- Все счета (foreach) ---");
        foreach (var acc in service.GetAll())
            acc.PrintInfo();
    }

    static void FindById(IItemService<Account> service)
    {
        Console.Write("Введите Id счёта: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Некорректный Id");
            return;
        }
        var acc = service.GetById(id);
        if (acc is null) Console.WriteLine("Счёт не найден");
        else acc.PrintInfo();
    }

    static void Deposit(IItemService<Account> service)
    {
        Console.Write("Id счёта: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) return;
        var acc = service.GetById(id);
        if (acc is null) { Console.WriteLine("Счёт не найден"); return; }

        Console.Write("Сумма пополнения: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal sum)) return;

        try { acc.Deposit(sum); Console.WriteLine($"Новый баланс: {acc.Balance:C}"); }
        catch (ArgumentException ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
    }

    static void Withdraw(IItemService<Account> service)
    {
        Console.Write("Id счёта: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) return;
        var acc = service.GetById(id);
        if (acc is null) { Console.WriteLine("Счёт не найден"); return; }

        Console.Write("Сумма снятия: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal sum)) return;

        Console.WriteLine(acc.Withdraw(sum)
            ? $"Успешно. Баланс: {acc.Balance:C}"
            : "Операция отклонена (недостаточно средств или условия вклада)");
    }

    static void Statistics(IItemService<Account> service)
    {
        var all = service.GetAll().ToList();

        Console.WriteLine($"\nКоличество счетов: {all.Count}");
        Console.WriteLine($"Суммарный баланс : {all.Sum(a => a.Balance):C}");
        Console.WriteLine($"Средний баланс   : {all.Average(a => a.Balance):C}");
        Console.WriteLine($"Мин. баланс      : {all.Min(a => a.Balance):C}");
        Console.WriteLine($"Макс. баланс     : {all.Max(a => a.Balance):C}");

        Console.WriteLine("\n--- Баланс > 1000, сортировка по убыванию ---");
        var rich = all
            .Where(a => a.Balance > 1000)
            .OrderByDescending(a => a.Balance)
            .Select(a => new { a.Number, a.Balance, Owner = a.Owner?.FullName });

        foreach (var r in rich)
            Console.WriteLine($"{r.Number,-12} {r.Balance,12:C}  {r.Owner}");

        Console.WriteLine("\n--- Группировка по типу счёта ---");
        var groups = all
            .GroupBy(a => a.GetType().Name)
            .Select(g => new
            {
                Type = g.Key,
                Count = g.Count(),
                Total = g.Sum(a => a.Balance),
                Avg = g.Average(a => a.Balance)
            });

        foreach (var g in groups)
            Console.WriteLine($"{g.Type,-18} шт={g.Count}  сумма={g.Total,12:C}  средний={g.Avg,12:C}");
    }

    static async Task LoadAsync(IItemService<Account> service)
    {
        Console.WriteLine("\n--- Асинхронная загрузка ---");
        var loaded = await service.LoadFromSourceAsync();
        foreach (var acc in loaded) acc.PrintInfo();
    }

    static async Task ParallelAsync()
    {
        Console.WriteLine("\n--- Параллельные задачи ---");
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(2000);

        var t1 = LoadPartAsync("Счета", 1200, cts.Token);
        var t2 = LoadPartAsync("Транзакции", 1800, cts.Token);
        var t3 = LoadPartAsync("Кредиты", 2500, cts.Token);

        try
        {
            await Task.WhenAll(t1, t2, t3);
            Console.WriteLine("Все задачи выполнены");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Часть задач отменена по таймауту");
        }
    }

    static async Task LoadPartAsync(string name, int delayMs, CancellationToken ct)
    {
        await Task.Delay(delayMs, ct);
        Console.WriteLine($"  {name}: загружено (за {delayMs} мс)");
    }
}