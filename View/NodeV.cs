using L_system.Model.core.Nodes;
using L_system.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace L_system.View
{
    public class NodeV
    {
        private NodeVM nodeCore;
        public Border face;

        public NodeV(Point position, NodeVM core)
        {
            Grid form = new Grid();
            for (int i = 0; i < 3; i++)
                form.RowDefinitions.Add(new RowDefinition());
            form.RowDefinitions[0].Height = new GridLength(20);
            form.RowDefinitions[1].Height = new GridLength(0.45, GridUnitType.Star);
            form.RowDefinitions[2].Height = new GridLength(0.45, GridUnitType.Star);
            for (int i = 0; i < 4; i++)
                form.ColumnDefinitions.Add(new ColumnDefinition());
            bool isInputEmpty = core.GetNameOfInputs().Length == 0;
            form.ColumnDefinitions[0].Width = new GridLength(isInputEmpty ? 0 : 15);
            form.ColumnDefinitions[1].Width = new GridLength(isInputEmpty ? 0 : 0.45, GridUnitType.Star);
            form.ColumnDefinitions[2].Width = new GridLength(0.45, GridUnitType.Star);
            form.ColumnDefinitions[3].Width = new GridLength(15);

            TextBox name = CreateSimpleText("Node");
            name.Padding = new Thickness(0, 5, 0, 0);

            Grid.SetRow(name, 0);
            Grid.SetColumn(name, 0);
            Grid.SetColumnSpan(name, 3);
            form.Children.Add(name);

            Button button = new Button();
            button.Background = Brushes.Transparent;
            button.BorderBrush = Brushes.Transparent;
            button.Content = ">";

            Grid.SetRow(button, 0);
            Grid.SetColumn(button, 3);
            form.Children.Add(button);


            Grid inputCircles = new Grid();
            for (int i = 0; i < core.GetNameOfInputs().Length; i++)
                inputCircles.RowDefinitions.Add(new RowDefinition());
            inputCircles.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < core.GetNameOfInputs().Length; i++)
            {
                Ellipse input = CreatePoint(i, 0, HorizontalAlignment.Left);
                inputCircles.Children.Add(input);
            }
            Grid.SetRow(inputCircles, 1);
            Grid.SetColumn(inputCircles, 0);
            form.Children.Add(inputCircles);


            Grid inputNames = new Grid();
            for (int i = 0; i < core.GetNameOfInputs().Length; i++)
                inputNames.RowDefinitions.Add(new RowDefinition());
            inputNames.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < core.GetNameOfInputs().Length; i++)
            {
                TextBox nameInput = CreateSimpleText(core.GetNameOfInputs()[i], HorizontalAlignment.Left);
                nameInput.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(nameInput, i);
                Grid.SetColumn(nameInput, 0);
                inputNames.Children.Add(nameInput);
            }
            Grid.SetRow(inputNames, 1);
            Grid.SetColumn(inputNames, 1);
            form.Children.Add(inputNames);


            Grid outputCircles = new Grid();
            for (int i = 0; i < core.GetNameOfOutputs().Length; i++)
                outputCircles.RowDefinitions.Add(new RowDefinition());
            outputCircles.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < core.GetNameOfOutputs().Length; i++)
            {
                Ellipse output = CreatePoint(i, 0, HorizontalAlignment.Right);
                outputCircles.Children.Add(output);
            }
            Grid.SetRow(outputCircles, 1);
            Grid.SetColumn(outputCircles, 3);
            form.Children.Add(outputCircles);


            Grid outputNames = new Grid();
            for (int i = 0; i < core.GetNameOfOutputs().Length; i++)
                outputNames.RowDefinitions.Add(new RowDefinition());
            outputNames.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < core.GetNameOfOutputs().Length; i++)
            {
                TextBox nameOutput = CreateSimpleText(core.GetNameOfOutputs()[i], HorizontalAlignment.Right);
                nameOutput.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetRow(nameOutput, i);
                Grid.SetColumn(nameOutput, 0);
                outputNames.Children.Add(nameOutput);
            }
            Grid.SetRow(outputNames, 1);
            Grid.SetColumn(outputNames, 2);
            form.Children.Add(outputNames);

            TextBox preview = CreateSimpleText("Здесь будет превью");
            Grid.SetRow(preview, 2);
            Grid.SetColumnSpan(preview, 4);
            form.Children.Add(preview);

            face = new Border()
            {
                Width = 200,
                Height = 150,
                Background = (Brush)new BrushConverter().ConvertFrom("#FF333333"),
                CornerRadius = new CornerRadius(10),
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(2),
                Child = form
            };

            Canvas.SetLeft(face, position.X);
            Canvas.SetTop(face, position.Y);
        }

        private static TextBox CreateSimpleText(string content, HorizontalAlignment alignment = HorizontalAlignment.Center)
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

        private static Ellipse CreatePoint(int row, int column, HorizontalAlignment alignment)
        {
            Ellipse point = new Ellipse();
            point.Stroke = Brushes.White; point.Fill = Brushes.White;
            point.Width = 10;
            point.Height = 10;
            point.HorizontalAlignment = alignment;
            Grid.SetRow(point, row);
            Grid.SetColumn(point, column);
            return point;
        }
    }
}
