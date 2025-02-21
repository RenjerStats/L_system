using DrawTest;
using L_system.Systems.ForDraw;
using L_system.Systems.ForNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace L_system.Systems
{
    public class UpdateSystem
    {
        private readonly DispatcherTimer timer;
        private DateTime lastUpdateTime;

        private DrawingCanvas drawingCanvas;
        private CommandTranslator translator;
        private double widthDrawingArea;
        private double heightDrawingArea;

        public UpdateSystem(DrawingCanvas canvas)
        {
            drawingCanvas = canvas;

            timer = new DispatcherTimer(DispatcherPriority.Render);
            timer.Tick += OnTimerTick;
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (DateTime.Now - lastUpdateTime < timer.Interval) return;

            Update();
            lastUpdateTime = DateTime.Now;
        }

        private void Update()
        {
            Drawer drawer = new Drawer(widthDrawingArea / 2, heightDrawingArea / 2);
            translator = new CommandTranslator(drawer);
            drawingCanvas.Clear();
            DrawBackground();


            foreach (var commands in NodeSystem.GetCommandsFromAllNodesEnd())
            {
                drawingCanvas.AddDrawingGroup(translator.Convert(commands));
            }
        }

        public void StartUpdating() => timer.Start();
        public void StopUpdating() => timer.Stop();

        public void SetFrameRate(int frameRate)
        {
            if (frameRate <= 0) throw new ArgumentException("frameRate must be positive");
            timer.Interval = TimeSpan.FromSeconds(1.0 / frameRate);
        }


        private void DrawBackground()
        {
            DrawingGroup group = new DrawingGroup();
            GeometryDrawing background = CreateRect(Brushes.DarkGreen, 0, 0, widthDrawingArea, heightDrawingArea);
            group.Children.Add(background);
            drawingCanvas.AddDrawingGroup(group);
        }
        private GeometryDrawing CreateRect(Brush fill, double x, double y, double width, double height)
        {
            GeometryDrawing drawingRect = new GeometryDrawing();
            drawingRect.Geometry = new RectangleGeometry(new Rect(x, y, width, height));
            drawingRect.Brush = fill;

            return drawingRect;
        }

        internal void ResetSizeDrawingArea(object sender, SizeChangedEventArgs e)
        {
            widthDrawingArea = e.NewSize.Width;
            heightDrawingArea = e.NewSize.Height;
        }
    }
}
