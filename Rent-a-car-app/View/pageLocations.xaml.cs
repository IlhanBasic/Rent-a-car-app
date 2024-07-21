using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace Rent_a_car_app.View
{
    /// <summary>
    /// Interaction logic for pageLocations.xaml
    /// </summary>
    public partial class pageLocations : Page,INotifyPropertyChanged
    {
        RENTACAREntities1 context = new RENTACAREntities1();
        private ObservableCollection<Location> places;

        public ObservableCollection<Location> Places
        {
            get { return places; }
            set 
            { 
                places = value;
                OnPropertyChanged();
            }
        }
        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set 
            { 
                isSelected = value;
                OnPropertyChanged();
            }
        }
        public pageLocations(UserLogin user)
        {
            InitializeComponent();
            Places = new ObservableCollection<Location>();
            var temp = context.Locations.ToList();
            foreach(var location in temp) 
            {
                Places.Add(location);
            }
            if (user.username == "admin")
            {

            }
        }
        public bool isSelectedButton()
        {
            isSelected = dgShow.SelectedItem != null;
            return isSelected;
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (isSelectedButton())
            {
                var v = dgShow.SelectedItem as Location;
                EditLocation edit = new EditLocation(context, v);
                edit.Show();
                if (edit.IsEdited)
                {
                    edit.Closed += _Closed;
                }
            }
            else
            {
                MessageBox.Show("Morate selektovati vozilo za izmenu");
            }
        }

        private void _Closed(object sender, EventArgs e)
        {
            refreshLocations();
        }

        private void refreshLocations()
        {
            Places.Clear();
            var temp = context.Locations.ToList();
            foreach(var location in temp)
            {
                Places.Add(location);
            }
        }
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (isSelectedButton())
            {
                if (MessageBox.Show("Da li stvarno zelite da obrisete ovu lokaciju ?", "Upozorenje", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    var selektovano = dgShow.SelectedItem as Location;
                    if (selektovano != null)
                    {
                        
                            context.Locations.Remove(selektovano);
                            context.SaveChanges();
                            dgShow.ItemsSource = null;
                            dgShow.ItemsSource = Places;
                            refreshLocations();
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
            //AddVehicle addVehicle = new AddVehicle();
            //addVehicle.Show();
            //addVehicle.Closed += _Closed;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private bool _isFirstEdit = true;
        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            if (_isFirstEdit)
            {
                _isFirstEdit = false;
                ((DataGrid)sender).IsReadOnly = true;
            }
        }
    }
}
