using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace InitialProject.Model
{
    public class CheckPoint : ISerializable
    {
        public int Id { get; set; } 

        public string Name { get; set; }    

        public bool Checked { get; set; }

        public int TourId { get; set; } 

        public int Order { get; set; }

        public CheckPoint() { }

        public CheckPoint( string name, bool @checked, int tourId, int order)
        {
            Name = name;
            Checked = @checked;
            TourId = tourId;
            Order = order;
        }

        
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Checked = Convert.ToBoolean(values[2]);
            TourId = Convert.ToInt32(values[3]);
            Order = Convert.ToInt32(values[4]);

        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name, Checked.ToString(),TourId.ToString(),Order.ToString()};
            return csvValues;
        }
        
    }
}
