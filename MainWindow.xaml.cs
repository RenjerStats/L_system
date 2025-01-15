using L_system.Model.core.Nodes;
using L_system.View;
using L_system.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace L_system
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            NodeV node1 = new NodeV(new(300, 50), new NodeVM(new NodeTime()), NodeCanvas);
            NodeV node2 = new NodeV(new(600, 50), new NodeVM(new NodeTime()), NodeCanvas);
            NodeV node3 = new NodeV(new(300, 200), new NodeVM(new NodeDiv()), NodeCanvas);
            NodeV node4 = new NodeV(new(600, 200), new NodeVM(new NodeSin()), NodeCanvas);
        }
    }
}
