namespace WtLab2Korbut.Models;

public class DepositAccount : Account
{
    public override decimal InterestRate => 0.12m;
    public DateTime MaturityDate { get; set; }

    public DepositAccount(int id, string number, decimal balance, DateTime maturity)
        : base(id, number, balance)
    {
        MaturityDate = maturity;
    }

    public override bool Withdraw(decimal amount)
    {
        if (DateTime.Now < MaturityDate)
        {
            Console.WriteLine("  ! Срок вклада не истёк — снятие запрещено");
            return false;
        }
        return base.Withdraw(amount);
    }
}
