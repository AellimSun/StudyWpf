using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfSmartHomeMonitoringApp.ViewModels
{
    public class MainViewModel : Conductor<object> // Screen에는 ActivateItemAsyncr[] 메서드가 없음
    {
        public MainViewModel()
        {
            DisplayName = "SmartHome Monitoring v2.0";
        }
        public void LoadDataBaseView()
        {
            ActivateItemAsync(new DataBaseViewModel());
        }
        public void LoadRealTimeView()
        {
            ActivateItemAsync(new RealTimeViewModel());
        }
        public void LoadHistoryView()
        {
            ActivateItemAsync(new HistoryViewModel());
        }
        public void ExitProgram()
        {
            Environment.Exit(0);
        }
    }
}
