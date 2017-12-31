using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Advanced_PortChecker.Classes
{
    /// <summary>
    /// A static helper class to export items to the disk
    /// </summary>
    internal static class ExportWriter
    {
        /// <summary>
        /// Export the listview items in plain text format to the drive
        /// </summary>
        /// <param name="path">The path where the export list should be saved</param>
        /// <param name="lvPorts">The listview control containing all the LvCheck items</param>
        internal static void SaveAsText(string path, ItemsControl lvPorts)
        {
            if (lvPorts.Items.Count == 0) return;

            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.WriteLine("Advanced PortChecker - " + DateTime.Now);
                    for (int i = 0; i < lvPorts.Items.Count; i++)
                    {
                        LvCheck l = (LvCheck)lvPorts.Items[i];
                        if (i == lvPorts.Items.Count - 1)
                        {
                            sw.Write(l.Address + "\t" + l.Port + "\t" + l.Type + "\t" + l.Description);
                        }
                        else
                        {
                            sw.WriteLine(l.Address + "\t" + l.Port + "\t" + l.Type + "\t" + l.Description);
                        }
                    }
                }

                MessageBox.Show("Successfully exported all items!", "Advanced PortChecker", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Advanced PortChecker", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Export the listview items in HTML format to the drive
        /// </summary>
        /// <param name="path">The path where the export list should be saved</param>
        /// <param name="lvPorts">The listview control containing all the LvCheck items</param>
        internal static void SaveAsHTML(string path, ItemsControl lvPorts)
        {
            if (lvPorts.Items.Count == 0) return;

            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.WriteLine("<HTML>");
                    sw.WriteLine("<head>");
                    sw.WriteLine("<title>Advanced PortChecker - " + DateTime.Now + "</title>");
                    sw.WriteLine("</head>");

                    sw.WriteLine("<body>");

                    sw.WriteLine("<h1>Export list</h1>");
                    sw.WriteLine("<table border='1'>");
                    sw.WriteLine("<tr><th>Address</th><th>Port</th><th>Type</th><th>Description</th></tr>");
                    foreach (LvCheck l in lvPorts.Items)
                    {
                        sw.WriteLine("<tr><td>" + l.Address + "</td><td>" + l.Port + "</td><td>" + l.Type + "</td><td>" + l.Description + "</td></tr>");
                    }
                    sw.WriteLine("</table>");

                    sw.WriteLine("</body>");
                    sw.Write("</HTML>");
                }

                MessageBox.Show("Successfully exported all items!", "Advanced PortChecker", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Advanced PortChecker", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Export the listview items in CSV format to the drive
        /// </summary>
        /// <param name="path">The path where the export list should be saved</param>
        /// <param name="lvPorts">The listview control containing all the LvCheck items</param>
        internal static void SaveAsCSV(string path, ItemsControl lvPorts)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.WriteLine("Advanced PortChecker;" + DateTime.Now);
                    for (int i = 0; i < lvPorts.Items.Count; i++)
                    {
                        LvCheck l = (LvCheck)lvPorts.Items[i];
                        if (i == lvPorts.Items.Count - 1)
                        {
                            sw.Write(l.Address + ";" + l.Port + ";" + l.Type + ";" + l.Description);
                        }
                        else
                        {
                            sw.WriteLine(l.Address + ";" + l.Port + ";" + l.Type + ";" + l.Description);
                        }
                    }
                }

                MessageBox.Show("Successfully exported all items!", "Advanced PortChecker", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Advanced PortChecker", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
