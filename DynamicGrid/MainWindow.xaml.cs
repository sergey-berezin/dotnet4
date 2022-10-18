using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DynamicGrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int n = Int32.Parse(value.Text);
            for(int i = 0; i < n+1; i++)
            {
                table.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                table.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

                if (i > 0)
                {
                    var l = new Label();
                    Grid.SetColumn(l, 0);
                    Grid.SetRow(l, i);
                    l.Content = i.ToString();
                    table.Children.Add(l);

                    var l2 = new Label();
                    Grid.SetColumn(l2, i);
                    Grid.SetRow(l2, 0);
                    l2.Content = i.ToString();
                    table.Children.Add(l2);
                }
            }

            for (int i = 0; i < n; i++)
            {
                for(int j =0;j < n;j++)
                {
                    var l = new Label();
                    Grid.SetColumn(l, i + 1);
                    Grid.SetRow(l, j + 1);
                    l.Content = ((i + 1) * (j + 1)).ToString();
                    table.Children.Add(l);
                }
            }

        }
    }
}
