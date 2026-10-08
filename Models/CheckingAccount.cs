namespace WtLab2Korbut.Models;

public class CheckingAccount : Account
{
    public override decimal InterestRate => 0.005m;
    public decimal OverdraftLimit { get; set; }

    public CheckingAccount(int id, string number, decimal balance, decimal overdraft = 0)
        : base(id, number, balance)
    {
        OverdraftLimit = overdraft;
    }

    public override bool Withdraw(decimal amount)
    {
        EnsurePositive(amount, nameof(amount));
        if (amount > Balance + OverdraftLimit) return false;
        base.Withdraw(Math.Min(amount, Balance));
        return true;
    }
}