﻿using AutoMapper;
using Employee_Management_System.Model;
using Employee_Management_System.Model.DTO;
using Employee_Management_System.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;
using System.Reflection.PortableExecutable;

namespace Employee_Management_System.Controllers
{
    [Route("api/EmployeeController")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeesRepository _dbEmployees;
        private readonly IDesignationRepository _dbDesignation;
        private readonly IDepartmentRepository _dbDepartment;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        private ApplicationDbContext db;
        public EmployeeController(IEmployeesRepository dbUser, IMapper mapper, ApplicationDbContext dbContext, 
            IDesignationRepository dbDesignation, IDepartmentRepository dbDepartment)
        {
            _dbEmployees = dbUser;
            _mapper = mapper;
            _response = new();
            db = dbContext;
            _dbDesignation = dbDesignation;
            _dbDepartment = dbDepartment;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<APIResponse>> GetEmployees()
        {
            try
            {
                IEnumerable<Employees> employees = await _dbEmployees.GetAllAsync();
                _response.Result = _mapper.Map<List<EmployeesDTO>>(employees);
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

        [HttpGet("{id:int}", Name = "GetEmployees")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<APIResponse>> GetEmployees(int id)
        {
            try
            {
                if (id == 0)
                {

                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);

                }

                var employees = await _dbEmployees.GetAsync(u => u.Id == id);
                if (employees == null)
                {

                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<EmployeesDTO>(employees);
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
        public async Task<ActionResult<APIResponse>> CreateEmployees([FromBody] EmployeesCreateDTO employeesCreateDTO)
        {
            try
            {
                if (await _dbEmployees.GetAsync(u => u.Name.ToLower() == employeesCreateDTO.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Employees already exists!");
                    return BadRequest(ModelState);
                }
                if (employeesCreateDTO == null)
                {
                    return BadRequest();
                }
                if (await _dbDesignation.GetAsync(u => u.Id == employeesCreateDTO.Designation_Id) == null)
                {
                    return NotFound();
                }
                if (await _dbDepartment.GetAsync(u => u.Id == employeesCreateDTO.Department_Id) == null)
                {
                    return NotFound();
                }
                    Employees employees = _mapper.Map<Employees>(employeesCreateDTO);
                await _dbEmployees.CreateAsync(employees);
                _response.Result = _mapper.Map<EmployeesDTO>(employees);
                _response.StatusCode = HttpStatusCode.OK;
                return CreatedAtRoute("GetUser", new { id = employees.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateEmployees(int id, [FromBody] EmployeesUpdateDTO employeesUpdateDTO)
        {
            try
            {
                if (employeesUpdateDTO == null || id != employeesUpdateDTO.Id)
                {
                    return BadRequest();
                }
                var employee = await _dbEmployees.GetAsync(v=>v.Id ==id, tracked: true);
                if (employee == null) {
                    return NotFound();
                }
                //var designation = await _dbDesignation.GetAsync(v=> v.Id == employeesUpdateDTO.Designation_Id);
                if (await _dbDesignation.GetAsync(u => u.Id == employeesUpdateDTO.Designation_Id) == null) {
                    return NotFound();
                }
                if (await _dbDepartment.GetAsync(u => u.Id == employeesUpdateDTO.Department_Id) == null)
                {
                    return NotFound();
                }


                db.Entry(employee).State = EntityState.Detached;

                Employees model = _mapper.Map<Employees>(employeesUpdateDTO);

                db.Entry(model).State = EntityState.Modified;

                await _dbEmployees.UpdateAsync(model);
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

        [HttpDelete("{id:int}", Name = "DeleteEmployees")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteEmployees(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var employees = await _dbEmployees.GetAsync(u => u.Id == id);
                if (employees == null)
                {
                    return NotFound();
                }

                await _dbEmployees.RemoveAsync(employees);
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
