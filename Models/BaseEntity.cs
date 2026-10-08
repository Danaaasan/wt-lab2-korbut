namespace WtLab2Korbut.Models;

public abstract class BaseEntity : IStorable
{
    public int Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; } = DateTime.Now;

    protected BaseEntity(int id)
    {
        Id = id;
    }

    protected void EnsurePositive(decimal amount, string paramName)
    {
        if (amount <= 0)
            throw new ArgumentException("Сумма должна быть положительной", paramName);
    }

    public virtual void PrintInfo()
    {
        Console.WriteLine($"[{GetType().Name}] Id={Id}, создан {CreatedAt:dd.MM.yyyy HH:mm}");
    }
}