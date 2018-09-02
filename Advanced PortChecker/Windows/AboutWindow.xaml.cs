using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using Advanced_PortChecker.Classes;

namespace Advanced_PortChecker.Windows
{
    /// <inheritdoc cref="Syncfusion.Windows.Shared.ChromelessWindow" />
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow
    {
        /// <inheritdoc />
        /// <summary>
        /// Initialize a new AboutWindow object
        /// </summary>
        public AboutWindow()
        {
            InitializeComponent();
            StyleManager.ChangeStyle(this);

            // Enable window dragging, if the setting is enabled
            try
            {
                if (Properties.Settings.Default.WindowDraggable)
                {
                    MouseDown += OnMouseDown;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Advanced PortChecker", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Method that is called when the Window should be dragged
        /// </summary>
        /// <param name="sender">The object that called this method</param>
        /// <param name="e">The MouseButtonEventArgs</param>
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        /// <summary>
        /// Method that is called when the Close Button is clicked
        /// </summary>
        /// <param name="sender">The object that called this method</param>
        /// <param name="e"> The RoutedEventArgs</param>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Method that is called when the License Button is clicked
        /// </summary>
        /// <param name="sender">The object that called this method</param>
        /// <param name="e">The RoutedEventArgs</param>
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

        /// <summary>
        /// Method that is called when the CodeDead Button is clicked
        /// </summary>
        /// <param name="sender">The object that called this method</param>
        /// <param name="e">The RoutedEventArgs</param>
        private void BtnCodeDead_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://codedead.com/");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Advanced PortChecker", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
