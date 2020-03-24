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
        /// <summary>
        /// Gets the file cabinet records.
        /// </summary>
        /// <value>
        /// The file cabinet records.
        /// </value>
        [XmlElement("record")]
        public List<FileCabinetRecord> FileCabinetRecords { get; }
    }
}
