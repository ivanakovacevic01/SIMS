using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Serializer;
using static System.Net.Mime.MediaTypeNames;

namespace InitialProject.Model
{
    public class AlertGuest2: ISerializable
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }

        public int Guest2Id { get; set; }    

        public int CheckPointId { get; set; }

        public bool Availability { get; set; }
        public int InstanceId { get; set; }

        public AlertGuest2() { }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), ReservationId.ToString(), Guest2Id.ToString(), CheckPointId.ToString(),Availability.ToString(),InstanceId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            ReservationId = Convert.ToInt32((string)values[1]);
            Guest2Id = Convert.ToInt32((string)values[2]);
            CheckPointId = Convert.ToInt32((string)values[3]);
            Availability = Convert.ToBoolean((string)values[4]);
            InstanceId = Convert.ToInt32((string)values[5]);

        }
    }
}
