using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    public class TourImageRepository
    {
        private const string FilePath = "../../../Resources/Data/tourImages.csv";

        private readonly Serializer<TourImage> _serializer;

        private List<TourImage> _tourImages;

        public TourImageRepository()
        {
            _serializer = new Serializer<TourImage>();
            _tourImages = _serializer.FromCSV(FilePath);
        }

        public List<TourImage> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public TourImage Save(TourImage tourImage)
        {
            tourImage.Id = NextId();
            _tourImages = _serializer.FromCSV(FilePath);
            _tourImages.Add(tourImage);
            _serializer.ToCSV(FilePath, _tourImages);
            return tourImage;
        }

        public int NextId()
        {
            _tourImages = _serializer.FromCSV(FilePath);
            if (_tourImages.Count < 1)
            {
                return 1;
            }
            return _tourImages.Max(c => c.Id) + 1;
        }

        public void Delete(TourImage tourImage)
        {
            _tourImages = _serializer.FromCSV(FilePath);
            TourImage founded = _tourImages.Find(c => c.Id == tourImage.Id);
            _tourImages.Remove(founded);
            _serializer.ToCSV(FilePath, _tourImages);
        }

        public TourImage Update(TourImage tourImage)
        {
            _tourImages = _serializer.FromCSV(FilePath);
            TourImage current = _tourImages.Find(c => c.Id == tourImage.Id);
            int index = _tourImages.IndexOf(current);
            _tourImages.Remove(current);
            _tourImages.Insert(index, tourImage);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _tourImages);
            return tourImage;
        }
    }
}
