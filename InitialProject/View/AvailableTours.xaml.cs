using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using InitialProject.Serializer;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for AvailableTours.xaml
    /// </summary>
    public partial class AvailableTours : Window
    {
        private ObservableCollection<TourInstance> listTourInstances;
        public ObservableCollection<TourInstance> TourInstances { get; set; }
        private ObservableCollection<TourReservation> TourReservations;
        public TourInstance _tourInstance;
        private TourInstanceRepository _tourInstanceRepository;
        public TourReservation _tourReservation;
        public TourReservationRepository _tourReservationRepository;
        public AvailableTours(TourInstance tourInstance)
        {
            InitializeComponent();
            DataContext = this;
            _tourInstanceRepository = new TourInstanceRepository();
            _tourReservationRepository = new TourReservationRepository();
            listTourInstances = new ObservableCollection<TourInstance>(_tourInstanceRepository.GetAll());
            _tourInstance = tourInstance;
            SetLocations();
            TourReservations = new ObservableCollection<TourReservation>(_tourReservationRepository.GetAll());
            TourInstances = new ObservableCollection<TourInstance>();
            GetTourInstances();
            SetLocations();
        }
        public void GetTourInstances()
        {
            foreach(TourInstance tourInstance in listTourInstances)
            {
                if(tourInstance.Tour.Location.City==_tourInstance.Tour.Location.City && tourInstance.Tour.Location.Country == _tourInstance.Tour.Location.Country && tourInstance.Id!=_tourInstance.Id)
                {
                    if (!isFilled(tourInstance))
                    {
                        TourInstances.Add(tourInstance);
                    }
                }
            }
        }
        public Boolean isFilled(TourInstance tourInstance)
        {
            foreach(TourReservation tourReservation in TourReservations)
            {
                if(tourReservation.TourInstanceId==tourInstance.Id && tourReservation.CurrentGuestsNumber == 0)
                {
                    return true;
                }
            }
            return false;
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

            foreach (TourInstance tourInstance in listTourInstances)
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
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void Reserve(object sender, RoutedEventArgs e)
        {
            TourInstance currentTourInstance = (TourInstance)TourListDataGrid.CurrentItem;
            foreach (TourInstance tourInstance in TourInstances)
            {
                if(tourInstance.Id==currentTourInstance.Id)
                { 
                    currentTourInstance = tourInstance;
                }
            }
            //TourReservationForm tourReservationForm = new TourReservationForm(currentTourInstance, 3);
            //tourReservationForm.Show();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
