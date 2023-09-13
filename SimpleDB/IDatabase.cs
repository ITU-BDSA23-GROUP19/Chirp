namespace SimpleDB;

public interface IDatabase<T> {
    public IEnumerable<T> Read();
    public void Store(T record);
}