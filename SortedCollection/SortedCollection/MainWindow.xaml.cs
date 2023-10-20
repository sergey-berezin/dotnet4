using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace SortedCollection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            ((CollectionViewSource)Resources["cvs"]).SortDescriptions.Add(new SortDescription() { Direction = ListSortDirection.Descending });
        }

        public int[] Numbers => new int[] { 1,5,-1,30,-4 };
    }
}
