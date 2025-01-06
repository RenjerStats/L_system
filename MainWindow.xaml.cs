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

            NodeV nodeDiv = new NodeV(new(300, 50), new NodeVM(new NodeDiv()));
            NodeV nodeTimer = new NodeV(new(100, 50), new NodeVM(new NodeTime()));
            NodeV nodeTimer1 = new NodeV(new(100, 50), new NodeVM(new NodeTime()));
            NodeV nodeTimer2 = new NodeV(new(100, 50), new NodeVM(new NodeTime()));
            NodeV nodeConst1 = new NodeV(new(100, 200), new NodeVM(new NodeConstant(10)));
            NodeV nodeConst2 = new NodeV(new(100, 200), new NodeVM(new NodeConstant(100)));

            NodeCanvas.Children.Add(nodeDiv.face);
            NodeCanvas.Children.Add(nodeTimer1.face);
            NodeCanvas.Children.Add(nodeTimer2.face);
            NodeCanvas.Children.Add(nodeConst1.face);
            NodeCanvas.Children.Add(nodeConst2.face);
        }
    }
}
