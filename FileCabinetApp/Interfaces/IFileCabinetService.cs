using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FileCabinetApp.Interfaces
{
    /// <summary>
    /// Interface IFileCabinetService.
    /// </summary>
    public interface IFileCabinetService
    {
        /// <summary>
        /// This method creates a record.
        /// </summary>
        /// <param name="parameters">Parameters.</param>
        /// <returns>Id new record.</returns>
        int CreateRecord(RecordsParameters parameters);

        /// <summary>
        /// This method return records.
        /// </summary>
        /// <returns>Array of the records.</returns>
        ReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>
        /// This method returns count of records.
        /// </summary>
        /// <returns>Count records.</returns>
        int GetStat();

        /// <summary>
        /// This method changes the record.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="parameters">Parameters.</param>
        void EditRecord(int id, RecordsParameters parameters);

        /// <summary>
        /// This method searches for a record by first name.
        /// </summary>
        /// <param name="firstName">First name.</param>
        /// <returns>Array of the records.</returns>
        ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// This method searches for a record by last name.
        /// </summary>
        /// <param name="lastName">last name.</param>
        /// <returns>Array of the records.</returns>
        ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>
        /// This method searches for a record by date of birthday.
        /// </summary>
        /// <param name="dateOfBirth">Date of birthday.</param>
        /// <returns>Array of the records.</returns>
        ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth);
    }
}
