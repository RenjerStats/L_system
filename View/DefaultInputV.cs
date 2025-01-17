﻿using L_system.ViewModel;
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
using Windows.Foundation.Collections;

namespace L_system.View
{
    public class DefaultInputV
    {
        private NodeVM node;
        private int inputIndex;
        private Ellipse connectionPoint;
        private Canvas canvas;

        public Border face;

        public DefaultInputV(NodeVM node, int inputIndex, Ellipse connectionPoint, Canvas canvas)
        {
            this.node = node;
            this.inputIndex = inputIndex;
            this.connectionPoint = connectionPoint;
            this.canvas = canvas;

            face = CreateForm();
            face.Loaded += Face_Loaded;

            face.MouseMove += DefaultInput_MouseMove;
            canvas.MouseMove += DefaultInput_MouseMove;
            face.MouseLeftButtonDown += DefaultInput_MouseLeftButtonDown;
            face.MouseLeftButtonUp += Reset;
            canvas.MouseLeftButtonUp += Reset;
            canvas.MouseLeave += Reset;
            RegisterPositionChanged(connectionPoint);

            canvas.Children.Add(face);
        }

        private void Off()
        {
            DeleteRegisterPositionChanged(connectionPoint);
            canvas.Children.Remove(face);
        }

        private void On()
        {
            RegisterPositionChanged(connectionPoint);
            canvas.Children.Add(face);
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
                child.Dispatcher.BeginInvoke(() =>
                {
                    child.Text = $"{node.GetValueFromDefInput(inputIndex):F3}";
                });
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
            face.SetValue(Canvas.LeftProperty, -60D); // Прячем face при перемещении
            face.SetValue(Canvas.TopProperty, -10D);
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
        }

        private void DefaultInput_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && movingFace != null)
            {
                double newTop = e.GetPosition(canvas).Y - firstYPos;

                if (newTop > canvas.ActualHeight - movingFace.ActualHeight)
                    newTop = canvas.ActualHeight - movingFace.ActualHeight;

                else if (newTop < 0)
                    newTop = 0;
                movingFace.SetValue(Canvas.TopProperty, newTop);

                UpdateDefaultValue();
            }
        }

        private void UpdateDefaultValue()
        {
            double difference = Mouse.GetPosition(canvas).Y - GetYPositionConnectionPoint();
            double offsetY = difference / 1000;
            offsetY = offsetY * Math.Abs(offsetY);

            double newValue = (double)node.GetValueFromDefInput(inputIndex) - offsetY;
            node.SetValueToDefInput(newValue, inputIndex);
        }

        private void Reset(object sender, MouseEventArgs e)
        {
            movingFace = null;
            ResetPosition();
        }

        #endregion
    }
}
