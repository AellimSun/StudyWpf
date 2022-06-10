using Caliburn.Micro;
using OxyPlot;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using WpfSmartHomeMonitoringApp.Helpers;
using WpfSmartHomeMonitoringApp.Models;

namespace WpfSmartHomeMonitoringApp.ViewModels
{
    public class HistoryViewModel : Screen
    {
        private BindableCollection<DivisionModel> divisions;
        private DivisionModel selectedDivision;
        private string startDate;
        private string endDate;
        private string initEndDate;
        private string initStartDate;
        private int totalCount;
        private PlotModel smartHomeModel;
        /*
             * Divisions
             * DivisionVal
             * SelectedDivision
             * StartDate
             * InitStartDate
             * EndDate
             * InitEndDate
             * TotalCount
             * SearchIoTData
             * SmartHomeModel
             */


        public BindableCollection<DivisionModel> Divisions
        {
            get => divisions;
            set
            {
                divisions = value;
                NotifyOfPropertyChange(() => Divisions);
            }
        }
        public DivisionModel SelectedDivision
        {
            get => selectedDivision;
            set
            {
                selectedDivision = value;
                NotifyOfPropertyChange(() => SelectedDivision);
            }
        }
        public string StartDate
        {
            get => startDate;
            set
            {
                startDate = value;
                NotifyOfPropertyChange(() => StartDate);
            }
        }
        public string EndDate
        {
            get => endDate;
            set
            {
                endDate = value;
                NotifyOfPropertyChange(() => EndDate);
            }
        }
        public string InitStartDate
        {
            get => initStartDate;
            set
            {
                initStartDate = value;
                NotifyOfPropertyChange(() => InitStartDate);
            }
        }
        public string InitEndDate
        {
            get => initEndDate;
            set
            {
                initEndDate = value;
                NotifyOfPropertyChange(() => InitEndDate);
            }
        }
        public int TotalCount
        {
            get => totalCount;
            set
            {
                totalCount = value;
                NotifyOfPropertyChange(() => TotalCount);
            }
        }
        public PlotModel SmartHomeModel
        {
            get => smartHomeModel;
            set
            {
                smartHomeModel = value;
                NotifyOfPropertyChange(() => SmartHomeModel);
            }
        }

        public HistoryViewModel()
        {
            Commons.CONNSTRING = "Data Source=PC01;Initial Catalog=OpenApiLab;Integrated Security=True";
            InitControl();
        }

        private void InitControl()
        {

            Divisions = new BindableCollection<DivisionModel>   //콤보박스용 데이터 생성
            {
                new DivisionModel {KeyVal = 0, DivisionVal = "-- SELECT --"},
                new DivisionModel {KeyVal = 1, DivisionVal = "DINING"},
                new DivisionModel {KeyVal = 2, DivisionVal = "LIVING"},
                new DivisionModel {KeyVal = 3, DivisionVal = "BED"},
                new DivisionModel {KeyVal = 4, DivisionVal = "BATH"}
            };
            //select 선택하여 초기화
            SelectedDivision = Divisions.Where(v => v.DivisionVal.Contains("SELECT")).FirstOrDefault();

            InitStartDate = DateTime.Now.ToShortDateString();
            InitEndDate = DateTime.Now.AddDays(1).ToShortDateString();
        }
        public void SearchIoTData()
        {
            //Validation Check
            if (SelectedDivision.KeyVal == 0)
            {
                MessageBox.Show("검색할 방을 선택하세요.");
                return;
            }
            if(DateTime.Parse(StartDate)>DateTime.Parse(EndDate))
            {
                MessageBox.Show("시작일이 종료일보다 앞설 수 없습니다.");
                return;
            }

            TotalCount = 0;

            using (SqlConnection conn = new SqlConnection(Commons.CONNSTRING))
            {
                string strQ = @"SELECT Id, CurrTime, Temp, Humid
                                FROM TblSmartHome
                                WHERE DevId = @DevId
                                 AND CurrTime BETWEEN @StartDate AND @EndDate
                                ORDER BY Id ASC";
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(strQ, conn);
                    SqlParameter paramDevId = new SqlParameter("@DevId", SelectedDivision.DivisionVal);
                    SqlParameter paramStartDate = new SqlParameter("@StartDate", StartDate);
                    SqlParameter paramEndDate = new SqlParameter("@EndDate", EndDate);

                    cmd.Parameters.Add(paramDevId);
                    cmd.Parameters.Add(paramStartDate);
                    cmd.Parameters.Add(paramEndDate);

                    SqlDataReader reader = cmd.ExecuteReader();

                    var i = 0;

                    while(reader.Read())
                    {
                        var temp = reader["Temp"];

                        i++;
                    }

                    TotalCount = i;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error : {ex}");
                    return;
                }
            }
        }
    }
}
