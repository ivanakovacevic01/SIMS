using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Serializer;

namespace InitialProject.Model
{
    public class Accommodation : ISerializable
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public Location Location { get; set; }
        public AccommodationType Type { get; set; }
        public int Capacity { get; set; }
        public int MinDaysForReservation { get; set; }
        public int MinDaysToCancel { get; set; }

        public Accommodation() { }
        public Accommodation(string name, Location location, AccommodationType type, int capacity, int minDaysForReservation, int minDaysToCancel)
        {
            Name = name;
            Location = location;
            Type = type;
            Capacity = capacity;
            MinDaysForReservation = minDaysForReservation;
            MinDaysToCancel = minDaysToCancel;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Capacity = Convert.ToInt32(values[2]);
            MinDaysForReservation = Convert.ToInt32(values[3]);
            MinDaysToCancel = Convert.ToInt32(values[4]);
            Location = new Location();
            Location.Id = Convert.ToInt32(values[5]);
            Type = new AccommodationType();
            Type.Id = Convert.ToInt32(values[6]);

        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name, Capacity.ToString(), MinDaysForReservation.ToString(), MinDaysToCancel.ToString(), Location.Id.ToString(), Type.Id.ToString() };
            return csvValues;
        }
    }
}
