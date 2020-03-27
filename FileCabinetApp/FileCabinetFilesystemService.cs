using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
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
        private const int DateLength = sizeof(int) * 3;
        private const int FirstNamePosition = StatusLength + sizeof(int);
        private const int LastNamePosition = FirstNamePosition + NameLength;
        private const int DayOfBirthPosition = LastNamePosition + NameLength;
        private const int SalaryPosition = DayOfBirthPosition + DateLength;

        private readonly Encoding encoding = Encoding.Unicode;
        private readonly FileStream fileStream;
        private readonly IRecordValidator validator;
        private readonly SortedList<int, long> idpositions = new SortedList<int, long>();

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
        public int CreateRecord(RecordArgs parameters)
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
            this.idpositions.Add(record.Id, this.fileStream.Position - RecordLength);

            return record.Id;
        }

        /// <summary>This method changes the record.</summary>
        /// <param name="id">Id.</param>
        /// <param name="parameters">Parameters.</param>
        public void EditRecord(int id, RecordArgs parameters)
        {
            if (id <= 0)
            {
                throw new ArgumentException($"{nameof(id)} have to be larger than zero.");
            }

            if (parameters is null)
            {
                throw new ArgumentNullException($"{nameof(parameters)} can't be null.");
            }

            if (!this.CheckId(id, out int index))
            {
                throw new ArgumentException($"Record #{nameof(id)} doesn't exist.");
            }

            this.validator.ValidateParameters(parameters);

            this.fileStream.Position = (RecordLength * index) + FirstNamePosition;
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
            this.fileStream.Seek(0, SeekOrigin.Begin);
            DateTime dateFromFile;

            while (count-- > 0)
            {
                if (binaryReader.ReadBytes(StatusLength)[0] == 0)
                {
                    this.fileStream.Seek(-StatusLength + DayOfBirthPosition, SeekOrigin.Current);
                    dateFromFile = new DateTime(binaryReader.ReadInt32(), binaryReader.ReadInt32(), binaryReader.ReadInt32());

                    if (DateTime.Compare(dateFromFile, dateOfBirth) == 0)
                    {
                        this.fileStream.Seek(-SalaryPosition, SeekOrigin.Current);
                        dateList.Add(this.ReadRecord(binaryReader));
                    }
                    else
                    {
                        this.fileStream.Seek(-SalaryPosition + RecordLength, SeekOrigin.Current);
                    }
                }
                else
                {
                    this.fileStream.Seek(-StatusLength + RecordLength, SeekOrigin.Current);
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
            this.fileStream.Seek(0, SeekOrigin.Begin);
            string firstNameFromFile;

            while (count-- > 0)
            {
                if (binaryReader.ReadBytes(StatusLength)[0] == 0)
                {
                    this.fileStream.Seek(-StatusLength + FirstNamePosition, SeekOrigin.Current);
                    firstNameFromFile = this.encoding.GetString(binaryReader.ReadBytes(NameLength), 0, NameLength).Trim();

                    if (firstNameFromFile.Equals(firstName, StringComparison.OrdinalIgnoreCase))
                    {
                        this.fileStream.Seek(-LastNamePosition, SeekOrigin.Current);
                        dateList.Add(this.ReadRecord(binaryReader));
                    }
                    else
                    {
                        this.fileStream.Seek(-LastNamePosition + RecordLength, SeekOrigin.Current);
                    }
                }
                else
                {
                    this.fileStream.Seek(-StatusLength + RecordLength, SeekOrigin.Current);
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
            this.fileStream.Seek(0, SeekOrigin.Begin);
            string lastNameFromFile;

            while (count-- > 0)
            {
                if (binaryReader.ReadBytes(StatusLength)[0] == 0)
                {
                    this.fileStream.Seek(-StatusLength + LastNamePosition, SeekOrigin.Current);
                    lastNameFromFile = this.encoding.GetString(binaryReader.ReadBytes(NameLength), 0, NameLength).Trim();

                    if (lastNameFromFile.Equals(lastName, StringComparison.OrdinalIgnoreCase))
                    {
                        this.fileStream.Seek(-DayOfBirthPosition, SeekOrigin.Current);
                        dateList.Add(this.ReadRecord(binaryReader));
                    }
                    else
                    {
                        this.fileStream.Seek(-DayOfBirthPosition + RecordLength, SeekOrigin.Current);
                    }
                }
                else
                {
                    this.fileStream.Seek(-StatusLength + RecordLength, SeekOrigin.Current);
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
                if (binaryReader.ReadBytes(StatusLength)[0] == 0)
                {
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
                else
                {
                    this.fileStream.Seek(RecordLength - StatusLength, SeekOrigin.Current);
                }
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
            var records = this.GetRecords().ToArray();
            if (records.Length == 0)
            {
                throw new ArgumentException($"Length {nameof(records)} can't be less 0.");
            }

            return new FileCabinetServiceSnapshot(records);
        }

        /// <summary>Restores the specified snapshot.</summary>
        /// <param name="snapshot">The snapshot.</param>
        /// <param name="exceptions">dictionary.</param>
        public void Restore(FileCabinetServiceSnapshot snapshot, out Dictionary<int, string> exceptions)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            var recordsFromFile = snapshot.FileCabinetRecords.ToList();

            exceptions = this.CheckException(recordsFromFile);

            using BinaryReader binaryReader = new BinaryReader(this.fileStream, Encoding.Unicode, true);
            this.AddIdPositionToSortedList(binaryReader);

            bool flag;

            foreach (var record in recordsFromFile)
            {
                flag = this.idpositions.Keys.Contains(record.Id);

                if (flag)
                {
                    long existRecordPosition = this.idpositions[record.Id];
                    this.fileStream.Seek(existRecordPosition, SeekOrigin.Begin);
                }
                else
                {
                    this.fileStream.Seek(0, SeekOrigin.End);
                }

                this.WriteToBinaryFile(record);
            }
        }

        /// <summary>
        ///   <para></para>
        ///   <para>Removes the specified identifier.
        /// </para>
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <exception cref="ArgumentException">Record #{id} doesn't exists.</exception>
        public void Remove(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException($"{nameof(id)} have to be larger than zero.");
            }

            if (!this.CheckId(id, out int index))
            {
                throw new ArgumentException($"Record #{id} doesn't exists.");
            }

            this.fileStream.Seek(RecordLength * index, SeekOrigin.Begin);
            this.fileStream.WriteByte(1);
        }

        /// <summary>Purges the specified deleted records count.</summary>
        /// <param name="deletedRecordsCount">The deleted records count.</param>
        /// <param name="recordsCount">The records count.</param>
        public void Purge(out int deletedRecordsCount, out int recordsCount)
        {
            this.fileStream.Seek(0, SeekOrigin.Begin);
            int count = (int)(this.fileStream.Length / RecordLength);
            recordsCount = count;
            deletedRecordsCount = 0;
            byte[] buffer = new byte[RecordLength];
            Queue<long> deletedRecordPositions = new Queue<long>();
            long lastDeletedRecordPosition = -1;
            long lastAliveRecordPosition = 0;

            using (BinaryReader reader = new BinaryReader(this.fileStream, Encoding.Unicode, true))
            using (BinaryWriter writer = new BinaryWriter(this.fileStream, Encoding.Unicode, true))
            {
                while (count-- > 0)
                {
                    if (reader.ReadBytes(StatusLength)[0] == 1)
                    {
                        this.fileStream.Seek(-StatusLength, SeekOrigin.Current);
                        deletedRecordPositions.Enqueue(this.fileStream.Position);
                        deletedRecordsCount++;
                    }
                    else
                    {
                        this.fileStream.Seek(-StatusLength, SeekOrigin.Current);
                        if (deletedRecordPositions.TryDequeue(out lastDeletedRecordPosition))
                        {
                            buffer = reader.ReadBytes(RecordLength);
                            this.fileStream.Seek(-RecordLength, SeekOrigin.Current);
                            this.fileStream.WriteByte(1); // deleted
                            this.fileStream.Seek(lastDeletedRecordPosition, SeekOrigin.Begin);
                            writer.Write(buffer, 0, RecordLength);
                        }
                        else
                        {
                            this.fileStream.Seek(RecordLength, SeekOrigin.Current);
                        }

                        lastAliveRecordPosition = this.fileStream.Position;
                    }
                }
            }

            this.fileStream.SetLength(lastAliveRecordPosition);
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
            using BinaryWriter binaryWriter = new BinaryWriter(this.fileStream, this.encoding, true);
            binaryWriter.Write(new byte[2] { 0, 0 }); // 0 - not deleted, 1 - deleted
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

        private bool CheckId(int id, out int index)
        {
            index = -1;
            using BinaryReader binaryReader = new BinaryReader(this.fileStream, this.encoding, true);

            int count = (int)(this.fileStream.Length / RecordLength);
            this.fileStream.Seek(0, SeekOrigin.Begin);
            int position = 0;

            while (count-- > 0)
            {
                if (binaryReader.ReadBytes(StatusLength)[0] == 0)
                {
                    if (binaryReader.ReadInt32() == id)
                    {
                        index = position;
                        return true;
                    }

                    this.fileStream.Seek(-FirstNamePosition, SeekOrigin.Current);
                }

                position++;
                this.fileStream.Seek(RecordLength, SeekOrigin.Current);
            }

            return false;
        }

        private Dictionary<int, string> CheckException(List<FileCabinetRecord> recordsFromFile)
        {
            var records = new List<FileCabinetRecord>(recordsFromFile);
            Dictionary<int, string> exceptions = new Dictionary<int, string>();

            foreach (var item in records)
            {
                try
                {
                    RecordArgs parameters = new RecordArgs()
                    {
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        DateOfBirth = item.DateOfBirth,
                        Salary = item.Salary,
                        WorkRate = item.WorkRate,
                        Gender = item.Gender,
                    };

                    this.validator.ValidateParameters(parameters);
                }
                catch (ArgumentException ex)
                {
                    exceptions.Add(item.Id, ex.Message);
                    recordsFromFile.Remove(item);
                }
            }

            return exceptions;
        }

        private void AddIdPositionToSortedList(BinaryReader binaryReader)
        {
            this.fileStream.Seek(0, SeekOrigin.Begin);
            int id;
            while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
            {
                this.fileStream.Seek(StatusLength, SeekOrigin.Current);
                id = binaryReader.ReadInt32();
                this.fileStream.Seek(-FirstNamePosition, SeekOrigin.Current);
                if (binaryReader.ReadBytes(StatusLength)[0] == 0)
                {
                    id = binaryReader.ReadInt32();
                    this.fileStream.Seek(-FirstNamePosition, SeekOrigin.Current);

                    if (!this.idpositions.Keys.Contains(id))
                    {
                        this.idpositions.Add(id, this.fileStream.Position);
                    }

                    this.fileStream.Seek(RecordLength, SeekOrigin.Current);
                }
                else
                {
                    this.fileStream.Seek(-StatusLength + RecordLength, SeekOrigin.Current);
                }
            }
        }

        private FileCabinetRecord ReadRecord(BinaryReader binaryReader)
        {
            binaryReader.ReadBytes(StatusLength);
            return new FileCabinetRecord
            {
                Id = binaryReader.ReadInt32(),
                FirstName = this.encoding.GetString(binaryReader.ReadBytes(NameLength), 0, NameLength).Trim(),
                LastName = this.encoding.GetString(binaryReader.ReadBytes(120), 0, 120).Trim(),
                DateOfBirth = new DateTime(binaryReader.ReadInt32(), binaryReader.ReadInt32(), binaryReader.ReadInt32()),
                Salary = binaryReader.ReadInt16(),
                WorkRate = ToDecimal(binaryReader.ReadBytes(16)),
                Gender = binaryReader.ReadChar(),
            };
        }
    }
}
