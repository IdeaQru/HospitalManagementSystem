namespace HospitalManagementSystem.Domain.Repositories
{
    // Generic repository interface - base untuk semua repositories
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
