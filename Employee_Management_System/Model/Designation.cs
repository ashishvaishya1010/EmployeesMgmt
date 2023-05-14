using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee_Management_System.Model
{
    public class Designation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
//        [ForeignKey("Empolyees")]
        public string Designation_Code { get; set; }
        
        public string Designation_Name { get; set; }
    }
}
