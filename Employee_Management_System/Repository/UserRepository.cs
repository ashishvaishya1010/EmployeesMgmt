using Employee_Management_System.Model;
using Employee_Management_System.Repository.IRepository;
using System.Collections;
using System.Linq.Expressions;

namespace Employee_Management_System.Repository
{
    public class UserRepository : Repository<User> , IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<User> UpdateAsync(User entity)
        {
            //entity.UpdatedDate = DateTime.Now;
            _db.users.Update(entity);
            await _db.SaveChangesAsync();
            return entity;

        }

        

    }
}