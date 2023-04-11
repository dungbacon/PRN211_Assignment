using QuanLyKho_Project.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace QuanLyKho_Project.ViewModel
{
    public class CustomerViewModel : BaseViewModel
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ReloadCommand { get; set; }

        private ObservableCollection<Customer> _List;
        public ObservableCollection<Customer> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private int _Id;
        public int Id { get => _Id; set { _Id = value; OnPropertyChanged(); } }

        private string _DisplayName;
        public string DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }

        private string _Address;
        public string Address { get => _Address; set { _Address = value; OnPropertyChanged(); } }

        private string _Phone;
        public string Phone { get => _Phone; set { _Phone = value; OnPropertyChanged(); } }

        private string _Email;
        public string Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }

        private string _MoreInfo;
        public string MoreInfo { get => _MoreInfo; set { _MoreInfo = value; OnPropertyChanged(); } }

        private Nullable<System.DateTime> _ContractDate;
        public Nullable<System.DateTime> ContractDate { get => _ContractDate; set { _ContractDate = value; OnPropertyChanged(); } }

        private Customer _selectedItem;
        public Customer SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    var item = DataProvider.Ins.DB.Customers.Where(o => o.Id == SelectedItem.Id).FirstOrDefault();
                    if (item == null)
                    {
                        MessageBox.Show("The item is no longer here!");
                    }
                    else
                    {
                        Id = SelectedItem.Id;
                        DisplayName = SelectedItem.DisplayName;
                        Address = SelectedItem.Address;
                        Phone = SelectedItem.Phone;
                        Email = SelectedItem.Email;
                        MoreInfo = SelectedItem.MoreInfo;
                        ContractDate = SelectedItem.ContractDate;
                    }

                }
            }
        }


        public CustomerViewModel()
        {
            LoadList();
            AddCommand = new RelayCommand<object>((p) =>
            {
                return CheckConditionAdd();
            },
            (p) =>
            { AddCommandMethod(); });

            EditCommand = new RelayCommand<object>((p) =>
            {
                return CheckConditionEdit();
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
            var objectsOutput = DataProvider.Ins.DB.OutputInfoes.Where(o => o.IdCustomer == Id).ToList();
            DataProvider.Ins.DB.OutputInfoes.RemoveRange(objectsOutput);
            DataProvider.Ins.DB.SaveChanges();

            var customer = DataProvider.Ins.DB.Customers.Where(o => o.Id == SelectedItem.Id).FirstOrDefault();
            DataProvider.Ins.DB.Customers.Remove(customer);
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

        private void LoadList()
        {
            List = new ObservableCollection<Customer>(DataProvider.Ins.DB.Customers);
        }

        private bool CheckConditionAdd()
        {

            if (string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(Address) || string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Email))
            {
                return false;
            }
            var check = DataProvider.Ins.DB.Customers.FirstOrDefault(o => o.DisplayName == DisplayName);
            if (check != null)
            {
                return false;
            }
            return true;
        }

        private bool CheckConditionEdit()
        {
            if (string.IsNullOrEmpty(DisplayName))
            {
                return false;
            }
            var check = DataProvider.Ins.DB.Customers.FirstOrDefault(o => o.Id == SelectedItem.Id);
            if (check == null)
            {
                return false;
            }
            return true;
        }

        private void AddCommandMethod()
        {
            var item = new Customer
            {
                DisplayName = DisplayName,
                Address = Address,
                Phone = Phone,
                Email = Email,
                MoreInfo = MoreInfo,
                ContractDate = ContractDate,
            };
            DataProvider.Ins.DB.Customers.Add(item);
            DataProvider.Ins.DB.SaveChanges();
            List.Add(item);
        }

        private void EditCommandMethod()
        {
            var unit = DataProvider.Ins.DB.Customers.Where(p => p.Id == SelectedItem.Id).SingleOrDefault();
            unit.DisplayName = DisplayName;
            unit.Address = Address;
            unit.Phone = Phone;
            unit.Email = Email;
            unit.MoreInfo = MoreInfo;
            unit.ContractDate = ContractDate;
            DataProvider.Ins.DB.SaveChanges();
        }
    }
}
