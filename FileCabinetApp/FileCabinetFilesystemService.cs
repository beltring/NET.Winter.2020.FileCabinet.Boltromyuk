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
            throw new NotImplementedException();
        }

        /// <summary>This method searches for a record by date of birthday.</summary>
        /// <param name="dateOfBirth">Date of birthday.</param>
        /// <returns>Array of the records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            throw new NotImplementedException();
        }

        /// <summary>This method searches for a record by first name.</summary>
        /// <param name="firstName">First name.</param>
        /// <returns>Array of the records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        /// <summary>This method searches for a record by last name.</summary>
        /// <param name="lastName">last name.</param>
        /// <returns>Array of the records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        /// <summary>This method return records.</summary>
        /// <returns>Array of the records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            throw new NotImplementedException();
        }

        /// <summary>This method returns count of records.</summary>
        /// <returns>Count records.</returns>
        public int GetStat()
        {
            throw new NotImplementedException();
        }

        /// <summary>Makes the snapshot.</summary>
        /// <returns>Snapshot.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
        }

        private void WriteToBinaryFile(FileCabinetRecord record)
        {
            using (BinaryWriter writeBinay = new BinaryWriter(this.fileStream, Encoding.Unicode, true))
            {
                var encoding = UnicodeEncoding.Unicode;
                short status = 0;

                writeBinay.Write(status); // 0 - not deleted, 1 - deleted
                writeBinay.Write(record.Id);
                writeBinay.Write(encoding.GetBytes(record.FirstName.PadRight(60)));
                writeBinay.Write(encoding.GetBytes(record.LastName.PadRight(60)));
                writeBinay.Write(record.DateOfBirth.Year);
                writeBinay.Write(record.DateOfBirth.Month);
                writeBinay.Write(record.DateOfBirth.Day);
                writeBinay.Write(record.Salary);
                writeBinay.Write(record.WorkRate);
                writeBinay.Write(record.Gender);
            }
        }
    }
}
