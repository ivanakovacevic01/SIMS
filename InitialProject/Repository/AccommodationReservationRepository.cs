using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InitialProject.Model;
using InitialProject.Serializer;

namespace InitialProject.Repository
{
    public class AccommodationReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationReservations.csv";

        private readonly Serializer<AccommodationReservation> _serializer;

        private List<AccommodationReservation> _accommodationReservations;

        public AccommodationReservationRepository()
        {
            _serializer = new Serializer<AccommodationReservation>();
            _accommodationReservations = new List<AccommodationReservation>();
            _accommodationReservations = _serializer.FromCSV(FilePath);
        }

        public List<AccommodationReservation> GetAll()
        {
            return _accommodationReservations;
        }

         public int NextId()
        {
            _accommodationReservations = _serializer.FromCSV(FilePath);
            if (_accommodationReservations.Count < 1)
            {
                return 1;
            }
            return _accommodationReservations.Max(c => c.Id) + 1;
        }
        public void Add(AccommodationReservation accommodationReservation)
        {
            accommodationReservation.Id = NextId();
            _accommodationReservations.Add(accommodationReservation);
            _serializer.ToCSV(FilePath, _accommodationReservations);
        }

       public List<Guest1> GetAllGuestsToReview()
       {
            List<Guest1> guests = new List<Guest1>();
            UserRepository userRepository = new UserRepository();
            GuestReviewRepository guestReviewRepository = new GuestReviewRepository();
            foreach(Guest1 guest in userRepository.GetAllGuests1())
            {
                AccommodationReservation reservation = _accommodationReservations.Find(n => n.GuestId == guest.Id);
                if (reservation != null)
                {
                    bool hasReservation = reservation != null;
                    bool stayedLessThan5DaysAgo = (reservation.LeavingDate.Date < DateTime.Now.Date) && (DateTime.Now.Date - reservation.LeavingDate.Date).TotalDays < 5;
                    bool alreadyReviewed = guestReviewRepository.HasReview(guest);
                    if (hasReservation && stayedLessThan5DaysAgo && !alreadyReviewed)
                        guests.Add(guest);
                }
            }
            return guests;
       }

    }
}
