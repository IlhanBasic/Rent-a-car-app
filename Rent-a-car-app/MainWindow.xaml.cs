using Rent_a_car_app.View;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Rent_a_car_app
{
    public partial class MainWindow : Window
    {
        Button clickedButton = null;
        public MainWindow(UserLogin User)
        {
            InitializeComponent();
            if(User != null && User.username=="admin")
            {
                btnVehicles.Visibility = Visibility.Visible;
                btnLocations.Visibility = Visibility.Visible;
                btnPayement.Visibility = Visibility.Visible;
                btnStat.Visibility = Visibility.Visible;
            }
            pageHome pageHome = new pageHome();
            mainShow.Content = pageHome;
            btnHome.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#bee6fd");
            btnHome.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#2b2f2f");
            clickedButton = btnHome;
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            pageHome pageHome = new pageHome();
            mainShow.Content = pageHome;
            if (clickedButton != null)
            {
                clickedButton.Background = Brushes.Transparent;
                clickedButton.Foreground = Brushes.Black;
            }
            btnHome.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#bee6fd");
            btnHome.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#2b2f2f");
            clickedButton = btnHome;
        }

        private void btnSchedule_Click(object sender, RoutedEventArgs e)
        {
            pageSchedule pageSchedule = new pageSchedule();
            mainShow.Content = pageSchedule;
            if (clickedButton != null)
            {
                clickedButton.Background = Brushes.Transparent;
                clickedButton.Foreground = Brushes.Black;
            }
            btnSchedule.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#bee6fd");
            btnSchedule.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#2b2f2f");
            clickedButton = btnSchedule;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }
    }

    
}
