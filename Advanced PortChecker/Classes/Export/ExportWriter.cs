using System;
using System.IO;
using System.Text;
using System.Windows.Controls;
using Advanced_PortChecker.Classes.Objects;

namespace Advanced_PortChecker.Classes.Export
{
    /// <summary>
    /// A static helper class to export items to the FileSystem
    /// </summary>
    internal static class ExportWriter
    {
        /// <summary>
        /// Export a scan result to the FileSystem
        /// </summary>
        /// <param name="path">The path where the scan result should be saved</param>
        /// <param name="lvPorts">The ListView control containing all the LvCheck items</param>
        /// <param name="exportType">The type of export that needs to be performed</param>
        internal static void Export(string path, ItemsControl lvPorts, ExportType exportType)
        {
            if (lvPorts.Items.Count == 0) return;
            if (string.IsNullOrEmpty(path)) return;

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (exportType)
            {
                case ExportType.Text:
                    SaveAsText(path, lvPorts);
                    break;
                case ExportType.Html:
                    SaveAsHTML(path, lvPorts);
                    break;
                case ExportType.Csv:
                    SaveAsCSV(path, lvPorts, ",");
                    break;
                case ExportType.Excel:
                    SaveAsCSV(path, lvPorts, ";");
                    break;
            }
        }

        /// <summary>
        /// Export the ListView items in plain text format to the FileSystem
        /// </summary>
        /// <param name="path">The path where the scan result should be saved</param>
        /// <param name="lvPorts">The ListView control containing all the LvCheck items</param>
        private static void SaveAsText(string path, ItemsControl lvPorts)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Advanced PortChecker - " + DateTime.Now);
            for (int i = 0; i < lvPorts.Items.Count; i++)
            {
                LvCheck l = (LvCheck)lvPorts.Items[i];
                if (i == lvPorts.Items.Count - 1)
                {
                    sb.Append(l.Address + "\t" + l.Port + "\t" + l.HostName + "\t" + l.Type + "\t" + l.Description + "\t" + l.ScanDate);
                }
                else
                {
                    sb.AppendLine(l.Address + "\t" + l.Port + "\t" + l.HostName + "\t" + l.Type + "\t" + l.Description + "\t" + l.ScanDate);
                }
            }
            Write(path, sb.ToString());
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Export the ListView items in HTML format to the drive
        /// </summary>
        /// <param name="path">The path where the scan result should be saved</param>
        /// <param name="lvPorts">The ListView control containing all the LvCheck items</param>
        private static void SaveAsHTML(string path, ItemsControl lvPorts)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<HTML>");
            sb.AppendLine("<head>");
            sb.AppendLine("<title>Advanced PortChecker - " + DateTime.Now + "</title>");
            sb.AppendLine("</head>");

            sb.AppendLine("<body>");

            sb.AppendLine("<h1>Export list</h1>");
            sb.AppendLine("<table border='1'>");
            sb.AppendLine("<tr><th>Address</th><th>Port</th><th>Host name</th><th>Type</th><th>Description</th><th>Scan date</th></tr>");
            foreach (LvCheck l in lvPorts.Items)
            {
                sb.AppendLine("<tr><td>" + l.Address + "</td><td>" + l.Port + "</td><td>" + l.HostName + "</td><td>" + l.Type + "</td><td>" + l.Description +"</td><td>" + l.ScanDate + "</td></tr>");
            }
            sb.AppendLine("</table>");

            sb.AppendLine("</body>");
            sb.Append("</HTML>");
            Write(path, sb.ToString());
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Export the ListView items in CSV format to the drive
        /// </summary>
        /// <param name="path">The path where the scan result should be saved</param>
        /// <param name="lvPorts">The ListView control containing all the LvCheck items</param>
        /// <param name="delimiter">The delimiter that can be used to separate items</param>
        private static void SaveAsCSV(string path, ItemsControl lvPorts, string delimiter)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Advanced PortChecker" + delimiter + DateTime.Now);
            for (int i = 0; i < lvPorts.Items.Count; i++)
            {
                LvCheck l = (LvCheck)lvPorts.Items[i];
                if (i == lvPorts.Items.Count - 1)
                {
                    sb.Append(l.Address + delimiter + l.Port + delimiter + l.HostName + delimiter + l.Type + delimiter + l.Description + delimiter + l.ScanDate);
                }
                else
                {
                    sb.AppendLine(l.Address + delimiter + l.Port + delimiter + l.HostName + delimiter + l.Type + delimiter + l.Description + delimiter + l.ScanDate);
                }
            }
            Write(path, sb.ToString());
        }

        /// <summary>
        /// Write data to the FileSystem
        /// </summary>
        /// <param name="path">The path where the data should be stored</param>
        /// <param name="data">The data that should be written to the FileSystem</param>
        private static void Write(string path, string data)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(data);
            }
        }
    }
}
