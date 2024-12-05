using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using InitialProject.Serializer;
namespace InitialProject.Model
{
    public class Guest2 :ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }



        public Guest2() { }
        public Guest2(string name, string lastName)
        {
            Name = name;
            LastName = lastName;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            LastName = values[2];
            Username = values[3];
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name, LastName, Username };
            return csvValues;
        }
    }
}
