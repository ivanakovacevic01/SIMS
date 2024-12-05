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
namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for TourDetails.xaml
    /// </summary>
    public partial class TourDetails : Window
    {
        public TourInstance TourInstance { get; set; }
        public TourDetails(List<string> imagesUrl,TourInstance tourInstance)
        {
            InitializeComponent();
            this.DataContext = this;
            TourInstance = tourInstance;
            foreach (string url in imagesUrl)
            {
                Image img = new Image();
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(url); ;
                bitmapImage.EndInit();
                img.Source = bitmapImage;
                setPhotoDimensions(ref img);
                imagesList.Items.Add(img);
            }
        }
        private void setPhotoDimensions(ref Image img)
        {
            img.Width = imagesList.Width;
            img.Height = imagesList.Height;
            img.VerticalAlignment = VerticalAlignment.Center;
            img.HorizontalAlignment = HorizontalAlignment.Center;
        }
    }
}
