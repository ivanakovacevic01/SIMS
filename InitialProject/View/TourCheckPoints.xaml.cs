using InitialProject.Model;
using InitialProject.Repository;
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

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for TourCheckPoints.xaml
    /// </summary>
    public partial class TourCheckPoints : Window, INotifyPropertyChanged
    {
        public ObservableCollection<CheckPoint> AllPoints { get; set; }
        public ObservableCollection<CheckPoint> CurrentPoint { get; set; }

        public ObservableCollection<int> count { get; set; }
        public ObservableCollection<TourInstance> Tours { get; set; }


        private CheckPointRepository _pointsRepository;
        private TourRepository _tourRepository;
        private TourInstanceRepository _tourInstanceRepository;
        private TourReservationRepository tourReservationRepository;
        private AlertGuest2Repository alertGuest2Repository;

        private int callId = 0;

        private TourInstance _selected;

        private int counter = 1;


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TourCheckPoints(TourInstance selected,ObservableCollection<TourInstance>tours)
        {
            InitializeComponent();
            DataContext = this;
            _pointsRepository = new CheckPointRepository();
            _tourInstanceRepository= new TourInstanceRepository();
            tourReservationRepository = new TourReservationRepository();
            alertGuest2Repository = new AlertGuest2Repository();
            AllPoints = new ObservableCollection<CheckPoint>();
            CurrentPoint = new ObservableCollection<CheckPoint>();
            count= new ObservableCollection<int>();
            List<CheckPoint> points = _pointsRepository.GetAll();
            
            if (selected != null)
            {
                foreach (CheckPoint point in points)
                {
                    if (point.TourId == selected.Tour.Id)
                    {
                        AllPoints.Add(point);
                    }
                }
                foreach (CheckPoint point in AllPoints)
                {
                    if (point.Order == 1)
                        point.Checked = true;
                }

                _selected = selected;
            }

            if (AllPoints.Count!=0)
            {
                CurrentPoint.Add(AllPoints.ToList().Find(n => n.Order == counter));
                AddAlerts(CurrentPoint[0].Id, callId);
                CountGuests(CurrentPoint[0].Id, callId);
                Tours = tours;
            }
        }

        private void FinishTour(object sender, RoutedEventArgs e)
        {
            _selected.Finished = true;
            List<TourInstance>tours=_tourInstanceRepository.GetAll();
            _tourInstanceRepository.Update(_selected);

            int pointsSize=AllPoints.Count;
            List<CheckPoint> points=AllPoints.ToList();
            AllPoints.Clear();
            for(int i=0;i<pointsSize;i++)
            {
                points[i].Checked = true;
                AllPoints.Add(points[i]);
       
            }
            CurrentPoint[0].Checked = true;
            Tours.Remove(_selected);
            Finish.IsEnabled = false;
            FinishMessage.Content = "This tour is finished";
            
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            List<CheckPoint> points = AllPoints.ToList();

            foreach(CheckPoint checkPoint in AllPoints)
            {
                if (checkPoint.Order == counter)
                    checkPoint.Checked = true;
            }
            CurrentPoint.Remove(points.Find(n => n.Order == counter));
            counter++;
            CurrentPoint.Add(points.Find(n => n.Order == counter));
            if (counter == AllPoints.ToList().Count)
            {
                FinishMessage.Content = "This tour is finished";
                this.Next.IsEnabled = false;
                _selected.Finished = true;
                List<TourInstance> tours = _tourInstanceRepository.GetAll();
                _tourInstanceRepository.Update(_selected);


                int size = AllPoints.Count;
                List<CheckPoint> list = AllPoints.ToList();
                AllPoints.Clear();
                for (int i = 0; i < size; i++)
                {
                    list[i].Checked = true;
                    AllPoints.Add(list[i]);

                }

                Finish.IsEnabled = false;
               
                Tours.Remove(_selected);
            }

            int pointsSize = AllPoints.Count;
            List<CheckPoint> pointList = AllPoints.ToList();
            AllPoints.Clear();
            for (int i = 0; i < counter; i++)
            {
                points[i].Checked = true;
                AllPoints.Add(points[i]);

            }
            for(int i=counter;i < pointsSize; i++)
            {
                points[i].Checked = false;
                AllPoints.Add(points[i]);
            }

            AddAlerts(CurrentPoint[0].Id,callId);
            CountGuests(CurrentPoint[0].Id,callId);
        }

        private void AddAlerts(int currentPointId,int _callId)
        {
            List<TourReservation> tourReservations= tourReservationRepository.GetAll();
            List<TourReservation> availableReservations = new List<TourReservation>();
            foreach(TourReservation tour in tourReservations)
            {
                if (tour.TourInstanceId == _selected.Id)
                {
                    availableReservations.Add(tour);
                }
            }
            foreach(TourReservation tour in availableReservations)
            {
                AlertGuest2 alertGuest2 = new AlertGuest2();
                alertGuest2.Availability = false;
                alertGuest2.ReservationId = tour.Id;
                alertGuest2.Guest2Id = tour.GuestId;
                alertGuest2.CheckPointId = currentPointId;
                alertGuest2.InstanceId = _callId;
                AlertGuest2 savedAlert=alertGuest2Repository.Save(alertGuest2);
                callId++;

            }
        }
        private void CountGuests(int currentPointId, int instanceId)
        {
            int counter = 0;
            List<AlertGuest2> allAlerts= alertGuest2Repository.GetAll();
            List<AlertGuest2> tourInstanceAlerts= new List<AlertGuest2>();
            foreach(AlertGuest2 alert in allAlerts)
            {
                if (alert.CheckPointId == currentPointId && alert.Availability && alert.InstanceId==instanceId)
                {
                    counter++;
                }
            }
            count.Clear();
            count.Add(counter);
        }


    }
}
