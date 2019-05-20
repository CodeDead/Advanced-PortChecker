using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Advanced_PortChecker.Classes.GUI;

namespace Advanced_PortChecker.Windows
{
    /// <inheritdoc cref="Syncfusion.Windows.Shared.ChromelessWindow" />
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow
    {
        #region Variables
        /// <summary>
        /// The MainWindow object that can be used to dynamically change settings
        /// </summary>
        private readonly MainWindow _mw;
        #endregion

        /// <inheritdoc />
        /// <summary>
        /// Initialize a new SettingsWindow object
        /// </summary>
        /// <param name="mainWindow">The MainWindow object that can be used to dynamically change settings</param>
        public SettingsWindow(MainWindow mainWindow)
        {
            _mw = mainWindow;

            InitializeComponent();
            StyleManager.ChangeStyle(this);
            LoadSettings();
        }

        /// <summary>
        /// Change the GUI to represent the current settings
        /// </summary>
        private void LoadSettings()
        {
            try
            {
                CboTimeOut.SelectedIndex = Properties.Settings.Default.TimeOutType;
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (Properties.Settings.Default.TimeOutType)
                {
                    case 0:
                        IntTimeOut.Value = Properties.Settings.Default.TimeOut;
                        break;
                    case 1:
                        IntTimeOut.Value = Properties.Settings.Default.TimeOut / 1000;
                        break;
                    case 2:
                        IntTimeOut.Value = (Properties.Settings.Default.TimeOut / 1000) / 60;
                        break;
                }

                if (Properties.Settings.Default.WindowDraggable)
                {
                    // Prevent duplicate handlers
                    MouseDown -= OnMouseDown;
                    MouseDown += OnMouseDown;
                }
                else
                {
                    MouseDown -= OnMouseDown;
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
        /// Method that is called when the Save Button is clicked
        /// </summary>
        /// <param name="sender">The object that called this method</param>
        /// <param name="e">The RoutedEventArgs</param>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Properties.Settings.Default.TimeOutType = CboTimeOut.SelectedIndex;
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (CboTimeOut.SelectedIndex)
                {
                    case 0:
                        if (IntTimeOut.Value != null) Properties.Settings.Default.TimeOut = (int)IntTimeOut.Value;
                        break;
                    case 1:
                        if (IntTimeOut.Value != null) Properties.Settings.Default.TimeOut = (int)IntTimeOut.Value * 1000;
                        break;
                    case 2:
                        if (IntTimeOut.Value != null) Properties.Settings.Default.TimeOut = (int)IntTimeOut.Value * 60 * 1000;
                        break;
                }

                Properties.Settings.Default.Save();

                LoadSettings();

                _mw.WindowDraggable();

                StyleManager.ChangeStyle(_mw);
                StyleManager.ChangeStyle(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Advanced PortChecker", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Method that is called when the Reset Button is clicked
        /// </summary>
        /// <param name="sender">The object that called this method</param>
        /// <param name="e">The RoutedEventArgs</param>
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to reset all settings?", "Advanced PortChecker", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                {
                    return;
                }

                Properties.Settings.Default.Reset();
                Properties.Settings.Default.Save();

                LoadSettings();

                _mw.WindowDraggable();

                StyleManager.ChangeStyle(_mw);
                StyleManager.ChangeStyle(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Advanced PortChecker", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Method that is called when the opacity should change dynamically
        /// </summary>
        /// <param name="sender">The object that called this method</param>
        /// <param name="e">The RoutedPropertyChangedEventArgs</param>
        private void SldOpacity_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Opacity = ((Slider)sender).Value / 100;
        }
        
        /// <summary>
        /// Method that is called when the border thickness should change
        /// </summary>
        /// <param name="sender">The object that called this method</param>
        /// <param name="e">The RoutedPropertyChangedEventArgs</param>
        private void SldBorderThickness_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                BorderThickness = new Thickness(((Slider)sender).Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Advanced PortChecker", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Method  that is called when the ResizeBorderThickness should change dynamically
        /// </summary>
        /// <param name="sender">The object that called this method</param>
        /// <param name="e">The RoutedPropertyChangedEventArgs</param>
        private void SldWindowResize_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ResizeBorderThickness = new Thickness(((Slider)sender).Value);
        }

        /// <summary>
        /// Method that is called when the visual style is changed by user input
        /// </summary>
        /// <param name="sender">The object that called this method</param>
        /// <param name="e">The SelectionChangedEventArgs</param>
        private void ChbStyle_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StyleManager.ChangeStyle(this);
        }

        /// <summary>
        /// Method that is called when the SettingsWindow object is closing
        /// </summary>
        /// <param name="sender">The object that called this method</param>
        /// <param name="e">The CancelEventArgs</param>
        private void SettingsWindow_OnClosing(object sender, CancelEventArgs e)
        {
            try
            {
                Properties.Settings.Default.Reload();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Advanced PortChecker", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
