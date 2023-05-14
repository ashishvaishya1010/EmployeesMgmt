using Employee_Management_System.Model;
using Employee_Management_System.Repository.IRepository;

namespace Employee_Management_System.Repository
{
    public class EmployeesRepository : Repository<Employees>, IEmployeesRepository
    {
        private readonly ApplicationDbContext _db;
        public EmployeesRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Employees> UpdateAsync(Employees entity)
        {

            _db.empolyees.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}