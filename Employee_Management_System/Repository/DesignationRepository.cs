using Employee_Management_System.Model;
using Employee_Management_System.Repository.IRepository;

namespace Employee_Management_System.Repository
{
    public class DesignationRepository : Repository<Designation>, IDesignationRepository
    {
        private readonly ApplicationDbContext _db;
        public DesignationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Designation> UpdateAsync(Designation entity)
        {
            
            _db.designations.Update(entity);
            await _db.SaveChangesAsync();
            return entity;

        }
    }
}
