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

            for (int i = 0; i < 2; i++)
            {
                NodeV nodeSum = new NodeV(new(100 + 200*i, 50), new NodeVM(new NodePlus()));
                NodeCanvas.Children.Add(nodeSum.face);
            }
            for (int i = 0; i < 3; i++)
            {
                NodeV nodeConst = new NodeV(new(100 + 200*i, 250), new NodeVM(new NodeConstant(5D)));
                NodeCanvas.Children.Add(nodeConst.face);
            }

        }
    }
}
