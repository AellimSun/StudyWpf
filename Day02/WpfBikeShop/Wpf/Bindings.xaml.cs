using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wpf
{
    /// <summary>
    /// Bindings.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Bindings : Page
    {
        public Bindings()
        {
            InitializeComponent();

            Car c = new Car()
            {
                Speed = 100,
                Color = Colors.Crimson,
                Driver = new Human()
                {
                    FirstName = "Nick",
                    HasDrivingLicense = true
                }
            };

            this.DataContext = c;
            //stxPanel.DataContext = c;        //아래 세 코드를 한번에 처리가능..?

            //txtSpeed.DataContext = c;      //데이터를 xaml으로 보내기 위함
            //txtColor.DataContext = c;      //데이터를 xaml으로 보내기 위함
            //txtFirstName.DataContext = c;      //데이터를 xaml으로 보내기 위함
            //txtSpeed.Text = c.Speed.ToString(); // 38번 코드 윈폼방식

            Color[] colors = {Colors.DarkSeaGreen, Colors.Black, Colors.LightCoral,Colors.BlueViolet,Colors.Maroon,
            Colors.GreenYellow, Colors.Red, Colors.Orange, Colors.Purple, Colors.Blue};
            var carlist = new List<Car>();
            for (int i = 0; i < 10; i++)
            {
                carlist.Add(new Car() {
                    Speed = (i+1) * 10,
                    Color = colors[i]
                });
            }
            lbxCars.DataContext = carlist;
        }
    }
}
