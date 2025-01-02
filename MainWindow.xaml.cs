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
            NodeV node1 = new NodeV(new(530, 100), new NodeVM(new NodeChangeTO()));
            NodeV node2 = new NodeV(new(270, 100), new NodeVM(new NodePlus()));
            NodeV node3 = new NodeV(new(10, 100), new NodeVM(new NodeConstant(5D)));


            NodeCanvas.Children.Add(node3.face);
            NodeCanvas.Children.Add(node2.face);
            NodeCanvas.Children.Add(node1.face);
        }
    }
}
