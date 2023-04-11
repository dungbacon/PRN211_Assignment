using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using QuanLyKho_Project.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace QuanLyKho_Project.ViewModel
{
    public class StatisticViewModel : BaseViewModel
    {
        public ICommand Search1Button { get; set; }
        public ICommand ReloadButton { get; set; }


        public class ChartModel
        {
            public DateTime DateTime { get; set; }
            public double Value { get; set; }

            public ChartModel(DateTime dateTime, double value)
            {
                this.DateTime = dateTime;
                this.Value = value;
            }
        }

        public class ChartModelColumn
        {
            public string DisplayName { get; set; }
            public double Value { get; set; }

            public ChartModelColumn(string _DisplayName, double value)
            {
                this.DisplayName = _DisplayName;
                this.Value = value;
            }
        }

        private ObservableCollection<Stock> _StockList;
        public ObservableCollection<Stock> StockList { get => _StockList; set { _StockList = value; OnPropertyChanged(); } }
        private SeriesCollection _PieSeriesCollection;
        public SeriesCollection PieSeriesCollection
        {
            get { return _PieSeriesCollection; }
            set
            {
                _PieSeriesCollection = value;
                OnPropertyChanged();
            }
        }
        public List<PieSeries> PieSeries { get; set; }
        public int SumOfAllProduct { get; set; } = 0;

        public ChartValues<ChartModel> listInputChartValues { get; set; }
        public ChartValues<ChartModel> listOutputChartValues { get; set; }
        public List<LineSeries> LineSerieCollection { get; set; }
        public DateTime InitialDateTime { get; set; }
        public Func<double, string> Formatter { get; set; }
        public List<InputInfo> InputList { get; set; }
        public List<OutputInfo> OutputList { get; set; }
        private SeriesCollection _CarteSeriesCollection;
        public SeriesCollection CarteSeriesCollection
        {
            get { return _CarteSeriesCollection; }
            set
            {
                _CarteSeriesCollection = value;
                OnPropertyChanged();
            }
        }


        public StatisticViewModel()
        {
            this.LoadStockList();
            this.LoadPriceDiff();
            this.ConfigPieChart();
            this.SetChartModelValues();
            this.CustomerLiveChart();
            ReloadButton = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                this.LoadStockList();
                this.LoadPriceDiff();
                this.ConfigPieChart();
                this.SetChartModelValues();
                this.CustomerLiveChart();
            });

        }

        private void LoadStockList()
        {
            StockList = new ObservableCollection<Stock>();
            var objectList = DataProvider.Ins.DB.Objects;
            int i = 1;
            foreach (var item in objectList)
            {
                var importList = DataProvider.Ins.DB.InputInfoes.Where(p => p.IdObject == item.Id);
                var outputList = DataProvider.Ins.DB.OutputInfoes.Where(p => p.IdObject == item.Id);

                int sumImport = 0;
                int sumOutput = 0;

                if (importList != null && importList.Count() > 0) sumImport = (int)importList.Sum(p => p.Count);
                if (outputList != null && outputList.Count() > 0) sumOutput = (int)outputList.Sum(p => p.Count);

                Stock s = new Stock();
                s.STT = i;
                s.StockQuantity = sumImport - sumOutput;
                s.Object = item;
                StockList.Add(s);
                i++;
                SumOfAllProduct += sumImport - sumOutput;
            }
        }

        private void LoadPriceDiff()
        {
            var InputListRaw = DataProvider.Ins.DB.InputInfoes.GroupBy(o => o.Input.DateInput).OrderByDescending(c => c.Key).Select(input => new
            {
                Key = input.Key,
                Total = input.Sum(o => o.InputPrice)
            });

            var OutputListRaw = DataProvider.Ins.DB.OutputInfoes.GroupBy(o => o.Output.DateOutput).OrderByDescending(c => c.Key).Select(input => new
            {
                Key = input.Key,
                Total = input.Sum(o => o.OutputPrice)
            }); ;

            LineSerieCollection = new List<LineSeries>();
            listInputChartValues = new ChartValues<ChartModel>();
            listOutputChartValues = new ChartValues<ChartModel>();

            foreach (var item in InputListRaw)
            {
                listInputChartValues.Add(new ChartModel((DateTime)item.Key, (double)item.Total));
            }


            foreach (var item in OutputListRaw)
            {
                listOutputChartValues.Add(new ChartModel((DateTime)item.Key, (double)item.Total));
            }
        }


        private void ConfigPieChart()
        {
            PieSeries = new List<PieSeries>();
            foreach (var item in StockList)
            {
                PieSeries.Add(new PieSeries { Title = item.Object.DisplayName, Values = new ChartValues<double> { (item.StockQuantity * 100) / SumOfAllProduct }, DataLabels = true });
            }

            PieSeriesCollection = new SeriesCollection();
            foreach (var pieSeries in PieSeries)
            {
                PieSeriesCollection.Add(pieSeries);
            }
        }


        private void SetChartModelValues()
        {
            var dayConfig = Mappers.Xy<ChartModel>()
                           .X(dayModel => dayModel.DateTime.Ticks)
                           .Y(dayModel => dayModel.Value);


            DateTime now = DateTime.Now;

            this.CarteSeriesCollection = new SeriesCollection(dayConfig) {
                new LineSeries()
                {
                    Values = listInputChartValues,
                },
                new LineSeries()
                {
                    Values = listOutputChartValues
                },
            };

            this.InitialDateTime = new DateTime(2023, 1, 1);
            this.Formatter = value => new DateTime((long)value).ToString("yyyy-MM-dd");
        }

        private void LoadCustomerRanking()
        {
            ListCustomerRanking = (List<OutputInfo>)DataProvider.Ins.DB.OutputInfoes.GroupBy(o => o.Customer.DisplayName).Select(a => new
            {
                Key = a.Key,
                TotalPrice = a.Sum(o => o.OutputPrice)
            }).Take(10).OrderByDescending(o => o.TotalPrice);
        }

        private void CustomerLiveChart()
        {
            var query = DataProvider.Ins.DB.OutputInfoes.GroupBy(o => o.Customer.DisplayName).Select(a => new
            {
                Key = a.Key,
                TotalPrice = a.Sum(o => o.OutputPrice)
            }).Take(10).OrderByDescending(o => o.TotalPrice);

            ListColumnSeries = new List<ColumnSeries>();

            foreach (var item in query)
            {
                ListColumnSeries.Add(new ColumnSeries
                {
                    Title = item.Key,
                    Values = new ChartValues<double> { (double)item.TotalPrice }
                });
            }

            ColumnSeriesCollection = new SeriesCollection();

            foreach (var item in ListColumnSeries)
            {
                ColumnSeriesCollection.Add(item);
            }

            ColumnXLabels = new ObservableCollection<string>(query.Select(x => x.Key));
            ColumnYLabels = value => value.ToString("N");
        }

        private List<OutputInfo> _ListCustomerRanking { get; set; }
        public List<OutputInfo> ListCustomerRanking { get => _ListCustomerRanking; set { _ListCustomerRanking = value; OnPropertyChanged(); } }

        public SeriesCollection ColumnSeriesCollection { get; set; }
        private List<ColumnSeries> _ListColumnSeries;
        public List<ColumnSeries> ListColumnSeries { get => _ListColumnSeries; set { _ListColumnSeries = value; OnPropertyChanged(); } }
        public ObservableCollection<string> ColumnXLabels { get; set; }
        public Func<double, string> ColumnYLabels { get; set; }
    }
}
