namespace SimpleDB {
    public interface IDatabase<T> {
        public IEnumerable<T> Read(int? limit = null);
        public void Store(T record);
    }
}