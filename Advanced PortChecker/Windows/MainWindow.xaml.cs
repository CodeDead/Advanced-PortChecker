using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shell;
using Advanced_PortChecker.Classes.Export;
using Advanced_PortChecker.Classes.GUI;
using Advanced_PortChecker.Classes.Objects;
using Advanced_PortChecker.Classes.Scanner;
using Advanced_PortChecker.Classes.Utils;
using CodeDead.UpdateManager.Classes;
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
        private readonly UpdateManager _updateManager;
        /// <summary>
        /// The list of ScanOperation objects
        /// </summary>
        private List<ScanOperation> _operations = new List<ScanOperation>();
        /// <summary>
        /// The number of threads that have completed their scan task
        /// </summary>
        private int _completedThreads;
        #endregion

        /// <inheritdoc />
        /// <summary>
        /// Initialize a new MainWindow object
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            StringVariables stringVariables = new StringVariables
            {
                CancelButtonText = "Cancel",
                DownloadButtonText = "Download",
                InformationButtonText = "Information",
                NoNewVersionText = "You are running the latest version!",
                TitleText = "Advanced PortChecker",
                UpdateNowText = "Would you like to update the application now?"
            };
            _updateManager = new UpdateManager(Assembly.GetExecutingAssembly().GetName().Version, "https://codedead.com/Software/Advanced%20PortChecker/update.xml", stringVariables);

            WindowDraggable();
            // Change the theme
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
        /// Check whether the Window should be draggable or not
        /// </summary>
        internal void WindowDraggable()
        {
            try
            {
                if (Properties.Settings.Default.WindowDraggable)
                {
                    // Delete event handler first to prevent duplicate handlers
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
        /// Method that is called when the About MenuItem is clicked
        /// </summary>
        /// <param name="sender">The object that called this method</param>
        /// <param name="e">The RoutedEventArgs</param>
        private void About_Click(object sender, RoutedEventArgs e)
        {
            new AboutWindow().ShowDialog();
        }

        /// <summary>
        /// Method that is called when the Settings MenuItem is clicked
        /// </summary>
        /// <param name="sender">The object that called this method</param>
        /// <param name="e">The RoutedEventArgs</param>
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            new SettingsWindow(this).ShowDialog();
        }

        /// <summary>
        /// Method that is called when the Update MenuItem is clicked
        /// </summary>
        /// <param name="sender">The object that called this method</param>
        /// <param name="e">The RoutedEventArgs</param>
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

        /// <summary>
        /// Method that is called when Scan Button is clicked
        /// </summary>
        /// <param name="sender">The object that called this method</param>
        /// <param name="e">The RoutedEventArgs</param>
        private void BtnScan_Click(object sender, RoutedEventArgs e)
        {
            if (IntStart.Value == null || IntStop.Value == null) return;
            if (IntStart.Value > IntStop.Value)
            {
                MessageBox.Show("The starting port cannot be greater than the ending port!", "Advanced PortChecker", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_operations != null && _operations.Count > 0)
            {
                MessageBox.Show("A previous scan is still running or in the process of being cancelled!", "Advanced PortChecker", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            LvPorts.Items.Clear();
            ControlsEnabled(false);

            PgbStatus.Value = 0;
            TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
            TaskbarItemInfo.ProgressValue = 0;

            _completedThreads = 0;

            PgbStatus.Minimum = (double)IntStart.Value - 1;
            PgbStatus.Maximum = (double)IntStop.Value;

            int totalTasks = Properties.Settings.Default.ScanThreads;
            int timeout = Properties.Settings.Default.TimeOut;

            int scanAmount = ((int)IntStop.Value - (int)IntStart.Value) + 1;
            int startPort = (int)IntStart.Value;

            TxtAddress.Text = TxtAddress.Text.Replace("https://", "");
            TxtAddress.Text = TxtAddress.Text.Replace("http://", "");
            TxtAddress.Text = TxtAddress.Text.Replace("ftp://", "");
            TxtAddress.Text = TxtAddress.Text.Replace("sftp://", "");

            string ipAddress = TxtAddress.Text;

            List<Action> functions = new List<Action>();

            foreach (int i in ThreadCalculator.GetActionsPerThreads(totalTasks, scanAmount))
            {
                int endPort = startPort + i - 1;
                int startPortCopy = startPort;
                ScanOperation scan = new ScanOperation
                {
                    Progress = new Progress<int>(value =>
                    {
                        PgbStatus.Value += 1;
                        TaskbarItemInfo.ProgressValue += (1 / (PgbStatus.Maximum - PgbStatus.Minimum));
                    }),
                    ItemProgress = new Progress<LvCheck>(value => LvPorts.Items.Add(value)),
                    IsCancelled = false
                };
                scan.ScanCompletedEvent += ScanThreadCompleted;

                _operations.Add(scan);

                switch (CbaMethod.Text)
                {
                    default:
                        functions.Add(() => PortScanner.CheckTCP(ipAddress, startPortCopy, endPort, timeout, scan, true));
                        break;
                    case "UDP":
                        functions.Add(() => PortScanner.CheckUDP(ipAddress, startPortCopy, endPort, timeout, scan, true));
                        break;
                    case "Both":
                        functions.Add(() => PortScanner.CheckTCPUDP(ipAddress, startPortCopy, endPort, timeout, scan));
                        break;
                }
                startPort = endPort + 1;
            }

            foreach (Action action in functions)
            {
                Thread thread = new Thread(new ThreadStart(action)) {IsBackground = true};
                thread.Start();
            }
        }

        /// <summary>
        /// Method that is called when the Exit MenuItem is clicked
        /// </summary>
        /// <param name="sender">The object that called this method</param>
        /// <param name="e">The RoutedEventArgs</param>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Method that is called when a ScanOperation has finished
        /// </summary>
        private void ScanThreadCompleted()
        {
            Dispatcher.Invoke(() =>
            {
                _completedThreads++;
                if (_completedThreads != _operations.Count) return;
                _operations = new List<ScanOperation>();

                ControlsEnabled(true);

                TaskbarItemInfo.ProgressState = TaskbarItemProgressState.None;
                TaskbarItemInfo.ProgressValue = 0;
            });
        }

        /// <summary>
        /// Method that is called when the Donate MenuItem is clicked
        /// </summary>
        /// <param name="sender">The object that called this method</param>
        /// <param name="e">The RoutedEventArgs</param>
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

        /// <summary>
        /// Method that is called when the scan operation should be canceled
        /// </summary>
        /// <param name="sender">The object that called this method</param>
        /// <param name="e">The RoutedEventArgs</param>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (_operations == null || _operations.Count == 0) return;

            foreach (ScanOperation scan in _operations)
            {
                scan.IsCancelled = true;
            }

            ControlsEnabled(true);

            TaskbarItemInfo.ProgressState = TaskbarItemProgressState.None;
            TaskbarItemInfo.ProgressValue = 0;
        }

        /// <summary>
        /// Method that is called when the License MenuItem is clicked
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
        /// Method that is called when the CodeDead MenuItem is clicked
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

        /// <summary>
        /// Method that is called when the Help MenuItem is clicked
        /// </summary>
        /// <param name="sender">The object that called this method</param>
        /// <param name="e">The RoutedEventArgs</param>
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

        /// <summary>
        /// Method that is called when the ListView should be cleared
        /// </summary>
        /// <param name="sender">The object that called this method</param>
        /// <param name="e">The RoutedEventArgs</param>
        private void BtnDeleteAllItems_Click(object sender, RoutedEventArgs e)
        {
            LvPorts.Items.Clear();
        }

        /// <summary>
        /// Method that is called when a ListViewItem should be removed
        /// </summary>
        /// <param name="sender">The object that called this method</param>
        /// <param name="e">The RoutedEventArgs</param>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            List<LvCheck> delete = LvPorts.SelectedItems.Cast<LvCheck>().ToList();
            foreach (LvCheck lv in delete)
            {
                LvPorts.Items.Remove(lv);
            }
        }

        /// <summary>
        /// Method that is called when a ListViewItem should be copied to the clipboard
        /// </summary>
        /// <param name="sender">The object that called this method</param>
        /// <param name="e">The sender</param>
        private void BtnCopy_Click(object sender, RoutedEventArgs e)
        {
            if (LvPorts.SelectedItems.Count == 0) return;

            List<LvCheck> selected = LvPorts.SelectedItems.Cast<LvCheck>().ToList();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < selected.Count; i++)
            {
                if (i != selected.Count - 1)
                {
                    sb.AppendLine(selected[i].Address + "\t" + selected[i].Port + "\t" + selected[i].HostName + "\t" + selected[i].Type + "\t" + selected[i].Description + "\t" + selected[i].ScanDate);
                }
                else
                {
                    sb.Append(selected[i].Address + "\t" + selected[i].Port + "\t" + selected[i].HostName + "\t" + selected[i].Type + "\t" + selected[i].Description + "\t" + selected[i].ScanDate);
                }
            }

            Clipboard.SetText(sb.ToString());
        }

        /// <summary>
        /// Method that is called when a scan result should be exported
        /// </summary>
        /// <param name="sender">The object that called this method</param>
        /// <param name="e">The RoutedEventArgs</param>
        private void BtnExportAs_Click(object sender, RoutedEventArgs e)
        {
            ExportData(1);
        }

        /// <summary>
        /// Method that is called when a scan result should be exported as HTML
        /// </summary>
        /// <param name="sender">The object that called this method</param>
        /// <param name="e">The RoutedEventArgs</param>
        private void BtnExportAsHtml_Click(object sender, RoutedEventArgs e)
        {
            ExportData(2);
        }

        /// <summary>
        /// Method that is called when a scan result should be exported as CSV
        /// </summary>
        /// <param name="sender">The object that called this method</param>
        /// <param name="e">The RoutedEventArgs</param>
        private void BtnExportAsCsv_Click(object sender, RoutedEventArgs e)
        {
            ExportData(3);
        }

        /// <summary>
        /// Method that is called when a scan result should be exported as CSV (Excel)
        /// </summary>
        /// <param name="sender">The object that called this method</param>
        /// <param name="e">The RoutedEventArgs</param>
        private void BtnExportAsCsvExcel_Click(object sender, RoutedEventArgs e)
        {
            ExportData(4);
        }

        /// <summary>
        /// Export a scan result to the FileSystem
        /// </summary>
        /// <param name="filterIndex">The filter index that was chosen by the user in the SaveFileDialog</param>
        private void ExportData(int filterIndex)
        {
            if (LvPorts.Items.Count == 0) return;

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Text (*.txt)|*.txt|HTML (*.html)|*.html|CSV (*.csv)|*.csv|Excel (*.csv)|*.csv",
                FilterIndex = filterIndex
            };

            if (sfd.ShowDialog() != true) return;

            ExportType type;
            switch (sfd.FilterIndex)
            {
                default:
                    type = ExportType.Text;
                    break;
                case 2:
                    type = ExportType.Html;
                    break;
                case 3:
                    type = ExportType.Csv;
                    break;
                case 4:
                    type = ExportType.Excel;
                    break;
            }

            try
            {
                ExportWriter.Export(sfd.FileName, LvPorts, type);
                MessageBox.Show("Successfully exported all items!", "Advanced PortChecker", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Advanced PortChecker", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
