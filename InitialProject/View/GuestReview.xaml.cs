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

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for GuestReview.xaml
    /// </summary>
    public partial class GuestReview : Window, INotifyPropertyChanged
    {
        public Guest1 guest { get; set; }
        private ReviewOfGuest review;
        public ObservableCollection<Guest1> guests { get; set; }
        public ReviewOfGuest Review
        {
            get => review;
            set
            {
                if (value != review)
                {
                    review = value;
                    OnPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public GuestReview(Guest1 guestToReview, ObservableCollection<Guest1> allGuests)
        {
            InitializeComponent();
            DataContext = this;
            guest = guestToReview;
            review = new ReviewOfGuest();
            review.Guest = guest;
            guests = allGuests;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            GuestReviewRepository guestReviewRepository = new GuestReviewRepository();
            guestReviewRepository.Save(Review);
            guests.Remove(guest);
            this.Close();
        }

        private void Cleanliness_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)Cleanliness1.IsChecked)
                Review.Cleanliness = 1;
            else if ((bool)Cleanliness2.IsChecked)
                Review.Cleanliness = 2;
            else if ((bool)Cleanliness3.IsChecked)
                Review.Cleanliness = 3;
            else if ((bool)Cleanliness4.IsChecked)
                Review.Cleanliness = 4;
            else if ((bool)Cleanliness5.IsChecked)
                Review.Cleanliness = 5;

        }

        private void RulesFollowing_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)RulesFollowing1.IsChecked)
                Review.RulesFollowing = 1;
            else if ((bool)RulesFollowing2.IsChecked)
                Review.RulesFollowing = 2;
            else if ((bool)RulesFollowing3.IsChecked)
                Review.RulesFollowing = 3;
            else if ((bool)RulesFollowing4.IsChecked)
                Review.RulesFollowing = 4;
            else if ((bool)RulesFollowing5.IsChecked)
                Review.RulesFollowing = 5;
        }


    }
}
