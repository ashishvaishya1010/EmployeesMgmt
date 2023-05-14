using Employee_Management_System.Model;

namespace Employee_Management_System.Repository.IRepository
{
    public interface IDesignationRepository : IRepository<Designation>
    {
        Task<Designation> UpdateAsync(Designation entity);
    }
}
