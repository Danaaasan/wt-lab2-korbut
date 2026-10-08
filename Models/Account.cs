namespace WtLab2Korbut.Models;

public abstract class Account : BaseEntity
{
    public string Number { get; init; } = string.Empty;
    public BankClient? Owner { get; set; }

    public decimal Balance { get; private set; }   // ← инкапсуляция

    protected Account(int id, string number, decimal initialBalance) : base(id)
    {
        Number = number;
        Balance = initialBalance;
    }

    public abstract decimal InterestRate { get; }   // ← контракт для наследников

    public virtual void Deposit(decimal amount)
    {
        EnsurePositive(amount, nameof(amount));
        Balance += amount;
    }

    public virtual bool Withdraw(decimal amount)
    {
        EnsurePositive(amount, nameof(amount));
        if (amount > Balance) return false;
        Balance -= amount;
        return true;
    }

    public override void PrintInfo()
    {
        Console.WriteLine($"{GetType().Name,-16} №{Number} | {Owner?.FullName,-18} | " +
                          $"Баланс: {Balance,12:C} | Ставка: {InterestRate:P}");
    }

    public override string ToString() => $"{GetType().Name} №{Number}";
}