using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Serializer;
namespace InitialProject.Model
{
    public class TourReservation:ISerializable
    {
        public int Id { get; set; } 
        public int TourInstanceId { get; set; }
        public int CurrentGuestsNumber { get; set; }
        public int GuestId { get; set; }
        public TourReservation() { }

        public TourReservation(int tourInstanceId, int currentGuestsNumber, int guestId)
        {
            TourInstanceId = tourInstanceId;
            CurrentGuestsNumber = currentGuestsNumber;
            GuestId = guestId;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourInstanceId = Convert.ToInt32(values[1]);
            CurrentGuestsNumber = Convert.ToInt32(values[2]);
            GuestId = Convert.ToInt32(values[3]);
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(),TourInstanceId.ToString(),CurrentGuestsNumber.ToString(),GuestId.ToString()};
            return csvValues;
        }
    }
}

