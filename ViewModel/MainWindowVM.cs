using System.Windows;

namespace L_system.ViewModel
{
    public class MainWindowVM : BaseVM
    {
        #region Private
        private int radiusBorderWindow = 7;
        private Window window;
        private int shadowMarginSize = 5;
        #endregion

        #region Public
        public string Name { get; set; } = "Hello World";
        public int ResizeBorder { get; set; } = 5;
        public int HeaderHeight { get; set; } = 50;


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
