using QuanLyKho_Project.CommonService;
using QuanLyKho_Project.CommonService.Repository;
using System;
using System.Net;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLyKho_Project.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        //public bool IsLogin { get; set; }
        private string _username;
        private string _password;
        private bool _isViewInvisible = true;
        public string UserName { get => _username; set { _username = value; OnPropertyChanged(); } }
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }
        public bool IsViewInvisible { get => _isViewInvisible; set { _isViewInvisible = value; OnPropertyChanged(); } }

        public ICommand LoginCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        private IUserRepository userRepository;


        public LoginViewModel()
        {
            userRepository = new UserRepository();
            //IsLogin = false;
            LoginCommand = new RelayCommand<Window>(CanExecuteLoginCmd, ExecuteLoginCmd);
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
            CloseCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });

        }

        private void ExecuteLoginCmd(Window o)
        {
            var user = userRepository.AuthenticateUser(new NetworkCredential(UserName, Password));
            if (user != null)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(UserName), new string[] { user.UserRole.DisplayName });
                o.Visibility = Visibility.Hidden;
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else
            {
                MessageBox.Show("Invalid username or password!");
            }
        }

        private bool CanExecuteLoginCmd(Window obj)
        {
            bool isValidate;
            if (String.IsNullOrEmpty(UserName) || String.IsNullOrEmpty(Password))
            {
                isValidate = false;
            }
            else
            {
                isValidate = true;
            }
            return isValidate;
        }

        //private void Login(Window p)
        //{
        //    if (p == null)
        //    {
        //        return;
        //    }
        //    var user = DataProvider.Ins.DB.Users.Where(o => o.UserName == UserName && o.Password == Password).FirstOrDefault();
        //    if (user != null)
        //    {
        //        IsLogin = true;
        //        p.Close();

        //    }
        //    else
        //    {
        //        IsLogin = false;
        //        MessageBox.Show("Invalid username or password!");
        //    }
        //}
    }
}
