using Employee_Management_System.Model;

namespace Employee_Management_System.Repository.IRepository
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<Department> UpdateAsync(Department entity);
    }
}
