using QuanLyKho_Project.CommonService;
using QuanLyKho_Project.CommonService.Repository;
using QuanLyKho_Project.Models;
using QuanLyKho_Project.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace QuanLyKho_Project.ViewModel
{
    internal class MainViewModel : BaseViewModel
    {
        private IUserRepository userRepo;

        public ICommand LoadedWindowCommand { get; set; }
        public ICommand UnitCommand { get; set; }
        public ICommand SupplierCommand { get; set; }
        public ICommand CustomerCommand { get; set; }
        public ICommand ObjectCommand { get; set; }
        public ICommand UserCommand { get; set; }
        public ICommand ImportCommand { get; set; }
        public ICommand OuputCommand { get; set; }
        public ICommand ReloadCommand { get; set; }
        public ICommand LogOutCommand { get; set; }
        public ICommand Search1Button { get; set; }
        public ICommand Search2Button { get; set; }
        public ICommand GenerateStatsCommand { get; set; }

        private ObservableCollection<Stock> _StockList;
        public ObservableCollection<Stock> StockList { get => _StockList; set { _StockList = value; OnPropertyChanged(); } }

        private int? _ImportNumber;
        public int? ImportNumber { get => _ImportNumber; set { _ImportNumber = value; OnPropertyChanged(); } }

        private int? _ExportNumber;
        public int? ExportNumber { get => _ExportNumber; set { _ExportNumber = value; OnPropertyChanged(); } }

        private int? _StockNumber;
        public int? StockNumber { get => _StockNumber; set { _StockNumber = value; OnPropertyChanged(); } }

        private DateTime? _StaticStartDate;
        public DateTime? StaticStartDate { get => _StaticStartDate; set { _StaticStartDate = value; OnPropertyChanged(); } }

        private DateTime? _StaticEndDate;
        public DateTime? StaticEndDate { get => _StaticEndDate; set { _StaticEndDate = value; OnPropertyChanged(); } }

        private string _TxtSearch;
        public string TxtSearch { get => _TxtSearch; set { _TxtSearch = value; OnPropertyChanged(); } }


        public MainViewModel()
        {
            userRepo = new UserRepository();
            Initialize();
            LoadedWindowCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { LoadedCommandMethod(p); });
            LogOutCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { LogoutMethod(p); });
            UnitCommand = new RelayCommand<object>((p) => { return true; }, (p) => { UnitWindowCommandMethod(); });
            SupplierCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SupplierWindowCommandMethod(); });
            CustomerCommand = new RelayCommand<object>((p) => { return true; }, (p) => { CustomerWindowCommandMethod(); });
            ObjectCommand = new RelayCommand<object>((p) => { return true; }, (p) => { ObjectWindowCommandMethod(); });
            UserCommand = new RelayCommand<object>((p) => { return true; }, (p) => { UserWindowCommandMethod(); });
            ImportCommand = new RelayCommand<object>((p) => { return true; }, (p) => { ImportWindowCommandMethod(); });
            OuputCommand = new RelayCommand<object>((p) => { return true; }, (p) => { OuputWindowCommandMethod(); });
            ReloadCommand = new RelayCommand<object>((p) => { return true; }, (p) => { LoadStockList(); });
            Search1Button = new RelayCommand<object>((p) => { return true; }, (p) => { LoadStockList(); });
            Search2Button = new RelayCommand<object>((p) => { return true; }, (p) => { SearchStatusProduct(); });
            GenerateStatsCommand = new RelayCommand<object>((p) => { return true; }, (p) => { StatisticsWindow view = new StatisticsWindow(); view.ShowDialog(); });
            LoadStockList();
        }

        private void Initialize()
        {
            ImportNumber = 0;
            ExportNumber = 0;
            StockNumber = 0;
        }

        private void SearchStatusProduct()
        {
            ImportNumber = DataProvider.Ins.DB.InputInfoes.Where(o => o.Object.DisplayName.Contains(TxtSearch)).Sum(a => a.Count);
            ExportNumber = DataProvider.Ins.DB.OutputInfoes.Where(o => o.Object.DisplayName.Contains(TxtSearch)).Sum(a => a.Count);
            StockNumber = ImportNumber - ExportNumber;
        }

        private void LogoutMethod(Window p)
        {
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(""), new string[0]);
            LoginWindow view = new LoginWindow();
            p.Hide();
            view.Show();
        }

        void LoadStockList()
        {
            StockList = new ObservableCollection<Stock>();
            var objectList = DataProvider.Ins.DB.Objects;
            int i = 1;
            foreach (var item in objectList)
            {
                var importList = DataProvider.Ins.DB.InputInfoes.Where(p => p.IdObject == item.Id && StaticStartDate <= p.Input.DateInput && StaticEndDate >= p.Input.DateInput);
                var outputList = DataProvider.Ins.DB.OutputInfoes.Where(p => p.IdObject == item.Id && StaticStartDate <= p.Output.DateOutput && StaticEndDate >= p.Output.DateOutput);

                int sumImport = 0;
                int sumOutput = 0;

                if (importList != null && importList.Count() > 0) sumImport = (int)importList.Sum(p => p.Count);
                if (outputList != null && outputList.Count() > 0) sumOutput = (int)outputList.Sum(p => p.Count);

                Stock s = new Stock()
                {
                    STT = i,
                    StockQuantity = sumImport - sumOutput,
                    Object = item
                };
                StockList.Add(s);
                i++;
            }
        }

        private void UserWindowCommandMethod()
        {
            UserWindow view = new UserWindow();
            view.ShowDialog();
        }

        private void OuputWindowCommandMethod()
        {
            OutputWindow view = new OutputWindow();
            view.ShowDialog();
        }

        private void ImportWindowCommandMethod()
        {
            ImportWindow view = new ImportWindow();
            view.ShowDialog();
        }

        private void LoadedCommandMethod(Window p)
        {
            if (p == null)
            {
                return;
            }
            var userValid = userRepo.GetByName(Thread.CurrentPrincipal.Identity.Name) != null ? true : false;
            if (userValid)
            {
                p.Show();
                LoadStockList();
            }
        }

        private void UnitWindowCommandMethod()
        {
            UnitWindow view = new UnitWindow();
            view.ShowDialog();
        }

        private void SupplierWindowCommandMethod()
        {
            SupplierWindow view = new SupplierWindow();
            view.ShowDialog();
        }

        private void CustomerWindowCommandMethod()
        {
            CustomerWindow view = new CustomerWindow();
            view.ShowDialog();
        }

        private void ObjectWindowCommandMethod()
        {
            ObjectWindow view = new ObjectWindow();
            view.ShowDialog();
        }
    }
}
