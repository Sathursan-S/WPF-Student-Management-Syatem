using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfApp_SMS.Models
{
    public class Student
    {
        public string StudentID { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DOB { get; set; }
        public float GPA { get; set; }
        public byte[] Img { get; set; }
        private static int _count = 0;

        //overdoad constructur 
        public Student(string firstName, string lastName, DateOnly dOB, float gPA, byte[] img)
        {
            //auto genarate id
            _count++;
            StudentID = $"S{_count:000}-{new Random().Next(1000, 9999)}" ;
            FirstName = firstName;
            LastName = lastName;
            DOB = dOB;
            GPA = gPA;
            Img = img;
        }
    }
}
