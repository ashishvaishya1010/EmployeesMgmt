namespace Employee_Management_System.Model.DTO
{
    public class EmployeesUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Eamil { get; set; }
        public string phone { get; set; }
        //public string Department { get; set; }
        //public string Designation { get; set; }
        public string Qualification { get; set; }
        public int Department_Id { get; set; }
        public int Designation_Id { get; set; }
    }
}
