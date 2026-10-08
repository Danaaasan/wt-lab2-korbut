using WtLab2Korbut.Models;

namespace WtLab2Korbut;

public interface IItemService<T> where T : BaseEntity
{
    void Add(T item);
    T? GetById(int id);
    IEnumerable<T> GetAll();
    bool Remove(int id);
    Task<List<T>> LoadFromSourceAsync(CancellationToken ct = default);
}
