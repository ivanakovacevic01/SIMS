using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for AccommodationReservationForm.xaml
    /// </summary>
    public partial class AccommodationReservationForm : Window
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        TimeSpan difference { get; set; }

        
        public Accommodation currentAccommodation { get; set; }
        public List<AccommodationReservation> reservations { get; set; }
        private AccommodationReservationRepository accommodationReservationRepository;



        public AccommodationReservationForm(Accommodation currentAccommodation, ref AccommodationRepository accommodationRepository)
        {
            InitializeComponent();
            this.DataContext = this;
            this.currentAccommodation = currentAccommodation;
            accommodationReservationRepository = new AccommodationReservationRepository();
            this.reservations = new List<AccommodationReservation>(accommodationReservationRepository.GetAll());
            foreach(AccommodationReservation reservation in reservations)
            {
                reservation.currentAccommodation = accommodationRepository.GetAll().Find(accommodationInstance => accommodationInstance.Id == reservation.currentAccommodation.Id);

            }
            
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

        public bool IsValidDateInput()
        {
            return (StartDate >= EndDate || Convert.ToInt32(difference.TotalDays) < (Convert.ToInt32(numberOfDays.Text)-1) || StartDate.Date < DateTime.Now.Date || StartDate == null || EndDate == null);
        }

        public bool IsEnteredCorrectDateRange()
        {
            return (Convert.ToInt32(difference.TotalDays) < currentAccommodation.MinDaysForReservation || Convert.ToInt32(numberOfDays.Text) < currentAccommodation.MinDaysForReservation);
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            difference = EndDate.Subtract(StartDate);
            int daysNumberFromCalendar = Convert.ToInt32(difference.TotalDays);

            if (IsValidDateInput())
            {
                MessageBox.Show("Non valid input, please enter values again!");

            }
            else if (IsEnteredCorrectDateRange())
            {
                MessageBox.Show("The minimum number of days for booking this accommodation is " + currentAccommodation.MinDaysForReservation.ToString());
            }

            else
            {
                    reservations=accommodationReservationRepository.GetAll();
                    FindAvailableDates(currentAccommodation.Id);
            }


        }

        public bool IsDayAvailable(int currentAccommodationId, DateTime date)
        {
            foreach (AccommodationReservation reservation in reservations)
            {
                if (currentAccommodationId == reservation.currentAccommodation.Id)
                {
                    if (date >= reservation.ComingDate && date <= reservation.LeavingDate)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        
        public void AddAvailableDateToList(DateTime date, ref List<DateTime> freeDays, ref List<DateTime> freeDaysHelp, ref List<List<DateTime>> dateTimes)
        {
            freeDays.Add(date);
            freeDaysHelp.Add(date);
            freeDays.Sort();
            freeDaysHelp.Sort();
            if (freeDays.Count == Convert.ToInt32(numberOfDays.Text))
            {
                dateTimes.Add(freeDaysHelp);
                freeDays.Remove(freeDays[0]);
                freeDaysHelp = new List<DateTime>(freeDays);

            }
        }

        public void AddAvailableDateOutRangeToList(DateTime date, ref List<DateTime> freeDays, ref List<DateTime> freeDaysHelp, ref List<List<DateTime>> dateTimes)
        {
            freeDays.Add(date);
            freeDaysHelp.Add(date);
            freeDays.Sort();
            freeDaysHelp.Sort();
            if (freeDays.Count == Convert.ToInt32(numberOfDays.Text))
            {
                if(AreDatesConsecutive(freeDays))
                    dateTimes.Add(freeDaysHelp);
                freeDays.Remove(freeDays[0]);
                freeDaysHelp = new List<DateTime>(freeDays);

            }
        }

        public bool AreDatesConsecutive(List<DateTime> dates)
        {
            for (int i = 0; i < Convert.ToInt32(numberOfDays.Text) - 1; i++)
            {
                if (dates[i + 1].Subtract(dates[i]).TotalDays > 1)
                {
                    return false;
                }
            }
            return true;
        }

        public void FindAvailableDates(int currentAccommodationId)
        {
            reservations = accommodationReservationRepository.GetAll();
            DateTime start = StartDate;
            DateTime end = EndDate;
            bool freeDateRangeExists = false;    //ptretvoriti u metodu dio vezan za ovo
            List<DateTime> freeDays = new List<DateTime>();
            List<DateTime> freeDaysHelp = new List<DateTime>();
            List<List<DateTime>> dateTimes = new List<List<DateTime>>();

            for (int i = 0; i <= difference.TotalDays; i++)
            {
                if (IsDayAvailable(currentAccommodationId, start))
                {
                    AddAvailableDateToList(start, ref freeDays, ref freeDaysHelp, ref dateTimes);  //potrebno isprazniti liste negdje
                }
                start = start.AddDays(1);
            }

            if (dateTimes.Count > 0)
            {
                DatesForAccommodationReservation datesListWindow = new DatesForAccommodationReservation(currentAccommodation,accommodationReservationRepository);
                
                foreach (List<DateTime> dates in dateTimes)
                {
                    if (AreDatesConsecutive(dates))
                    {
                        freeDateRangeExists = true;
                        DateTime startDate = dates[0];
                        DateTime endDate = dates[Convert.ToInt32(numberOfDays.Text)-1];
                        datesListWindow.AddNewDateRange(startDate, endDate);
                    } 
                }

                if (freeDateRangeExists)
                    datesListWindow.Show();
                else
                    FindAvailableDatesOutRange();
            }

            else
            {
                FindAvailableDatesOutRange();
            }
        }

        public void FindAvailableDatesOutRange()
        {
            List<DateTime> freeDays = new List<DateTime>();
            List<DateTime> freeDaysHelp = new List<DateTime>();
            List<List<DateTime>> dateTimes = new List<List<DateTime>>();
            //pronalazim odakle poceti traziti?
            DateTime start = EndDate;
            while (IsDayAvailable(currentAccommodation.Id, start))
            {
                start = start.AddDays(-1);
            }

            start = start.AddDays(1);
            
            //sad trazim 3 slobodna niza
            while(dateTimes.Count < 3)
            {
                if (IsDayAvailable(currentAccommodation.Id, start))
                {
                    AddAvailableDateOutRangeToList(start, ref freeDays, ref freeDaysHelp, ref dateTimes);  //potrebno isprazniti liste negdje
                }
                start = start.AddDays(1);
            }

            DatesForAccommodationReservation datesListWindow = new DatesForAccommodationReservation(currentAccommodation, accommodationReservationRepository);
            foreach (List<DateTime> dates in dateTimes)
            {
                if (AreDatesConsecutive(dates))
                {
                    DateTime startDate = dates[0];
                    DateTime endDate = dates[Convert.ToInt32(numberOfDays.Text) - 1];
                    datesListWindow.AddNewDateRange(startDate, endDate);
                }
            }
            datesListWindow.Show();

        }



    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            if (Mouse.Captured is CalendarItem)
            {
                Mouse.Capture(null);
            }
        }


    }   
}

