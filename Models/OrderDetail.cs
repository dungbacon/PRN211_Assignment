using QuanLyKho_Project.ViewModel;
using System;

namespace QuanLyKho_Project.Models
{
    public class OrderDetail : BaseViewModel
    {
        private string _Id;
        public string Id { get => _Id; set { _Id = value; OnPropertyChanged(); } }

        private string _ObjId;
        public string ObjId { get => _ObjId; set { _ObjId = value; OnPropertyChanged(); } }

        private string _InputId;
        public string InputId { get => _InputId; set { _InputId = value; OnPropertyChanged(); } }

        private int _CustomerId;
        public int CustomerId { get => _CustomerId; set { _CustomerId = value; OnPropertyChanged(); } }

        private string _ObjDisplayName;
        public string ObjDisplayName { get => _ObjDisplayName; set { _ObjDisplayName = value; OnPropertyChanged(); } }

        private Nullable<System.DateTime> _DateOutput;
        public Nullable<System.DateTime> DateOutput { get => _DateOutput; set { _DateOutput = value; OnPropertyChanged(); } }

        private int? _Count;
        public int? Count { get => _Count; set { _Count = value; OnPropertyChanged(); } }

        private double? _OutputPrice;
        public double? OutputPrice { get => _OutputPrice; set { _OutputPrice = value; OnPropertyChanged(); } }

        private string _CustomerDisplayName;
        public string CustomerDisplayName { get => _CustomerDisplayName; set { _CustomerDisplayName = value; OnPropertyChanged(); } }

        private string _Status;
        public string Status { get => _Status; set { _Status = value; OnPropertyChanged(); } }
    }
}
