using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Model.DTO
{
    public class RegistrationRequestDTO
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
