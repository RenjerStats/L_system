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
using Windows.Storage;

namespace L_system.View.Elements
{
    /// <summary>
    /// Логика взаимодействия для Head.xaml
    /// </summary>
    public partial class Head : UserControl
    {
        public Head()
        {
            InitializeComponent();
        }

        private void MainGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MouseButtonState.Pressed == Mouse.LeftButton)
            {
                if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
                {
                    Application.Current.MainWindow.Left = Application.Current.MainWindow.Width/2;
                    Application.Current.MainWindow.Top = 0;
                    Application.Current.MainWindow.WindowState = WindowState.Normal;
                }
                Application.Current.MainWindow.DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }
    }
}
