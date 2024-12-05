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
    /// Interaction logic for CheckPointForm.xaml
    /// </summary>
    public partial class CheckPointForm : Window, INotifyPropertyChanged
    {
        private readonly CheckPointRepository _checkPointRepository;
        public ObservableCollection<CheckPoint> _checkPoints;
        public int _pointCounter;
        public Button _button;
        private string _name;
        public string NameT
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }
       
        public CheckPointForm(CheckPointRepository checkPointRepository,ObservableCollection<CheckPoint> tourPoints,Button addTour)
        {
            InitializeComponent();
            DataContext = this;
            _checkPointRepository = checkPointRepository;
            _checkPoints=tourPoints;
            _button = addTour;

        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AddCheckPoint(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                CheckPoint newCheckPoint = new CheckPoint();
                newCheckPoint.Name = NameT;
                newCheckPoint.Order = -1;
                newCheckPoint.TourId = -1;
                newCheckPoint.Checked = false;
                CheckPoint savedCheckPoint = _checkPointRepository.Save(newCheckPoint);
                _checkPoints.Add(savedCheckPoint);
                List<CheckPoint> tourCheckPoints = new List<CheckPoint>();
                foreach (CheckPoint checkPoint in _checkPoints)
                    if (checkPoint.TourId == -1)
                        tourCheckPoints.Add(checkPoint);
                if (tourCheckPoints.Count >= 2)
                    _button.IsEnabled = true;
                this.Close();
            }
        }

        private void CancelCheckPoint(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private bool Validate()
        {
            bool isValid = false;
            if (CheckPointName.Text.Trim().Equals(""))
            {
                isValid = false;
                CheckPointName.BorderBrush = Brushes.Red;
                CheckPointName.BorderThickness = new Thickness(1);
                NameLabel.Content = "This field can't be empty";
            }
            else
            {
                isValid = true;
                CheckPointName.BorderBrush = Brushes.Green;
                NameLabel.Content = string.Empty;
            }
            return isValid;
        }
    }
}
