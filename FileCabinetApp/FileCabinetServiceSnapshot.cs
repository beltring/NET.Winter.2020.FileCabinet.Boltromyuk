using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace FileCabinetApp
{
    /// <summary>Class FileCabinetServiceSnapshot.</summary>
    public class FileCabinetServiceSnapshot
    {
        private FileCabinetRecord[] records;

        /// <summary>Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.</summary>
        /// <param name="records">The records.</param>
        /// <exception cref="ArgumentNullException">Throw when records is null.</exception>
        public FileCabinetServiceSnapshot(FileCabinetRecord[] records)
        {
            if (records is null)
            {
                throw new ArgumentNullException($"{nameof(records)} can't be null.");
            }

            this.records = new FileCabinetRecord[records.Length];
            Array.Copy(records, this.records, records.Length);
        }

        /// <summary>Saves to CSV.</summary>
        /// <param name="streamWriter">The stream writer.</param>
        public void SaveToCsv(StreamWriter streamWriter)
        {
            var csvWriter = new FileCabinetRecordCsvWriter(streamWriter);

            foreach (var record in this.records)
            {
                csvWriter.Write(record);
            }
        }

        /// <summary>Saves to XML.</summary>
        /// <param name="xmlWriter">The XML writer.</param>
        public void SaveToXML(XmlWriter xmlWriter)
        {
            var xmlWrite = new FileCabinetRecordXmlWriter(xmlWriter);

            foreach (var record in this.records)
            {
                xmlWrite.Write(record);
            }
        }
    }
}
