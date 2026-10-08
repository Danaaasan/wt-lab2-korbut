namespace WtLab2Korbut.Models;

public class SavingsAccount : Account
{
    public override decimal InterestRate => 0.05m;

    public SavingsAccount(int id, string number, decimal balance) : base(id, number, balance) { }

    public void Deposit(decimal amount, string comment)
    {
        Deposit(amount);
        Console.WriteLine($"  → Зачисление {amount:C} ({comment})");
    }
}