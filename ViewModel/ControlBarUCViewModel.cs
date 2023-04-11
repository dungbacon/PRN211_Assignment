using QuanLyKho_Project.UCs;
using System.Windows;
using System.Windows.Input;

namespace QuanLyKho_Project.ViewModel
{
    public class ControlBarUCViewModel : BaseViewModel
    {
        public ICommand CloseWindowCommand { get; set; }
        public ICommand MaximizeWindowCommand { get; set; }
        public ICommand MinimizeWindowCommand { get; set; }
        public ICommand MouseLeftButtonDownWindowCommand { get; set; }



        public ControlBarUCViewModel()
        {
            CloseWindowCommand = new RelayCommand<object>((p) => { return p == null ? false : true; }, (p) => { CloseCommandMethod(p); });
            MaximizeWindowCommand = new RelayCommand<object>((p) => { return p == null ? false : true; }, (p) => { MaximizeCommandMethod(p); });
            MinimizeWindowCommand = new RelayCommand<object>((p) => { return p == null ? false : true; }, (p) => { MinimizeCommandMethod(p); });
            MouseLeftButtonDownWindowCommand = new RelayCommand<object>((p) => { return p == null ? false : true; }, (p) => { MouseLeftButtonDownCommandMethod(p); });
        }

        private void CloseCommandMethod(object obj)
        {
            ControlBarUC controlBarUC = obj as ControlBarUC;
            Window window = Window.GetWindow(controlBarUC);
            if (window != null)
            {
                window.Close();
            }
        }

        private void MouseLeftButtonDownCommandMethod(object obj)
        {
            ControlBarUC controlBarUC = obj as ControlBarUC;
            Window window = Window.GetWindow(controlBarUC);
            if (window != null)
            {
                window.DragMove();
            }
        }


        private void MaximizeCommandMethod(object obj)
        {
            ControlBarUC controlBarUC = obj as ControlBarUC;
            Window window = Window.GetWindow(controlBarUC);
            if (window != null)
            {
                if (window.WindowState != WindowState.Maximized)
                {
                    window.WindowState = WindowState.Maximized;
                }
                else
                {
                    window.WindowState = WindowState.Normal;
                }
            }
        }

        private void MinimizeCommandMethod(object obj)
        {
            ControlBarUC controlBarUC = obj as ControlBarUC;
            Window window = Window.GetWindow(controlBarUC);
            if (window != null)
            {
                if (window.WindowState != WindowState.Minimized)
                {
                    window.WindowState = WindowState.Minimized;
                }
            }
        }
    }
}
