using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp
{
    /// <summary>
    /// This class provides functionality for working with record.
    /// </summary>
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();
        private CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
        private IRecordValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetMemoryService"/> class.
        /// </summary>
        /// <param name="validator">Type IRecordValidator.</param>
        public FileCabinetMemoryService(IRecordValidator validator)
        {
            this.validator = validator ?? throw new ArgumentNullException($"{nameof(validator)} can't be null.");
        }

        /// <summary>
        /// This method creates a record.
        /// </summary>
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
                Id = this.list.Count + 1,
                FirstName = parameters.FirstName,
                LastName = parameters.LastName,
                DateOfBirth = parameters.DateOfBirth,
                Salary = parameters.Salary,
                WorkRate = parameters.WorkRate,
                Gender = parameters.Gender,
            };

            this.list.Add(record);
            this.AddToDictionaries(record);

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
        public void EditRecord(int id, RecordArgs parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException($"{nameof(parameters)} can't be null.");
            }

            if (this.list.Exists(x => x.Id == id))
            {
                this.validator.ValidateParameters(parameters);

                FileCabinetRecord record = this.list.Find(x => x.Id == id);
                int index = this.list.IndexOf(record);
                this.RemoveFromDictionaries(index);

                this.list[index].FirstName = parameters.FirstName;
                this.list[index].LastName = parameters.LastName;
                this.list[index].DateOfBirth = parameters.DateOfBirth;
                this.list[index].Salary = parameters.Salary;
                this.list[index].WorkRate = parameters.WorkRate;
                this.list[index].Gender = parameters.Gender;

                this.EditDictionaries(parameters, index);
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

        /// <summary>Makes the snapshot.</summary>
        /// <returns>Snapshot.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            if (this.list.Count == 0)
            {
                throw new ArgumentException($"Length {nameof(this.list)} can't be less 0.");
            }

            return new FileCabinetServiceSnapshot(this.list.ToArray());
        }

        /// <summary>Restores the specified snapshot.</summary>
        /// <param name="snapshot">The snapshot.</param>
        /// <param name="exceptions">dictionary.</param>
        public void Restore(FileCabinetServiceSnapshot snapshot, out Dictionary<int, string> exceptions)
        {
            if (snapshot is null)
            {
                throw new ArgumentNullException($"{nameof(snapshot)} can't be null.");
            }

            var recordsFromFile = snapshot.FileCabinetRecords.ToList();

            exceptions = this.CheckException(recordsFromFile);

            for (int i = 0; i < this.list.Count; i++)
            {
                bool flag = recordsFromFile.Exists(x => x.Id == this.list[i].Id);
                if (flag)
                {
                    this.RemoveFromDictionaries(i);
                }
            }

            this.AddToDictionaries(recordsFromFile);

            recordsFromFile.AddRange(this.list);
            var distinct = from item in recordsFromFile
                           group item by new { item.Id, } into matches
                           select matches.First();

            this.list = distinct.ToList();
        }

        private void AddToDictionaries(FileCabinetRecord record)
        {
            string firstName = record.FirstName.ToLower(this.cultureInfo);
            string lastName = record.LastName.ToLower(this.cultureInfo);

            if (!this.firstNameDictionary.ContainsKey(firstName))
            {
                this.firstNameDictionary.Add(firstName, new List<FileCabinetRecord>());
            }

            if (!this.lastNameDictionary.ContainsKey(lastName))
            {
                this.lastNameDictionary.Add(lastName, new List<FileCabinetRecord>());
            }

            if (!this.dateOfBirthDictionary.ContainsKey(record.DateOfBirth))
            {
                this.dateOfBirthDictionary.Add(record.DateOfBirth, new List<FileCabinetRecord>());
            }

            this.firstNameDictionary[firstName].Add(record);
            this.lastNameDictionary[lastName].Add(record);
            this.dateOfBirthDictionary[record.DateOfBirth].Add(record);
        }

        private void AddToDictionaries(IEnumerable<FileCabinetRecord> records)
        {
            foreach (var item in records)
            {
                this.AddToDictionaries(item);
            }
        }

        private void RemoveFromDictionaries(int index)
        {
            string firstName = this.list[index].FirstName.ToLower(this.cultureInfo);
            string lastName = this.list[index].LastName.ToLower(this.cultureInfo);

            this.firstNameDictionary[firstName].Remove(this.list[index]);
            this.lastNameDictionary[lastName].Remove(this.list[index]);
            this.dateOfBirthDictionary[this.list[index].DateOfBirth].Remove(this.list[index]);
        }

        private void EditDictionaries(RecordArgs record, int index)
        {
            string firstName = record.FirstName.ToLower(this.cultureInfo);
            string lastName = record.LastName.ToLower(this.cultureInfo);

            if (!this.firstNameDictionary.ContainsKey(firstName))
            {
                this.firstNameDictionary.Add(firstName, new List<FileCabinetRecord>());
            }

            if (!this.lastNameDictionary.ContainsKey(lastName))
            {
                this.lastNameDictionary.Add(lastName, new List<FileCabinetRecord>());
            }

            if (!this.dateOfBirthDictionary.ContainsKey(record.DateOfBirth))
            {
                this.dateOfBirthDictionary.Add(record.DateOfBirth, new List<FileCabinetRecord>());
            }

            this.firstNameDictionary[firstName].Add(this.list[index]);
            this.lastNameDictionary[lastName].Add(this.list[index]);
            this.dateOfBirthDictionary[record.DateOfBirth].Add(this.list[index]);
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
    }
}
