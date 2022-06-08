namespace WpfCaliburnApp.Models
{
    /// <summary>
    /// Database의 Table과 같은 이름으로 사용하면 실수 줄일 수 있음
    /// </summary>
    public class EmployeesModel
    {
        public int Id { get; set; }
        public string EmpName { get; set; }
        public decimal Salary { get; set; }
        public string DeptName { get; set; }
        public string Destination { get; set; }
    }
}
