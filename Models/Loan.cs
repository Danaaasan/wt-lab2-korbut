namespace WtLab2Korbut.Models;

public class Loan : BaseEntity
{
    public BankClient? Borrower { get; set; }
    public decimal Amount { get; set; }
    public decimal Rate { get; set; }
    public int TermMonths { get; set; }

    public Loan(int id, decimal amount, decimal rate, int termMonths) : base(id)
    {
        Amount = amount;
        Rate = rate;
        TermMonths = termMonths;
    }

    public decimal MonthlyPayment()
    {
        var m = Rate / 12m;
        if (m == 0) return Amount / TermMonths;
        var k = (double)m * Math.Pow(1 + (double)m, TermMonths) /
                (Math.Pow(1 + (double)m, TermMonths) - 1);
        return Amount * (decimal)k;
    }

    public override void PrintInfo()
    {
        Console.WriteLine($"Кредит #{Id}: {Amount:C} на {TermMonths} мес., ставка {Rate:P}, " +
                          $"платёж {MonthlyPayment():C}, заёмщик {Borrower?.FullName}");
    }
}