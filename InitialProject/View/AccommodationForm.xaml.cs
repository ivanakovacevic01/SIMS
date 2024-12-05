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
using InitialProject.Repository;

namespace InitialProject.View.Owner
{
    /// <summary>
    /// Interaction logic for AccommodationForm.xaml
    /// </summary>
    public partial class AccommodationForm : Window
    {
        private AccommodationRepository accommodationRepository;
        public Accommodation accommodation { get; set; }
        private AccommodationTypeRepository accommodationTypeRepository;
        public List<AccommodationType> accommodationTypes { get; set; }
        private LocationRepository locationRepository;
        public Location location { get; set; }
        public ObservableCollection<AccommodationImage> Images { get; set; }
        public string Url { get; set; }
        private AccommodationImageRepository accommodationImageRepository;
        private ObservableCollection<Accommodation> accommodations;
        

        public AccommodationForm(ObservableCollection<Accommodation> oldAccommodations)
        {
            InitializeComponent();
            this.DataContext = this;
            accommodationRepository = new AccommodationRepository();
            accommodation = new Accommodation();
            accommodations = oldAccommodations;
            accommodationTypeRepository = new AccommodationTypeRepository();
            accommodationTypes = accommodationTypeRepository.GetAll();
            locationRepository = new LocationRepository();
            location = new Location();
            Images = new ObservableCollection<AccommodationImage>();
            accommodationImageRepository = new AccommodationImageRepository();

        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            accommodation.Id = accommodationRepository.NextId();
            locationRepository.Add(location);
            accommodation.Location = location;
            accommodations.Add(accommodation); 
            accommodationRepository.Add(accommodation);
            AddImages();
            
            this.Close();
        }

        private void AddImages()
        {
            foreach (AccommodationImage image in Images)
            {
                image.Accommodation = accommodation;
                image.Id = accommodationImageRepository.Add(image.Url, image.Accommodation);
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NewImage_Click(object sender, RoutedEventArgs e)
        {
            AccommodationImage image = new AccommodationImage();
            image.Url = Url;
            image.Id = -1;
            Images.Add(image);
            TextBoxUrl.Clear();
        }
    }
}
