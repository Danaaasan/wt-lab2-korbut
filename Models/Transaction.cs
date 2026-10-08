namespace WtLab2Korbut.Models;

public enum TransactionKind { Deposit, Withdraw, Transfer }

public class Transaction : BaseEntity
{
    public int AccountId { get; set; }
    public decimal Amount { get; set; }
    public TransactionKind Kind { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;

    public Transaction(int id, int accountId, decimal amount, TransactionKind kind) : base(id)
    {
        AccountId = accountId;
        Amount = amount;
        Kind = kind;
    }

    public override void PrintInfo()
    {
        Console.WriteLine($"{Date:dd.MM.yyyy HH:mm} | {Kind,-9} | счёт #{AccountId} | {Amount:C}");
    }
}