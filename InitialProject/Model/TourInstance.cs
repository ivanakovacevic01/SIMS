using System;
using System.Collections.Generic;
using System.Linq;
using InitialProject.Serializer;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Windows;
using System.Xml.Linq;

namespace InitialProject.Model
{
    public class TourInstance:ISerializable
    {
        public int Id { get; set; }

        public Tour Tour { get; set; }

        public DateTime StartDate { get; set; }

        public string StartClock { get; set; }
        public bool Finished { get; set; }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Tour = new Tour() { Id = Convert.ToInt32(values[1]) };
            StartDate = Convert.ToDateTime(values[2]);
            StartClock = values[3];
            Finished = Convert.ToBoolean(values[4]);

        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Tour.Id.ToString(), StartDate.ToString(), StartClock ,Finished.ToString()};
            return csvValues;
        }
        public TourInstance() 
        {
            Finished = false;
        }

        public TourInstance( Tour tour, DateTime startDate, string startClock)
        {
            Tour = tour;
            StartDate = startDate;
            StartClock = startClock;
            Finished = false;
        }

        public override string ToString()
        {
            return StartDate.ToShortDateString() + " " + StartClock.ToString();
        }
    }
}
