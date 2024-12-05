using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using InitialProject.Serializer;

namespace InitialProject.Model
{
    public class AccommodationReservation : ISerializable
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public Accommodation currentAccommodation { get; set; }

        public DateTime ComingDate  { get; set; }
        public DateTime LeavingDate { get; set; }

        public AccommodationReservation() { 
            currentAccommodation=new Accommodation();
        }
        public AccommodationReservation(int guestId, Accommodation currentAccommodation, DateTime comingDate, DateTime leavingDate)
        {
            GuestId = guestId;
            this.currentAccommodation = currentAccommodation;
            ComingDate = comingDate;
            LeavingDate = leavingDate;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            GuestId = Convert.ToInt32(values[1]);
            currentAccommodation.Id = Convert.ToInt32(values[2]);
            ComingDate = Convert.ToDateTime(values[3]);
            LeavingDate = Convert.ToDateTime(values[4]);


        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), GuestId.ToString(), currentAccommodation.Id.ToString(), ComingDate.ToString(), LeavingDate.ToString()};
            return csvValues;
        }
    }
}
