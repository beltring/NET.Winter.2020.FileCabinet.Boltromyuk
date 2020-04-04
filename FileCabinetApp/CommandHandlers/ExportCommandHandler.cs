using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>Export command handler.</summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.CommandHandlerBase" />
    internal class ExportCommandHandler : CommandHandlerBase
    {
        /// <summary>Handles the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <returns>Class AppCommandRequest Instance.</returns>
        public override AppCommandRequest Handle(AppCommandRequest request)
        {
            if (request.Command == "export")
            {
                Export(request.Parameters);
                return null;
            }

            return base.Handle(request);
        }

        private static void Export(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                string[] param = parameters.Split(' ');
                string path = param[1];
                if (param[0] == "csv")
                {
                    ExportToCsv(path);
                }

                if (param[0] == "xml")
                {
                    ExportToXml(path);
                }
            }
            else
            {
                throw new ArgumentException($"{nameof(parameters)} can't be empty.");
            }
        }

        private static void ExportToCsv(string path)
        {
            if (IsExists(path))
            {
                try
                {
                    using StreamWriter sw = new StreamWriter(path);
                    var snapshot = Program.FileCabinetService.MakeSnapshot();
                    snapshot.SaveToCsv(sw);
                    Console.WriteLine($"All records are exported to file {path}.");
                }
                catch (IOException)
                {
                    Console.WriteLine($"Export failed: can't open file {path}.");
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Add at least one record.");
                }
            }
            else
            {
                Console.WriteLine($"File {path} didn't found.");
            }
        }

        private static void ExportToXml(string path)
        {
            if (IsExists(path))
            {
                try
                {
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.WriteEndDocumentOnClose = true;

                    using XmlWriter xmlWriter = XmlWriter.Create(path, settings);
                    var snapshot = Program.FileCabinetService.MakeSnapshot();
                    snapshot.SaveToXML(xmlWriter);
                    Console.WriteLine($"All records are exported to file {path}.");
                }
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine($"Export failed: can't open file {path}.");
                }
            }
            else
            {
                Console.WriteLine($"File {path} didn't found.");
            }
        }

        private static bool IsExists(string path)
        {
            if (File.Exists(path))
            {
                Console.WriteLine($"File is exist - rewrite {path}?[Y / n]");
                string result = Console.ReadLine();
                return result == "Y" ? true : false;
            }

            return true;
        }
    }
}
