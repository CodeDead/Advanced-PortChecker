using Advanced_PortChecker.Classes;

namespace Advanced_PortChecker.Windows
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow
    {

        #region Variables

        private MainWindow _mw;

        #endregion

        public SettingsWindow(MainWindow mainWindow)
        {
            _mw = mainWindow;

            InitializeComponent();
            ChangeVisualStyle();
        }

        private void ChangeVisualStyle()
        {
            StyleManager.ChangeStyle(this);
        }
    }
}
