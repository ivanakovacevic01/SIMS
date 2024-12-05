using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.View.Owner;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for OwnerOverview.xaml
    /// </summary>
    public partial class OwnerOverview : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Accommodation> accommodations { get; set; }
        public ObservableCollection<Guest1> guests { get; set; }
        private Guest1 selectedGuest;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Guest1 SelectedGuest
        {
            get => selectedGuest;
            set
            {
                if (value != selectedGuest)
                {
                    selectedGuest = value;
                    OnPropertyChanged();
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public OwnerOverview()
        {
            InitializeComponent();
            DataContext = this;
            AccommodationRepository accommodationRepository = new AccommodationRepository();
            AccommodationReservationRepository reservationRepository = new AccommodationReservationRepository();
            accommodations = new ObservableCollection<Accommodation>(accommodationRepository.GetAll());
            guests = new ObservableCollection<Guest1>(reservationRepository.GetAllGuestsToReview());
        }

        private void AddAccommodationClick(object sender, RoutedEventArgs e)
        {
            AccommodationForm accommodationForm = new AccommodationForm(accommodations);
            accommodationForm.Show();
        }

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            this.Close();
        }

        private void Review_Click(object sender, RoutedEventArgs e)
        {
            GuestReview guestReview = new GuestReview(SelectedGuest, guests);
            guestReview.Show();
        }

        private void ReviewOverview_Click(object sender, RoutedEventArgs e)
        {
            GuestReviewsOverview guestReviewsOverview = new GuestReviewsOverview();
            guestReviewsOverview.Show();

        }
    }
}
