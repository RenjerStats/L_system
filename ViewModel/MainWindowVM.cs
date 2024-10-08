using System.Windows;

namespace L_system.ViewModel
{
    public class MainWindowVM : BaseVM
    {
        #region Private
        private int radiusBorderWindow;
        private Window window;
        #endregion

        #region Public
        public string Name { get; set; } = "Hello World";
        #endregion

        public MainWindowVM(Window window)
        {
            this.window = window;
        }

    }
}
