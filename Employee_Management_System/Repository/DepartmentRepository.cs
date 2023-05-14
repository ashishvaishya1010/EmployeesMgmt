using Employee_Management_System.Model;
using Employee_Management_System.Repository.IRepository;

namespace Employee_Management_System.Repository
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        private readonly ApplicationDbContext _db;
        public DepartmentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Department> UpdateAsync(Department entity)
        {

            _db.departments.Update(entity);
            await _db.SaveChangesAsync();
            return entity;

        }
    }
}

