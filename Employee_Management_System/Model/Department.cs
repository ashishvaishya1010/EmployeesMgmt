using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee_Management_System.Model
{
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
 //       [ForeignKey("Empolyees")]
        public string Department_Code { get; set; }

        public string Department_Name { get; set; }
    }
}
