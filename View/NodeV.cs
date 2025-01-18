using L_system.Model.core.Nodes;
using L_system.Systems;
using L_system.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace L_system.View
{
    public class NodeV : IDisposable
    {
        public NodeVM nodeCore;
        public Border face;
        public Canvas canvas;
        public Ellipse[] inputsPoint;
        public Ellipse[] outputsPoint;
        public DefaultInputV[] defaultInputs;

        public bool MiniSize { get; private set; } = false;

        public NodeV(Point position, NodeVM core, Canvas canvas)
        {
            nodeCore = core;
            this.canvas = canvas;

            inputsPoint = new Ellipse[core.GetInputsCount()];
            defaultInputs = new DefaultInputV[core.GetInputsCount()];

            for (int i = 0; i < core.GetInputsCount(); i++)
                inputsPoint[i] = CreatePoint(i, true);
            outputsPoint = new Ellipse[core.GetOutputsCount()];
            for (int i = 0; i < core.GetOutputsCount(); i++)
                outputsPoint[i] = CreatePoint(i, false);


            face = new Border()
            {
                Width = 150,
                Height = 150,
                Background = (Brush)new BrushConverter().ConvertFrom("#FF333333"),
                CornerRadius = new CornerRadius(10),
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(2),
                Child = CreateNormalForm()
            };

            face.PreviewMouseLeftButtonDown += Node_MouseLeftButtonDown; // Превью обрабатывается раньше, чем дочерние события. Сначала parent, потом children
            face.PreviewMouseMove += Node_MouseMove;
            face.MouseLeave += Node_MouseLeave;

            Canvas.SetLeft(face, position.X);
            Canvas.SetTop(face, position.Y);
            canvas.Children.Add(face);

            for (int i = 0; i < core.GetInputsCount(); i++)
                defaultInputs[i] = new DefaultInputV(core, i, inputsPoint[i], canvas); // Зависит от canvas.Children.Add(face);
        }

        #region Visual

        private Grid CreateNormalForm()
        {
            Grid form = new Grid();
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

            TextBox name = CreateSimpleText(nodeCore.GetNameOfNode());
            name.FontSize = 12;
            name.Padding = new Thickness(0, 5, 0, 0);

            Grid.SetRow(name, 0);
            Grid.SetColumn(name, 0);
            Grid.SetColumnSpan(name, 3);
            form.Children.Add(name);

            Button button = new Button();
            button.Click += OnCollapse;
            button.Foreground = Brushes.White;
            button.Background = Brushes.Transparent;
            button.BorderBrush = Brushes.Transparent;
            button.Content = ">";

            Grid.SetRow(button, 0);
            Grid.SetColumn(button, 3);
            form.Children.Add(button);


            Grid inputCircles = new Grid();
            for (int i = 0; i < nodeCore.GetInputsCount(); i++)
                inputCircles.RowDefinitions.Add(new RowDefinition());
            inputCircles.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < nodeCore.GetInputsCount(); i++)
            {
                Ellipse input = inputsPoint[i];
                inputCircles.Children.Add(input);
            }
            Grid.SetRow(inputCircles, 1);
            Grid.SetColumn(inputCircles, 0);
            form.Children.Add(inputCircles);


            Grid inputNames = new Grid();
            for (int i = 0; i < nodeCore.GetInputsCount(); i++)
                inputNames.RowDefinitions.Add(new RowDefinition());
            inputNames.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < nodeCore.GetInputsCount(); i++)
            {
                TextBox nameInput = CreateSimpleText(nodeCore.GetNameOfInputs()[i], HorizontalAlignment.Left);
                nameInput.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(nameInput, i);
                Grid.SetColumn(nameInput, 0);
                inputNames.Children.Add(nameInput);
            }
            Grid.SetRow(inputNames, 1);
            Grid.SetColumn(inputNames, 1);
            form.Children.Add(inputNames);


            Grid outputCircles = new Grid();
            for (int i = 0; i < nodeCore.GetOutputsCount(); i++)
                outputCircles.RowDefinitions.Add(new RowDefinition());
            outputCircles.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < nodeCore.GetOutputsCount(); i++)
            {
                Ellipse output = outputsPoint[i];
                outputCircles.Children.Add(output);
            }
            Grid.SetRow(outputCircles, 1);
            Grid.SetColumn(outputCircles, 3);
            form.Children.Add(outputCircles);


            Grid outputNames = new Grid();
            for (int i = 0; i < nodeCore.GetOutputsCount(); i++)
                outputNames.RowDefinitions.Add(new RowDefinition());
            outputNames.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < nodeCore.GetOutputsCount(); i++)
            {
                TextBox nameOutput = CreateSimpleText(nodeCore.GetNameOfOutputs()[i], HorizontalAlignment.Right);
                nameOutput.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(nameOutput, i);
                Grid.SetColumn(nameOutput, 0);
                outputNames.Children.Add(nameOutput);
            }
            Grid.SetRow(outputNames, 1);
            Grid.SetColumn(outputNames, 2);
            form.Children.Add(outputNames);

            UIElement preview = GetPreview();
            Grid.SetRow(preview, 2);
            Grid.SetColumnSpan(preview, 4);
            form.Children.Add(preview);
            return form;
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
                        // Используем Dispatcher для изменения текста в UI-потоке
                        preview.Dispatcher.BeginInvoke(() =>
                        {
                            preview.Text = $"Выходное значение:\n\n{nodeCore.GetValueFromOutput(0):F5}";
                        });
                    });
                    preview.VerticalAlignment = VerticalAlignment.Center;
                    preview.FontSize = 14;
                    return preview;
                default:
                    return CreateSimpleText("");
            }
        }

        private Grid CreateMiniForm()
        {
            Grid form = new Grid();
            form.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < 4; i++)
                form.ColumnDefinitions.Add(new ColumnDefinition());
            bool isInputEmpty = nodeCore.GetInputsCount() == 0;
            bool isOutputEmpty = nodeCore.GetOutputsCount() == 0;
            form.ColumnDefinitions[0].Width = new GridLength(isInputEmpty ? 0 : 15);
            form.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
            form.ColumnDefinitions[2].Width = new GridLength(15);
            form.ColumnDefinitions[3].Width = new GridLength(isOutputEmpty ? 0 : 15);

            TextBox name = CreateSimpleText(nodeCore.GetNameOfNode());
            name.VerticalContentAlignment = VerticalAlignment.Center;
            name.FontSize = 14;

            Grid.SetRow(name, 0);
            Grid.SetColumn(name, 1);
            form.Children.Add(name);

            Button button = new Button();
            button.Click += OnCollapse;
            button.Foreground = Brushes.White;
            button.Background = Brushes.Transparent;
            button.BorderBrush = Brushes.Transparent;
            button.Content = "<";

            Grid.SetRow(button, 0);
            Grid.SetColumn(button, 2);
            form.Children.Add(button);


            Grid inputCircles = new Grid();
            for (int i = 0; i < nodeCore.GetInputsCount(); i++)
                inputCircles.RowDefinitions.Add(new RowDefinition());
            inputCircles.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < nodeCore.GetInputsCount(); i++)
            {
                Ellipse input = inputsPoint[i];
                inputCircles.Children.Add(input);
            }
            Grid.SetRow(inputCircles, 0);
            Grid.SetColumn(inputCircles, 0);
            form.Children.Add(inputCircles);

            Grid outputCircles = new Grid();
            for (int i = 0; i < nodeCore.GetOutputsCount(); i++)
                outputCircles.RowDefinitions.Add(new RowDefinition());
            outputCircles.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < nodeCore.GetOutputsCount(); i++)
            {
                Ellipse output = outputsPoint[i];
                outputCircles.Children.Add(output);
            }
            Grid.SetRow(outputCircles, 0);
            Grid.SetColumn(outputCircles, 3);
            form.Children.Add(outputCircles);

            return form;
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

        private Ellipse CreatePoint(int row, bool isInput)
        {
            Ellipse point = new Ellipse();
            point.Stroke = Brushes.White; point.Fill = Brushes.White;
            point.Width = 10;
            point.Height = 10;
            point.Tag = isInput ? row.ToString() : '-' + row.ToString();
            point.HorizontalAlignment = isInput ? HorizontalAlignment.Left : HorizontalAlignment.Right;
            point.MouseLeftButtonDown += StartConnection;
            point.MouseLeftButtonUp += EndConnection;
            Grid.SetRow(point, row);
            Grid.SetColumn(point, 0);
            return point;
        }

        #endregion

        #region Drag
        double firstXPos, firstYPos;
        Border? movingFace;

        private void Node_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            movingFace = sender as Border;
            NodeSystem.ActiveNode = this;

            UpdateZIndex();
            foreach (var defaultInput in defaultInputs)
            {
                defaultInput.face.SetValue(Canvas.ZIndexProperty, Canvas.GetZIndex(movingFace));
            }

            firstXPos = e.GetPosition(movingFace).X;
            firstYPos = e.GetPosition(movingFace).Y;
        }

        private void UpdateZIndex()
        {
            int maxZIndex = Canvas.GetZIndex(movingFace);
            foreach (UIElement child in canvas.Children)
            {
                if (maxZIndex < Canvas.GetZIndex(child))
                    maxZIndex = Canvas.GetZIndex(child);
            }

            Canvas.SetZIndex(movingFace, maxZIndex + 1);
        }

        private void Node_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && movingFace == sender as Border)
            {
                double newLeft = e.GetPosition(canvas).X - firstXPos;
                // newLeft inside canvas right-border?
                if (newLeft > canvas.ActualWidth - movingFace.ActualWidth)
                    newLeft = canvas.ActualWidth - movingFace.ActualWidth;
                // newLeft inside canvas left-border?
                else if (newLeft < 0)
                    newLeft = 0;
                movingFace.SetValue(Canvas.LeftProperty, newLeft);

                double newTop = e.GetPosition(canvas).Y - firstYPos;
                // newTop inside canvas bottom-border?
                if (newTop > canvas.ActualHeight - movingFace.ActualHeight)
                    newTop = canvas.ActualHeight - movingFace.ActualHeight;
                // newTop inside canvas top-border?
                else if (newTop < 0)
                    newTop = 0;
                movingFace.SetValue(Canvas.TopProperty, newTop);
            }
        }

        private void Node_MouseLeave(object sender, MouseEventArgs e)
        {
            movingFace = null;
        }


        #endregion

        private void OnCollapse(object sender, RoutedEventArgs e)
        {
            MiniSize = !MiniSize;

            for (int i = 0; i < nodeCore.GetInputsCount(); i++) // Удаляем старые значения parent для Ellipses
            {
                Grid parent = (Grid)inputsPoint[i].Parent;
                parent.Children.Remove(inputsPoint[i]);
            }
            for (int i = 0; i < nodeCore.GetOutputsCount(); i++)
            {
                Grid parent = (Grid)outputsPoint[i].Parent;
                parent.Children.Remove(outputsPoint[i]);
            }

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

            canvas.UpdateLayout(); //Обновляем позиции Ellipse на Canvas
            face.SetValue(Canvas.TopProperty, (double)face.GetValue(Canvas.TopProperty) - 0.01);
            foreach (var defaultInput in defaultInputs)
            {
                defaultInput.ResetPosition();
            }
        }

        private void StartConnection(object sender, MouseButtonEventArgs e)
        {
            movingFace = null; // Сбрасываем ссылку на движимый нод, чтобы он не двигался при создании Connection 
            Ellipse point = sender as Ellipse;
            string rowID = point.Tag as string;

            ConnectionSystem.StartNewConnection(face.Parent as Canvas);

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

        public void Dispose()
        {
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
                defInput.Dispose();
            }

            canvas.Children.Remove(face);
        }
    }
}
