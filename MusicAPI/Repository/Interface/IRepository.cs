namespace MusicAPI.Repository.Interface
{
    public partial interface IRepository<T>
    {
        public bool Create(T entity);
        public bool Update(T entity);
        public bool Delete(T entity);

    }
}
