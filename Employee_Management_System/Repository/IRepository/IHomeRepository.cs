using Employee_Management_System.Model;
using Employee_Management_System.Model.DTO;

namespace Employee_Management_System.Repository.IRepository
{
    public interface IHomeRepository : IRepository<User>
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<User> Register(RegistrationRequestDTO registrationRequestDTO);
        Task<User> UpdateAsync(User entity);

    }
}
