using InitialProject.Forms;
using InitialProject.Model;
using InitialProject.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for TourForm.xaml
    /// </summary>
    public partial class TourForm : Window, INotifyPropertyChanged
    {


        private readonly TourRepository _tourRepository;
        private readonly LocationRepository _locationRepository;
        private readonly CheckPointRepository _checkPointRepository;
        private readonly TourImageRepository _tourImageRepository;
        private readonly TourInstanceRepository _tourInstanceRepository;
        public int pointCounter = 0;
        public ObservableCollection<CheckPoint> TourPoints { get; set; }
        public ObservableCollection<TourImage> TourImages { get; set; }
        public ObservableCollection<TourInstance> Instances { get; set; }
        public ObservableCollection<TourInstance> TodayInstances { get; set; }

        public Tour saved;
        private int _tourId;
        private string _hour;
        public string Hours
        {
            get => _hour;
            set
            {
                if (value != _hour)
                {
                    _hour = value;
                    OnPropertyChanged();
                }
            }
        }
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
        private string _city;
        public string City
        {
            get => _city;
            set
            {
                if (value != _city)
                {
                    _city = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _country;
        public string Country
        {
            get => _country;
            set
            {
                if (value != _country)
                {
                    _country = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _language;
        public string LanguageT
        {
            get => _language;
            set
            {
                if (value != _language)
                {
                    _language = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _duration;

        public string Duration
        {
            get => _duration;
            set
            {
                if (value != _duration)
                {
                    _duration = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _maxGuests;

        public string MaxGuests
        {
            get => _maxGuests;
            set
            {
                if (value != _maxGuests)
                {
                    _maxGuests = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime _start;
        public DateTime Start
        {
            get => _start;
            set
            {
                if (value != _start)
                {
                    _start = value;
                    OnPropertyChanged();
                }
            }
        }
        public TourForm(ObservableCollection<TourInstance> todayInstances)
        {
            InitializeComponent();
            DataContext = this;
            _tourRepository = new TourRepository();
            _locationRepository = new LocationRepository();
            _checkPointRepository = new CheckPointRepository();
            _tourImageRepository = new TourImageRepository();
            _tourInstanceRepository = new TourInstanceRepository();
            TourPoints = new ObservableCollection<CheckPoint>();
            TourImages = new ObservableCollection<TourImage>();
            Instances = new ObservableCollection<TourInstance>();
            TodayInstances = todayInstances;



        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        private void AddTour(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {

                Location newLocation = new Location(_city, _country);
                Location savedLocation = _locationRepository.Save(newLocation);

                Tour newTour = new Tour(NameT, Convert.ToInt32(_maxGuests), Convert.ToDouble(_duration), newLocation, _description, LanguageT);
                Tour savedTour = _tourRepository.Save(newTour);
                _tourId = savedTour.Id;
                saved = savedTour;
                //TourInstance newTourInstance = new TourInstance(savedTour, Start, Hours);
                //TourInstance savedTourInstance = _tourInstanceRepository.Save(newTourInstance);

                UpdateCheckPoints();
                AddImages();
                SaveInstances(savedTour);
                this.Close();
            }

        }
        private bool IsValid()
        {
            return IsNameValid() && IsMaximuGuestsNumberValid() && IsDurationValid() && IsCityValid() && IsCountryValid() && IsLanguageValid()
                   && IsDescriptionValid() && IsCheckPointsValid() && IsImagesValid() && IsDateTimeValid();

        }
        private bool IsNameValid()
        {
            var content = TourNameTB.Text;
            var regex = @"[A-Za-z]+(\\ [A-Za-z]+)*$";
            Match match = Regex.Match(content, regex, RegexOptions.IgnoreCase);
            bool isValid = false;
            if (TourNameTB.Text.Trim().Equals(""))
            {
                isValid = false;
                TourNameTB.BorderBrush = Brushes.Red;
                TourNameTB.BorderThickness = new Thickness(1);
                NameLabel.Content = "This field can't be empty";
            }
            else if (!match.Success)
            {
                isValid = false;
                TourNameTB.BorderBrush = Brushes.Red;
                TourNameTB.BorderThickness = new Thickness(1);
                NameLabel.Content = "Invalid name";
            }
            else
            {
                isValid = true;
                TourNameTB.BorderBrush = Brushes.Green;
                NameLabel.Content = string.Empty;
            }
            return isValid;
        }
        private bool IsMaximuGuestsNumberValid()
        {
            var content = MaxGuestsTB.Text;
            var regex = @"^[1-9]\d*$";
            Match match = Regex.Match(content, regex, RegexOptions.IgnoreCase);
            bool isValid = false;
            if (MaxGuestsTB.Text.Trim().Equals(""))
            {
                isValid = false;
                MaxGuestsTB.BorderBrush = Brushes.Red;
                MaxGuestsTB.BorderThickness = new Thickness(1);
                NameLabel.Content = "This field can't be empty";
            }
            else if (!match.Success)
            {
                MaxGuestLabel.Content = "This field should be positive number";
                MaxGuestsTB.BorderBrush = Brushes.Red;
                MaxGuestsTB.BorderThickness = new Thickness(1);
            }
            else
            {
                isValid = true;
                MaxGuestsTB.BorderBrush = Brushes.Green;
                MaxGuestLabel.Content = string.Empty;
            }
            return isValid;
        }

        private bool IsDurationValid()
        {
            var content = DurationTB.Text;
            var regex = "^(0|([1-9][0-9]*))(\\.[0-9]+)?$";
            var regexZero = "(0\\.0)$";
            var regexzero = "0$";
            Match match = Regex.Match(content, regex, RegexOptions.IgnoreCase);
            Match matchZero = Regex.Match(content, regexZero, RegexOptions.IgnoreCase);
            Match matchzero = Regex.Match(content, regexzero, RegexOptions.IgnoreCase);
            bool isValid = false;
            if (DurationTB.Text.Trim().Equals(""))
            {
                isValid = false;
                DurationTB.BorderBrush = Brushes.Red;
                DurationTB.BorderThickness = new Thickness(1);
                DurationLabel.Content = "This field can't be empty";
            }
            else if (!match.Success)
            { 
                DurationTB.BorderBrush = Brushes.Red;
                DurationLabel.Content = "This field should be positive double number";
                DurationTB.BorderThickness = new Thickness(1);
            }
            else if(matchZero.Success || matchzero.Success)
            {
                DurationTB.BorderBrush = Brushes.Red;
                DurationLabel.Content = "This field should be positive double number";
                DurationTB.BorderThickness = new Thickness(1);
            }
            else if(match.Success && (!matchZero.Success) && (!matchzero.Success))
            {
                isValid = true;
                DurationTB.BorderBrush = Brushes.Green;
                DurationLabel.Content = string.Empty;
            }
            return isValid;
        }



        private bool IsCityValid()
        {
            var content = CityTB.Text;
            var regex = @"[A-Za-z]+(\\ [A-Za-z]+)*$";
            Match match = Regex.Match(content, regex, RegexOptions.IgnoreCase);
            bool isValid = false;
            if (CityTB.Text.Trim().Equals(""))
            {
                isValid = false;
                CityTB.BorderBrush = Brushes.Red;
                CityTB.BorderThickness = new Thickness(1);
                CityLabel.Content = "This field can't be empty";
            }
            else if (!match.Success)
            {
                isValid = false;
                CityTB.BorderBrush = Brushes.Red;
                CityTB.BorderThickness = new Thickness(1);
                CityLabel.Content = "Only can contains letters and one space between words";
            }
            else
            {
                isValid = true;
                CityTB.BorderBrush = Brushes.Green;
                CityLabel.Content = string.Empty;
            }
            return isValid;
        }


        private bool IsCountryValid()
        {
            var content = CountryTB.Text;
            var regex = @"[A-Za-z]+(\\ [A-Za-z]+)*$";
            Match match = Regex.Match(content, regex, RegexOptions.IgnoreCase);
            bool isValid = false;
            if (CountryTB.Text.Trim().Equals(""))
            {
                isValid = false;
                CountryTB.BorderBrush = Brushes.Red;
                CountryTB.BorderThickness = new Thickness(1);
                CountryLabel.Content = "This field can't be empty";
            }
            else if (!match.Success)
            {
                isValid = false;
                CountryTB.BorderBrush = Brushes.Red;
                CountryTB.BorderThickness = new Thickness(1);
                CountryLabel.Content = "Only can contains letters and one space between words";
            }
            else
            {
                isValid = true;
                CountryTB.BorderBrush = Brushes.Green;
                CountryLabel.Content = string.Empty;
            }
            return isValid;
        }


        private bool IsLanguageValid()
        {
            var content = LanguageTB.Text;
            var regex = @"[A-Za-z]+(\\ [A-Za-z]+)*$";
            Match match = Regex.Match(content, regex, RegexOptions.IgnoreCase);
            bool isValid = false;
            if (LanguageTB.Text.Trim().Equals(""))
            {
                isValid = false;
                LanguageTB.BorderBrush = Brushes.Red;
                LanguageTB.BorderThickness = new Thickness(1);
                LanguageLabel.Content = "This field can't be empty";
            }
            else if (!match.Success)
            {
                isValid = false;
                LanguageTB.BorderBrush = Brushes.Red;
                LanguageTB.BorderThickness = new Thickness(1);
                LanguageLabel.Content = "Only can contains letters and one space between words";
            }
            else
            {
                isValid = true;
                LanguageTB.BorderBrush = Brushes.Green;
                LanguageLabel.Content = string.Empty;
            }
            return isValid;
        }


        private bool IsDescriptionValid()
        {
            var content = DescriptionTB.Text;
            var regex = @"[A-Za-z]([A-Za-z0-9]|.)*(\\ [A-Za-z0-9]+)*$";
            Match match = Regex.Match(content, regex, RegexOptions.IgnoreCase);
            bool isValid = false;
            if (DescriptionTB.Text.Trim().Equals(""))
            {
                isValid = false;
                DescriptionTB.BorderBrush = Brushes.Red;
                DescriptionTB.BorderThickness = new Thickness(1);
                DescriptionLabel.Content = "This field can't be empty";
            }
            else if (!match.Success)
            {
                isValid = false;
                DescriptionTB.BorderBrush = Brushes.Red;
                DescriptionTB.BorderThickness = new Thickness(1);
                DescriptionLabel.Content = "Invalid description";
            }
            else
            {
                isValid = true;
                DescriptionTB.BorderBrush = Brushes.Green;
                DescriptionLabel.Content = string.Empty;
            }
            return isValid;

        }

        private bool IsCheckPointsValid()
        {
            if (TourPoints.Count >= 2)
            {
                PointsGrid.BorderBrush = Brushes.Green;
                PointLabel.Content = string.Empty;
                return true;
            }
            else
            {
                PointsGrid.BorderBrush = Brushes.Red;
                PointsGrid.BorderThickness = new Thickness(1);
                PointLabel.Content = "There must be at least 2 checkpoints";
                return false;
            }
        }

        private bool IsImagesValid()
        {
            if (TourImages.Count >= 1)
            {
                ImagesGrid.BorderBrush = Brushes.Green;
                ImageLabel.Content = string.Empty;
                return true;
            }
            else
            {
                ImagesGrid.BorderBrush = Brushes.Red;
                ImageLabel.Content = "There should be at least 1 tour image";
                return false;
            }
        }
        

        private bool IsDateTimeValid()
        {
            if (Instances.Count == 0)
            {
                DateTimeBox.BorderBrush = Brushes.Red;
                DateTimeLabel.Content = "This field can't be empty";
                return false;
            }
            else
            {
                DateTimeBox.BorderBrush = Brushes.Green;
                DateTimeLabel.Content = string.Empty;
                return true;
            }
        }
        private void SaveInstances(Tour savedTour)
        {
            TourInstanceRepository tourInstanceRepository = new TourInstanceRepository();
            foreach(TourInstance instance in Instances)
            {
                instance.Tour = savedTour;
                tourInstanceRepository.Save(instance);
                DisplayIfToday(instance);
            }
        }

        private void DisplayIfToday(TourInstance instance)
        {
            if (instance.StartDate.Equals(DateTime.Today))
            {
                if (instance.Finished == false)
                {
                    string h = instance.StartClock.Split(':')[0];
                    string m = instance.StartClock.Split(":")[1];
                    string s = instance.StartClock.Split(":")[2];


                    string time = DateTime.Today.TimeOfDay.ToString();
                    string hour = time.Split(":")[0];
                    string minute = time.Split(":")[1];
                    string second = DateTime.Now.Second.ToString();
                    if (Convert.ToInt32(h) > Convert.ToInt32(hour))
                    {
                        TodayInstances.Add(instance);
                    }
                    else if (Convert.ToInt32(h) == Convert.ToInt32(hour) && Convert.ToInt32(m) > Convert.ToInt32(minute))
                    {
                        TodayInstances.Add(instance);
                    }
                    else if (Convert.ToInt32(h) == Convert.ToInt32(hour) && Convert.ToInt32(m) == Convert.ToInt32(minute) && Convert.ToInt32(s) > Convert.ToInt32(second))
                    {
                        TodayInstances.Add(instance);
                    }
                }
            }
        }
        private void AddImages()
        {
            List<TourImage> tourImages = _tourImageRepository.GetAll();
            foreach (TourImage image in tourImages)
            {
                if (image.TourId == -1)
                {
                    image.TourId = _tourId;
                    _tourImageRepository.Update(image);
                }
            }
        }

        private void UpdateCheckPoints()
        {
            List<CheckPoint> checkPoints = _checkPointRepository.GetAll();
            int i = 1;
            foreach (CheckPoint checkPoint in checkPoints)
            {
                if (checkPoint.TourId == -1)
                {
                    checkPoint.TourId = _tourId;
                    checkPoint.Order = i;
                    _checkPointRepository.Update(checkPoint);
                    i++;
                }
            }
        }

        private void AddCheckPoint(object sender, RoutedEventArgs e)
        {
            CheckPointForm form = new CheckPointForm(_checkPointRepository,TourPoints,this.AddNewTour);
            form.Show();

        }

        private void CancelTour(object sender, RoutedEventArgs e)
        {
            List<CheckPoint> checkPoints = _checkPointRepository.GetAll();
            foreach (CheckPoint checkPoint in checkPoints)
            {
                if (checkPoint.TourId == -1)
                {
                    _checkPointRepository.Delete(checkPoint);
                }
            }

            List<TourImage> tourImages = _tourImageRepository.GetAll();
            foreach (TourImage image in tourImages)
            {
                if (image.TourId == -1)
                { 
                    _tourImageRepository.Delete(image);
                }
            }
            
            this.Close();
        }

        private void AddTourImage(object sender, RoutedEventArgs e)
        {
            TourImageForm tourImageForm = new TourImageForm(_tourImageRepository,TourImages);
            tourImageForm.Show();
        }

        private void NewInstance(object sender, RoutedEventArgs e)
        {
            NewTourInstanceDate newTourInstance = new NewTourInstanceDate(saved, Instances);
            newTourInstance.Show();
        }

    }
}
