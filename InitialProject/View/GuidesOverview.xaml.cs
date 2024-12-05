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
    /// Interaction logic for GuidesOverview.xaml
    /// </summary>
    public partial class GuidesOverview : Window,INotifyPropertyChanged
    {
        public ObservableCollection<TourInstance> Tours { get; set; }
        public TourInstance _selected;
        private TourRepository _tourRepository;
        private TourInstanceRepository _tourInstanceRepository;

        public TourInstance Selected
        {
            get { return _selected; }
            set
            {
                if (value != _selected)
                    _selected = value;
                StartButton.IsEnabled= true;
                OnPropertyChanged();
            }
        }
        public GuidesOverview()
        {
            InitializeComponent();
            DataContext = this;
            _tourRepository = new TourRepository();
            _tourInstanceRepository = new TourInstanceRepository();
            Tours = new ObservableCollection<TourInstance>(_tourInstanceRepository.GetByStart(DateTime.Now));
            if(Selected ==null)
                StartButton.IsEnabled=false;
            //string todayDate = DateTime.Now.Date.ToString();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TourForm tourForm = new TourForm(Tours);
            tourForm.Show();

        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void StartTour(object sender, RoutedEventArgs e)
        {
            if (Selected != null)
            {
                TourCheckPoints checkPoints = new TourCheckPoints(Selected, Tours);
                checkPoints.Show();
            }
            
        }

        private void SignOutClick(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            this.Close();
        }

    }
}
