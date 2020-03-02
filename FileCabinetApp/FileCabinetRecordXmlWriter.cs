using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;

namespace FileCabinetApp
{
    /// <summary>class FileCabinetRecordXmlWriter.</summary>
    public class FileCabinetRecordXmlWriter
    {
        private readonly XmlWriter writer;
        private readonly CultureInfo cultureInfo;

        /// <summary>Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.</summary>
        /// <param name="writer">The writer.</param>
        /// <exception cref="ArgumentNullException">Throw when writer is null.</exception>
        public FileCabinetRecordXmlWriter(XmlWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException($"{nameof(writer)} can't be null.");
            }

            this.cultureInfo = CultureInfo.CreateSpecificCulture("es-eS");
            this.cultureInfo.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";

            this.writer = writer;
            writer.WriteStartDocument();
            writer.WriteStartElement("records");
        }

        /// <summary>Writes the specified record.</summary>
        /// <param name="record">The record.</param>
        /// <exception cref="ArgumentNullException">Throw when writer is null.</exception>
        public void Write(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException($"{nameof(record)} can't be null");
            }

            this.writer.WriteStartElement("record");
            this.writer.WriteAttributeString("id", record.Id.ToString(this.cultureInfo));

            this.writer.WriteStartElement("name");
            this.writer.WriteAttributeString("last", record.LastName);
            this.writer.WriteAttributeString("first", record.FirstName);
            this.writer.WriteEndElement();

            this.writer.WriteStartElement("dateOfBirth");
            this.writer.WriteString(record.DateOfBirth.ToString("d", this.cultureInfo));
            this.writer.WriteEndElement();

            this.writer.WriteStartElement("salary");
            this.writer.WriteString(record.Salary.ToString(this.cultureInfo));
            this.writer.WriteEndElement();

            this.writer.WriteStartElement("workRate");
            this.writer.WriteString(record.WorkRate.ToString(this.cultureInfo));
            this.writer.WriteEndElement();

            this.writer.WriteStartElement("gender");
            this.writer.WriteString(record.Gender.ToString(this.cultureInfo));
            this.writer.WriteEndElement();

            this.writer.WriteEndElement();
        }
    }
}
