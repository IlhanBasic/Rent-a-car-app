using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Rent_a_car_app.View
{
    public partial class pageVehicles : Page, INotifyPropertyChanged
    {
        RENTACAREntities1 context = new RENTACAREntities1();
        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }

        private ObservableCollection<Vehicle> vehicles;
        public ObservableCollection<Vehicle> Vehicles
        {
            get { return vehicles; }
            set
            {
                vehicles = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public pageVehicles(UserLogin user)
        {
            InitializeComponent();

            Vehicles = new ObservableCollection<Vehicle>();
            var temp = context.Vehicles.ToList();
            foreach (var v in temp)
            {
                Vehicles.Add(v);
            }
            if (user.username == "admin")
            {
                btnAdminGroup.Visibility = Visibility.Visible;
            }
        }

        public bool isSelectedButton()
        {
            isSelected = lbShow.SelectedItem != null;
            return isSelected;
        }

        public void refreshVehicles()
        {
            Vehicles.Clear();
            var temp = context.Vehicles.ToList();
            foreach (var v in temp)
            {
                Vehicles.Add(v);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (isSelectedButton())
            {
                var v = lbShow.SelectedItem as Vehicle;
                EditVehicle edit = new EditVehicle(context, v);
                edit.Show();
                if (edit.isEdited)
                {
                edit.Closed += _Closed;
                }
            }
            else
            {
                MessageBox.Show("Morate selektovati vozilo za izmenu");
            }
        }

        private void _Closed(object sender, System.EventArgs e)
        {
            
            refreshVehicles();
            
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (isSelectedButton())
            {
                if (MessageBox.Show("Da li stvarno zelite da obrisete ovo vozilo ?", "Upozorenje", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    var selektovano = lbShow.SelectedItem as Vehicle;
                    if (selektovano != null)
                    {
                        if (selektovano.isReserved == false)
                        {
                            context.Vehicles.Remove(selektovano);
                            context.SaveChanges();
                            lbShow.ItemsSource = null;
                            lbShow.ItemsSource = Vehicles;
                            refreshVehicles();
                        }
                        else
                        {
                            MessageBox.Show("Vozilo ne sme biti rezervisano");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Neka greska");
                    }
                }
            }
            else
            {
                MessageBox.Show("Morate selektovati vozilo za brisanje");
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddVehicle addVehicle = new AddVehicle();
            addVehicle.Show();
            addVehicle.Closed += _Closed;
        }
    }
}
