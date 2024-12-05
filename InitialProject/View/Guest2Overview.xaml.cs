using InitialProject.Model;
using InitialProject.Repository;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Policy;
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

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for Guest2Overview.xaml
    /// </summary>
    public partial class Guest2Overview : Window
    {
        private const string FilePath = "../../../Resources/Data/alertsGuest2.csv";
        private const string filePath = "../../../Resources/Data/users.csv";
        public ObservableCollection<TourInstance> TourInstances { get; set; }
        private ObservableCollection<TourImage> TourImages;
        private ObservableCollection<TourReservation> TourReservations;
        public TourInstance _selected;
        public TourInstance Selected
        {
            get { return _selected; }
            set
            {
                if (value != _selected)
                    _selected = value;
                OnPropertyChanged("Selected");
            }
        }
        private TourRepository _tourRepository;
        private TourInstanceRepository _tourInstanceRepository;
        private TourImageRepository _tourImageRepository;
        private TourReservationRepository _tourReservationRepository;
        private Serializer<AlertGuest2> _alertGuestSerializer;
        private Serializer<User> _userSerializer;
        private List<AlertGuest2> alerts;
        private List<User> users;
        private int GuestId;
        public Guest2Overview()
        {
            InitializeComponent();
            DataContext = this;
            _alertGuestSerializer = new Serializer<AlertGuest2>();
            _userSerializer = new Serializer<User>();
            _tourRepository = new TourRepository();
            _tourInstanceRepository = new TourInstanceRepository();
            _tourReservationRepository = new TourReservationRepository();
            TourInstances = new ObservableCollection<TourInstance>(_tourInstanceRepository.GetAll());
            TourReservations = new ObservableCollection<TourReservation>(_tourReservationRepository.GetAll());
            _tourImageRepository = new TourImageRepository();
            TourImages=new ObservableCollection<TourImage>(_tourImageRepository.GetAll());
            SetLocations();
            users = _userSerializer.FromCSV(filePath);
            alerts = _alertGuestSerializer.FromCSV(FilePath);
            GuestId = GetGuest2Id();
            if (alerts.Count() != 0)
            {
                foreach (AlertGuest2 alert in alerts)
                {
                    AlertGuestForm alertGuestForm = new AlertGuestForm(alert.Id);
                    if (GuestId == alert.Guest2Id && alert.Availability == false)
                        alertGuestForm.Show();
                }
            }
            
        }
        public void SetLocations()
        {
            Serializer<Location> _serializerLocation = new Serializer<Location>();
            List<Location> locations = _serializerLocation.FromCSV("../../../Resources/Data/locations.csv");
            Serializer<Tour> _serializerTour = new Serializer<Tour>();
            List<Tour> tours = _serializerTour.FromCSV("../../../Resources/Data/tours.csv");
            foreach (Location location in locations)
            {
                foreach (Tour tour in tours)
                {
                    if (location.Id == tour.Location.Id)
                        tour.Location = location;
                }
            }
            foreach (TourInstance tourInstance in TourInstances)
            {
                foreach (Tour tour in tours)
                {
                    if (tour.Id == tourInstance.Tour.Id)
                    {
                        tourInstance.Tour = tour;
                    }
                }
            }
        }
        private void SignOut_Click(object sender, RoutedEventArgs e)
        {

            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            this.Close();

        }
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            List<TourInstance> listTours = _tourInstanceRepository.GetAll();
            TourInstances.Clear();
            foreach (TourInstance tourInstance in listTours)
            {
                TourInstances.Add(tourInstance);
            }
            foreach (TourInstance tourInstance in listTours)
            {
                if (tourInstance.Tour.Location.City != null)
                {
                    if (!tourInstance.Tour.Location.City.ToLower().Contains(cityInput.Text.ToLower()))
                    {
                        TourInstances.Remove(tourInstance);
                    }
                }
                if (tourInstance.Tour.Location.Country != null)
                {
                    if (!tourInstance.Tour.Location.Country.ToLower().Contains(countryInput.Text.ToLower()))
                    {
                        TourInstances.Remove(tourInstance);
                    }
                }
                if (tourInstance.Tour.Duration != null)
                {
                    if (durationInput.Text != "")
                    {
                        if (tourInstance.Tour.Duration < Convert.ToDouble(durationInput.Text))
                        {
                            TourInstances.Remove(tourInstance);
                        }
                    }
                }
                if (tourInstance.Tour.Language != null)
                {
                    if (!tourInstance.Tour.Language.ToLower().Contains(languageInput.Text.ToLower()))
                    {
                        TourInstances.Remove(tourInstance);
                    }
                }
                if (tourInstance.Tour.MaxGuests != null)
                {
                    if (Convert.ToInt32(capacityNumber.Text) > tourInstance.Tour.MaxGuests)
                    {
                        TourInstances.Remove(tourInstance);
                    }
                    
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void incrementCapacityNumber_Click(object sender, RoutedEventArgs e)
        {
            int changedCapacityNumber;
            changedCapacityNumber = Convert.ToInt32(capacityNumber.Text) + 1;
            capacityNumber.Text = changedCapacityNumber.ToString();
        }

        private void decrementCapacityNumber_Click(object sender, RoutedEventArgs e)
        {
            int changedCapacityNumber;
            if (Convert.ToInt32(capacityNumber.Text) > 1)
            {
                changedCapacityNumber = Convert.ToInt32(capacityNumber.Text) - 1;
                capacityNumber.Text = changedCapacityNumber.ToString();
            }
        }
        private int GetGuest2Id() //ovo ce morati drugacije kada budemo imali vise gostiju
        {
            foreach(User user in users)
            {
                if (user.Role.ToString()=="GUEST2")
                {
                    return user.Id;
                }
            }
            return 0;
        }
        private void Reserve(object sender, RoutedEventArgs e)
        {
            TourInstance currentTourInstance = (TourInstance)TourListDataGrid.CurrentItem;
            foreach(TourInstance tourInstance in TourInstances)
            {
                if (tourInstance.Id==currentTourInstance.Id)
                {
                    currentTourInstance = tourInstance;
                }
            }
            TourReservationForm tourReservationForm = new TourReservationForm(currentTourInstance,GuestId,TourInstances,_tourInstanceRepository,Label,Restart);
            tourReservationForm.Show();
        }
        private void ViewDetails(object sender, RoutedEventArgs e)
        {
            try
            {
                TourInstance currentTourInstance = (TourInstance)TourListDataGrid.CurrentItem;
                List<string> imagesUrl = new List<string>();

                foreach (TourImage image in TourImages)
                {
                    if (image.TourId== currentTourInstance.Tour.Id)
                    {
                        imagesUrl.Add(image.Url);
                    }
                }
                TourDetails detailsView = new TourDetails(imagesUrl,currentTourInstance);
                detailsView.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            List<TourInstance> listTours = _tourInstanceRepository.GetAll();
            TourInstances.Clear();
            foreach (TourInstance tourInstance in listTours)
            {
                TourInstances.Add(tourInstance);
            }
        }
    }
}
