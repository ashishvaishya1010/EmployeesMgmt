
using Employee_Management_System.Model.DTO;
using Employee_Management_System.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Azure;
using System.Net;
using AutoMapper;
using Employee_Management_System.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Employee_Management_System.Controllers
{

    [Route("api/DesignationController")]
    [ApiController]
    public class DesignationController : ControllerBase
    {

        private readonly IDesignationRepository _dbDesignation;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public DesignationController(IDesignationRepository dbUser, IMapper mapper)
        {
            _dbDesignation = dbUser;
            _mapper = mapper;
            _response = new();

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse>> GetDesignation()
        {
            try
            {
                IEnumerable<Designation> designations = await _dbDesignation.GetAllAsync();
                _response.Result = _mapper.Map<List<DesignationDTO>>(designations);
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

        [HttpGet("{id:int}", Name = "GetDesignation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<APIResponse>> GetDesignation(int id)
        {
            try
            {
                if (id == 0)
                {

                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);

                }

                var designation = await _dbDesignation.GetAsync(u => u.Id == id);
                if (designation == null)
                {

                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<DesignationDTO>(designation);
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
        public async Task<ActionResult<APIResponse>> CreateDesignation([FromBody] DesignationCreateDTO designationCreateDTO)
        {
            try
            {
                if (await _dbDesignation.GetAsync(u => u.Designation_Name.ToLower() == designationCreateDTO.Designation_Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Designation already exists!");
                    return BadRequest(ModelState);
                }
                if (designationCreateDTO == null)
                {
                    return BadRequest();
                }

                Designation designation = _mapper.Map<Designation>(designationCreateDTO);
                await _dbDesignation.CreateAsync(designation);
                _response.Result = _mapper.Map<DesignationDTO>(designation);
                _response.StatusCode = HttpStatusCode.OK;
                return CreatedAtRoute("GetUser", new { id = designation.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPut("{id:int}", Name = "UpdateDesignation")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateDesignation(int id, [FromBody] DesignationUpdateDTO designationUpdateDTO)
        {
            try
            {
                if (designationUpdateDTO == null || id != designationUpdateDTO.Id)
                {
                    return BadRequest();
                }

                Designation model = _mapper.Map<Designation>(designationUpdateDTO);



                await _dbDesignation.UpdateAsync(model);
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

        [HttpDelete("{id:int}", Name = "DeleteDesignation")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteDesignation(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var designation = await _dbDesignation.GetAsync(u => u.Id == id);
                if (designation == null)
                {
                    return NotFound();
                }

                await _dbDesignation.RemoveAsync(designation);
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


    }
}
