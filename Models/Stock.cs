using QuanLyKho_Project.ViewModel;

namespace QuanLyKho_Project.Models
{
    public class Stock : BaseViewModel
    {
        private Object _Object;
        public Object Object { get => _Object; set { _Object = value; OnPropertyChanged(); } }

        private int _STT;
        public int STT { get => _STT; set { _STT = value; OnPropertyChanged(); } }

        private int _StockQuantity;
        public int StockQuantity { get => _StockQuantity; set { _StockQuantity = value; OnPropertyChanged(); } }
    }
}
