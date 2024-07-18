using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Rent_a_car_app.View
{
    public partial class pageHome : Page
    {
        public pageHome()
        {
            InitializeComponent();
        }

        private void OpenFacebook(object sender, RoutedEventArgs e)
        {
            OpenUrl("https://www.facebook.com/");
        }

        private void OpenInstagram(object sender, RoutedEventArgs e)
        {
            OpenUrl("https://www.instagram.com/");
        }

        private void OpenX(object sender, RoutedEventArgs e)
        {
            OpenUrl("https://www.x.com/");
        }

        private void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening {url}: {ex.Message}");
            }
        }
    }
}
