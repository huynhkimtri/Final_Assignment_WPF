using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Assignment_PROG8145
{
    class Appointments
    {
        private List<Customer> appointments;
        public Appointments()
        {
            appointments = new List<Customer>();
        }
        public int Count
        {
            get { return appointments.Count; }
        }
        public Customer this[int index]
        {
            get { return appointments[index]; }
            set { appointments[index] = value; }
        }

        public void AddItem(Customer c)
        {
            appointments.Add(c);
        }

        public void RemoveItem(Customer c)
        {
            appointments.Remove(c);
        }

        public void Sort()
        {
            appointments.Sort();
        }
    }
}
