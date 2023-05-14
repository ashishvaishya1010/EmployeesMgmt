using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Model.DTO
{
    public class DesignationUpdateDTO
    {

        public int Id { get; set; }
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        public string Designation_Code { get; set; }
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Only Alphabets allowed.")]
        public string Designation_Name { get; set; }
    }
}
