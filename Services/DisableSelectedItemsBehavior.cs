using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace QuanLyKho_Project.Services
{
    public class DisableSelectedItemsBehavior : Behavior<ListView>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseDown += OnPreviewMouseDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewMouseDown -= OnPreviewMouseDown;
        }

        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = e.OriginalSource as ListViewItem;
            if (item != null && item.IsSelected)
            {
                e.Handled = true;
            }
        }
    }
}
