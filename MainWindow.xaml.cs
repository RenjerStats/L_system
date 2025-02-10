using L_system.Model.core.Nodes;
using L_system.Systems;
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

            MenuItem createNodes = HeaderMenu.Items[0] as MenuItem;
            AddAllNodes(createNodes);
            KeyDown += DeleteNode;
            NodeCanvas.PreviewMouseLeftButtonDown += NodeCanvas_PreviewMouseLeftButtonDown;
        }

        private void AddAllNodes(MenuItem createNodes)
        {
            MenuItem createMenu = HeaderMenu.Items[0] as MenuItem;
            for (int i = 0; i < NodeSystem.namesOfGroups.Keys.ToArray().Length; i++)
            {
                string key = NodeSystem.namesOfGroups.Keys.ToArray()[i];
                string[] content = NodeSystem.namesOfGroups[key];


                MenuItem group = new MenuItem() { Header = key };
                for (int j = 0; j < content.Length; j++)
                {
                    MenuItem node = new MenuItem() { Header = content[j] };
                    node.Click += CreateNode;
                    group.Items.Add(node);
                }

                createMenu.Items.Add(group);
            }
        }

        private void CreateNode(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            Random random = new Random();
            Vector randomOffset = new Point(random.NextDouble(), random.NextDouble()) - new Point(0.5, 0.5);
            randomOffset *= 75;
            randomOffset -= new Vector(75, 75); // Центрирование ноды

            Vector position = new Vector(NodeCanvas.ActualWidth/2, NodeCanvas.ActualHeight/2) + randomOffset;
            NodeSystem.CreateNode((Point)position, NodeCanvas, item.Header.ToString());
        }

        private void NodeCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) => NodeSystem.ActiveNode = null;

        private void DeleteNode(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && NodeSystem.ActiveNode != null)
            {
                NodeSystem.ActiveNode.Dispose();
            }
        }
    }
}
