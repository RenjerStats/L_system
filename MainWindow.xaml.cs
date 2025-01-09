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

            NodeV nodeSin = new NodeV(new(300, 50), new NodeVM(new NodeSin()));
            NodeV nodeDiv = new NodeV(new(300, 50), new NodeVM(new NodeDiv()));
            NodeV nodeDi1 = new NodeV(new(300, 50), new NodeVM(new NodeDiv()));
            NodeV nodeTimer = new NodeV(new(100, 50), new NodeVM(new NodeTime()));
            NodeV nodeTimer1 = new NodeV(new(100, 50), new NodeVM(new NodeTime()));
            NodeV nodeConst = new NodeV(new(100, 200), new NodeVM(new NodeConstant(100D)));

            NodeCanvas.Children.Add(nodeSin.face);
            NodeCanvas.Children.Add(nodeDiv.face);
            NodeCanvas.Children.Add(nodeDi1.face);
            NodeCanvas.Children.Add(nodeTimer1.face);
            NodeCanvas.Children.Add(nodeConst.face);
        }
    }
}
