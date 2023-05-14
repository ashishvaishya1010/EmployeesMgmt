using Employee_Management_System.Model;

namespace Employee_Management_System.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        //Task<IEnumerable<User>> GetAllAsync();
        //Task GetAsync(Func<object, bool> value);
        Task<User> UpdateAsync(User entity);




    }
}
