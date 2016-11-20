using System;
using System.Reflection;
using System.Windows;
using Advanced_PortChecker.Classes;
using Syncfusion.Windows.Shared;

namespace Advanced_PortChecker.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Variables
        private readonly UpdateManager _updateManager;
        #endregion


        public MainWindow()
        {
            _updateManager = new UpdateManager("http://codedead.com/Software/Advanced%20PortChecker/update.xml");

            InitializeComponent();
            ChangeVisualStyle();
            LoadSettings();
        }

        /// <summary>
        /// Change the GUI to represent the current settings.
        /// </summary>
        private void LoadSettings()
        {
            LblVersion.Content += " " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            try
            {
                if (Properties.Settings.Default.AutoUpdate)
                {
                    _updateManager.CheckForUpdate(false, false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Advanced PortChecker", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Change the visual style of the controls, depending on the settings.
        /// </summary>
        internal void ChangeVisualStyle()
        {
            StyleManager.ChangeStyle(this);
        }

        private void HypSettings_Click(object sender, RoutedEventArgs e)
        {
            new SettingsWindow(this).ShowDialog();
        }

        private void HypUpdate_Click(object sender, RoutedEventArgs e)
        {
            _updateManager.CheckForUpdate(true, true);
        }
    }
}
