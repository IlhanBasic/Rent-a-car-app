using Rent_a_car_app.View;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Rent_a_car_app
{
    public partial class MainWindow : Window
    {

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
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            pageHome pageHome = new pageHome();
            mainShow.Content = pageHome;
        }

        private void btnSchedule_Click(object sender, RoutedEventArgs e)
        {
            pageSchedule pageSchedule = new pageSchedule();
            mainShow.Content = pageSchedule;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }
    }

    
}
