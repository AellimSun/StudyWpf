using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfCaliburnApp.ViewModels
{
    public class MainViewModel : Screen
    {
        string name;

        //public string Name
        //{
        //    get => name;
        //    set
        //    {
        //        name = value;
        //        NotifyOfPropertyChange(() => Name);
        //        NotifyOfPropertyChange(() => CanSayHello);
        //    }
        //}
        //public bool CanSayHello
        //{
        //    get => !string.IsNullOrEmpty(Name); //null이면 false, null아니면 true return
        //}
        //public void SayHello()
        //{
        //    MessageBox.Show($"Hello~ {Name}");
        //}
    }
}
