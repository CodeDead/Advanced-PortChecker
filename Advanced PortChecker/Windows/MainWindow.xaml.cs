using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Shell;
using Advanced_PortChecker.Classes;
using Microsoft.Win32;

namespace Advanced_PortChecker.Windows
{
    /// <inheritdoc cref="Syncfusion.Windows.Shared.ChromelessWindow" />
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Variables
        /// <summary>
        /// The UpdateManager object that can be used to check for updates
        /// </summary>
        private readonly UpdateManager.UpdateManager _updateManager;
        /// <summary>
        /// The OperationInformation object
        /// </summary>
        private OperationInformation _oI;
        #endregion

        /// <inheritdoc />
        /// <summary>
        /// Initialize a new MainWindow object
        /// </summary>
        public MainWindow()
        {
            _updateManager = new UpdateManager.UpdateManager(Assembly.GetExecutingAssembly().GetName().Version, "https://codedead.com/Software/Advanced%20PortChecker/update.xml", "Advanced PortChecker");

            InitializeComponent();

            ChangeVisualStyle();
            LoadSettings();
        }

        /// <summary>
        /// Change the GUI to represent the current settings
        /// </summary>
        private void LoadSettings()
        {
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
        /// Change the visual style of the controls, depending on the settings
        /// </summary>
        internal void ChangeVisualStyle()
        {
            StyleManager.ChangeStyle(this);
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            new AboutWindow().ShowDialog();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            new SettingsWindow(this).ShowDialog();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            _updateManager.CheckForUpdate(true, true);
        }

        /// <summary>
        /// Determine whether the user can change the settings or not
        /// </summary>
        /// <param name="enabled">A boolean to represent whether the user can change the settings or not</param>
        private void ControlsEnabled(bool enabled)
        {
            BtnCancel.IsEnabled = !enabled;
            BtnScan.IsEnabled = enabled;
            TxtAddress.IsEnabled = enabled;
            IntStart.IsEnabled = enabled;
            IntStop.IsEnabled = enabled;
            CbaMethod.IsEnabled = enabled;
        }

        private async void BtnScan_Click(object sender, RoutedEventArgs e)
        {
            LvPorts.Items.Clear();
            ControlsEnabled(false);

            PgbStatus.Value = PgbStatus.Minimum;
            TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
            TaskbarItemInfo.ProgressValue = 0;

            _oI = new OperationInformation
            {
                Progress = new Progress<int>(value =>
                {
                    PgbStatus.Value = value;
                    TaskbarItemInfo.ProgressValue = value/(PgbStatus.Maximum - PgbStatus.Minimum);
                }),
                ItemProgress = new Progress<LvCheck>(value => { LvPorts.Items.Add(value); }),
                IsCancelled = false
            };

            if (IntStart.Value != null)
            {
                PgbStatus.Minimum = (double) IntStart.Value - 1;
                if (IntStop.Value != null)
                {
                    PgbStatus.Maximum = (double) IntStop.Value;
                    int timeout = Properties.Settings.Default.TimeOut;

                    TxtAddress.Text = TxtAddress.Text.Replace("https://", "");
                    TxtAddress.Text = TxtAddress.Text.Replace("http://", "");

                    switch (CbaMethod.Text)
                    {
                        case "TCP":
                            await PortChecker.CheckTCP(TxtAddress.Text, (int) IntStart.Value, (int) IntStop.Value, timeout, _oI, true);
                            break;
                        case "UDP":
                            await PortChecker.CheckUDP(TxtAddress.Text, (int) IntStart.Value, (int) IntStop.Value, timeout, _oI, true);
                            break;
                        case "Both":
                            await PortChecker.CheckTCPUDP(TxtAddress.Text, (int) IntStart.Value, (int) IntStop.Value, timeout, _oI);
                            break;
                    }
                }
            }
            ControlsEnabled(true);

            TaskbarItemInfo.ProgressState = TaskbarItemProgressState.None;
            TaskbarItemInfo.ProgressValue = 0;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Donate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("https://codedead.com/?page_id=302");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Advanced PortChecker", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (_oI == null) return;

            _oI.IsCancelled = true;
            ControlsEnabled(true);

            TaskbarItemInfo.ProgressState = TaskbarItemProgressState.None;
            TaskbarItemInfo.ProgressValue = 0;
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
                Process.Start("https://codedead.com/");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Advanced PortChecker", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(AppDomain.CurrentDomain.BaseDirectory + "\\help.pdf");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Advanced PortChecker", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDeleteAllItems_Click(object sender, RoutedEventArgs e)
        {
            LvPorts.Items.Clear();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            List<LvCheck> delete = new List<LvCheck>();
            foreach (LvCheck l in LvPorts.SelectedItems)
            {
                delete.Add(l);
            }

            foreach (LvCheck lv in delete)
            {
                LvPorts.Items.Remove(lv);
            }
        }

        private void BtnCopy_Click(object sender, RoutedEventArgs e)
        {
            if (LvPorts.SelectedItems.Count == 0) return;

            LvCheck selected = (LvCheck) LvPorts.SelectedItems[0];
            Clipboard.SetText(selected.Address + " " + selected.Port + " " + selected.Type + " " + selected.Description);
        }

        private void BtnExportAs_Click(object sender, RoutedEventArgs e)
        {
            if (LvPorts.Items.Count == 0) return;

            SaveFileDialog sfd = new SaveFileDialog { Filter = "Text file (*.txt)|*.txt|HTML file (*.html)|*.html|CSV file (*.csv)|*.csv" };
            if (sfd.ShowDialog() != true) return;

            switch (sfd.FilterIndex)
            {
                default:
                    ExportWriter.SaveAsText(sfd.FileName, LvPorts);
                    break;
                case 2:
                    ExportWriter.SaveAsHTML(sfd.FileName, LvPorts);
                    break;
                case 3:
                    ExportWriter.SaveAsCSV(sfd.FileName, LvPorts);
                    break;
            }
        }

        private void BtnExportAsText_Click(object sender, RoutedEventArgs e)
        {
            if (LvPorts.Items.Count == 0) return;

            SaveFileDialog sfd = new SaveFileDialog { Filter = "Text file (*.txt)|*.txt" };
            if (sfd.ShowDialog() != true) return;
            ExportWriter.SaveAsText(sfd.FileName, LvPorts);
        }

        private void BtnExportAsHtml_Click(object sender, RoutedEventArgs e)
        {
            if (LvPorts.Items.Count == 0) return;

            SaveFileDialog sfd = new SaveFileDialog { Filter = "HTML file (*.html)|*.html" };
            if (sfd.ShowDialog() != true) return;
            ExportWriter.SaveAsHTML(sfd.FileName, LvPorts);
        }

        private void BtnExportAsCsv_Click(object sender, RoutedEventArgs e)
        {
            if (LvPorts.Items.Count == 0) return;

            SaveFileDialog sfd = new SaveFileDialog { Filter = "CSV file (*.csv)|*.csv" };
            if (sfd.ShowDialog() != true) return;
            ExportWriter.SaveAsCSV(sfd.FileName, LvPorts);
        }
    }
}
