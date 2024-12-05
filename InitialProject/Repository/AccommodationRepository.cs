using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Serializer;

namespace InitialProject.Repository
{
    public class AccommodationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodations.csv";

        private readonly Serializer<Accommodation> _serializer;

        private List<Accommodation> _accommodations;

        public AccommodationRepository()
        {
            _serializer = new Serializer<Accommodation>();
            _accommodations = _serializer.FromCSV(FilePath);
            SetLocations();
            SetType();

        }
        public int NextId()
        {
            _accommodations = _serializer.FromCSV(FilePath);
            if (_accommodations.Count < 1)
            {
                return 1;
            }
            return _accommodations.Max(c => c.Id) + 1;
        }
        public void Add(Accommodation accommodation)
        {
            accommodation.Id = NextId();
            _accommodations.Add(accommodation);
            _serializer.ToCSV(FilePath, _accommodations);
        }

        public List<Accommodation> GetAll()
        {
            return _accommodations;
        }

        public void SetLocations()
        {
            Serializer<Location> _serializerLocation = new Serializer<Location>();
            List<Location> locations = _serializerLocation.FromCSV("../../../Resources/Data/locations.csv");
            foreach (Accommodation accommodation in _accommodations)
            {
                if(locations.Find(n=>n.Id == accommodation.Location.Id) != null)
                {
                    accommodation.Location = locations.Find(n => n.Id == accommodation.Location.Id);
                }
            }
        }

        public void SetType()
        {
            Serializer<AccommodationType> _serializerType = new Serializer<AccommodationType>();
            List<AccommodationType> types = _serializerType.FromCSV("../../../Resources/Data/accommodationTypes.csv");
            foreach(Accommodation accommodation in _accommodations)
            {
                if (types.Find(n=>n.Id == accommodation.Type.Id) != null)
                {
                    accommodation.Type = types.Find(n => n.Id == accommodation.Type.Id);
                }
            }
        }
    }
}
