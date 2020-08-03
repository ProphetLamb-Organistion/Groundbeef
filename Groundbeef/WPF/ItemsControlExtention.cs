using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace Groundbeef.WPF
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public static class ItemsControlExtention
    {

        public static ICollectionView GetDefaultView(this ItemsControl self) => CollectionViewSource.GetDefaultView(self);
    }
}
