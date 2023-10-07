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

namespace AsyncCommand
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ICommand startCommand;

        public MainWindow()
        {
            InitializeComponent();

            startCommand = new AsyncRelayCommand(async _ =>
            {
                for (int i = 1; i <= 5; i++)
                {
                    label.Content = i;
                    await Task.Delay(1000);
                }
            });

            DataContext = this;
        }

        public ICommand StartCommand => startCommand;
        
    }
}
