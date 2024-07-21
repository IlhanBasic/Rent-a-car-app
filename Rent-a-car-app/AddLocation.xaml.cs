using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.Win32;

namespace Rent_a_car_app
{
    public partial class AddLocation : Window, INotifyPropertyChanged
    {
        private RENTACAREntities1 context = new RENTACAREntities1();
        private Location place;

        public Location Place
        {
            get { return place; }
            set
            {
                place = value;
                OnPropertyChanged();
            }
        }

        

        public AddLocation()
        {
            InitializeComponent();
            Place = new Location();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (string.IsNullOrEmpty(Place.nameLocation))
                {
                    MessageBox.Show("Ime lokacije je prazno ili null");
                    return;
                }

                var locations = context.Locations.ToList();
                bool existedLocation = locations.Any(loc => loc.nameLocation == Place.nameLocation);

                if (!existedLocation)
                {
                    context.Locations.Add(Place);
                    context.SaveChanges();
                    MessageBox.Show("Promene su uspešno sačuvane");
                }
                else
                {
                    MessageBox.Show("Ta lokacija već postoji u listi");
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo je do greške: {ex.Message}\n\n{ex.StackTrace}");
            }
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
