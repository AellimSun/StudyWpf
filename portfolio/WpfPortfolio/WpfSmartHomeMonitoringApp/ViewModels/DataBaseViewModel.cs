using Caliburn.Micro;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using WpfSmartHomeMonitoringApp.Helpers;
using WpfSmartHomeMonitoringApp.Models;

namespace WpfSmartHomeMonitoringApp.ViewModels
{
    public class DataBaseViewModel : Screen
    {
        private string brokeUrl;
        private string topic;
        private string coonString;
        private string dbLog;
        private bool isConnected;

        public string BrokeUrl
        {
            get => brokeUrl;
            set
            {
                brokeUrl = value;
                NotifyOfPropertyChange(() => BrokeUrl);
            }
        }
        public string Topic
        {
            get => topic;
            set
            {
                topic = value;
                NotifyOfPropertyChange(() => Topic);
            }
        }
        public string ConnString
        {
            get => coonString;
            set
            {
                coonString = value;
                NotifyOfPropertyChange(() => ConnString);
            }
        }

        public string DbLog
        { 
            get => dbLog;
            set
            {
                dbLog = value;
                NotifyOfPropertyChange(() => DbLog);
            }
        }
        public bool IsConnected
        {
            get => isConnected;
            set
            {
                isConnected = value;
                NotifyOfPropertyChange(() => IsConnected);
            }
        }

        public DataBaseViewModel()
        {
            BrokeUrl = Commons.BROKERHOST = "localhost";
            Topic = Commons.PUB_TOPIC = "home/device/#"; // multipleTopic
            ConnString = Commons.CONNSTRING = "Data Source=PC01;Initial Catalog=OpenApiLab;Integrated Security=True";

            //Topic = Commons.PUB_TOPIC = "home/+/#"; :
            //Single Level wildcard : +
            //Multi Level WildCard : #

            if (Commons.IS_CONNECT)
            {
                IsConnected = true;
                ConnectDb();
            }
        }
        /// <summary>
        /// DB연결 + MQTT Broker 접속
        /// </summary>
        public void ConnectDb()
        {
            if(IsConnected)
            {
                Commons.MQTT_CLIENT = new MqttClient(BrokeUrl);
                try
                {
                    if(Commons.MQTT_CLIENT.IsConnected != true)
                    {
                        Commons.MQTT_CLIENT.MqttMsgPublishReceived += MQTT_CLIENT_MqttMsgPublishReceived;
                        Commons.MQTT_CLIENT.Connect("MONITOR");
                        Commons.MQTT_CLIENT.Subscribe(new string[] { Commons.PUB_TOPIC },
                            new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE});
                        UpdateText(">>> MQTT Broker Connected");
                        IsConnected = Commons.IS_CONNECT = true;
                    }
                }
                catch (Exception ex)
                {
                    //pass
                }
            }
            else //접속끄기
            {
                try
                {
                    if(Commons.MQTT_CLIENT.IsConnected)
                    {
                        Commons.MQTT_CLIENT.MqttMsgPublishReceived -= MQTT_CLIENT_MqttMsgPublishReceived;
                        Commons.MQTT_CLIENT.Disconnect();
                        UpdateText(">>> MQTT Broker Disconnected...");
                        IsConnected = Commons.IS_CONNECT = false;
                    }
                }
                catch (Exception)
                {

                    //throw;
                }
            }
        }

        private void UpdateText(string msg)
        {
            DbLog += $"{msg}\n";
        }
        /// <summary>
        /// Subscribe한 메세지 처리 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MQTT_CLIENT_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Message);
            UpdateText(message);
            SetDataBase(message);
        }

        private void SetDataBase(string message)
        {
            var currDatas = JsonConvert.DeserializeObject<Dictionary<string,string>>(message);
            //
            //

            var smartHomeModel = new SmartHomeModel();
            smartHomeModel.DevId = currDatas["DevId"];
            smartHomeModel.CurrTime = DateTime.Parse(currDatas["CurrTime"]);
            smartHomeModel.Temp = double.Parse(currDatas["Temp"]);
            smartHomeModel.Humid = double.Parse(currDatas["Humid"]);

            Debug.WriteLine(currDatas);
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();

                string strInQ = @"INSERT INTO TblSmartHome
                                       (DevId
                                       ,CurrTime
                                       ,Temp
                                       ,Humid)
                                 VALUES
                                       (@DevId  
                                       ,@CurrTime
                                       ,@Temp   
                                       ,@Humid)";  

                try
                {
                    SqlCommand cmd = new SqlCommand(strInQ, conn);
                    SqlParameter parmDevId = new SqlParameter("@DevId", currDatas["DevId"]);
                    cmd.Parameters.Add(parmDevId);
                    SqlParameter parmCurrTime = new SqlParameter("@CurrTime", DateTime.Parse(currDatas["CurrTime"]));
                    cmd.Parameters.Add(parmCurrTime);
                    SqlParameter parmTemp = new SqlParameter("@Temp", currDatas["Temp"]);
                    cmd.Parameters.Add(parmTemp);
                    SqlParameter parmHumid = new SqlParameter("@Humid", currDatas["Humid"]);
                    cmd.Parameters.Add(parmHumid);

                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        UpdateText(">>> DB Inserted.");
                    }
                    else
                        UpdateText(">>> DB Failed!!!!!!");

                }
                catch (Exception ex)
                {
                    UpdateText($">>> DB Error! {ex.Message}");
                }
            }
        }
    }
}
