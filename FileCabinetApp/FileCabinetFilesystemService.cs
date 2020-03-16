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
        private readonly FileStream fileStream;
        private readonly IRecordValidator validator;

        /// <summary>Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.</summary>
        /// <param name="validator">The validator.</param>
        /// <param name="fileStream">The file stream.</param>
        public FileCabinetFilesystemService(IRecordValidator validator, FileStream fileStream)
        {
            this.validator = validator;
            this.fileStream = fileStream;
        }

        /// <summary>This method creates a record.</summary>
        /// <param name="parameters">Parameters.</param>
        /// <returns>Id new record.</returns>
        public int CreateRecord(RecordEventArgs parameters)
        {
            throw new NotImplementedException();
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
    }
}
