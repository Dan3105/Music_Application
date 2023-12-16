namespace MusicManager.Repsitory
{
    public interface IRepoUser
    {
        public Task<Model.User> GetUser(int id);
        public Task<IEnumerable<Model.User>> GetUsers();
        public Task<bool> PatchUser(Model.User user);
        public Task<IEnumerable<Model.Role>> GetRoles();
    }
}
