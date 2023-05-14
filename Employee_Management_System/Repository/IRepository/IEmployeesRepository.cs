using Employee_Management_System.Model;

namespace Employee_Management_System.Repository.IRepository
{
    public interface IEmployeesRepository : IRepository<Employees>
    {
        Task<Employees> UpdateAsync(Employees entity);
    }
}
