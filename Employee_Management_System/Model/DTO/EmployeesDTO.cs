using System.ComponentModel.DataAnnotations.Schema;

namespace Employee_Management_System.Model.DTO
{
    public class EmployeesDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Eamil { get; set; }
        public string phone { get; set; }
        public string Qualification { get; set; }

 //       [ForeignKey("Department")]
        public int Department_Id { get; set; }
        public DepartmentDTO Department { get; set; }

 //       [ForeignKey("Designation")]
        public int Designation_Id { get; set; }
        public DesignationDTO Designation { get; set; }
    }
}
