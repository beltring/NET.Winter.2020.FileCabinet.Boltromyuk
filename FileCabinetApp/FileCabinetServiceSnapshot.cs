using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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

        /// <summary>Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.</summary>
        public FileCabinetServiceSnapshot()
        {
            this.records = Array.Empty<FileCabinetRecord>();
        }

        /// <summary>Gets the file cabinet records.</summary>
        /// <value>The file cabinet records.</value>
        public ReadOnlyCollection<FileCabinetRecord> FileCabinetRecords => new ReadOnlyCollection<FileCabinetRecord>(this.records);

        /// <summary>Saves to CSV.</summary>
        /// <param name="streamWriter">The stream writer.</param>
        internal void SaveToCsv(StreamWriter streamWriter)
        {
            if (streamWriter is null)
            {
                throw new ArgumentNullException($"{nameof(streamWriter)} can't be null.");
            }

            var csvWriter = new FileCabinetRecordCsvWriter(streamWriter);

            foreach (var record in this.records)
            {
                csvWriter.Write(record);
            }
        }

        /// <summary>Saves to XML.</summary>
        /// <param name="xmlWriter">The XML writer.</param>
        internal void SaveToXML(XmlWriter xmlWriter)
        {
            if (xmlWriter is null)
            {
                throw new ArgumentNullException($"{nameof(xmlWriter)} can't be null.");
            }

            var xmlWrite = new FileCabinetRecordXmlWriter(xmlWriter);

            foreach (var record in this.records)
            {
                xmlWrite.Write(record);
            }
        }

        /// <summary>Loads from CSV.</summary>
        /// <param name="reader">The reader.</param>
        /// <param name="countRecords">Count of records.</param>
        /// <exception cref="ArgumentNullException">throw when reader is null.</exception>
        internal void LoadFromCSV(StreamReader reader, out int countRecords)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var csvReader = new FileCabinetRecordCsvReader(reader);
            var recordsFromFile = csvReader.ReadAll();
            countRecords = recordsFromFile.Count;

            this.records = recordsFromFile.ToArray();
        }

        /// <summary>Loads from XML.</summary>
        /// <param name="reader">The reader.</param>
        /// <param name="countRecords">The count records.</param>
        /// <exception cref="ArgumentNullException">throw when reader is null.</exception>
        internal void LoadFromXML(StreamReader reader, out int countRecords)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var xmlReader = new FileCabinetRecordXmlReader(reader);
            var recordsFromFile = xmlReader.ReadAll();

            countRecords = recordsFromFile.Count;

            this.records = recordsFromFile.ToArray();
        }
    }
}
