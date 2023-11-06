namespace MusicServerAPI.Repository
{
    public interface IRepository<T> 
    {
        public void Update(T entity);
        public void Delete(T entity);

    }
}
