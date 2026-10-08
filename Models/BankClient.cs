namespace WtLab2Korbut.Models;

public class BankClient : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Passport { get; set; } = string.Empty;

    public BankClient(int id, string fullName, string passport) : base(id)
    {
        FullName = fullName;
        Passport = passport;
    }

    public override void PrintInfo()
    {
        Console.WriteLine($"Клиент #{Id}: {FullName}, паспорт {Passport}");
    }
}