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
    public partial class OldMainWindow : Window
    {

        private UIElement? selectedNode = null;
        private Vector mouseOffset;

        public OldMainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowVM(this);
            CreateNode(new Point(100, 100));
            CreateNode(new Point(300, 150));
        }

        private void CreateNode(Point position)
        {
            var node = new Border
            {
                Width = 150,
                Height = 100,
                Background = Brushes.LightBlue,
                CornerRadius = new CornerRadius(10),
                BorderBrush = Brushes.DarkBlue,
                BorderThickness = new Thickness(2),
                Child = new TextBlock
                {
                    Text = "Node",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 16
                },
                Tag = false
            };

            Canvas.SetLeft(node, position.X);
            Canvas.SetTop(node, position.Y);
            NodeCanvas.Children.Add(node);

            node.MouseLeftButtonDown += Node_MouseLeftButtonDown;
        }

        private void Node_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is UIElement element)
            {
                selectedNode = element;
                mouseOffset = e.GetPosition(NodeCanvas) - new Point(Canvas.GetLeft(element), Canvas.GetTop(element));
                NodeCanvas.CaptureMouse();
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectedNode != null)
            {
                var mousePosition = e.GetPosition(NodeCanvas);
                Canvas.SetLeft(selectedNode, mousePosition.X - mouseOffset.X);
                Canvas.SetTop(selectedNode, mousePosition.Y - mouseOffset.Y);
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (selectedNode != null)
            {
                NodeCanvas.ReleaseMouseCapture();
                selectedNode = null;
            }
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource == NodeCanvas)
            {
                CreateNode(e.GetPosition(NodeCanvas));
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
       {
            if (selectedNode != null)
            {
                if (e.Key == Key.Delete)
                {
                    NodeCanvas.Children.Remove(selectedNode);
                    selectedNode = null;
                }
                else
                {
                    var node = selectedNode as Border;
                    bool isCollapsed = false;
                    if (e.Key == Key.Back && node != null)
                    {
                        isCollapsed = (bool)node.Tag;
                        node.Width = isCollapsed ? 150 : 50;
                        node.Height = isCollapsed ? 100 : 50;
                    }
                    node.Child = isCollapsed
                            ? new TextBlock
                            {
                                Text = "Node",
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center,
                                FontSize = 16
                            }
                            : null;
                    node.Tag = !isCollapsed;
                }
            }
        }
    }
}
