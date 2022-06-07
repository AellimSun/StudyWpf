using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfMvvmApp.Models;

namespace WpfMvvmApp.ViewModels
{
    /// <summary>
    /// View에서 쓰기 위함
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private string inFirstName;
        private string inLastName;
        private string inEmail;
        private DateTime inDate;

        private string outFirstName;
        private string outLastName;
        private string outEmail;
        private string outDate;

        private string outAdult;
        private string outBirthday;
        private string outFullName;
       
        public string InFirstName
        {
            get => inFirstName;
            set
            {
                inFirstName = value;
                RaisePropertyChanged("InFirstName");    // 값이 바뀜 공지!!
            }
        }
        public string InLastName
        {
            get => inLastName;
            set
            {
                inLastName = value;
                RaisePropertyChanged("InLastName");
            }
        }

        public string InEmail
        {
            get => inEmail;
            set
            {
                inEmail = value;
                RaisePropertyChanged("InEmail");    // 값이 바뀜 공지!!
            }
        }
        public DateTime InDate
        {
            get { return inDate; }
            set
            {
                inDate = value;
                RaisePropertyChanged("InDate");    // 값이 바뀜 공지!!
            }
        }

        public string OutFirstName
        {
            get => outFirstName;
            set
            {
                outFirstName = value;
                RaisePropertyChanged("OutFirstName");    // 값이 바뀜 공지!!
            }
        }
        public string OutLastName
        {
            get => outLastName;
            set
            {
                outLastName = value;
                RaisePropertyChanged("OutLastName");    // 값이 바뀜 공지!!
            }
        }
        public string OutEmail
        {
            get => outEmail;
            set
            {
                outEmail = value;
                RaisePropertyChanged("OutEmail");    // 값이 바뀜 공지!!
            }
        }
        public string OutDate
        {
            get => outDate;
            set
            {
                outDate = value;
                RaisePropertyChanged("OutDate");    // 값이 바뀜 공지!!
            }
        }

        public string OutAdult
        {
            get => outAdult;
            set
            {
                outAdult = value;
                RaisePropertyChanged("OutAdult");    // 값이 바뀜 공지!!
            }
        }
        public string OutBirthday
        {
            get => outBirthday;
            set
            {
                outBirthday = value;
                RaisePropertyChanged("OutBirthday");    // 값이 바뀜 공지!!
            }
        }

        // 값이 전부 적용되어 버튼을 활성화하기 위한 명령 
        private ICommand proceedCommand;
        public ICommand ProceedCommand
        {
            get
            {
                return proceedCommand ?? (
                    proceedCommand = new RelayCommand<object>(
                        o => Proceed(), o => !string.IsNullOrEmpty(inEmail) &&
                                             !string.IsNullOrEmpty(InFirstName) &&
                                             !string.IsNullOrEmpty(inLastName) &&
                                             !string.IsNullOrEmpty(inDate.ToString())
                                             ));
            }
        }
        //버튼 클릭시 일어나는 실제 명령의 실체
        private async void Proceed()
        {
            try
            {
                Person person = new Person(inFirstName, inLastName, inEmail, inDate);
                await Task.Run(() => OutFirstName = person.FirstName);
                await Task.Run(() => OutLastName = person.LastName);
                await Task.Run(() => OutEmail = person.Email);
                await Task.Run(() => OutDate = person.Date.ToString("yyyy-MM-dd"));
                await Task.Run(() => OutAdult = (person.IsAdult ? "예" : "아니오"));
                await Task.Run(() => OutBirthday = (person.IsBirthday ? "예" : "아니오"));

                //ToDo
            }

            catch (Exception ex)
            {
                MessageBox.Show($"예외발생 : {ex.Message}");
            }

        }

        public MainViewModel()
        {
            this.inDate = DateTime.Parse("1999-09-01");
        }
    }
}
