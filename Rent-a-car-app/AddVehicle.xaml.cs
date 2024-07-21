using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.Win32;

namespace Rent_a_car_app
{
    public partial class AddVehicle : Window, INotifyPropertyChanged
    {
        private RENTACAREntities1 context = new RENTACAREntities1();
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

        private Model newModel;

        public Model NewModel
        {
            get { return newModel; }
            set
            {
                newModel = value;
                OnPropertyChanged();
            }
        }

        public AddVehicle()
        {
            InitializeComponent();
            NewModel = new Model();
            _Vehicle = new Vehicle
            {
                Model = newModel
            };
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

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"{NewModel}");
            if (string.IsNullOrEmpty(_Vehicle.Brand) || string.IsNullOrEmpty(_Vehicle.description) || string.IsNullOrEmpty(_Vehicle.imageUrl)
                || _Vehicle.pricePerDay <= 0 || NewModel == null || string.IsNullOrEmpty(NewModel.name))
            {
                MessageBox.Show("Neko od polja nije popunjeno");
                return;
            }

            try
            {
                Model _newModel = new Model
                {
                    name = NewModel.name,
                };

                Model existedModel = context.Models
                    .FirstOrDefault(m => m.name.Equals(_newModel.name, StringComparison.OrdinalIgnoreCase));

                if (existedModel == null)
                {
                    context.Models.Add(_newModel);
                    context.SaveChanges(); 
                    existedModel = _newModel;
                }

                Vehicle newVehicle = new Vehicle
                {
                    imageUrl = _Vehicle.imageUrl,
                    Brand = _Vehicle.Brand,
                    description = _Vehicle.description,
                    isReserved = false,
                    modelId = existedModel.id,
                    pricePerDay = _Vehicle.pricePerDay
                };

                context.Vehicles.Add(newVehicle);
                context.SaveChanges();

                MessageBox.Show("Promene su uspešno sačuvane");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo je do greške: {ex.Message}");
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
