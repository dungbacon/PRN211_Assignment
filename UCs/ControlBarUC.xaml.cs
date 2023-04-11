using QuanLyKho_Project.ViewModel;
using System.Windows.Controls;

namespace QuanLyKho_Project.UCs
{
    /// <summary>
    /// Interaction logic for ControlBarUC.xaml
    /// </summary>
    public partial class ControlBarUC : UserControl
    {
        public ControlBarUCViewModel ViewModel { get; set; }

        public ControlBarUC()
        {
            InitializeComponent();
            this.DataContext = ViewModel = new ControlBarUCViewModel();
        }

        private void Button_ColorChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<System.Windows.Media.Color> e)
        {

        }
    }
}
