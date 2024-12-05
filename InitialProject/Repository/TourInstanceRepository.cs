using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    public class TourInstanceRepository
    {
        private const string FilePath = "../../../Resources/Data/tourInstances.csv";
        private const string FilePathTour = "../../../Resources/Data/tours.csv";
        private const string FilePathLocation= "../../../Resources/Data/locations.csv";

        private readonly Serializer<TourInstance> _serializer;
        private readonly Serializer<Tour> _serializerTour;
        private readonly Serializer<Location> _serializerLocation;

        private List<Tour> _tours;
        private List<TourInstance> _tourInstances;
        private List<Location> _locations;

        public TourInstanceRepository()
        {

            _serializer = new Serializer<TourInstance>();
            _serializerTour = new Serializer<Tour>();
            _serializerLocation= new Serializer<Location>();

            _tourInstances = _serializer.FromCSV(FilePath);
            SetTours();
           
        }
        
        public List<TourInstance> GetAll()
        {
            return _tourInstances;
        }

        public void SetTours()
        {
            Serializer<Tour> _serializerTour = new Serializer<Tour>();
            List<Tour> tours = _serializerTour.FromCSV("../../../Resources/Data/tours.csv");
            foreach (TourInstance tourInstance in _tourInstances)
            {
                if (tours.Find(n => n.Id == tourInstance.Tour.Id) != null)
                {
                    tourInstance.Tour = tours.Find(n => n.Id == tourInstance.Tour.Id);
                }
            }
        }


        public TourInstance Save(TourInstance tour)
        {
            tour.Id = NextId();
            _tourInstances = _serializer.FromCSV(FilePath);
            _tourInstances.Add(tour);
            _serializer.ToCSV(FilePath, _tourInstances);
            return tour;
        }

        public int NextId()
        {
            _tourInstances = _serializer.FromCSV(FilePath);
            if (_tourInstances.Count < 1)
            {
                return 1;
            }
            return _tourInstances.Max(c => c.Id) + 1;
        }

        public void Delete(TourInstance tour)
        {
            _tourInstances = _serializer.FromCSV(FilePath);
            TourInstance founded = _tourInstances.Find(c => c.Id == tour.Id);
            _tourInstances.Remove(founded);
            _serializer.ToCSV(FilePath, _tourInstances);
        }

        public TourInstance Update(TourInstance tour)
        {
            _tourInstances = _serializer.FromCSV(FilePath);
            TourInstance current = _tourInstances.Find(c => c.Id == tour.Id);
            int index = _tourInstances.IndexOf(current);
            _tourInstances.Remove(current);
            _tourInstances.Insert(index, tour);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _tourInstances);
            return tour;
        }
        public List<TourInstance> GetByStart(DateTime today)
        {

            _tourInstances = _serializer.FromCSV(FilePath);
            List<TourInstance> list = new List<TourInstance>();
            foreach (TourInstance tour in _tourInstances)

                if(tour.StartDate.Date==today.Date && tour.Finished==false) 
                {   
                    string h = tour.StartClock.Split(':')[0];
                    string m = tour.StartClock.Split(":")[1];
                    string s = tour.StartClock.Split(":")[2];

        
                    string time=today.TimeOfDay.ToString();
                    string hour = time.Split(":")[0];
                    string minute = time.Split(":")[1];
                    string second = DateTime.Now.Second.ToString();
                    if(Convert.ToInt32(h)> Convert.ToInt32(hour))
                    {
                        list.Add(tour);
                    }else if(Convert.ToInt32(h)== Convert.ToInt32(hour) && Convert.ToInt32(m)>Convert.ToInt32(minute))
                    {
                        list.Add(tour);
                    }else if(Convert.ToInt32(h)== Convert.ToInt32(hour) && Convert.ToInt32(m)== Convert.ToInt32(minute) && Convert.ToInt32(s) > Convert.ToInt32(second))
                    {
                        list.Add(tour);
                    }
                }
            Serializer<Tour> _serializerTour = new Serializer<Tour>();
            List<Tour> tours = _serializerTour.FromCSV("../../../Resources/Data/tours.csv");

            _locations = _serializerLocation.FromCSV(FilePathLocation);

            foreach (Location location in _locations)
            {
                foreach (Tour tour in tours)
                {
                    if (location.Id == tour.Location.Id)
                        tour.Location = location;
                }
            }

            foreach (TourInstance tourInstance in _tourInstances)
            {
                foreach (Tour tour in tours)
                {
                    if(tour.Id == tourInstance.Tour.Id)
                    {
                        tourInstance.Tour = tour;
                    }
                }
            }



            return list;
    
        }
    }
}
