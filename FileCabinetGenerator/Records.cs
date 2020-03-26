using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace FileCabinetGenerator
{
    /// <summary>XMl export model.</summary>
    [XmlRoot("records")]
    public class Records
    {
        /// <summary>Initializes a new instance of the <see cref="Records"/> class.</summary>
        /// <param name="records">The records.</param>
        public Records(List<FileCabinetRecord> records)
        {
            this.FileCabinetRecords = records;
        }

        /// <summary>
        /// Gets the file cabinet records.
        /// </summary>
        /// <value>
        /// The file cabinet records.
        /// </value>
        [XmlElement("record")]
        public List<FileCabinetRecord> FileCabinetRecords { get; private set; }
    }
}
