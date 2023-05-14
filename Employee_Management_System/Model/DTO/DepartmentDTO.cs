using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Model.DTO
{
    public class DepartmentDTO
    {
        public int Id { get; set; }
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]

        public string Department_Code { get; set; }
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Only Alphabets allowed.")]
        public string Department_Name { get; set; }
    }
}
