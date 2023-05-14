using AutoMapper;
using Employee_Management_System.Model.DTO;
using Employee_Management_System.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Management_System.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeRepository _homeRepo;

        private readonly IMapper _mapper;
        public HomeController(IHomeRepository homeRepo, IMapper mapper)
        {

            _homeRepo = homeRepo;
            _mapper = mapper;

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var loginResponse = await _homeRepo.Login(model);
            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            return Ok(loginResponse);
        }
        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO model)
        //{
        //    bool ifUserNameUnique = _homeRepo.IsUniqueUser(model.UserName);
        //    if (!ifUserNameUnique)
        //    {
        //        return BadRequest(new { message = "Username already exists" });
        //    }
        //    var user = await _homeRepo.Register(model);
        //    if (user == null)
        //    {
        //        return BadRequest(new { message = "Error while registering" });
        //    }
        //    return Ok(model);
        //}

    }
}

