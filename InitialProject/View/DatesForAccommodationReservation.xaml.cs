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
    /// Interaction logic for DatesForAccommodationReservation.xaml
    /// </summary>
    public partial class DatesForAccommodationReservation : Window
    {
        public ObservableCollection<FreeDatesForAccommodationReservation> freeDatesForAccommodations { get; set; }
        Accommodation currentAccommodation;
        private AccommodationReservationRepository accommodationReservationRepository;
        private FreeDatesForAccommodationReservation selectedDateRange;
        public FreeDatesForAccommodationReservation SelectedDateRange
        {
            get { return selectedDateRange; }
            set
            {
                if (selectedDateRange != value)
                {
                    selectedDateRange = value;
                    this.OnPropertyChanged("SelectedDateRange");
                }
            }
        }

        public DatesForAccommodationReservation(Accommodation currentAccommodation, AccommodationReservationRepository accommodationReservationRepository)
        {
            InitializeComponent();
            this.DataContext = this;
            this.currentAccommodation = currentAccommodation;
            freeDatesForAccommodations = new ObservableCollection<FreeDatesForAccommodationReservation>();
            this.accommodationReservationRepository = accommodationReservationRepository;
            
        }

        private void ChooseDateButtonClick(object sender, RoutedEventArgs e)
        {
            AccommodationGuestsNumberInput guestsNumber = new AccommodationGuestsNumberInput(currentAccommodation, selectedDateRange, accommodationReservationRepository, freeDatesForAccommodations);
            guestsNumber.Owner = this;
            guestsNumber.Show();
          
        }
        public void AddNewDateRange(DateTime startDate, DateTime endDate)
        {
            freeDatesForAccommodations.Add(new FreeDatesForAccommodationReservation(startDate, endDate));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CancelChoosingDate(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
