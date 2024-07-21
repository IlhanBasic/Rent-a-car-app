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
    /// Interaction logic for pagePayment.xaml
    /// </summary>
    public partial class pagePayment : Page,INotifyPropertyChanged
    {
        RENTACAREntities1 context = new RENTACAREntities1();
        private ObservableCollection<Booking> reservations;

        public ObservableCollection<Booking> Reservations
        {
            get { return reservations; }
            set 
            { 
                reservations = value;
                OnPropertyChanged();
            }
        }


        public pagePayment()
        {
            InitializeComponent();
            Reservations = new ObservableCollection<Booking>();
            var temp = context.Bookings.ToList();
            foreach ( var booking in temp )
            {
                Reservations.Add( booking );
            }
        }
        public void refreshReservations()
        {
            reservations.Clear();
            var temp = context.Bookings.ToList();
            foreach (var booking in temp)
            {
                Reservations.Add(booking);
            }
        }
        private bool _isFirstEdit = true;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            if (_isFirstEdit)
            {
                _isFirstEdit = false;
                ((DataGrid)sender).IsReadOnly = true;
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if(dgShow.SelectedItem == null)
            {
                MessageBox.Show("Morate selektovati rezervaciju za brisanje");
            }
            else
            {
                if(MessageBox.Show("Da li ste sigurni da želite da obrišete rezervaciju?","Upozorenje",MessageBoxButton.YesNo)==MessageBoxResult.Yes)
                {
                    var selektovano = dgShow.SelectedItem as Booking;
                    if(selektovano!= null )
                    {
                        context.Bookings.Remove(selektovano);
                        context.Vehicles.Find(selektovano.vehicleId).isReserved = false;
                        var customer = selektovano.customerId;
                        var _customer = context.Customers.FirstOrDefault(v=>v.id == customer);
                        if (_customer != null)
                        {
                            context.Customers.Remove(_customer);
                            context.SaveChanges();
                            refreshReservations();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Rezervacija ne moze biti obrisana");
                    }
                }
            }
        }
        
    }
}
