using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Model.DTO
{
    public class UserCreateDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string  Password { get; set; }
        public string Role { get; set; }
       
    }
}

