using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp
{
    /// <summary>
    /// This class provides functionality for working with record.
    /// </summary>
    public class FileCabinetService : IFileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();
        private readonly CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
        private readonly IRecordValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetService"/> class.
        /// </summary>
        /// <param name="validator">Type IRecordValidator.</param>
        public FileCabinetService(IRecordValidator validator)
        {
            this.validator = validator;
        }

        /// <summary>
        /// This method creates a record.
        /// </summary>
        /// <param name="parameters">Parameters.</param>
        /// <returns>Id new record.</returns>
        public int CreateRecord(RecordsParameters parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException($"{nameof(parameters)} can't be null.");
            }

            this.validator.ValidateParameters(parameters);

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = parameters.FirstName,
                LastName = parameters.LastName,
                DateOfBirth = parameters.DateOfBirth,
                Salary = parameters.Salary,
                WorkRate = parameters.WorkRate,
                Gender = parameters.Gender,
            };

            this.list.Add(record);
            this.AddToFirstNameDictionary(parameters.FirstName, record);
            this.AddToLastNameDictionary(parameters.LastName, record);
            this.AddToDateOfBirthDictionary(parameters.DateOfBirth, record);

            return record.Id;
        }

        /// <summary>
        /// This method return records.
        /// </summary>
        /// <returns>Array of the records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            ReadOnlyCollection<FileCabinetRecord> records = new ReadOnlyCollection<FileCabinetRecord>(this.list);

            return records;
        }

        /// <summary>
        /// This method returns count of records.
        /// </summary>
        /// <returns>Count records.</returns>
        public int GetStat()
        {
            return this.list.Count;
        }

        /// <summary>
        /// This method changes the record.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="parameters">Parameters.</param>
        public void EditRecord(int id, RecordsParameters parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException($"{nameof(parameters)} can't be null.");
            }

            if (this.list.Exists(x => x.Id == id))
            {
                this.validator.ValidateParameters(parameters);

                FileCabinetRecord record = this.list.Find(x => x.Id == id);
                string initialFirstName = record.FirstName;
                string initialLastName = record.LastName;
                DateTime initialDateOfBirth = record.DateOfBirth;

                record.FirstName = parameters.FirstName;
                record.LastName = parameters.LastName;
                record.DateOfBirth = parameters.DateOfBirth;
                record.Salary = parameters.Salary;
                record.WorkRate = parameters.WorkRate;
                record.Gender = parameters.Gender;

                this.UpdateFirstNameDictionary(id, initialFirstName, record);
                this.UpdateLastNameDictionary(id, initialLastName, record);
                this.UpdateDateOfBirthDictionary(id, initialDateOfBirth, record);
            }
            else
            {
                throw new ArgumentException("There is no record with this id");
            }
        }

        /// <summary>
        /// This method searches for a record by first name.
        /// </summary>
        /// <param name="firstName">First name.</param>
        /// <returns>Array of the records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (firstName is null)
            {
                throw new ArgumentNullException($"{nameof(firstName)} can't be null");
            }

            firstName = firstName.ToLower(this.cultureInfo);
            ReadOnlyCollection<FileCabinetRecord> firstNameList;

            if (this.firstNameDictionary.ContainsKey(firstName))
            {
                firstNameList = new ReadOnlyCollection<FileCabinetRecord>(this.firstNameDictionary[firstName]);
                return firstNameList;
            }

            return null;
        }

        /// <summary>
        /// This method searches for a record by last name.
        /// </summary>
        /// <param name="lastName">last name.</param>
        /// <returns>Array of the records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (lastName is null)
            {
                throw new ArgumentNullException($"{nameof(lastName)} can't be null");
            }

            lastName = lastName.ToLower(this.cultureInfo);
            ReadOnlyCollection<FileCabinetRecord> lastNameList;

            if (this.lastNameDictionary.ContainsKey(lastName))
            {
                lastNameList = new ReadOnlyCollection<FileCabinetRecord>(this.lastNameDictionary[lastName]);

                return lastNameList;
            }

            return null;
        }

        /// <summary>
        /// This method searches for a record by date of birthday.
        /// </summary>
        /// <param name="dateOfBirth">Date of birthday.</param>
        /// <returns>Array of the records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            if (this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                ReadOnlyCollection<FileCabinetRecord> listElements = new ReadOnlyCollection<FileCabinetRecord>(this.dateOfBirthDictionary[dateOfBirth]);

                return listElements;
            }

            return null;
        }

        private void AddToFirstNameDictionary(string firstName, FileCabinetRecord record)
        {
            firstName = firstName.ToLower(this.cultureInfo);

            if (this.firstNameDictionary.ContainsKey(firstName))
            {
                this.firstNameDictionary[firstName].Add(record);
            }
            else
            {
                List<FileCabinetRecord> list = new List<FileCabinetRecord>();
                list.Add(record);
                this.firstNameDictionary.Add(firstName, list);
            }
        }

        private void UpdateFirstNameDictionary(int id, string firstName, FileCabinetRecord modifiedRecord)
        {
            List<FileCabinetRecord> list = this.firstNameDictionary[firstName.ToLower(this.cultureInfo)];
            FileCabinetRecord record = list.Find(x => x.Id == id);

            if (firstName == modifiedRecord.FirstName)
            {
                record = modifiedRecord;
            }
            else
            {
                list.Remove(record);
                this.AddToFirstNameDictionary(modifiedRecord.FirstName, modifiedRecord);
            }
        }

        private void AddToLastNameDictionary(string lastName, FileCabinetRecord record)
        {
            lastName = lastName.ToLower(this.cultureInfo);

            if (this.lastNameDictionary.ContainsKey(lastName))
            {
                this.lastNameDictionary[lastName].Add(record);
            }
            else
            {
                List<FileCabinetRecord> list = new List<FileCabinetRecord>();
                list.Add(record);
                this.lastNameDictionary.Add(lastName, list);
            }
        }

        private void UpdateLastNameDictionary(int id, string lastName, FileCabinetRecord modifiedRecord)
        {
            List<FileCabinetRecord> list = this.lastNameDictionary[lastName.ToLower(this.cultureInfo)];
            FileCabinetRecord record = list.Find(x => x.Id == id);

            if (lastName == modifiedRecord.LastName)
            {
                record = modifiedRecord;
            }
            else
            {
                list.Remove(record);
                this.AddToLastNameDictionary(modifiedRecord.LastName, modifiedRecord);
            }
        }

        private void AddToDateOfBirthDictionary(DateTime dateOfBirth, FileCabinetRecord record)
        {
            if (this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                this.dateOfBirthDictionary[dateOfBirth].Add(record);
            }
            else
            {
                List<FileCabinetRecord> list = new List<FileCabinetRecord>();
                list.Add(record);
                this.dateOfBirthDictionary.Add(dateOfBirth, list);
            }
        }

        private void UpdateDateOfBirthDictionary(int id, DateTime dateOfBirth, FileCabinetRecord modifiedRecord)
        {
            List<FileCabinetRecord> list = this.dateOfBirthDictionary[dateOfBirth];
            FileCabinetRecord record = list.Find(x => x.Id == id);

            if (dateOfBirth == modifiedRecord.DateOfBirth)
            {
                record = modifiedRecord;
            }
            else
            {
                list.Remove(record);
                this.AddToDateOfBirthDictionary(modifiedRecord.DateOfBirth, modifiedRecord);
            }
        }
    }
}
