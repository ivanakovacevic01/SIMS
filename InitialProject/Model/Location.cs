using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace InitialProject.Model
{
    public class Location : ISerializable
    {
        public int Id { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public Location() { }

        public Location(string city, string country)
        {
            City = city;
            Country = country;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            City=values[1];
            Country = values[2];
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), City, Country };
            return csvValues;
        }
        public override string ToString()
        {
            return City + "," + Country;
        }
    }
}
