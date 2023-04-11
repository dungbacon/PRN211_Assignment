using QuanLyKho_Project.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace QuanLyKho_Project.ViewModel
{
    public class SupplierViewModel : BaseViewModel
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ReloadCommand { get; set; }

        private ObservableCollection<Suplier> _List;
        public ObservableCollection<Suplier> List { get => _List; set { _List = value; OnPropertyChanged(); } }

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

        private Suplier _selectedItem;
        public Suplier SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    var item = DataProvider.Ins.DB.Supliers.Where(o => o.Id == SelectedItem.Id).FirstOrDefault();
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

        public SupplierViewModel()
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
            var objectsInput = DataProvider.Ins.DB.InputInfoes.Where(o => o.Object.IdSuplier == Id).ToList();
            DataProvider.Ins.DB.InputInfoes.RemoveRange(objectsInput);

            var objectsOutput = DataProvider.Ins.DB.OutputInfoes.Where(o => o.Object.IdSuplier == Id).ToList();
            DataProvider.Ins.DB.OutputInfoes.RemoveRange(objectsOutput);
            DataProvider.Ins.DB.SaveChanges();

            var objects = DataProvider.Ins.DB.Objects.Where(o => o.IdSuplier == Id).ToList();
            DataProvider.Ins.DB.Objects.RemoveRange(objects);
            DataProvider.Ins.DB.SaveChanges();

            var item = DataProvider.Ins.DB.Supliers.Where(o => o.Id == Id).FirstOrDefault();
            if (item != null)
            {
                DataProvider.Ins.DB.Supliers.Remove(item);
                DataProvider.Ins.DB.SaveChanges();
            }
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
            List = new ObservableCollection<Suplier>(DataProvider.Ins.DB.Supliers);
        }

        private bool CheckConditionAdd()
        {

            if (string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(Address) || string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(Email))
            {
                return false;
            }
            var check = DataProvider.Ins.DB.Supliers.FirstOrDefault(o => o.DisplayName == DisplayName);
            if (check != null)
            {
                return false;
            }
            return true;
        }

        private bool CheckConditionEdit()
        {
            if (SelectedItem == null)
            {
                return false;
            }
            return true;
        }

        private void AddCommandMethod()
        {
            var item = new Suplier
            {
                DisplayName = DisplayName,
                Address = Address,
                Phone = Phone,
                Email = Email,
                MoreInfo = MoreInfo,
                ContractDate = ContractDate,
            };
            DataProvider.Ins.DB.Supliers.Add(item);
            DataProvider.Ins.DB.SaveChanges();
            List.Add(item);
        }

        private void EditCommandMethod()
        {
            var unit = DataProvider.Ins.DB.Supliers.Where(p => p.Id == SelectedItem.Id).SingleOrDefault();
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
