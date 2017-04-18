using System;
using System.Diagnostics;
using System.Windows;
using Advanced_PortChecker.Classes;

namespace Advanced_PortChecker.Windows
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow
    {
        public AboutWindow()
        {
            InitializeComponent();

            ChangeVisualStyle();
        }

        /// <summary>
        /// Change the visual style of the controls, depending on the settings.
        /// </summary>
        private void ChangeVisualStyle()
        {
            StyleManager.ChangeStyle(this);
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnLicense_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(AppDomain.CurrentDomain.BaseDirectory + "\\gpl.pdf");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Advanced PortChecker", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCodeDead_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("http://codedead.com/");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Advanced PortChecker", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
