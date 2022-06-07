using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMvvmApp.Helpers;

namespace WpfMvvmApp.Models
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        private string email;   //멤버변수
        private string Email    //멤버속성
        { 
            get => email; // get{return email;}
            set
            {
                if (Commons.IsValidEmail(email))
                {
                    throw new Exception("Invalid Email");
                }
                else email = value; 
            } 
        } 

        public DateTime date { get; set; }
        public DateTime Date
        {
            get { return date; }
            set
            {
                var result = Commons.CalcAge(value);
                if (result > 135 || result < 0)
                {
                    throw new Exception("Invalid Date");
                }
                else date = value;
            } 
        }
        public bool IsBirthday
        {
            get
            {
                return DateTime.Now.Year == Date.Year &&
                  DateTime.Now.Month == Date.Month &&
                  DateTime.Now.Day == Date.Day;
            }
        }
        public bool IsAdult
        {
            get
            {
                return Commons.CalcAge(date) > 18; // 성인
            }
        }

        public Person(string firstName, string lastName, string email, DateTime date)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Date = date;
        }
    }
}
