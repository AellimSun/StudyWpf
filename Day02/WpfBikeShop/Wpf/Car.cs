using System.Windows.Media;

namespace Wpf
{
    public class Car : Notifier
    {
        private double speed {
            get { return speed; }
            set { speed = value;
                OnPropertyChanged("Speed"); //speed 속성의 값이 변경되었습니다.
            } }
        public double Speed { get; set; }
        public Color Color { get; set; }

        public Human Driver { get; set; }
        public Car()
        {

        }
        
    }
    public class Human
    {
        public string FirstName { get; set; }
        public bool HasDrivingLicense { get; set; }
    }
}