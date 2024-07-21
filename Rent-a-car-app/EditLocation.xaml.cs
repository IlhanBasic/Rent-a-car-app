using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Rent_a_car_app
{
    /// <summary>
    /// Interaction logic for EditLocation.xaml
    /// </summary>
    public partial class EditLocation : Window,INotifyPropertyChanged
    {
        RENTACAREntities1 context;
        private bool isEdited;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsEdited
        {
            get { return isEdited; }
            set 
            { 
                isEdited = value;
                OnPropertyChanged();
            }
        }
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


        public EditLocation(RENTACAREntities1 context,Location l)
        {
            InitializeComponent();
            this.context = context;
            Place = l;
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(Place.nameLocation))
            {
                MessageBox.Show("Ime ne sme biti prazno");
            }
            else
            {
                if (context.Entry(Place).State == System.Data.Entity.EntityState.Detached)
                {
                    context.Locations.Attach(Place);
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
