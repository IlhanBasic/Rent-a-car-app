using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Data.Entity;
using System.Windows.Controls;

namespace Rent_a_car_app
{
    public partial class Login : Window, INotifyPropertyChanged
    {
        RENTACAREntities1 context = new RENTACAREntities1();
        private UserLogin user;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public UserLogin User
        {
            get { return user; }
            set
            {
                user = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<UserLogin> Users { get; set; } = new ObservableCollection<UserLogin>();

        public Login()
        {
            InitializeComponent();
            User = new UserLogin();
            var temp = context.UserLogins.ToList();
            foreach (var u in temp)
            {
                Users.Add(u);
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.WindowState = WindowState.Minimized;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var isExist = Users.FirstOrDefault(u => u.username == User.username);
            if (isExist != null)
            {
                if (VerifyPassword(txtPass.Password, isExist.passwordHash))
                {
                    MainWindow mainWindow = new MainWindow(User);
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Pogresna lozinka");
                }
            }
            else
            {
                MessageBox.Show("Uneti korisnik ne postoji, probajte drugo ime");
            }
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
        }
    }
}
