using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using System.Net;
using System.IO;
using System.DirectoryServices.ActiveDirectory;


namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for AccommodationPhotosView.xaml
    /// </summary>
    public partial class AccommodationPhotosView : Window
    {
        public AccommodationPhotosView(List<string> imagesUrl)
        {
            InitializeComponent();
            this.DataContext = this;

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

