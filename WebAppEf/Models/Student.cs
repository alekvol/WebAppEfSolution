using System.ComponentModel.DataAnnotations;

namespace WebAppEF.Models
{
    public class Student
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? Age { get; set; }

        public string Adress { get; set; }
        public Student() { }
        public Student(int id, string name, int age, string adress)
        {
            Id = id;
            Name = name;
            Age = age;
            Adress = adress;
        }

    }
}
