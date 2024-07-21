using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class Reservation : Window, INotifyPropertyChanged
    {
        RENTACAREntities1 context = new RENTACAREntities1();
        private Customer _customer;

        public Customer _Customer
        {
            get { return _customer; }
            set 
            { 
                _customer = value;
                OnPropertyChanged();
            }
        }

        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int startLocation { get; set; }
        public int endLocation { get; set; }
        public Vehicle vehicle { get; set; }

        private decimal totalBill;
        public decimal TotalBill
        {
            get { return totalBill; }
            set
            {
                totalBill = value;
                OnPropertyChanged();
            }
        }

        public Reservation(Vehicle v, DateTime fromDate, DateTime toDate, int fromLocation, int toLocation)
        {
            InitializeComponent();
            _Customer = new Customer();
            startDate = fromDate;
            endDate = toDate;
            startLocation = fromLocation;
            endLocation = toLocation;
            vehicle = v;
            calculateTotalBill();
        }

        public void calculateTotalBill()
        {
            TimeSpan razlika = endDate - startDate;
            TotalBill = (int)razlika.Days * (vehicle.pricePerDay ?? 0); 
        }

        public Booking makeReservation(Customer c, Vehicle v)
        {
            bool canMake = isValidReservation();
            if (canMake)
            {
                Customer customer = new Customer
                {
                    id = _Customer.id,
                    firstName = c.firstName,
                    lastName = c.lastName,
                    phone = c.phone,
                    noCredCard = c.noCredCard,
                    email = c.email,
                    securityNo = c.securityNo,
                    PIN = c.PIN
                };

                Booking reservation = new Booking
                {
                    customerId = customer.id,
                    vehicleId = vehicle.id,
                    fromDate = startDate,
                    toDate = endDate,
                    makeReservationDate = DateTime.Now,
                    pickupLocationId = startLocation,
                    returnLocationId = endLocation
                };

                context.Vehicles.Find(v.id).isReserved=true;
                context.Customers.Add(c);
                context.Bookings.Add(reservation);
                context.SaveChanges();
                return reservation;
            }
            return null;
        }
        bool isValidReservation()
        {
            if (string.IsNullOrWhiteSpace(_Customer.firstName))
            {
                MessageBox.Show("Ime nije uneseno.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(_Customer.lastName))
            {
                MessageBox.Show("Prezime nije uneseno.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(_Customer.phone))
            {
                MessageBox.Show("Telefon nije unesen.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(_Customer.noCredCard))
            {
                MessageBox.Show("Broj računa nije unesen.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(_Customer.email))
            {
                MessageBox.Show("Email nije unesen.");
                return false;
            }

            if (!_Customer.PIN.HasValue)
            {
                MessageBox.Show("PIN nije unesen.");
                return false;
            }

            if (!_Customer.securityNo.HasValue)
            {
                MessageBox.Show("Sigurnosni broj nije unesen.");
                return false;
            }

            // Ako su svi podaci prisutni
            MessageBox.Show("Uspesno ste rezervisali vozilo koje vas ceka na naznačenoj adresi.");
            return true;
        }

        //bool isValidReservation()
        //{
        //    if (string.IsNullOrWhiteSpace(_Customer.firstName) ||
        //        string.IsNullOrWhiteSpace(_Customer.lastName) ||
        //        string.IsNullOrWhiteSpace(_Customer.phone) ||
        //        string.IsNullOrWhiteSpace(_Customer.noCredCard) ||
        //        string.IsNullOrWhiteSpace(_Customer.email) ||
        //        !_Customer.PIN.HasValue || !_Customer.securityNo.HasValue)
        //    {
        //        return false;
        //    }
        //    else if (!isValidPIN(_Customer.PIN.Value) ||
        //             !isValidSecurityNumber(_Customer.securityNo.Value) ||
        //             !IsValidEmail(_Customer.email) ||
        //             !IsValidInput(_Customer.firstName) ||
        //             !IsValidInput(_Customer.lastName) ||
        //             !isValidPhoneNumber(_Customer.phone))
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}


        //string PINPattern = @"^\d{4}$";
        //bool isValidPIN(int? PIN)
        //{
        //    return Regex.IsMatch(PIN.ToString(), PINPattern);
        //}

        //string SecurityNumberPattern = @"^\d{4}$";
        //bool isValidSecurityNumber(int? SecurityNumber)
        //{
        //    return Regex.IsMatch(SecurityNumber.ToString(), SecurityNumberPattern);
        //}

        //string SimpleEmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        //bool IsValidEmail(string email)
        //{
        //    return Regex.IsMatch(email, SimpleEmailPattern, RegexOptions.IgnoreCase);
        //}

        //string LettersOnlyPattern = @"^[a-zA-Z]+$";
        //bool IsValidInput(string input)
        //{
        //    return Regex.IsMatch(input, LettersOnlyPattern);
        //}

        //string PhoneNumbersPattern = @"^\d{9,15}$|^\d{3}-\d{3}-\d{4}$";
        //bool isValidPhoneNumber(string phoneNumber)
        //{
        //    return Regex.IsMatch(phoneNumber, PhoneNumbersPattern);
        //}

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            Booking booking = makeReservation(_Customer, vehicle);
            if (booking != null)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("Morate popuniti sva polja da biste rezervisali vozilo!");
            }
        }
    }
}
