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
using InitialProject.Serializer;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for TourReservationForm.xaml
    /// </summary>
    public partial class TourReservationForm : Window, INotifyPropertyChanged
    {
        private const string FilePath = "../../../Resources/Data/tourReservations.csv";
        private const string filePath = "../../../Resources/Data/tourInstances.csv";
        private int CurrentGuestsNumber;
        private int GuestsNumber;
        private int GuestId;
        private TourInstance CurrentTourInstance;
        private TourReservationRepository _tourReservationRepository;
        private List<TourReservation> _tourReservations;
        private readonly Serializer<TourReservation> _serializerTourReservations;
        private List<TourInstance> _tourInstances;
        private TourInstanceRepository _tourInstanceRepository;
        private readonly Serializer<TourInstance> _serializerTourInstances;
        public ObservableCollection<TourInstance> TourInstances { get; set; }
        public Label Label { get; set; }
        private Button Restart;
        public TourReservationForm(TourInstance currentTourInstance,int guestId,ObservableCollection<TourInstance> TourInstance,TourInstanceRepository tourInstanceRepository,Label label,Button restart)
        {
            InitializeComponent();
            DataContext = this;
            _serializerTourReservations = new Serializer<TourReservation>();
            _serializerTourInstances = new Serializer<TourInstance>();
            CurrentTourInstance = currentTourInstance;
            this.TourInstances = TourInstance;
            this.Restart = restart;
            this.Label = label;
            this._tourInstanceRepository = tourInstanceRepository;
            _tourReservations = _serializerTourReservations.FromCSV(FilePath);
            _tourInstances=_serializerTourInstances.FromCSV(filePath);
            _tourReservationRepository = new TourReservationRepository();
            GetCurrentGuestsNumber();
            GuestId = guestId;
        }
        public int GetCurrentGuestsNumber()
        {
            int reservationsNumber = 0;
            foreach (TourReservation tourReservation in _tourReservations)
            {
                if (CurrentTourInstance.Id == tourReservation.TourInstanceId)
                {
                    CurrentGuestsNumber = tourReservation.CurrentGuestsNumber;
                    continue;
                }
                reservationsNumber++;
            }
            if (_tourReservations.Count == 0 || reservationsNumber==_tourReservations.Count)
            {
                CurrentGuestsNumber = CurrentTourInstance.Tour.MaxGuests;
            }
            return CurrentGuestsNumber;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void incrementGuestsNumber_Click(object sender, RoutedEventArgs e)
        {
            int changedGuestsNumber;
            changedGuestsNumber = Convert.ToInt32(capacityNumber.Text) + 1;
            capacityNumber.Text = changedGuestsNumber.ToString();
        }
        private void decrementGuestsNumber_Click(object sender, RoutedEventArgs e)
        {
            int changedGuestsNumber;
            if (Convert.ToInt32(capacityNumber.Text) > 1)
            {
                changedGuestsNumber = Convert.ToInt32(capacityNumber.Text) - 1;
                capacityNumber.Text = changedGuestsNumber.ToString();
            }
        }
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            GuestsNumber = CurrentGuestsNumber - Convert.ToInt32(capacityNumber.Text);
            if (CurrentGuestsNumber == 0)
            {
                MessageBox.Show("There is no enough places for choosen number of people. Tour is completed.");
                List<TourInstance> listTours = _tourInstanceRepository.GetAll();
                TourInstances.Clear();
                foreach (TourInstance tourInstance in listTours)
                {
                    foreach (TourReservation tourReservation in _tourReservations)
                    {
                        if (tourReservation.TourInstanceId==tourInstance.Id && tourInstance.Id!=CurrentTourInstance.Id && tourInstance.Tour.Location.City == CurrentTourInstance.Tour.Location.City && tourInstance.Tour.Location.Country == CurrentTourInstance.Tour.Location.Country && tourReservation.TourInstanceId == CurrentTourInstance.Id && tourReservation.CurrentGuestsNumber > 0)
                            TourInstances.Add(tourInstance);
                    }
                }
                //Content = "Showing available tours: "; 
                Label.Content = "Showing available tours: ";
                Restart.Visibility = Visibility.Visible;
                this.Close();
                return;
            }
            if (GuestsNumber < 0 && CurrentGuestsNumber!=0)
            {
                MessageBox.Show("There is no enough places for choosen number of people. Available number of places for guest is "+CurrentGuestsNumber+".");
                return;
            }
            foreach(TourReservation tourReservation in _tourReservations)
            {
                if (CurrentTourInstance.Id==tourReservation.TourInstanceId)
                {
                    _tourReservationRepository.Delete(tourReservation);
                }
            }

            TourReservation newTourReservation = new TourReservation(CurrentTourInstance.Id,GuestsNumber,GuestId);
            _tourReservationRepository.Save(newTourReservation);
            this.Close();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
