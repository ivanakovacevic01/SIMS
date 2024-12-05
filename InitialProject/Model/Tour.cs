using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;

namespace InitialProject.Model
{
    public class Tour : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int MaxGuests { get; set; }

        public double Duration { get; set; }

        public Location Location { get; set; }


        public string Description { get; set; }

        public string Language { get; set; }

        public Tour() 
        {
           
        }

        public Tour(string name, int maxGuests, double duration, Location location,string description, string language)
        {
            Name=name;
            MaxGuests=maxGuests;
            Duration=duration;
            Location=location;
            Description=description;
            Language=language;

        }


        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            MaxGuests = Convert.ToInt32(values[2]);
            Duration = Convert.ToDouble(values[3]);
            Location = new Location() { Id = Convert.ToInt32(values[4]) };
            Description = values[5];
            Language = values[6];

        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name, MaxGuests.ToString(),Duration.ToString(), Location.Id.ToString(),Description,Language };
            return csvValues;
        }
    }
}
