using System.Reflection;
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
        private void ChangeVisualStyle()
        {
            StyleManager.ChangeStyle(this);
        }
    }
}
