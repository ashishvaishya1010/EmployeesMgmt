using Microsoft.AspNetCore.Identity;

namespace Employee_Management_System.Model
{
    public class ApplicationUser : IdentityUser
    {
        //public string UserName { get; set; }

        public string Name { get; set; }
    }
}
