using AutoMapper;
using Employee_Management_System.Model;
using Employee_Management_System.Model.DTO;
using Employee_Management_System.Repository.IRepository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Employee_Management_System.Repository
{
    public class HomeRepository : Repository<User>, IHomeRepository
    {
        private readonly ApplicationDbContext _db;

        private readonly IMapper _mapper;
        private string? secretKey;
        public HomeRepository(ApplicationDbContext db, IMapper mapper, IConfiguration configuration) : base(db)
        {
            _db = db;
            _mapper = mapper;
            secretKey = configuration.GetValue<string>("AppSettings:Secret");
        }



        public bool IsUniqueUser(string username)
        {
            var user = _db.users.FirstOrDefault(x => x.Name == username);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _db.users.FirstOrDefault(u => u.Name.ToLower() == loginRequestDTO.UserName.ToLower()
            && u.Password == loginRequestDTO.Password);
            if (user == null)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token = tokenHandler.WriteToken(token),

                User = _mapper.Map<User>(user),

            };
            return loginResponseDTO;


        }

        public async Task<User> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            User user = new()
            {
                Name = registrationRequestDTO.UserName,
                Password = registrationRequestDTO.Password,
                Email = registrationRequestDTO.Email,
                Role = registrationRequestDTO.Role
            };
            _db.users.Add(user);
            await _db.SaveChangesAsync();
            user.Password = "";
            return user;
        }
        public async Task<User> UpdateAsync(User entity)
        {
            _db.users.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}

