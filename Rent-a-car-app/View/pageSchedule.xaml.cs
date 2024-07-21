using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace Rent_a_car_app.View
{
    /// <summary>
    /// Interaction logic for pageSchedule.xaml
    /// </summary>
    public partial class pageSchedule : Page, INotifyPropertyChanged
    {
        RENTACAREntities1 context = new RENTACAREntities1();

        private ObservableCollection<Vehicle> vehicles;
        private ObservableCollection<Location> locations;
        private double selectedPrice;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<Vehicle> Vehicles
        {
            get { return vehicles; }
            set
            {
                vehicles = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Location> Locations
        {
            get { return locations; }
            set
            {
                locations = value;
                OnPropertyChanged();
            }
        }

        public double SelectedPrice
        {
            get { return selectedPrice; }
            set
            {
                selectedPrice = value;
                OnPropertyChanged();
                FilterVehicles();
            }
        }

        public pageSchedule()
        {
            InitializeComponent();
            Vehicles = new ObservableCollection<Vehicle>();
            Locations = new ObservableCollection<Location>();

            var temp = context.Vehicles.Where(v=>v.isReserved==false).ToList();
            foreach (var v in temp)
            {
                Vehicles.Add(v);
            }

            var tmp = context.Locations.ToList();
            foreach (var loc in tmp)
            {
                Locations.Add(loc);
            }

            
            slajderOcena.ValueChanged += (s, e) => SelectedPrice = e.NewValue;
        }

        private void FilterVehicles()
        {
            var maxPrice = (decimal)SelectedPrice;

            var filteredVehicles = context.Vehicles
                .Where(v => v.pricePerDay.HasValue && v.pricePerDay.Value <= maxPrice)
                .ToList();

            Vehicles.Clear();
            foreach (var v in filteredVehicles)
            {
                Vehicles.Add(v);
            }
        }


        //private void btnCommit_Click(object sender, RoutedEventArgs e)
        //{
        //    ValidateInputs();
        //}
        private void btnCommit_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                ListBoxItem listBoxItem = FindVisualParent<ListBoxItem>(button);
                if (listBoxItem != null)
                {
                    ListBox listBox = FindVisualParent<ListBox>(listBoxItem);
                    if (listBox != null)
                    {
                        Vehicle selectedVehicle = listBoxItem.Content as Vehicle;
                        if (selectedVehicle != null)
                        {
                            ValidateInputs(selectedVehicle);
                        }
                    }
                }
            }
        }

        public static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            while (parentObject != null)
            {
                if (parentObject is T parent)
                {
                    return parent;
                }
                parentObject = VisualTreeHelper.GetParent(parentObject);
            }
            return null;
        }
        private void refreshVehicle()
        {
            var temp = context.Vehicles.Where(v=>v.isReserved==false).ToList();
            Vehicles.Clear();
            foreach (var v in temp)
            {
                Vehicles.Add(v);
            }
        }
        private void ValidateInputs(Vehicle v)
        {
            if (!dtStart.SelectedDate.HasValue || !dtEnd.SelectedDate.HasValue)
            {
                MessageBox.Show("Morate odabrati datume!");
                return; 
            }
            if (cbStart.SelectedItem == null || cbEnd.SelectedItem == null)
            {
                MessageBox.Show("Morate odabrati lokacije!");
                return;
            }
            DateTime DtStart = dtStart.SelectedDate.Value;
            DateTime DtEnd = dtEnd.SelectedDate.Value;
            Location now = cbStart.SelectedItem as Location;
            int idNow = now.id;
            Location after = cbEnd.SelectedItem as Location;
            int idLater = after.id;
            if (DtStart < DateTime.Now || DtEnd < DateTime.Now || DtStart >= DtEnd)
            {
                MessageBox.Show("Morate izabrati validne datume. Datum početka ne može biti pre sadašnjeg datuma i datum završetka ne može biti pre datuma početka.");
                return;
            }
            Reservation reservation = new Reservation(v,DtStart, DtEnd, idNow, idLater);
            reservation.Show();
            reservation.Closed += (s, e) => { refreshVehicle(); };
        }

    }


}
