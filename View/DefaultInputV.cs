using L_system.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Windows.Foundation.Collections;

namespace L_system.View
{
    public class DefaultInputV : IDisposable
    {
        private readonly DispatcherTimer timer;

        private NodeVM node;
        private int inputIndex;
        private Ellipse connectionPoint;
        private Canvas canvas;

        public Border face;
        public bool isShow { get; private set; } = true;

        public DefaultInputV(NodeVM node, int inputIndex, Ellipse connectionPoint, Canvas canvas)
        {
            this.node = node;
            this.inputIndex = inputIndex;
            this.connectionPoint = connectionPoint;
            this.canvas = canvas;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1.0 / 60);
            timer.Tick += OnTimerTick;

            node.OnInputsChanged(ShowOrNotDefaultInput);

            face = CreateForm();
            face.Loaded += Face_Loaded;

            face.MouseLeftButtonDown += DefaultInput_MouseLeftButtonDown;
            face.MouseLeftButtonUp += Reset;
            canvas.MouseLeftButtonUp += Reset;
            canvas.MouseLeave += Reset;
            RegisterPositionChanged(connectionPoint);

            canvas.Children.Add(face);
        }

        private void ShowOrNotDefaultInput()
        {
            if (isShow != node.IsInputFree(inputIndex))
            {
                isShow = node.IsInputFree(inputIndex);

                if (isShow) Show();
                else Hide();
            }
        }

        public void Show()
        {
            RegisterPositionChanged(connectionPoint);
            canvas.Children.Add(face);
        }

        public void Hide()
        {
            DeleteRegisterPositionChanged(connectionPoint);
            canvas.Children.Remove(face);
        }

        private void MoveOffScreen()
        {
            face.SetValue(Canvas.LeftProperty, -60D);
            face.SetValue(Canvas.TopProperty, -10D);
        }

        private void Face_Loaded(object sender, RoutedEventArgs e)
        {
            ResetPosition();
        }
        public void ResetPosition()
        {
            face.SetValue(Canvas.LeftProperty, GetXPositionConnectionPoint() - 60);
            face.SetValue(Canvas.TopProperty, GetYPositionConnectionPoint() - 10);
        }

        private double GetXPositionConnectionPoint()
        {
            return connectionPoint.TranslatePoint(new Point(connectionPoint.Width / 2, connectionPoint.Height / 2), canvas).X;
        }
        private double GetYPositionConnectionPoint()
        {
            return connectionPoint.TranslatePoint(new Point(connectionPoint.Width / 2, connectionPoint.Height / 2), canvas).Y;
        }

        private Border CreateForm()
        {
            TextBox child = CreateSimpleText($"{node.GetValueFromDefInput(inputIndex):F3}");

            node.OnOutputChanged(() => {
                child.Text = $"{node.GetValueFromDefInput(inputIndex):F3}";
            });

            return new Border()
            {
                Width = 52,
                Height = 20,
                Background = (Brush)new BrushConverter().ConvertFrom("#FF333333"),
                CornerRadius = new CornerRadius(3),
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(2),
                Child = child
            };
        }

        private TextBox CreateSimpleText(string content)
        {
            TextBox name = new TextBox();
            name.Foreground = Brushes.White;
            name.FontFamily = new FontFamily("Arial");
            name.FontSize = 12;
            name.Background = Brushes.Transparent;
            name.BorderBrush = Brushes.Transparent;
            name.Text = content;
            name.HorizontalContentAlignment = HorizontalAlignment.Center;
            name.VerticalContentAlignment = VerticalAlignment.Center;
            name.IsEnabled = false;
            return name;
        }

        private void RegisterPositionChanged(Ellipse ellipse)
        {
            Border faceNode = (Border)((Grid)((Grid)ellipse.Parent).Parent).Parent;

            DependencyPropertyDescriptor.FromProperty(Canvas.LeftProperty, typeof(Border))
                .AddValueChanged(faceNode, OnPositionChanged);
            DependencyPropertyDescriptor.FromProperty(Canvas.TopProperty, typeof(Border))
                .AddValueChanged(faceNode, OnPositionChanged);
        }
        private void DeleteRegisterPositionChanged(Ellipse ellipse)
        {
            Border faceNode = (Border)((Grid)((Grid)ellipse.Parent).Parent).Parent;
            DependencyPropertyDescriptor.FromProperty(Canvas.LeftProperty, typeof(Border))
                .RemoveValueChanged(faceNode, OnPositionChanged);
            DependencyPropertyDescriptor.FromProperty(Canvas.TopProperty, typeof(Border))
                .RemoveValueChanged(faceNode, OnPositionChanged);
        }
        public void OnPositionChanged(object? sender, EventArgs e)
        {
            MoveOffScreen();
        }


        #region Drag
        double firstYPos;
        Border? movingFace;

        private void DefaultInput_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            movingFace = sender as Border;

            int top = Canvas.GetZIndex(movingFace);
            foreach (UIElement child in canvas.Children)
            {
                if (top < Canvas.GetZIndex(child))
                    top = Canvas.GetZIndex(child);
            }

            Canvas.SetZIndex(movingFace, top + 1);

            firstYPos = e.GetPosition(movingFace).Y;

            timer.Start();
        }

        private void OnTimerTick(object? sender, EventArgs e)
        {
            double newTop = Mouse.GetPosition(canvas).Y - firstYPos;

            if (newTop > canvas.ActualHeight - movingFace.ActualHeight)
                newTop = canvas.ActualHeight - movingFace.ActualHeight;

            else if (newTop < 0)
                newTop = 0;
            movingFace.SetValue(Canvas.TopProperty, newTop);

            UpdateDefaultValue();
        }

        private void UpdateDefaultValue()
        {
            double difference = Mouse.GetPosition(canvas).Y - GetYPositionConnectionPoint();
            double offsetY = difference / 300;
            offsetY = offsetY * Math.Abs(offsetY);

            double newValue = (double)node.GetValueFromDefInput(inputIndex) - offsetY;
            node.SetValueToDefInput(newValue, inputIndex);
        }

        private void Reset(object sender, MouseEventArgs e)
        {
            movingFace = null;
            timer.Stop();
            ResetPosition();
        }

        public void Dispose()
        {
            Hide();
            canvas.MouseLeftButtonUp -= Reset;
            canvas.MouseLeave -= Reset;
        }

        #endregion
    }
}
