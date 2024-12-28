using System.Windows;

namespace L_system.ViewModel
{
    public class MainWindowVM : BaseVM
    {
        #region Private
        private Window window;

        private int resizeBorder = 5;
        private int headerHeight = 50;
        private int shadowMarginSize = 10;
        private int radiusBorderWindow = 6;
        #endregion

        #region Public
        public string Name { get; set; } = "Hello World";

        public Thickness ResizeBorder
        {
            get { return new Thickness(resizeBorder + shadowMarginSize); }
        }

        public double HeaderHeight
        {
            get { return headerHeight; }
        }

        public Thickness ShadowMarginSize
        {
            get
            { 
                return new Thickness(window.WindowState == WindowState.Maximized ? 0 : shadowMarginSize);
            }
        }

        public CornerRadius RadiusBorderWindow
        {
            get
            {
                return new CornerRadius(window.WindowState == WindowState.Maximized ? 0 : radiusBorderWindow);
            }
        }

        public CornerRadius RadiusBorderShadow
        {
            get
            {
                return new CornerRadius(window.WindowState == WindowState.Maximized ? 0 : radiusBorderWindow*2);
            }
        }

        #endregion

        public MainWindowVM(Window window)
        {
            this.window = window;
        }

    }
}
