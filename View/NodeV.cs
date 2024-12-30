using L_system.Model.core.Nodes;
using L_system.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
            form.RowDefinitions[0].Height = new GridLength(0.10, GridUnitType.Star);
            form.RowDefinitions[1].Height = new GridLength(0.45, GridUnitType.Star);
            form.RowDefinitions[2].Height = new GridLength(0.45, GridUnitType.Star);
            for (int i = 0; i < 5; i++)
                form.ColumnDefinitions.Add(new ColumnDefinition());
            bool isInputEmpty = core.GetNameOfInputs().Length == 0;
            form.ColumnDefinitions[0].Width = new GridLength(isInputEmpty ? 0 : 0.10, GridUnitType.Star);
            form.ColumnDefinitions[1].Width = new GridLength(isInputEmpty ? 0 : 0.15, GridUnitType.Star);
            form.ColumnDefinitions[2].Width = new GridLength(0.50, GridUnitType.Star);
            form.ColumnDefinitions[3].Width = new GridLength(0.15, GridUnitType.Star);
            form.ColumnDefinitions[4].Width = new GridLength(0.10, GridUnitType.Star);

            TextBox name = new TextBox();
            name.Text = "Node";
            name.IsEnabled = false;
            name.HorizontalContentAlignment = HorizontalAlignment.Center;

            Grid.SetRow(name, 0);
            Grid.SetColumn(name, 0);
            Grid.SetColumnSpan(name, 4);
            form.Children.Add(name);

            Button button = new Button();
            button.Content = ">";

            Grid.SetRow(button, 0);
            Grid.SetColumn(button, 4);
            form.Children.Add(button);


            Grid inputCircles = new Grid();
            for (int i = 0; i < core.GetNameOfInputs().Length; i++)
                inputCircles.RowDefinitions.Add(new RowDefinition());
            inputCircles.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < core.GetNameOfInputs().Length; i++)
            {
                Button input = new Button();
                input.Content = "o";
                Grid.SetRow(input, i);
                Grid.SetColumn(input, 0);
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
                TextBox nameInput = new TextBox();
                nameInput.Text = core.GetNameOfInputs()[i];
                nameInput.IsEnabled = false;
                nameInput.VerticalContentAlignment = VerticalAlignment.Center;
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
                Button output = new Button();
                output.Content = "o";
                Grid.SetRow(output, i);
                Grid.SetColumn(output, 0);
                outputCircles.Children.Add(output);
            }
            Grid.SetRow(outputCircles, 1);
            Grid.SetColumn(outputCircles, 4);
            form.Children.Add(outputCircles);


            Grid outputNames = new Grid();
            for (int i = 0; i < core.GetNameOfOutputs().Length; i++)
                outputNames.RowDefinitions.Add(new RowDefinition());
            outputNames.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < core.GetNameOfOutputs().Length; i++)
            {
                TextBox nameOutput = new TextBox();
                nameOutput.Text = core.GetNameOfOutputs()[i];
                nameOutput.IsEnabled = false;
                nameOutput.VerticalContentAlignment = VerticalAlignment.Center;
                Grid.SetRow(nameOutput, i);
                Grid.SetColumn(nameOutput, 0);
                outputNames.Children.Add(nameOutput);
            }
            Grid.SetRow(outputNames, 1);
            Grid.SetColumn(outputNames, 3);
            form.Children.Add(outputNames);


            TextBox info = new TextBox();
            info.Text = "Тут будет описание";
            info.IsEnabled = false;
            Grid.SetRow(info, 1);
            Grid.SetColumn(info, 2);
            form.Children.Add(info);

            TextBox preview = new TextBox();
            preview.Text = "Тут будет превью";
            preview.IsEnabled = false;
            Grid.SetRow(preview, 2);
            Grid.SetColumnSpan(preview, 5);
            form.Children.Add(preview);

            face = new Border()
            {
                Width = 250,
                Height = 200,
                Background = Brushes.LightBlue,
                CornerRadius = new CornerRadius(10),
                BorderBrush = Brushes.DarkBlue,
                BorderThickness = new Thickness(2),
                Child = form
            };

            Canvas.SetLeft(face, position.X);
            Canvas.SetTop(face, position.Y);
        }
    }
}
