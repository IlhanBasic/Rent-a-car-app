using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.Win32;

namespace Rent_a_car_app
{
    public partial class EditVehicle : Window, INotifyPropertyChanged
    {
        private RENTACAREntities1 context;
        private Vehicle vehicle;
        
        public Vehicle _Vehicle
        {
            get { return vehicle; }
            set
            {
                vehicle = value;
                OnPropertyChanged();
            }
        }
        private bool _isEdited;

        public bool isEdited 
        {
            get { return _isEdited; }
            set { _isEdited = value; }
        }


        public EditVehicle(RENTACAREntities1 dbContext, Vehicle v)
        {
            InitializeComponent();
            context = dbContext;
            _Vehicle = v;

            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ChooseImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                _Vehicle.imageUrl = openFileDialog.FileName;
                OnPropertyChanged(nameof(_Vehicle));
            }
        }

        private void rbYes_Click(object sender, RoutedEventArgs e)
        {
            _Vehicle.isReserved = true;
            OnPropertyChanged(nameof(_Vehicle));
        }

        private void rbNo_Click(object sender, RoutedEventArgs e)
        {
            _Vehicle.isReserved = false;
            OnPropertyChanged(nameof(_Vehicle));
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            
            if (string.IsNullOrEmpty(_Vehicle.Brand) || string.IsNullOrEmpty(_Vehicle.description) || string.IsNullOrEmpty(_Vehicle.imageUrl)
                || _Vehicle.pricePerDay <= 0 || string.IsNullOrEmpty(_Vehicle.Model.name))
            {
                MessageBox.Show("Neko od polja nije popunjeno");
            }
            else
            {
                if (context.Entry(_Vehicle).State == System.Data.Entity.EntityState.Detached)
                {
                    context.Vehicles.Attach(_Vehicle);
                }

                context.SaveChanges();
                MessageBox.Show("Promene su uspesno sacuvane");
                isEdited = true;
                this.Close();
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            isEdited = false;
            this.Close();
        }
    }
}
