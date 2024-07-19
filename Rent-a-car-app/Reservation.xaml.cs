using System;
using System.Collections.Generic;
using System.Linq;
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
    /// <summary>
    /// Interaction logic for Reservation.xaml
    /// </summary>
    public partial class Reservation : Window
    {
        RENTACAREntities1 context= new RENTACAREntities1();
        private Customer  _customer;

        public Customer  _Customer
        {
            get { return _customer; }
            set { _customer = value; }
        }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int startLocation { get; set; }
        public int endLocation { get; set; }
        public Vehicle vehicle { get; set; }
        public Reservation(Vehicle v,DateTime fromDate,DateTime toDate,int fromLocation,int toLocation)
        {
            InitializeComponent();
            _Customer = new Customer();
            startDate = fromDate;
            endDate = toDate;
            startLocation = fromLocation;
            endLocation = toLocation;
            vehicle = new Vehicle();
        }

        public Booking makeReservation(Customer c, Vehicle v)
        {
            bool canMake = isValidReservation();
            if (canMake)
            {
                context.Customers.Append(c);
                context.Vehicles.Append(v);
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
                return reservation;
            }
            return null;
        }
        bool isValidReservation()
        {
            if (_Customer.firstName == null || _Customer.lastName == null ||
                _Customer.phone == null || _Customer.noCredCard == null || _Customer.email == null ||
                _Customer.PIN == null || _Customer.securityNo == null)
            {
                return false;
            }
            else if (!isValidPIN(_Customer.PIN) || !isValidSecurityNumber(_Customer.securityNo) ||
                !IsValidEmail(_Customer.email) || !IsValidInput(_Customer.firstName) ||
                !IsValidInput(_Customer.lastName) || !isValidPhoneNumber(_Customer.phone))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        string PINPattern = @"^\d{4}$";
        bool isValidPIN(int? PIN)
        {
            return Regex.IsMatch(PIN.ToString(), PINPattern);
        }
        string SecurityNumberPattern = @"^\d{4}$";
        bool isValidSecurityNumber(int? SecurityNumber)
        {
            return Regex.IsMatch(SecurityNumber.ToString(), SecurityNumberPattern);
        }
        string SimpleEmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, SimpleEmailPattern, RegexOptions.IgnoreCase);
        }
        string LettersOnlyPattern = @"^[a-zA-Z]+$";

        bool IsValidInput(string input)
        {
            return Regex.IsMatch(input, LettersOnlyPattern);
        }
        string PhoneNumbersPattern = @"^\d{9,15}$|^\d{3}-\d{3}-\d{4}$";

        bool isValidPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, PhoneNumbersPattern);
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            Booking booking = makeReservation(_Customer, vehicle);
                MessageBox.Show(booking.fromDate.ToString());
            if (booking!=null)
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
