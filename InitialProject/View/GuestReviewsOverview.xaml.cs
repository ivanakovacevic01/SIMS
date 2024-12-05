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

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for GuestReviewsOverview.xaml
    /// </summary>
    public partial class GuestReviewsOverview : Window
    {
        public ObservableCollection<ReviewOfGuest> reviews { get; set; }
        public GuestReviewsOverview()
        {
            InitializeComponent();
            DataContext = this;
            GuestReviewRepository guestReviewRepository = new GuestReviewRepository();
            reviews = new ObservableCollection<ReviewOfGuest>(guestReviewRepository.GetAll());
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
