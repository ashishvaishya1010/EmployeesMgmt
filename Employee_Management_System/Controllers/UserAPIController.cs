using Employee_Management_System.Model;
using Employee_Management_System.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.Identity.Client;
using Employee_Management_System.Model.DTO;
using Azure;
using System.Net;
using Employee_Management_System.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Employee_Management_System.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserAPIController : ControllerBase
    {
        private readonly IUserRepository _dbUser;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public UserAPIController(IUserRepository dbUser, IMapper mapper)
        {
            _dbUser = dbUser;
            _mapper = mapper;
            _response = new();

        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse>> GetUsers()
        {
            try
            {
                IEnumerable<User> users = await _dbUser.GetAllAsync();
                _response.Result = _mapper.Map<List<UserDTO>>(users);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { e.ToString() };
            }
            return _response;

        }

        [HttpGet("{id:int}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<APIResponse>> GetUser(int id)
        {
            try
            {
                if (id == 0)
                {

                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);

                }

                var user = await _dbUser.GetAsync(u => u.Id == id);
                if (user == null)
                {

                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);


                }

                _response.Result = _mapper.Map<UserDTO>(user);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateUsers([FromBody] UserCreateDTO userCreateDTO)
        {
            try
            {
                if (await _dbUser.GetAsync(u => u.Name.ToLower() == userCreateDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "User already exists!");
                    return BadRequest(ModelState);
                }
                if (userCreateDTO == null)
                {
                    return BadRequest();
                }

                User user = _mapper.Map<User>(userCreateDTO);
                await _dbUser.CreateAsync(user);
                _response.Result = _mapper.Map<UserDTO>(user);
                _response.StatusCode = HttpStatusCode.OK;
                return CreatedAtRoute("GetUser", new { id = user.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPut("{id:int}", Name = "UpdateUsers")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateUser(int id, [FromBody] UserUpdateDTO userUpdateDTO)
        {
            try
            {
                if (userUpdateDTO == null || id != userUpdateDTO.Id)
                {
                    return BadRequest();
                }

                User model = _mapper.Map<User>(userUpdateDTO);



                await _dbUser.UpdateAsync(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete("{id:int}", Name = "DeleteUser")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteUsers(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var user = await _dbUser.GetAsync(u => u.Id == id);
                if (user == null)
                {
                    return NotFound();
                }

                await _dbUser.RemoveAsync(user);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;

        }


        //private readonly ApplicationDbContext _db;
        //public UserAPIController(ApplicationDbContext db)
        //{
        //    _db = db;
        //}

        // Get all User [HttpGet]
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        //{
        //    //return Ok(await _dbUser.users.ToListAsync());
        //    IEnumerable<User> users = await _dbUser.GetAllAsync();
        //    _response.Result = _mapper.Map<List<UserDTO>>(users);
        //}

        //// Get User by Id [HttpGet]
        //[HttpGet("Id:int")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult> GetUsers(int id)
        //{
        //    if (id == 0)
        //    {
        //        return BadRequest();
        //    }
        //    var user = await _dbUser.users.FirstOrDefaultAsync(u => u.Id == id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(user);
        //}

        //// Create a User [HttpPost]
        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<ActionResult<UserDTO>> CreateUser([FromBody] UserCreateDTO userDTO)
        //{
        //    // Custom validation for User's Name
        //    if (await _dbUser.users.FirstOrDefaultAsync(u => u.Name.ToLower() == userDTO.Name.ToLower()) != null)
        //    {
        //        ModelState.AddModelError("", "User Name Already Exists");
        //        return BadRequest(ModelState);
        //    }
        //    User model = new()
        //    {
        //        Name = userDTO.Name,
        //        Email = userDTO.Email,
        //        Password = userDTO.Password,
        //        Role = userDTO.Role
        //    };
        //    await _dbUser.users.AddAsync(model);
        //    await _dbUser.SaveChangesAsync();

        //    return Ok(userDTO);
        //}

        //// Delete a User [HttpDelete] based on Id
        //[HttpDelete("{id:int}", Name = "DeleteUser")]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> DeleteUser(int id)
        //{
        //    if (id == 0)
        //    {
        //        return BadRequest();
        //    }
        //    var user = await _dbUser.users.FirstOrDefaultAsync(u => u.Id == id);
        //    if (user == null)
        //    {
        //        return NotFound();

        //    }
        //    _dbUser.users.Remove(user);
        //    await _dbUser.SaveChangesAsync();
        //    return NoContent();
        //}

        //// Update a User Data [HttpPut] based on id
        //[HttpPut]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDTO userDTO)
        //{
        //    if (userDTO == null || id != userDTO.Id)
        //    {
        //        return BadRequest();
        //    }
        //    User model = new()
        //    {
        //        Id = userDTO.Id,
        //        Name = userDTO.Name,
        //        Email = userDTO.Email,
        //        Password = userDTO.Password,
        //        Role = userDTO.Role
        //    };
        //    _dbUser.users.Update(model);
        //    await _dbUser.SaveChangesAsync();
        //    return NoContent();
        //}


        ////public async IEnumerable<User> GetUsers()
        ////{
        ////    try
        ////    {
        ////        //IEnumerable<User> userList = await _dbUser.GetAllAsync();
        ////        IEnumerable<User> userList = await _dbUser.GetAllAsync();
        ////        _response.Result = _mapper.Map<List<UserDTO>>(userList);
        ////        _response.StatusCode = HttpStatusCode.OK;
        ////        return Ok(_response);
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        _response.IsSuccess = false;
        ////        _response.ErrorMessages = new List<string> { ex.ToString() };
        ////    }
        ////    return _response;
        ////}

    }



    //[HttpPost]
    //public ActionResult<UserDTO> CreateUser([FromBody] UserDTO userDTO) 
    //{ 
    //  if (userDTO == null)
    //    {
    //        return BadRequest(userDTO);
    //    }
    //  if 
    //  }




}

