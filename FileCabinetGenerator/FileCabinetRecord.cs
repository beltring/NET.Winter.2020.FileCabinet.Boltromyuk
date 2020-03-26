using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace FileCabinetGenerator
{
    /// <summary>This class describes the record entity.</summary>
    [XmlRoot("record")]
    public class FileCabinetRecord
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        /// <value>
        /// Integer number.
        /// </value>
        [XmlAttribute("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets full name.
        /// </summary>
        /// <value>
        /// Full name.
        /// </value>
        [XmlElement("name")]
        public FullName FullName { get; set; }

        /// <summary>
        /// Gets or sets DateOfBirth.
        /// </summary>
        /// <value>
        /// DateOfBirth.
        /// </value>
        [XmlElement("dateOfBirth", DataType = "date")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets Salary.
        /// </summary>
        /// <value>
        /// Salary.
        /// </value>
        [XmlElement("salary")]
        public short Salary { get; set; }

        /// <summary>
        /// Gets or sets WorkRate.
        /// </summary>
        /// <value>
        /// WorkRate.
        /// </value>
        [XmlElement("workRate")]
        public decimal WorkRate { get; set; }

        /// <summary>
        /// Gets or sets Gender.
        /// </summary>
        /// <value>
        /// Gender.
        /// </value>
        [XmlElement("gender", Type = typeof(char))]
        public char Gender { get; set; }

        /// <summary>Converts to string.</summary>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public override string ToString()
        {
            string result = $"{this.Id},{this.FullName.FirstName},{this.FullName.LastName}," +
                $"{this.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}," +
                $"{this.Salary},{this.WorkRate},{this.Gender}";

            return result;
        }
    }
}
