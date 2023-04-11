using QuanLyKho_Project.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace QuanLyKho_Project.ViewModel
{
    public class ObjectViewModel : BaseViewModel
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ReloadCommand { get; set; }

        private ObservableCollection<Models.Object> _List;
        public ObservableCollection<Models.Object> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private string _Id;
        public string Id { get => _Id; set { _Id = value; OnPropertyChanged(); } }

        private string _DisplayName;
        public string DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }

        private string _QRCode;
        public string QRCode { get => _QRCode; set { _QRCode = value; OnPropertyChanged(); } }

        private string _BarCode;
        public string BarCode { get => _BarCode; set { _BarCode = value; OnPropertyChanged(); } }

        private ObservableCollection<Suplier> _Suplier;
        public ObservableCollection<Suplier> Suplier { get => _Suplier; set { _Suplier = value; OnPropertyChanged(); } }

        private ObservableCollection<Unit> _Unit;
        public ObservableCollection<Unit> Unit { get => _Unit; set { _Unit = value; OnPropertyChanged(); } }

        private Models.Object _selectedItem;
        public Models.Object SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    var item = DataProvider.Ins.DB.Objects.Where(o => o.Id == SelectedItem.Id).FirstOrDefault();
                    if (item == null)
                    {
                        MessageBox.Show("The item is no longer here!");
                    }
                    else
                    {
                        Id = SelectedItem.Id;
                        DisplayName = SelectedItem.DisplayName;
                        QRCode = SelectedItem.QRCode;
                        BarCode = SelectedItem.BarCode;
                        SelectedUnit = SelectedItem.Unit;
                        SelectedSuplier = SelectedItem.Suplier;
                    }
                }
            }
        }

        private Models.Unit _SelectedUnit;
        public Models.Unit SelectedUnit
        {
            get => _SelectedUnit;
            set
            {
                _SelectedUnit = value;
                OnPropertyChanged();
            }
        }

        private Models.Suplier _SelectedSuplier;
        public Models.Suplier SelectedSuplier
        {
            get => _SelectedSuplier;
            set
            {
                _SelectedSuplier = value;
                OnPropertyChanged();
            }
        }


        public ObjectViewModel()
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
            var objectsInput = DataProvider.Ins.DB.InputInfoes.Where(o => o.IdObject == SelectedItem.Id).ToList();
            DataProvider.Ins.DB.InputInfoes.RemoveRange(objectsInput);
            DataProvider.Ins.DB.SaveChanges();


            var objectsOutput = DataProvider.Ins.DB.OutputInfoes.Where(o => o.IdObject == SelectedItem.Id).ToList();
            DataProvider.Ins.DB.OutputInfoes.RemoveRange(objectsOutput);
            DataProvider.Ins.DB.SaveChanges();

            var objects = DataProvider.Ins.DB.Objects.Where(o => o.Id == SelectedItem.Id).FirstOrDefault();
            DataProvider.Ins.DB.Objects.Remove(objects);
            DataProvider.Ins.DB.SaveChanges();
            List.Remove(objects);
        }

        private bool checkConditionDelete()
        {
            if (SelectedItem != null)
            {
                return true;
            }
            return false;
        }

        private void LoadList()
        {
            List = new ObservableCollection<Models.Object>(DataProvider.Ins.DB.Objects);
            Unit = new ObservableCollection<Unit>(DataProvider.Ins.DB.Units);
            Suplier = new ObservableCollection<Suplier>(DataProvider.Ins.DB.Supliers);

        }

        private bool CheckConditionAdd()
        {

            if (string.IsNullOrEmpty(DisplayName))
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
            var check = DataProvider.Ins.DB.Objects.FirstOrDefault(o => o.Id == SelectedItem.Id);
            if (check == null)
            {
                return false;
            }
            return true;
        }

        private void AddCommandMethod()
        {
            var item = new Models.Object
            {
                Id = Guid.NewGuid().ToString(),
                DisplayName = DisplayName,
                IdUnit = SelectedUnit.Id,
                IdSuplier = SelectedSuplier.Id,
                QRCode = QRCode,
                BarCode = BarCode,
            };
            DataProvider.Ins.DB.Objects.Add(item);
            DataProvider.Ins.DB.SaveChanges();
            List.Add(item);
        }

        private void EditCommandMethod()
        {
            var unit = DataProvider.Ins.DB.Objects.Where(p => p.Id == SelectedItem.Id).SingleOrDefault();
            unit.DisplayName = DisplayName;
            unit.IdUnit = SelectedUnit.Id;
            unit.IdSuplier = SelectedSuplier.Id;
            unit.QRCode = QRCode;
            unit.BarCode = BarCode;
            DataProvider.Ins.DB.SaveChanges();
        }
    }
}
