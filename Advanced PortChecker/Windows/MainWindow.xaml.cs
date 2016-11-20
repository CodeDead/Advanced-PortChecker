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
        public MainWindow()
        {
            InitializeComponent();
            ChangeVisualStyle();

            LblVersion.Content += Assembly.GetExecutingAssembly().GetName().Version.ToString();
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
            throw new System.NotImplementedException();
        }
    }
}
