using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabazeOsob
{
    public class Person
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RNnumber { get; set; }
        public string Sex { get; set; }
        public DateTime TimeStamp { get; set; }
        public DateTime UpdateTimeStamp { get; set; }

        public Person()
        {
        }

        public override string ToString()
        {
            return ID + " " + FirstName + " " + LastName + " " + RNnumber + " " + Sex;
        }
    }
}
