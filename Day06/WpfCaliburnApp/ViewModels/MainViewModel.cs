using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfCaliburnApp.Models;

namespace WpfCaliburnApp.ViewModels
{
    public class MainViewModel : Screen
    {
        //private EmployeesModel employees; // VS가 제안하는 이름 사용 권장
        private BindableCollection<EmployeesModel> listEmployees;       //List 사용 지양. MVVM 호환성 문제인지 리스트가 다 나오지 않음
        public BindableCollection<EmployeesModel> ListEmployees
        { 
            get => listEmployees;
            set
            {
                listEmployees = value;
                NotifyOfPropertyChange(() => ListEmployees);
            }
        }
        string connString = "Data Source=PC01;Initial Catalog=OpenApiLab;Integrated Security=True";
        //서버탐색기 - 데이터연결(연결추가) - PC01 - '고급'
        public MainViewModel()
        {
            GetEmployees();
        }
        public void GetEmployees()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string strQuery = "SELECT * FROM TblEmployees";
                SqlCommand cmd = new SqlCommand(strQuery, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                ListEmployees = new BindableCollection<EmployeesModel>();

                while (reader.Read())
                {
                    var temp = new EmployeesModel
                    {
                        Id = (int)reader["Id"],
                        EmpName = reader["EmpName"].ToString(),
                        Salary = (decimal)reader["Salary"],
                        DeptName = reader["DeptName"].ToString(),
                        Destination = reader["Destination"].ToString()
                    };
                    ListEmployees.Add(temp);
                }
            }

        }
        int id;
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                NotifyOfPropertyChange(() => Id);
                NotifyOfPropertyChange(() => CanDelEmployee);
            }
        }
        string empName;
        public string EmpName
        {
            get { return empName; }
            set
            {
                empName = value;
                NotifyOfPropertyChange(() => EmpName);
                NotifyOfPropertyChange(() => CanSaveEmployee);
            }
        }
        decimal salary;
        public decimal Salary
        {
            get { return salary; }
            set
            {
                salary = value;
                NotifyOfPropertyChange(() => Salary);
                NotifyOfPropertyChange(() => CanSaveEmployee);
            }
        }
        string deptName;
        public string DeptName
        {
            get { return deptName; }
            set
            {
                deptName = value;
                NotifyOfPropertyChange(() => DeptName);
                NotifyOfPropertyChange(() => CanSaveEmployee);
            }
        }
        string destination;
        public string Destination
        {
            get { return destination; }
            set
            {
                destination = value;
                NotifyOfPropertyChange(() => Destination);
                NotifyOfPropertyChange(() => CanSaveEmployee);
            }
        }
        private EmployeesModel selectedEmployee;
        public EmployeesModel SelectedEmployee
        {
            get { return selectedEmployee; }
            set
            {
                selectedEmployee = value;
                if(value != null)
                {
                    Id = value.Id;
                    EmpName = value.EmpName;
                    Salary = value.Salary;
                    DeptName = value.DeptName;
                    Destination = value.Destination;
                }
                NotifyOfPropertyChange(() => SelectedEmployee);
                NotifyOfPropertyChange(() => CanSaveEmployee);
            }
        }
        public void NewEmployee()
        {
            Id = 0;
            EmpName = string.Empty;
            Salary = 0;
            DeptName = string.Empty;
            Destination = string.Empty;
        }
        //버튼 활성-비활성화 속성
        public bool CanSaveEmployee
        {
            get {
                return !string.IsNullOrEmpty(EmpName) &&
                       !string.IsNullOrEmpty(DeptName) &&
                       !string.IsNullOrEmpty(Destination) &&
                       Salary != 0;
            }
        }
        public void SaveEmployee()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                if (Id == 0)   //insert
                    cmd.CommandText = @"INSERT INTO TblEmployees
                                               (EmpName
                                               , Salary
                                               , DeptName
                                               , Destination)
                                         VALUES
                                               (@EmpName,
                                                @Salary,
                                                @DeptName,
                                                @Destination)";
                else           //update
                    cmd.CommandText = @"UPDATE TblEmployees
                                           SET EmpName = @EmpName
                                              , Salary = @Salary
                                              , DeptName = @DeptName
                                              , Destination = @Destination
                                         WHERE Id = @Id";

                SqlParameter paramEmpName = new SqlParameter("@EmpName",EmpName);
                SqlParameter paramSalary = new SqlParameter("@Salary", Salary);
                SqlParameter paramDeptName = new SqlParameter("@DeptName", DeptName);
                SqlParameter paramDestination = new SqlParameter("@Destination", Destination);
                cmd.Parameters.Add(paramEmpName);
                cmd.Parameters.Add(paramSalary);
                cmd.Parameters.Add(paramDeptName);
                cmd.Parameters.Add(paramDestination);

                if(Id != 0)
                {
                    SqlParameter parmId = new SqlParameter("@Id", Id);
                    cmd.Parameters.Add(parmId);
                }

                cmd.ExecuteNonQuery();
            }

            NewEmployee();
            GetEmployees();
        }

        public bool CanDelEmployee
        {
            get { return !(Id == 0); }  //Id에 값이 있으면 true
        }
        public void DelEmployee()
        {
            using(SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string strQuery = "DELETE FROM TblEmployees WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(strQuery, conn);
                SqlParameter paramId = new SqlParameter("@Id",Id);
                cmd.Parameters.Add(paramId);

                cmd.ExecuteNonQuery();
            }
            NewEmployee();
            GetEmployees();
        }
    }
}
