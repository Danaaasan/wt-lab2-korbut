using WtLab2Korbut.Models;

namespace WtLab2Korbut.Services;

public class AccountService : IItemService<Account>
{
    private readonly List<Account> _list = new();
    private readonly Dictionary<int, Account> _items = new();

    public void Add(Account item)
    {
        if (_items.ContainsKey(item.Id))
            throw new ArgumentException($"Счёт с Id={item.Id} уже существует");

        _list.Add(item);
        _items[item.Id] = item;
    }

    public Account? GetById(int id)
    {
        return _items.TryGetValue(id, out var item) ? item : null;
    }

    public IEnumerable<Account> GetAll()
    {
        return _list;
    }

    public bool Remove(int id)
    {
        if (!_items.TryGetValue(id, out var item))
            return false;

        _items.Remove(id);
        _list.Remove(item);
        return true;
    }

    public async Task<List<Account>> LoadFromSourceAsync(CancellationToken ct = default)
    {
        await Task.Delay(1000, ct);
        return _list.ToList();
    }
}