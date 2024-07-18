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
    /// Interaction logic for pageSchedule.xaml
    /// </summary>
    public partial class pageSchedule : Page,INotifyPropertyChanged
    {
        RENTACAREntities1 context = new RENTACAREntities1 ();

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
        public pageSchedule()
        {
            InitializeComponent();
            Vehicles = new ObservableCollection<Vehicle>();
            var temp = context.Vehicles.ToList();
            foreach(var v in temp)
            {
                Vehicles.Add(v);
            }
        }


    }
 
}
