using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp
{
    /// <summary>This class provides functionality for working with record and the file system.</summary>
    /// <seealso cref="FileCabinetApp.Interfaces.IFileCabinetService" />
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private const int RecordLength // 278
            = sizeof(short) // status
            + sizeof(int) // id
            + (NameLength * 2) // first and last name
            + (sizeof(int) * 3) // dateofbirth
            + sizeof(short) // salary
            + sizeof(decimal) // workRate
            + sizeof(char); // gender

        private const int NameLength = 120;
        private const int StatusLength = sizeof(short);
        private const int FirstNamePosition = StatusLength + sizeof(int);
        private const int LastNamePosition = FirstNamePosition + NameLength;
        private const int DayOfBirthPosition = LastNamePosition + NameLength;
        private const int DateLength = sizeof(int) * 3;

        private readonly Encoding encoding = Encoding.Unicode;
        private readonly FileStream fileStream;
        private readonly IRecordValidator validator;

        /// <summary>Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.</summary>
        /// <param name="validator">The validator.</param>
        /// <param name="fileStream">The file stream.</param>
        public FileCabinetFilesystemService(IRecordValidator validator, FileStream fileStream)
        {
            this.validator = validator ?? throw new ArgumentNullException($"{nameof(validator)} can't be null.");
            this.fileStream = fileStream ?? throw new ArgumentNullException($"{nameof(fileStream)} can't be null.");
        }

        /// <summary>This method creates a record.</summary>
        /// <param name="parameters">Parameters.</param>
        /// <returns>Id new record.</returns>
        public int CreateRecord(RecordEventArgs parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException($"{nameof(parameters)} can't be null.");
            }

            this.validator.ValidateParameters(parameters);

            var record = new FileCabinetRecord
            {
                Id = this.fileStream.Position != 0 ? (int)(this.fileStream.Position / RecordLength) + 1 : 1,
                FirstName = parameters.FirstName,
                LastName = parameters.LastName,
                DateOfBirth = parameters.DateOfBirth,
                Salary = parameters.Salary,
                WorkRate = parameters.WorkRate,
                Gender = parameters.Gender,
            };

            this.WriteToBinaryFile(record);

            return record.Id;
        }

        /// <summary>This method changes the record.</summary>
        /// <param name="id">Id.</param>
        /// <param name="parameters">Parameters.</param>
        public void EditRecord(int id, RecordEventArgs parameters)
        {
            if (id < 0)
            {
                throw new ArgumentException($"{nameof(id)} can't be less than zero.");
            }

            if (parameters is null)
            {
                throw new ArgumentNullException($"{nameof(parameters)} can't be null.");
            }

            int index = this.CheckId(id);
            if (index == -1)
            {
                throw new ArgumentException($"The {nameof(id)} doesn't exist.");
            }

            this.validator.ValidateParameters(parameters);

            this.fileStream.Position = (RecordLength * (index - 1)) + 2 + BitConverter.GetBytes(default(int)).Length;
            using BinaryWriter binaryWriter = new BinaryWriter(this.fileStream, this.encoding, true);

            binaryWriter.Write(this.encoding.GetBytes(parameters.FirstName.PadRight(60)));
            binaryWriter.Write(this.encoding.GetBytes(parameters.LastName.PadRight(60)));
            binaryWriter.Write(parameters.DateOfBirth.Year);
            binaryWriter.Write(parameters.DateOfBirth.Month);
            binaryWriter.Write(parameters.DateOfBirth.Day);
            binaryWriter.Write(parameters.Salary);
            binaryWriter.Write(parameters.WorkRate);
            binaryWriter.Write(parameters.Gender);
        }

        /// <summary>This method searches for a record by date of birthday.</summary>
        /// <param name="dateOfBirth">Date of birthday.</param>
        /// <returns>Array of the records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            using BinaryReader binaryReader = new BinaryReader(this.fileStream, Encoding.Unicode, true);
            var dateList = new List<FileCabinetRecord>();
            int count = (int)(this.fileStream.Length / RecordLength);
            this.fileStream.Position = DayOfBirthPosition;
            DateTime dateFromFile;
            while (count-- > 0)
            {
                dateFromFile = new DateTime(binaryReader.ReadInt32(), binaryReader.ReadInt32(), binaryReader.ReadInt32());
                if (DateTime.Compare(dateFromFile, dateOfBirth) == 0)
                {
                    this.fileStream.Position -= DayOfBirthPosition + DateLength;
                    binaryReader.ReadBytes(StatusLength);
                    dateList.Add(new FileCabinetRecord
                    {
                        Id = binaryReader.ReadInt32(),
                        FirstName = this.encoding.GetString(binaryReader.ReadBytes(120), 0, 120).Trim(),
                        LastName = this.encoding.GetString(binaryReader.ReadBytes(120), 0, 120).Trim(),
                        DateOfBirth = new DateTime(binaryReader.ReadInt32(), binaryReader.ReadInt32(), binaryReader.ReadInt32()),
                        Salary = binaryReader.ReadInt16(),
                        WorkRate = ToDecimal(binaryReader.ReadBytes(16)),
                        Gender = binaryReader.ReadChar(),
                    });

                    this.fileStream.Position += DayOfBirthPosition;
                }
                else
                {
                    this.fileStream.Position += RecordLength - DateLength;
                }
            }

            var dateCollection = new ReadOnlyCollection<FileCabinetRecord>(dateList);
            return dateCollection;
        }

        /// <summary>This method searches for a record by first name.</summary>
        /// <param name="firstName">First name.</param>
        /// <returns>Array of the records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (firstName is null)
            {
                throw new ArgumentNullException($"{nameof(firstName)} can't be null.");
            }

            using BinaryReader binaryReader = new BinaryReader(this.fileStream, this.encoding, true);
            var dateList = new List<FileCabinetRecord>();
            int count = (int)(this.fileStream.Length / RecordLength);
            this.fileStream.Position = FirstNamePosition;
            string firstNameFromFile;

            while (count-- > 0)
            {
                firstNameFromFile = this.encoding.GetString(binaryReader.ReadBytes(120), 0, 120).Trim();
                if (firstNameFromFile.Equals(firstName, StringComparison.OrdinalIgnoreCase))
                {
                    this.fileStream.Position -= FirstNamePosition + NameLength;
                    binaryReader.ReadBytes(StatusLength);
                    dateList.Add(new FileCabinetRecord
                    {
                        Id = binaryReader.ReadInt32(),
                        FirstName = this.encoding.GetString(binaryReader.ReadBytes(120), 0, 120).Trim(),
                        LastName = this.encoding.GetString(binaryReader.ReadBytes(120), 0, 120).Trim(),
                        DateOfBirth = new DateTime(binaryReader.ReadInt32(), binaryReader.ReadInt32(), binaryReader.ReadInt32()),
                        Salary = binaryReader.ReadInt16(),
                        WorkRate = ToDecimal(binaryReader.ReadBytes(16)),
                        Gender = binaryReader.ReadChar(),
                    });
                    this.fileStream.Position += FirstNamePosition;
                }
                else
                {
                    this.fileStream.Position += RecordLength - NameLength;
                }
            }

            var dateCollection = new ReadOnlyCollection<FileCabinetRecord>(dateList);
            return dateCollection;
        }

        /// <summary>This method searches for a record by last name.</summary>
        /// <param name="lastName">last name.</param>
        /// <returns>Array of the records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (lastName is null)
            {
                throw new ArgumentNullException($"{nameof(lastName)} can't be null.");
            }

            using BinaryReader binaryReader = new BinaryReader(this.fileStream, Encoding.Unicode, true);
            var dateList = new List<FileCabinetRecord>();
            int count = (int)(this.fileStream.Length / RecordLength);
            this.fileStream.Position = LastNamePosition;
            string lastNameFromFile;

            while (count-- > 0)
            {
                lastNameFromFile = this.encoding.GetString(binaryReader.ReadBytes(120), 0, 120).Trim();
                if (lastNameFromFile.Equals(lastName, StringComparison.OrdinalIgnoreCase))
                {
                    this.fileStream.Position -= LastNamePosition + NameLength;
                    binaryReader.ReadBytes(StatusLength);
                    dateList.Add(new FileCabinetRecord
                    {
                        Id = binaryReader.ReadInt32(),
                        FirstName = this.encoding.GetString(binaryReader.ReadBytes(120), 0, 120).Trim(),
                        LastName = this.encoding.GetString(binaryReader.ReadBytes(120), 0, 120).Trim(),
                        DateOfBirth = new DateTime(binaryReader.ReadInt32(), binaryReader.ReadInt32(), binaryReader.ReadInt32()),
                        Salary = binaryReader.ReadInt16(),
                        WorkRate = ToDecimal(binaryReader.ReadBytes(16)),
                        Gender = binaryReader.ReadChar(),
                    });

                    this.fileStream.Position += LastNamePosition;
                }
                else
                {
                    this.fileStream.Position += RecordLength - NameLength;
                }
            }

            var dateCollection = new ReadOnlyCollection<FileCabinetRecord>(dateList);
            return dateCollection;
        }

        /// <summary>This method return records.</summary>
        /// <returns>Array of the records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            this.fileStream.Position = 0;
            using BinaryReader binaryReader = new BinaryReader(this.fileStream, this.encoding, true);
            long count = this.fileStream.Length / RecordLength;
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();

            while (count-- > 0)
            {
                binaryReader.ReadBytes(StatusLength);
                records.Add(new FileCabinetRecord
                {
                    Id = binaryReader.ReadInt32(),
                    FirstName = this.encoding.GetString(binaryReader.ReadBytes(120), 0, 120).Trim(),
                    LastName = this.encoding.GetString(binaryReader.ReadBytes(120), 0, 120).Trim(),
                    DateOfBirth = new DateTime(binaryReader.ReadInt32(), binaryReader.ReadInt32(), binaryReader.ReadInt32()),
                    Salary = binaryReader.ReadInt16(),
                    WorkRate = ToDecimal(binaryReader.ReadBytes(16)),
                    Gender = binaryReader.ReadChar(),
                });
            }

            ReadOnlyCollection<FileCabinetRecord> result = new ReadOnlyCollection<FileCabinetRecord>(records);
            return result;
        }

        /// <summary>This method returns count of records.</summary>
        /// <returns>Count records.</returns>
        public int GetStat()
        {
            return (int)(this.fileStream.Position / RecordLength);
        }

        /// <summary>Makes the snapshot.</summary>
        /// <returns>Snapshot.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
        }

        private static decimal ToDecimal(byte[] bytes)
        {
            if (bytes.Length != 16)
            {
                throw new ArgumentException("A decimal must be created from exactly 16 bytes");
            }

            int[] bits = new int[4];
            for (int i = 0; i <= 15; i += 4)
            {
                bits[i / 4] = BitConverter.ToInt32(bytes, i);
            }

            return new decimal(bits);
        }

        private void WriteToBinaryFile(FileCabinetRecord record)
        {
            using (BinaryWriter binaryWriter = new BinaryWriter(this.fileStream, this.encoding, true))
            {
                short status = 0;

                binaryWriter.Write(status); // 0 - not deleted, 1 - deleted
                binaryWriter.Write(record.Id);
                binaryWriter.Write(this.encoding.GetBytes(record.FirstName.PadRight(60)));
                binaryWriter.Write(this.encoding.GetBytes(record.LastName.PadRight(60)));
                binaryWriter.Write(record.DateOfBirth.Year);
                binaryWriter.Write(record.DateOfBirth.Month);
                binaryWriter.Write(record.DateOfBirth.Day);
                binaryWriter.Write(record.Salary);
                binaryWriter.Write(record.WorkRate);
                binaryWriter.Write(record.Gender);
            }
        }

        private int CheckId(int id)
        {
            int index = -1;
            using BinaryReader binaryReader = new BinaryReader(this.fileStream, this.encoding, true);
            int count = (int)(this.fileStream.Length / RecordLength);
            this.fileStream.Position = 2;
            while (count-- > 0)
            {
                if (binaryReader.ReadInt32() == id)
                {
                    index = id;
                    return index;
                }

                this.fileStream.Position += RecordLength - BitConverter.GetBytes(default(int)).Length;
            }

            return index;
        }
    }
}
