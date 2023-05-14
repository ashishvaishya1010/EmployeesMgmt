namespace Employee_Management_System.Model.DTO
{
    public class LoginResponseDTO
    {
        public User User { get; set; }
        public string Role { get; set; }
        public string Token { get; internal set; }
    }
}
