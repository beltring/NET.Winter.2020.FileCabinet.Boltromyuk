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
        int CreateRecord(RecordArgs parameters);

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
        void EditRecord(int id, RecordArgs parameters);

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

        /// <summary>Makes the snapshot.</summary>
        /// <returns>Snapshot.</returns>
        FileCabinetServiceSnapshot MakeSnapshot();

        /// <summary>Restores the specified snapshot.</summary>
        /// <param name="snapshot">The snapshot.</param>
        /// <param name="exceptions">Exception dictionary.</param>
        void Restore(FileCabinetServiceSnapshot snapshot, out Dictionary<int, string> exceptions);

        /// <summary>Removes the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        void Remove(int id);

        /// <summary>Purges the specified deleted records count.</summary>
        /// <param name="deletedRecordsCount">The deleted records count.</param>
        /// <param name="recordsCount">The records count.</param>
        void Purge(out int deletedRecordsCount, out int recordsCount);
    }
}
