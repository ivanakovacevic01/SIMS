using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    public class TourRepository
    {
        private const string FilePath = "../../../Resources/Data/tours.csv";
        private const string FilePathLocation= "../../../Resources/Data/locations.csv";

        private readonly Serializer<Tour> _serializer;
        private readonly Serializer<Location> _serializerLocation;

        private List<Tour> _tours;
        private List<Location> _locations;
        
        public TourRepository()
        {
            
            _serializer = new Serializer<Tour>();
            _serializerLocation = new Serializer<Location>();

            _tours = _serializer.FromCSV(FilePath);
            SetLocations();

        }
        
        public List<Tour> GetAll()
        {
            return _tours;
        }

        public void SetLocations()
        {
            Serializer<Location> _serializerLocation = new Serializer<Location>();
            List<Location> locations = _serializerLocation.FromCSV("../../../Resources/Data/locations.csv");
            foreach (Tour tour in _tours)
            {
                if (locations.Find(n => n.Id == tour.Location.Id) != null)
                {
                    tour.Location = locations.Find(n => n.Id == tour.Location.Id);
                }
            }
        }
       

        public Tour Save(Tour tour)
        {

            tour.Id = NextId();
            _tours = _serializer.FromCSV(FilePath);
            _tours.Add(tour);
            _serializer.ToCSV(FilePath, _tours);
            return tour;
        }

        public int NextId()
        {
            _tours = _serializer.FromCSV(FilePath);
            if (_tours.Count < 1)
            {
                return 1;
            }
            return _tours.Max(c => c.Id) + 1;
        }

        public void Delete(Tour tour)
        {
            _tours = _serializer.FromCSV(FilePath);
            Tour founded = _tours.Find(c => c.Id == tour.Id);
            _tours.Remove(founded);
            _serializer.ToCSV(FilePath, _tours);
        }

        public Tour Update(Tour tour)
        {
            _tours = _serializer.FromCSV(FilePath);
            Tour current = _tours.Find(c => c.Id == tour.Id);
            int index = _tours.IndexOf(current);
            _tours.Remove(current);
            _tours.Insert(index, tour);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _tours);
            return tour;
        }
        /*public List<Tour> GetByStart(DateTime date)
        {

            var day = date.Month;
            var month = date.Day;
            var year = date.Year;
            _tours = _serializer.FromCSV(FilePath);
            List<Tour> list = new List<Tour>();
            foreach (Tour tour in _tours)
            {
                var tourDay = tour.Start.Day;
                var tourMonth = tour.Start.Month;
                var tourYear = tour.Start.Year;
                if(tourDay == day && month==tourMonth && year==tourYear) 
                { 
                    list.Add(tour);
                }
            }
           

            _locations = _serializerLocation.FromCSV(FilePathLocation);

            foreach (Location location in _locations)
            {
                foreach (Tour tour in _tours)
                {
                    if (location.Id == tour.Location.Id)
                        tour.Location = location;
                }
            }
            return list;
        }*/
       
    }
}
