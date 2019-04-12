using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Assignment_PROG8145
{
    public class Customer 
    {
        public String  Name { get; set; }
        public Int16 Age { get; set; }
        public String Phone { get; set; }
        public String Gender { get; set; }
        public String Service { get; set; }
        public String Provider { get; set; }
        public String Date { get; set; }
        public String Time { get; set; }
        public String Location { get; set; }
        //public abstract CustomerDelegate CusDel { get; set; }

        public Customer()
        {
        }

        public Customer(string name, short age, string phone, string gender, string service, string provider, string date, string time, string location)
        {
            Name = name;
            Age = age;
            Phone = phone;
            Gender = gender;
            Service = service;
            Provider = provider;
            Date = date;
            Time = time;
            Location = location;
        }
    }
}
