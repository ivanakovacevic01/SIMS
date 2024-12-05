using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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
    /// Interaction logic for Guest1Overview.xaml
    /// </summary>
    public partial class Guest1Overview : Window
    {
        private AccommodationImageRepository accommodationImageRepository;
        private List<AccommodationImage> accommodationImages;


        private AccommodationRepository accommodationRepository;
        
        private ObservableCollection<Accommodation> accommodations;
        public ObservableCollection<Accommodation> Accommodations 
        { 
            get { return accommodations; } 
            set
            {
                if(value != accommodations)
                    accommodations=value;
                OnPropertyChanged("Accommodations");
            }
                
        }
      
        
        public Guest1Overview()
        {
            InitializeComponent();
            DataContext = this;
            accommodationRepository = new AccommodationRepository();
            Accommodations = new ObservableCollection<Accommodation>(accommodationRepository.GetAll());
            accommodationImageRepository = new AccommodationImageRepository();
            accommodationImages = new List<AccommodationImage>(accommodationImageRepository.GetAll());



        }

        

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            this.Close();
        }

        private void searchNameButton(object sender, RoutedEventArgs e)
        {
            
            List<Accommodation> listAccommodation = accommodationRepository.GetAll();
            Accommodations.Clear();
            
                foreach(Accommodation accommodation in listAccommodation)
                {
                    Accommodations.Add(accommodation);
                }


            

            foreach (Accommodation accommodation in listAccommodation)
            {
                if(!accommodation.Name.ToLower().Contains(nameInput.Text.ToLower()))
                {
                    Accommodations.Remove(accommodation);
                }
                if (!accommodation.Location.City.ToLower().Contains(cityInput.Text.ToLower()))
                {
                    Accommodations.Remove(accommodation);
                }
                if (!accommodation.Location.Country.ToLower().Contains(countryInput.Text.ToLower()))
                {
                    Accommodations.Remove(accommodation);
                }
                if(apartment.IsChecked==true || house.IsChecked==true || cottage.IsChecked==true)
                {
                    if (apartment.IsChecked==false)
                    {
                        if(accommodation.Type.Name == "apartment")
                            Accommodations.Remove(accommodation);
                    }
                    if (house.IsChecked==false)
                    {
                        if (accommodation.Type.Name == "house")
                            Accommodations.Remove(accommodation);
                    }
                    if (cottage.IsChecked==false)
                    {
                        if (accommodation.Type.Name == "cottage")
                            Accommodations.Remove(accommodation);
                    }
                }
                if(Convert.ToInt32(numberOfGuests.Text) > accommodation.Capacity)
                {
                    Accommodations.Remove(accommodation);
                }
                if (Convert.ToInt32(numberOfDays.Text) < accommodation.MinDaysForReservation)
                {
                    Accommodations.Remove(accommodation);
                }




            }
            

        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void DecrementGuestsNumber(object sender, RoutedEventArgs e)
        {
            int changedGuestsNumber;
            if(Convert.ToInt32(numberOfGuests.Text)>1)
            {
                changedGuestsNumber = Convert.ToInt32(numberOfGuests.Text) - 1;
                numberOfGuests.Text = changedGuestsNumber.ToString();
            }
            
        }

        private void IncrementGuestsNumber(object sender, RoutedEventArgs e)
        {
            int changedGuestsNumber;
            changedGuestsNumber = Convert.ToInt32(numberOfGuests.Text) + 1;
            numberOfGuests.Text = changedGuestsNumber.ToString();
            
        }

        private void DecrementDaysNumber(object sender, RoutedEventArgs e)
        {
            int changedDaysNumber;
            if (Convert.ToInt32(numberOfDays.Text) > 1)
            {
                changedDaysNumber = Convert.ToInt32(numberOfDays.Text) - 1;
                numberOfDays.Text = changedDaysNumber.ToString();
            }
        }

        private void IncrementDaysNumber(object sender, RoutedEventArgs e)
        {
            int changedDaysNumber;
            changedDaysNumber = Convert.ToInt32(numberOfDays.Text) + 1;
            numberOfDays.Text = changedDaysNumber.ToString();
        }

        private void ViewPhotos(object sender, RoutedEventArgs e)
        {
            try
            {
                Accommodation currentAccommodation = (Accommodation)AccommodationListDataGrid.CurrentItem;


                List<string> imagesUrl = new List<string>();

                foreach (AccommodationImage image in accommodationImages)
                {
                    if (image.Accommodation.Id == currentAccommodation.Id)
                    {
                        imagesUrl.Add(image.Url);
                    }
                }

                if (imagesUrl.Count == 0)
                {
                    MessageBox.Show("There are currently no images for the selected accommodation.");
                }
                else
                {
                    AccommodationPhotosView photosView = new AccommodationPhotosView(imagesUrl);
                    photosView.Show();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void Reserve(object sender, RoutedEventArgs e)
        {
            Accommodation currentAccommodation = (Accommodation)AccommodationListDataGrid.CurrentItem;
            AccommodationReservationForm accommodationReservationForm = new AccommodationReservationForm(currentAccommodation, ref accommodationRepository);
            accommodationReservationForm.Show();
        }


    }
}
