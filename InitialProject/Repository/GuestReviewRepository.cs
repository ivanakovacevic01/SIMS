using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Serializer;

namespace InitialProject.Repository
{
    public class GuestReviewRepository
    {
        private const string FilePath = "../../../Resources/Data/guestReviews.csv";

        private readonly Serializer<ReviewOfGuest> _serializer;

        private List<ReviewOfGuest> _reviews;

        public GuestReviewRepository()
        {
            _serializer = new Serializer<ReviewOfGuest>();
            _reviews = _serializer.FromCSV(FilePath);
            AddGuest();
        }

        private void AddGuest()
        {
            Guest1Repository guest1Repository = new Guest1Repository();
            foreach(ReviewOfGuest review in _reviews)
            {
                review.Guest = guest1Repository.GetAll().Find(n => n.Id == review.Guest.Id);
            }
        }
        public int NextId()
        {
            _reviews = _serializer.FromCSV(FilePath);
            if (_reviews.Count < 1)
            {
                return 1;
            }
            return _reviews.Max(c => c.Id) + 1;
        }

        public bool HasReview(Guest1 guest)
        {
            return _reviews.Find(n => n.Guest.Id == guest.Id) != null;
        }

        public void Save(ReviewOfGuest review)
        {
            review.Id = NextId();
            _reviews.Add(review);
            _serializer.ToCSV(FilePath, _reviews);
        }

        public List<ReviewOfGuest> GetAll()
        {
            return _reviews;
        }
    }
}
