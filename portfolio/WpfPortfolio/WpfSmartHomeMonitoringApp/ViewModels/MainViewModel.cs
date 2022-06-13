using Caliburn.Micro;
using System;
using System.Threading;
using System.Threading.Tasks;
using WpfSmartHomeMonitoringApp.Helpers;
using static WpfSmartHomeMonitoringApp.Views.CustomPopupView;

namespace WpfSmartHomeMonitoringApp.ViewModels
{
    public class MainViewModel : Conductor<object> // Screen에는 ActivateItemAsyncr[] 메서드가 없음
    {
        public MainViewModel()
        {
            DisplayName = "SmartHome Monitoring v2.0";
        }
        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            if (Commons.MQTT_CLIENT.IsConnected)
            {
                Commons.MQTT_CLIENT.Disconnect();
                Commons.MQTT_CLIENT = null;
            }//비활성화 처리
            return base.OnDeactivateAsync(close, cancellationToken);
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
        public void ExitToolBar()
        {
            Environment.Exit(0);
        }
        //  Start메뉴, 아이콘 눌렀을 때 처리할 이벤트
        public void PopInfoDialog()
        {
            TaskPopUp();
        }

        public void StartSubscribe()
        {
            TaskPopUp();
        }
        private void TaskPopUp()
        {
            //  CustomPopupView

            var winManager = new WindowManager();
            var result = winManager.ShowDialogAsync(new CustomPopupViewModel("New Broker"));

            if (result.Result == true)
            {
                ActivateItemAsync(new DataBaseViewModel());
            }
        }
        
        public void PopInfoView()
        {
            var winManager = new WindowManager();
            winManager.ShowDialogAsync(new CustomInfoViewModel("About"));
        }
    }
}
