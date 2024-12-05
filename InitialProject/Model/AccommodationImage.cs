using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Serializer;

namespace InitialProject.Model
{
    public class AccommodationImage : ISerializable
    {
        public int Id { get; set; }
        public string Url { get; set; }

        public Accommodation Accommodation { get; set; }

        public AccommodationImage() { }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Url = values[1];
            Accommodation = new Accommodation();
            Accommodation.Id = Convert.ToInt32(values[2]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Url, Accommodation.Id.ToString() };
            return csvValues;
        }
    }
}
