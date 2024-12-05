using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Serializer;

namespace InitialProject.Model
{
    public class AccommodationType : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AccommodationType() { }
        public AccommodationType(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name.ToString() };
            return csvValues;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
