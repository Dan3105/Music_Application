namespace UserService.Repository
{
    public interface IRepository<T> 
    {
        public bool Update(T entity);
        public bool Delete(T entity);
        public bool SaveChanges();
    }
}
