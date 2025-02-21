using L_system.Model.core.Nodes;
using L_system.Systems.ForNodes;
using L_system.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace L_system.View
{
    public class NodeV  // весит примерно 2 мб в оперативке 1 квартал 2025
    {
        public NodeVM nodeCore;
        public Border face;
        private bool isActive;

        public bool IsActive
        {
            get { return isActive; }
            set
            {
                if (isActive == value) return;
                isActive = value;
                OnActiveChanged();
            }
        }


        public Canvas canvas;
        public Ellipse[] inputsCircles;
        public Ellipse[] outputsCircles;
        public DefaultInputV[] defaultInputs;

        public bool MiniSize { get; private set; } = false;

        public NodeV(Point position, NodeVM core, Canvas canvas)
        {
            nodeCore = core;
            this.canvas = canvas;

            inputsCircles = new Ellipse[core.GetInputsCount()];
            defaultInputs = new DefaultInputV[core.GetInputsCount()];

            for (int i = 0; i < inputsCircles.Length; i++)
                inputsCircles[i] = CreateCircle(i, true);
            outputsCircles = new Ellipse[core.GetOutputsCount()];
            for (int i = 0; i < outputsCircles.Length; i++)
                outputsCircles[i] = CreateCircle(i, false);


            face = new Border()
            {
                Width = 150,
                Height = 150,
                Background = (Brush)new BrushConverter().ConvertFrom("#FF333333"),
                CornerRadius = new CornerRadius(10),
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(2),
                Child = CreateNormalForm(),
                RenderTransform = new ScaleTransform(), // Для анимации масштабирования
                RenderTransformOrigin = new Point(0.5, 0.5) // Центр трансформации
            };


            Canvas.SetLeft(face, position.X);
            Canvas.SetTop(face, position.Y);
            canvas.Children.Add(face);

            for (int i = 0; i < defaultInputs.Length; i++)
            {
                if (nodeCore.GetTypeOfInput(i) == "Double")
                    defaultInputs[i] = new DefaultInputV(core, i, inputsCircles[i], canvas);
            }

            face.DataContext = this;
            InitializeAnimationHoverForNode();
        }

        public void Dispose()
        {
            foreach (var ellipse in inputsCircles.Concat(outputsCircles))
            {
                ellipse.MouseLeftButtonDown -= StartConnection;
                ellipse.MouseLeftButtonUp -= EndConnection;
            }
            foreach (var connection in ConnectionSystem.connections.ToList().Where((a) => a.connectionCore.inputNode == nodeCore))
            {
                connection.Dispose();
            }
            foreach (var connection in ConnectionSystem.connections.ToList().Where((a) => a.connectionCore.outputNode == nodeCore))
            {
                connection.Dispose();
            }
            foreach (var defInput in defaultInputs)
            {
                defInput?.Dispose();
            }
            nodeCore.Dispose();

            Grid grid = face.Child as Grid;
            var button = grid.Children.OfType<Button>().FirstOrDefault();
            button.Click -= OnCollapse;


            canvas.Children.Remove(face);
            face.DataContext = null;
            DeleteAnimations();
            face = null;
            canvas = null;
        }

        #region Visual
        #region Animation

        private void InitializeAnimationHoverForNode()
        {
            face.MouseEnter += OnMouseEnter;
            face.MouseLeave += OnMouseLeave;
        }
        private void DeleteAnimations()
        {
            face.MouseEnter -= OnMouseEnter;
            face.MouseLeave -= OnMouseLeave;
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            // Анимация увеличения масштаба
            DoubleAnimation scaleXAnimation = new DoubleAnimation
            {
                To = 1.05,
                Duration = TimeSpan.FromSeconds(0.2)
            };

            // Применяем анимации
            face.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleXAnimation);
            face.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleXAnimation);

            //foreach (var defaultInput in defaultInputs)
            //{
            //    defaultInput?.AnimationMove(scaleXAnimation.Duration, true);
            //}
        }
        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            // Анимация возврата к исходному состоянию
            DoubleAnimation scaleXAnimation = new DoubleAnimation
            {
                To = 1.0,
                Duration = TimeSpan.FromSeconds(0.2)
            };
            // Применяем анимации
            face.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleXAnimation);
            face.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleXAnimation);

            //foreach (var defaultInput in defaultInputs)
            //{
            //    defaultInput?.AnimationMove(scaleXAnimation.Duration, false);
            //}
        }
        #endregion

        private Grid CreateNormalForm()
        {
            Grid form = new Grid();

            InitializeForm(form);
            InitializeNameOfNode(form, 0, 3);
            InitializeButtonMiniForm(form, 3);

            InitializeGridForCircles(form, inputsCircles, 1, nodeCore.Flipped ? 3 : 0);
            InitializeGridForCircles(form, outputsCircles, 1, nodeCore.Flipped ? 0 : 3);

            HorizontalAlignment left = HorizontalAlignment.Left;
            HorizontalAlignment right = HorizontalAlignment.Right;
            InitializeGridForNames(form, nodeCore.GetNameOfInputs(), 1, nodeCore.Flipped ? 2 : 1, nodeCore.Flipped ? right : left);
            InitializeGridForNames(form, nodeCore.GetNameOfOutputs(), 1, nodeCore.Flipped ? 1 : 2, nodeCore.Flipped ? left : right);

            InitializePreview(form);

            return form;
        }
        private Grid CreateMiniForm()
        {
            Grid form = new Grid();

            InitializeMiniForm(form);
            InitializeNameOfNode(form, 1);
            InitializeButtonMiniForm(form, 2);

            InitializeGridForCircles(form, inputsCircles, 0, nodeCore.Flipped ? 3 : 0);
            InitializeGridForCircles(form, outputsCircles, 0, nodeCore.Flipped ? 0 : 3);

            return form;
        }


        private void InitializeButtonMiniForm(Grid form, int column)
        {
            Button button_MiniSize = new Button();
            button_MiniSize.Click += OnCollapse;
            button_MiniSize.Foreground = Brushes.White;
            button_MiniSize.Background = Brushes.Transparent;
            button_MiniSize.BorderBrush = Brushes.Transparent;
            button_MiniSize.Content = ">";

            Grid.SetRow(button_MiniSize, 0);
            Grid.SetColumn(button_MiniSize, column);
            form.Children.Add(button_MiniSize);
        }

        private void InitializeNameOfNode(Grid form, int column, int columnSpan = 1)
        {
            TextBox name = CreateSimpleText(nodeCore.GetNameOfNode());
            name.FontSize = MiniSize ? 14 : 12;
            name.Padding = new Thickness(0, 5, 0, 0);
            name.VerticalAlignment = VerticalAlignment.Center;

            Grid.SetRow(name, 0);
            Grid.SetColumn(name, column);
            Grid.SetColumnSpan(name, columnSpan);
            form.Children.Add(name);
        }

        private void InitializeForm(Grid form)
        {
            for (int i = 0; i < 3; i++)
                form.RowDefinitions.Add(new RowDefinition());
            form.RowDefinitions[0].Height = new GridLength(20);
            form.RowDefinitions[1].Height = new GridLength(0.45, GridUnitType.Star);
            form.RowDefinitions[2].Height = new GridLength(0.45, GridUnitType.Star);

            for (int i = 0; i < 4; i++)
                form.ColumnDefinitions.Add(new ColumnDefinition());
            bool isInputEmpty = nodeCore.GetInputsCount() == 0;
            bool isOutputEmpty = nodeCore.GetOutputsCount() == 0;
            form.ColumnDefinitions[0].Width = new GridLength(isInputEmpty ? 0 : 15);
            form.ColumnDefinitions[1].Width = new GridLength(isInputEmpty ? 0 : 0.45, GridUnitType.Star);
            form.ColumnDefinitions[2].Width = new GridLength(isOutputEmpty ? 0 : 0.45, GridUnitType.Star);
            form.ColumnDefinitions[3].Width = new GridLength(isOutputEmpty ? 0 : 15);
        }

        private void InitializeMiniForm(Grid form)
        {
           form.RowDefinitions.Add(new RowDefinition());

            for (int i = 0; i < 4; i++)
                form.ColumnDefinitions.Add(new ColumnDefinition());
            bool isInputEmpty = inputsCircles.Length == 0;
            bool isOutputEmpty = outputsCircles.Length == 0;
            form.ColumnDefinitions[0].Width = new GridLength(isInputEmpty ? 0 : 15);
            form.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
            form.ColumnDefinitions[2].Width = new GridLength(15);
            form.ColumnDefinitions[3].Width = new GridLength(isOutputEmpty ? 0 : 15);
        }

        private void InitializeGridForCircles(Grid form, Ellipse[] circles, int row, int column)
        {
            Grid gridForCircles = new Grid();
            for (int i = 0; i < circles.Length; i++)
                gridForCircles.RowDefinitions.Add(new RowDefinition());
            gridForCircles.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < circles.Length; i++)
            {
                Ellipse circle = circles[i];
                gridForCircles.Children.Add(circle);
            }
            Grid.SetRow(gridForCircles, row);
            Grid.SetColumn(gridForCircles, column);
            form.Children.Add(gridForCircles);
        }

        private void InitializeGridForNames(Grid form, string[] names, int row, int column, HorizontalAlignment alignment)
        {
            Grid gridForNames = new Grid();
            for (int i = 0; i < names?.Length; i++)
                gridForNames.RowDefinitions.Add(new RowDefinition());
            gridForNames.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < names?.Length; i++)
            {
                TextBox name = CreateSimpleText(names[i], alignment);
                name.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(name, i);
                Grid.SetColumn(name, 0);
                gridForNames.Children.Add(name);
            }
            Grid.SetRow(gridForNames, row);
            Grid.SetColumn(gridForNames, column);
            form.Children.Add(gridForNames);
        }

        private void InitializePreview(Grid form)
        {
            UIElement preview = GetPreview();
            Grid.SetRow(preview, 2);
            Grid.SetColumnSpan(preview, 4);
            form.Children.Add(preview);
        }

        private UIElement GetPreview()
        {
            switch (nodeCore.GetTypeOfOutput(0))
            {
                case "Double":
                    object output = nodeCore.GetValueFromOutput(0);
                    double value = Math.Round(Convert.ToDouble(output), 5);
                    TextBox preview = CreateSimpleText($"Выходное значение:\n\n{value:G10}");
                    nodeCore.OnOutputChanged(() =>
                    {
                        preview.Text = $"Выходное значение:\n\n{nodeCore.GetValueFromOutput(0):F5}";
                    });
                    preview.VerticalAlignment = VerticalAlignment.Center;
                    preview.FontSize = 14;
                    return preview;
                default:
                    return CreateSimpleText("");
            }
        }

        private TextBox CreateSimpleText(string content, HorizontalAlignment alignment = HorizontalAlignment.Center)
        {
            TextBox name = new TextBox();
            name.Foreground = Brushes.White;
            name.FontFamily = new FontFamily("Arial");
            name.FontSize = 10;
            name.Background = Brushes.Transparent;
            name.BorderBrush = Brushes.Transparent;
            name.Text = content;
            name.HorizontalContentAlignment = alignment;
            name.IsEnabled = false;
            return name;
        }

        private Ellipse CreateCircle(int row, bool isInput)
        {
            Ellipse circle = new Ellipse();
            circle.Stroke = Brushes.White; circle.Fill = Brushes.White;
            circle.Width = 10;
            circle.Height = 10;
            circle.Tag = isInput ? row.ToString() : '-' + row.ToString();
            circle.HorizontalAlignment = isInput ? HorizontalAlignment.Left : HorizontalAlignment.Right;
            circle.MouseLeftButtonDown += StartConnection;
            circle.MouseLeftButtonUp += EndConnection;
            Grid.SetRow(circle, row);
            Grid.SetColumn(circle, 0);
            return circle;
        }

        #endregion

        #region Drag
        private Point _position = new Point();
        public Point Position
        {
            get
            {
                _position.X = Canvas.GetLeft(face);
                _position.Y = Canvas.GetTop(face);
                return _position;
            }
            set
            {
                if (_position == value) return;
                _position = ClampPosition(value);
                OnPositionChanged();
            }
        }

        private Point ClampPosition(Point value)
        {
            if (value.X > canvas.ActualWidth - face.ActualWidth)
                value.X = canvas.ActualWidth - face.ActualWidth;
            // newLeft inside canvas left-border?
            else if (value.X < 0)
                value.X = 0;

            // newTop inside canvas bottom-border?
            if (value.Y > canvas.ActualHeight - face.ActualHeight)
                value.Y = canvas.ActualHeight - face.ActualHeight;
            // newTop inside canvas top-border?
            else if (value.Y < 0)
                value.Y = 0;

            return value;
        }
        private void OnPositionChanged()
        {
            Canvas.SetLeft(face, _position.X);
            Canvas.SetTop(face, _position.Y);
        }

        public void MoveOn(Vector offset)
        {
            Position += offset;
        }
        #endregion

        #region ActionsWithNode

        private void OnCollapse(object sender, RoutedEventArgs e)
        {
            MiniSize = !MiniSize;

            RecreateFace();
        }
        public void Flip()
        {
            nodeCore.Flip();
            FlipCircleAlignment(inputsCircles);
            FlipCircleAlignment(outputsCircles);

            RecreateFace();
        }

        public void ChangeForm() => OnCollapse(null, null);


        private void FlipCircleAlignment(Ellipse[] circles)
        {
            foreach (var point in circles)
            {
                bool isLeft = point.HorizontalAlignment == HorizontalAlignment.Left;
                point.HorizontalAlignment = isLeft ? HorizontalAlignment.Right : HorizontalAlignment.Left;
            }
        }

        private void RecreateFace()
        {
            DetachDependencies();

            if (MiniSize)
            {
                face.Child = CreateMiniForm();
                face.Width = 150;
                face.Height = 50;
            }
            else
            {
                face.Child = CreateNormalForm();
                face.Width = 150;
                face.Height = 150;
            }

            UpdateDependencies();
        }

        private void UpdateDependencies()
        {
            canvas.UpdateLayout(); //Обновляем позиции Ellipse на Canvas
            face.SetValue(Canvas.TopProperty, (double)face.GetValue(Canvas.TopProperty) - 0.001); // Обновляем позиции соединяющих линий PositionChanged
            foreach (var defaultInput in defaultInputs)
            {
                defaultInput?.ResetPosition();
            }
        }

        private void DetachDependencies()
        {
            for (int i = 0; i < nodeCore.GetInputsCount(); i++) // Удаляем старые значения parent для Ellipses
            {
                Grid parent = (Grid)inputsCircles[i].Parent;
                parent.Children.Remove(inputsCircles[i]);
            }
            for (int i = 0; i < nodeCore.GetOutputsCount(); i++)
            {
                Grid parent = (Grid)outputsCircles[i].Parent;
                parent.Children.Remove(outputsCircles[i]);
            }
        }

        private void OnActiveChanged()
        {
            if (isActive)
            {
                UpdateZIndex();
                foreach (var defaultInput in defaultInputs)
                {
                    defaultInput?.face.SetValue(Canvas.ZIndexProperty, Canvas.GetZIndex(face));
                }

                face.BorderBrush = Brushes.White;
            }
            else face.BorderBrush = Brushes.Black;
        }

        public void UpdateZIndex()
        {
            int maxZIndex = Canvas.GetZIndex(face);
            foreach (UIElement child in canvas.Children)
            {
                if (maxZIndex < Canvas.GetZIndex(child))
                    maxZIndex = Canvas.GetZIndex(child);
            }

            Canvas.SetZIndex(face, maxZIndex + 1);
        }
        #endregion

        #region Connection

        private void StartConnection(object sender, MouseButtonEventArgs e)
        { 
            Ellipse point = sender as Ellipse;
            string rowID = point.Tag as string;

            if (rowID[0] == '-')
            {
                int outputIndex = int.Parse(rowID[1..]);
                ConnectionSystem.SetOutput(this, outputIndex, point);
            }
            else
            {
                int inputIndex = int.Parse(rowID);
                ConnectionSystem.SetInput(this, inputIndex, point);
            }
        }

        private void EndConnection(object sender, MouseButtonEventArgs e)
        {
            Ellipse point = sender as Ellipse;
            string rowID = point.Tag as string;

            if (rowID[0] == '-')
            {
                ConnectionSystem.SetOutput(this, int.Parse(rowID[1..]), point);
            }
            else
            {
                ConnectionSystem.SetInput(this, int.Parse(rowID), point);
            }

            ConnectionSystem.EndNewConnection();
        }

        #endregion
    }
}
