using QuanLyKho_Project.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace QuanLyKho_Project.ViewModel
{
    public class AccountViewModel : BaseViewModel
    {

        private int _Id;
        public int Id { get => _Id; set { _Id = value; OnPropertyChanged(); } }

        private string _DisplayName;
        public string DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }

        private string _UserName;
        public string UserName { get => _UserName; set { _UserName = value; OnPropertyChanged(); } }

        private string _Password;
        public string Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }

        private ObservableCollection<UserRole> _Role;
        public ObservableCollection<UserRole> Role { get => _Role; set { _Role = value; OnPropertyChanged(); } }

        private UserRole _SelectedRole;
        public UserRole SelectedRole
        {
            get { return _SelectedRole; }
            set
            {
                _SelectedRole = value;
                OnPropertyChanged();
            }
        }

        private User _SelectedItem;
        public User SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (_SelectedItem != null)
                {
                    var item = DataProvider.Ins.DB.Users.Where(o => o.Id == SelectedItem.Id).FirstOrDefault();
                    if (item == null)
                    {
                        MessageBox.Show("The item is no longer here!");
                    }
                    else
                    {
                        Id = SelectedItem.Id;
                        DisplayName = SelectedItem.DisplayName;
                        UserName = SelectedItem.UserName;
                        Password = SelectedItem.Password;
                        SelectedRole = SelectedItem.UserRole;
                    }
                }
            }
        }

        private ObservableCollection<User> _List;
        public ObservableCollection<User> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ReloadCommand { get; set; }

        public AccountViewModel()
        {
            LoadList();
            AddCommand = new RelayCommand<object>((p) =>
            {
                var check = CheckCondition("add");
                return check;
            },
            (p) =>
            { AddCommandMethod(); });

            EditCommand = new RelayCommand<object>((p) =>
            {
                var check = CheckCondition("edit");
                return check;
            },
            (p) =>
            { EditCommandMethod(); });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                return checkConditionDelete();
            },
            (p) =>
            {
                DeleteMethod();
            }
            );
            ReloadCommand = new RelayCommand<object>((p) => { return true; }, (p) => { LoadList(); });

        }

        private void DeleteMethod()
        {
            var user = DataProvider.Ins.DB.Users.Where(o => o.Id == SelectedItem.Id).FirstOrDefault();
            DataProvider.Ins.DB.Users.Remove(user);
            DataProvider.Ins.DB.SaveChanges();
            LoadList();
        }

        private bool checkConditionDelete()
        {
            if (SelectedItem == null)
            {
                return false;
            }
            return true;
        }

        private void EditCommandMethod()
        {
            var item1 = DataProvider.Ins.DB.Users.Where(o => o.Id == Id).FirstOrDefault();
            item1.DisplayName = DisplayName;
            item1.UserName = UserName;
            item1.Password = Password;
            item1.IdRole = SelectedRole.Id;
            DataProvider.Ins.DB.SaveChanges();

        }

        private void LoadList()
        {
            List = new ObservableCollection<User>(DataProvider.Ins.DB.Users);
            Role = new ObservableCollection<UserRole>(DataProvider.Ins.DB.UserRoles);
        }

        private bool CheckCondition(string checkpoint)
        {
            switch (checkpoint)
            {
                case "add":
                    if (string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password) || SelectedRole == null)
                    {
                        return false;
                    }
                    var item1 = DataProvider.Ins.DB.Users.Where(o => o.UserName == UserName).FirstOrDefault();
                    if (item1 != null)
                    {
                        return false;
                    }
                    break;
                case "edit":
                    var item2 = DataProvider.Ins.DB.Users.Where(o => o.UserName == UserName).FirstOrDefault();
                    if (item2 == null || SelectedItem == null)
                    {
                        return false;
                    }
                    break;

            }
            return true;
        }

        void AddCommandMethod()
        {
            var item = new User
            {
                DisplayName = DisplayName,
                UserName = UserName,
                Password = Password,
                IdRole = SelectedRole.Id,
            };
            DataProvider.Ins.DB.Users.Add(item);
            DataProvider.Ins.DB.SaveChanges();
            List.Add(item);
        }
    }
}
