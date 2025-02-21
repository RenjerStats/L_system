using DrawTest;
using L_system.Systems;
using L_system.Systems.ForNodes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace L_system
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UpdateSystem updateSystem;
        public MainWindow()
        {
            InitializeComponent();

            MenuItem menuCreateNodes = HeaderMenu.Items[0] as MenuItem;
            InitializeNodeInMenu(menuCreateNodes);

            KeyDown += NodeSystem.KeyPressed;
            KeyUp += NodeSystem.ResetAction;

            NodeCanvas.MouseLeftButtonDown += ActiveNodesSystem.StartCreateRectangle;
            NodeCanvas.MouseMove += ActiveNodesSystem.DrawRectangle;
            NodeCanvas.MouseLeftButtonUp += ActiveNodesSystem.EndCreateRectangle;
            NodeCanvas.MouseLeave += (a, e) => ActiveNodesSystem.DeleteRectangle();

            NodeCanvas.MouseLeftButtonDown += ActiveNodesSystem.ActionHandler;
            NodeCanvas.MouseMove += NodeSystem.NodeMove;
            NodeCanvas.MouseLeave += (s, e) => ActiveNodesSystem.EndDrag();
            NodeCanvas.MouseLeftButtonUp += (s, e) => ActiveNodesSystem.EndDrag();

            InitializeUpdateSystem();

            updateSystem.StartUpdating();
        }



        private void InitializeUpdateSystem()
        {
            DrawingCanvas canvas = new DrawingCanvas();
            RenderCanvas.Children.Add(canvas);
            updateSystem = new UpdateSystem(canvas);
            updateSystem.SetFrameRate(30);

            RenderCanvas.SizeChanged += updateSystem.ResetSizeDrawingArea;
        }

        private void InitializeNodeInMenu(MenuItem createNodes)
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

            Vector position = new Vector(NodeCanvas.ActualWidth / 2, NodeCanvas.ActualHeight / 2) + randomOffset;
            NodeSystem.CreateNode((Point)position, NodeCanvas, item.Header.ToString());
        }

    }
}
