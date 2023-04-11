using QuanLyKho_Project.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace QuanLyKho_Project.ViewModel
{
    public class OutputViewModel : BaseViewModel
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand PreviewTextInputCommand { get; set; }
        public ICommand ReloadCommand { get; set; }

        private ObservableCollection<OutputInfo> _List;
        public ObservableCollection<OutputInfo> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private ObservableCollection<Models.Object> _Objects;
        public ObservableCollection<Models.Object> Objects { get => _Objects; set { _Objects = value; OnPropertyChanged(); } }

        private ObservableCollection<Customer> _Customers;
        public ObservableCollection<Customer> Customers { get => _Customers; set { _Customers = value; OnPropertyChanged(); } }

        private ObservableCollection<Models.Suplier> _Supliers;
        public ObservableCollection<Models.Suplier> Supliers { get => _Supliers; set { _Supliers = value; OnPropertyChanged(); } }

        private string _Id;
        public string Id { get => _Id; set { _Id = value; OnPropertyChanged(); } }

        private string _InputId;
        public string InputId { get => _InputId; set { _InputId = value; OnPropertyChanged(); } }

        private int _CustomerId;
        public int CustomerId { get => _CustomerId; set { _CustomerId = value; OnPropertyChanged(); } }

        private int? _Count;
        public int? Count
        {
            get => _Count;
            set
            {
                _Count = value;
                OnPropertyChanged();
                var totalImportRaw = (from i in DataProvider.Ins.DB.InputInfoes
                                      where i.IdObject == SelectedObj.Id
                                      group i by i.IdObject into g
                                      select new { Total = g.Sum(i => i.Count) }).FirstOrDefault();
                var totalOuputRaw = (from i in DataProvider.Ins.DB.OutputInfoes
                                     where i.IdObject == SelectedObj.Id
                                     group i by i.IdObject into g
                                     select new { Total = g.Sum(i => i.Count) }).FirstOrDefault();
                int totalImport = totalImportRaw?.Total ?? 0;
                int totalOutput = totalOuputRaw?.Total ?? 0;
                int left = totalImport - totalOutput;
                if (Count > left)
                {
                    Count = left;
                    MessageBox.Show("Invalid input!");
                }
            }
        }

        private double? _OutputPrice;
        public double? OutputPrice
        {
            get => _OutputPrice;
            set
            {
                _OutputPrice = value;
                OnPropertyChanged();
            }
        }

        private string _Status;
        public string Status { get => _Status; set { _Status = value; OnPropertyChanged(); } }

        private Nullable<System.DateTime> _DateOutput;
        public Nullable<System.DateTime> DateOutput { get => _DateOutput; set { _DateOutput = value; OnPropertyChanged(); } }

        private Models.Object _SelectedObj;
        public Models.Object SelectedObj
        {
            get { return _SelectedObj; }
            set
            {
                _SelectedObj = value;
                OnPropertyChanged();
            }
        }

        private Models.Customer _SelectedCustomer;
        public Models.Customer SelectedCustomer
        {
            get { return _SelectedCustomer; }
            set
            {
                _SelectedCustomer = value;
                OnPropertyChanged();
            }
        }

        private Models.Suplier _SelectedSuplier;
        public Models.Suplier SelectedSuplier
        {
            get { return _SelectedSuplier; }
            set
            {
                _SelectedSuplier = value;
                OnPropertyChanged();
                if (SelectedSuplier != null)
                {
                    var listObjs = DataProvider.Ins.DB.Objects.Where(o => o.IdSuplier == SelectedSuplier.Id).ToList();
                    Objects = new ObservableCollection<Models.Object>(listObjs);
                }
            }
        }

        private OutputInfo _SelectedItem;
        public OutputInfo SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    var item = DataProvider.Ins.DB.OutputInfoes.Where(o => o.Id == SelectedItem.Id).FirstOrDefault();
                    if (item == null)
                    {
                        MessageBox.Show("The item is no longer here!");
                    }
                    else
                    {
                        Id = SelectedItem.Id;
                        SelectedSuplier = DataProvider.Ins.DB.Supliers.Where(o => o.Id == SelectedItem.Object.IdSuplier).FirstOrDefault();
                        SelectedObj = DataProvider.Ins.DB.Objects.Where(o => o.Id == SelectedItem.IdObject).FirstOrDefault();
                        DateOutput = SelectedItem.Output.DateOutput;
                        Count = SelectedItem.Count;
                        OutputPrice = SelectedItem.OutputPrice;
                        SelectedCustomer = DataProvider.Ins.DB.Customers.Where(o => o.Id == SelectedItem.IdCustomer).FirstOrDefault();
                        Status = SelectedItem.Status;
                    }
                }
            }
        }

        public OutputViewModel()
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
            ReloadCommand = new RelayCommand<object>((p) => { return true; }, (p) => { LoadList(); });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                return checkConditionDelete();
            },
            (p) =>
            {
                DeleteMethod();
            }
            );
        }

        private void DeleteMethod()
        {
            DataProvider.Ins.DB.OutputInfoes.Remove(SelectedItem);
            DataProvider.Ins.DB.SaveChanges();
            List.Remove(SelectedItem);
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
            List = new ObservableCollection<OutputInfo>(DataProvider.Ins.DB.OutputInfoes);
            Objects = new ObservableCollection<Models.Object>(DataProvider.Ins.DB.Objects);
            Customers = new ObservableCollection<Customer>(DataProvider.Ins.DB.Customers);
            Supliers = new ObservableCollection<Models.Suplier>(DataProvider.Ins.DB.Supliers);
        }

        private bool CheckConditionAdd()
        {
            if (SelectedObj == null)
            {
                return false;
            }
            return true;
        }

        private bool CheckConditionEdit()
        {
            var item = DataProvider.Ins.DB.OutputInfoes.Where(o => o.Id == SelectedItem.Id).FirstOrDefault();
            if (item == null)
            {
                return false;
            }
            return true;
        }

        private void AddCommandMethod()
        {
            var output = new Output
            {
                Id = Guid.NewGuid().ToString(),
                DateOutput = DateOutput
            };
            DataProvider.Ins.DB.Outputs.Add(output);
            DataProvider.Ins.DB.SaveChanges();

            var outputInfo = new OutputInfo
            {
                Id = Guid.NewGuid().ToString(),
                IdObject = SelectedObj.Id,
                IdCustomer = SelectedCustomer.Id,
                IdOutputInfo = output.Id,
                Count = Count,
                Status = Status,
                OutputPrice = OutputPrice,
            };
            DataProvider.Ins.DB.OutputInfoes.Add(outputInfo);
            DataProvider.Ins.DB.SaveChanges();
            List.Add(outputInfo);
        }

        private void EditCommandMethod()
        {
            var output = DataProvider.Ins.DB.Outputs.Where(o => o.Id == SelectedItem.IdOutputInfo).FirstOrDefault();
            if (output != null)
            {
                output.DateOutput = DateOutput;
            }
            var outputinfo = DataProvider.Ins.DB.OutputInfoes.Where(o => o.Id == SelectedItem.Id).FirstOrDefault();

            if (outputinfo != null)
            {
                outputinfo.IdObject = SelectedObj.Id;
                outputinfo.IdCustomer = SelectedCustomer.Id;
                outputinfo.IdOutputInfo = output.Id;
                outputinfo.Count = Count;
                outputinfo.Status = Status;
                outputinfo.OutputPrice = OutputPrice;
            }
            DataProvider.Ins.DB.SaveChanges();
        }
    }
}
