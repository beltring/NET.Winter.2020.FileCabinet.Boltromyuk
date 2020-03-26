using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FileCabinetGenerator;

namespace FileCabinetApp
{
    /// <summary>Xml reader.</summary>
    internal class FileCabinetRecordXmlReader
    {
        /// <summary>The reader.</summary>
        private readonly StreamReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlReader"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <exception cref="ArgumentNullException">throw when reader is null.</exception>
        public FileCabinetRecordXmlReader(StreamReader reader)
        {
            this.reader = reader ?? throw new ArgumentNullException($"{nameof(reader)} can't be null.");
        }

        /// <summary>
        /// Reads all.
        /// </summary>
        /// <returns>List of records.</returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            var list = new List<FileCabinetRecord>();

            XmlSerializer serializer = new XmlSerializer(typeof(Records));
            using XmlReader xmlReader = XmlReader.Create(this.reader);

            var recordsOrder = (Records)serializer.Deserialize(xmlReader);

            foreach (var record in recordsOrder.FileCabinetRecords)
            {
                list.Add(new FileCabinetRecord
                {
                    Id = record.Id,
                    FirstName = record.FullName.FirstName,
                    LastName = record.FullName.LastName,
                    DateOfBirth = record.DateOfBirth,
                    Salary = record.Salary,
                    WorkRate = record.WorkRate,
                    Gender = record.Gender,
                });
            }

            return list;
        }
    }
}
