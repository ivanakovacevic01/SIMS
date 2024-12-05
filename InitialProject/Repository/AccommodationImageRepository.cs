using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Serializer;

namespace InitialProject.Repository
{
    public class AccommodationImageRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationImages.csv";

        private readonly Serializer<AccommodationImage> _serializer;

        private List<AccommodationImage> _accommodationImages;

        public AccommodationImageRepository()
        {
            _serializer = new Serializer<AccommodationImage>();
            _accommodationImages = _serializer.FromCSV(FilePath);
        }
        public int NextId()
        {
            _accommodationImages = _serializer.FromCSV(FilePath);
            if (_accommodationImages.Count < 1)
            {
                return 1;
            }
            return _accommodationImages.Max(c => c.Id) + 1;
        }
        public int Add(string url, Accommodation accommodation)
        {
            AccommodationImage image = new AccommodationImage();
            image.Accommodation = accommodation;
            image.Url = url;
            image.Id = NextId();
            _accommodationImages.Add(image);
            _serializer.ToCSV(FilePath, _accommodationImages);
            return image.Id;
        }

        public List<AccommodationImage> GetAll()
        {
            return _accommodationImages;
        }

    }
}
