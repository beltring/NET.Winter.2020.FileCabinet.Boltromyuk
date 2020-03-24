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
        /// The file cabinet records.
        /// </summary>
        [XmlElement("record")]
        public List<FileCabinetRecord> FileCabinetRecords { get; set; }
    }
}
