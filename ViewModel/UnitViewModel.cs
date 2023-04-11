using QuanLyKho_Project.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace QuanLyKho_Project.ViewModel
{
    public class UnitViewModel : BaseViewModel
    {
        private ObservableCollection<Unit> _UnitList;
        public ObservableCollection<Unit> UnitList { get => _UnitList; set { _UnitList = value; OnPropertyChanged(); } }

        private string _DisplayName;
        public string DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }

        private Unit _selectedUnit;
        public Unit SelectedUnit
        {
            get { return _selectedUnit; }
            set
            {
                _selectedUnit = value;
                OnPropertyChanged();
                if (SelectedUnit != null)
                {
                    var item = DataProvider.Ins.DB.Units.Where(o => o.Id == SelectedUnit.Id).FirstOrDefault();
                    if (item == null)
                    {
                        MessageBox.Show("The item is no longer here!");
                    }
                    else
                    {
                        DisplayName = SelectedUnit.DisplayName;
                    }
                }
            }
        }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ReloadCommand { get; set; }


        public UnitViewModel()
        {
            LoadUnitList();
            AddCommand = new RelayCommand<object>((p) =>
            {
                var check = CheckCondition();
                return check;
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
            ReloadCommand = new RelayCommand<object>((p) => { return true; }, (p) => { LoadUnitList(); });

        }

        private void DeleteMethod()
        {
            var objectsInput = DataProvider.Ins.DB.InputInfoes.Where(o => o.Object.IdUnit == SelectedUnit.Id).ToList();
            DataProvider.Ins.DB.InputInfoes.RemoveRange(objectsInput);

            var objectsOutput = DataProvider.Ins.DB.OutputInfoes.Where(o => o.Object.IdUnit == SelectedUnit.Id).ToList();
            DataProvider.Ins.DB.OutputInfoes.RemoveRange(objectsOutput);
            DataProvider.Ins.DB.SaveChanges();

            var objects = DataProvider.Ins.DB.Objects.Where(o => o.IdUnit == SelectedUnit.Id).ToList();
            DataProvider.Ins.DB.Objects.RemoveRange(objects);
            DataProvider.Ins.DB.SaveChanges();

            var item = DataProvider.Ins.DB.Units.Where(o => o.Id == SelectedUnit.Id).FirstOrDefault();
            DataProvider.Ins.DB.Units.Remove(item);
            DataProvider.Ins.DB.SaveChanges();
        }

        private bool checkConditionDelete()
        {
            if (SelectedUnit == null)
            {
                return false;
            }
            return true;
        }

        private bool CheckConditionEdit()
        {
            if (SelectedUnit != null)
            {
                return true;
            }
            return false;
        }

        private bool CheckCondition()
        {
            if (string.IsNullOrEmpty(DisplayName))
            {
                return false;
            }
            var checkUnit = DataProvider.Ins.DB.Units.FirstOrDefault(o => o.DisplayName.Equals(DisplayName));
            if (checkUnit != null)
            {
                return false;
            }
            return true;
        }

        private void LoadUnitList()
        {
            UnitList = new ObservableCollection<Unit>(DataProvider.Ins.DB.Units);
        }

        private void AddCommandMethod()
        {
            var unit = new Unit { DisplayName = DisplayName };
            DataProvider.Ins.DB.Units.Add(unit);
            DataProvider.Ins.DB.SaveChanges();
            UnitList.Add(unit);
        }

        private void EditCommandMethod()
        {
            var unit = DataProvider.Ins.DB.Units.Where(p => p.Id == SelectedUnit.Id).SingleOrDefault();
            unit.DisplayName = DisplayName;
            DataProvider.Ins.DB.SaveChanges();
            SelectedUnit.DisplayName = DisplayName;
        }

    }
}
