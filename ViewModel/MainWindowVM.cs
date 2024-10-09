using System.Windows;

namespace L_system.ViewModel
{
    public class MainWindowVM : BaseVM
    {
        #region Private
        private int radiusBorderWindow;
        private Window window;
        private int shadowMarginSize = 10;
        #endregion

        #region Public
        public string Name { get; set; } = "Hello World";
        public int ResizeBorder { get; set; } = 5;
        public int HeadHeight { get; set; } = 50;


        public Thickness ShadowMarginSize
        {
            get
            { 
                return new Thickness(window.WindowState == WindowState.Maximized ? 0 : shadowMarginSize);
            }
        }

        #endregion

        public MainWindowVM(Window window)
        {
            this.window = window;
        }

    }
}
