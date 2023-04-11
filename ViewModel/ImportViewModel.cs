using QuanLyKho_Project.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace QuanLyKho_Project.ViewModel
{
    public class ImportViewModel : BaseViewModel
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ReloadCommand { get; set; }
        public ICommand SearchComand { get; set; }
        public ICommand MouseLeftButtonDownWindowCommand { get; set; }

        private ObservableCollection<InputInfo> _List;
        public ObservableCollection<InputInfo> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private ObservableCollection<Models.Suplier> _Supliers;
        public ObservableCollection<Models.Suplier> Supliers { get => _Supliers; set { _Supliers = value; OnPropertyChanged(); } }

        private ObservableCollection<Models.Object> _Objects;
        public ObservableCollection<Models.Object> Objects { get => _Objects; set { _Objects = value; OnPropertyChanged(); } }

        private string _Id;
        public string Id { get => _Id; set { _Id = value; OnPropertyChanged(); } }

        private string _ObjectDisplayName;
        public string ObjectDisplayName { get => _ObjectDisplayName; set { _ObjectDisplayName = value; OnPropertyChanged(); } }

        private int? _Count;
        public int? Count { get => _Count; set { _Count = value; OnPropertyChanged(); } }

        private double? _InputPrice;
        public double? InputPrice { get => _InputPrice; set { _InputPrice = value; OnPropertyChanged(); } }

        private string _Status;
        public string Status { get => _Status; set { _Status = value; OnPropertyChanged(); } }

        private Nullable<System.DateTime> _DateInput;
        public Nullable<System.DateTime> DateInput { get => _DateInput; set { _DateInput = value; OnPropertyChanged(); } }

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

        private InputInfo _selectedItem;
        public InputInfo SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    var item = DataProvider.Ins.DB.InputInfoes.Where(o => o.Id == SelectedItem.Id).FirstOrDefault();
                    if (item == null)
                    {
                        MessageBox.Show("The item is no longer here!");
                    }
                    else
                    {
                        Id = SelectedItem.Id;
                        SelectedObj = SelectedItem.Object;
                        DateInput = SelectedItem.Input.DateInput;
                        Count = SelectedItem.Count;
                        InputPrice = SelectedItem.InputPrice;
                        Status = SelectedItem.Status;
                        SelectedSuplier = SelectedItem.Object.Suplier;
                    }
                }
            }
        }

        public ImportViewModel()
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
            SearchComand = new RelayCommand<object>((p) => { return true; }, (p) => { LoadList(); });
            MouseLeftButtonDownWindowCommand = new RelayCommand<InputInfo>((p) => { return p == null ? false : true; }, (p) => { MouseLeftButtonDownCommandMethod(p); });

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
            DataProvider.Ins.DB.InputInfoes.Remove(SelectedItem);
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

        private void MouseLeftButtonDownCommandMethod(InputInfo p)
        {
            var item = DataProvider.Ins.DB.InputInfoes.Where(o => o.Id == p.Id).FirstOrDefault();
            if (item == null)
            {
                List.Remove(item);
                MessageBox.Show("The item is no longer here!");
            }
        }

        private void LoadList()
        {
            Id = null;
            SelectedObj = null;
            DateInput = null;
            Count = null;
            InputPrice = null;
            Status = null;
            SelectedSuplier = null;
            List = new ObservableCollection<InputInfo>(DataProvider.Ins.DB.InputInfoes);
            Objects = new ObservableCollection<Models.Object>(DataProvider.Ins.DB.Objects);
            Supliers = new ObservableCollection<Models.Suplier>(DataProvider.Ins.DB.Supliers);
        }

        private bool CheckConditionAdd()
        {
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
            var input = new Input
            {
                Id = Guid.NewGuid().ToString(),
                DateInput = DateInput,
            };
            DataProvider.Ins.DB.Inputs.Add(input);
            DataProvider.Ins.DB.SaveChanges();

            var inputInfo = new InputInfo
            {
                Id = Guid.NewGuid().ToString(),
                IdObject = SelectedObj.Id,
                IdInput = input.Id,
                Count = Count,
                InputPrice = InputPrice,
                Status = Status,
            };
            DataProvider.Ins.DB.InputInfoes.Add(inputInfo);
            DataProvider.Ins.DB.SaveChanges();
            LoadList();
        }

        private void EditCommandMethod()
        {
            var input = DataProvider.Ins.DB.Inputs.Where(o => o.Id == SelectedItem.IdInput).FirstOrDefault();
            input.DateInput = DateInput;
            DataProvider.Ins.DB.SaveChanges();

            var inputInfo = DataProvider.Ins.DB.InputInfoes.Where(o => o.Id == SelectedItem.Id).FirstOrDefault();
            inputInfo.IdObject = SelectedObj.Id;
            inputInfo.Count = Count;
            inputInfo.InputPrice = InputPrice;
            inputInfo.Status = Status;

            DataProvider.Ins.DB.SaveChanges();
        }
    }
}
