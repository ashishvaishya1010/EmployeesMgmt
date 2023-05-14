using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee_Management_System.Model
{
    public class Employees
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Eamil { get; set; }
        public string  phone { get; set; }
        //public string Department { get; set; }
        //public string Designation { get; set; }
        public string Qualification { get; set; }

        [ForeignKey("Department")]
        public int Department_Id { get; set; }
        public Department Department { get; set; }

        [ForeignKey("Designation")]
        public int Designation_Id { get; set; }
        public Designation Designation { get; set; }
    }
}
