using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// This class provides functionality for working with record.
    /// </summary>
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();
        private readonly CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");

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

            Validation(parameters);

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
        public FileCabinetRecord[] GetRecords()
        {
            FileCabinetRecord[] arrayRecords = this.list.ToArray();

            return arrayRecords;
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
                Validation(parameters);

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
        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            if (firstName is null)
            {
                throw new ArgumentNullException($"{nameof(firstName)} can't be null");
            }

            firstName = firstName.ToLower(this.cultureInfo);

            if (this.firstNameDictionary.ContainsKey(firstName))
            {
                List<FileCabinetRecord> firstNameList = this.firstNameDictionary[firstName];
                return firstNameList.ToArray();
            }

            return Array.Empty<FileCabinetRecord>();
        }

        /// <summary>
        /// This method searches for a record by last name.
        /// </summary>
        /// <param name="lastName">last name.</param>
        /// <returns>Array of the records.</returns>
        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            if (lastName is null)
            {
                throw new ArgumentNullException($"{nameof(lastName)} can't be null");
            }

            lastName = lastName.ToLower(this.cultureInfo);

            if (this.lastNameDictionary.ContainsKey(lastName))
            {
                List<FileCabinetRecord> lastNameList = this.lastNameDictionary[lastName];

                return lastNameList.ToArray();
            }

            return Array.Empty<FileCabinetRecord>();
        }

        /// <summary>
        /// This method searches for a record by date of birthday.
        /// </summary>
        /// <param name="dateOfBirth">Date of birthday.</param>
        /// <returns>Array of the records.</returns>
        public FileCabinetRecord[] FindByDateOfBirth(DateTime dateOfBirth)
        {
            if (this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                List<FileCabinetRecord> listElements = this.dateOfBirthDictionary[dateOfBirth];

                return listElements.ToArray();
            }

            return Array.Empty<FileCabinetRecord>();
        }

        private static void Validation(RecordsParameters parameters)
        {
            if (parameters.FirstName == null)
            {
                throw new ArgumentNullException($"The {nameof(parameters.FirstName)} parameter must not be null");
            }

            if (parameters.LastName == null)
            {
                throw new ArgumentNullException($"The {nameof(parameters.LastName)} parameter must not be null");
            }

            if (parameters.FirstName.Length < 2 || parameters.FirstName.Length > 60)
            {
                throw new ArgumentException($"The length of {nameof(parameters.FirstName)} must be between 2 and 60 characters");
            }

            if (parameters.LastName.Length < 2 || parameters.LastName.Length > 60)
            {
                throw new ArgumentException($"The length of {nameof(parameters.LastName)} must be between 2 and 60 characters");
            }

            if (parameters.DateOfBirth.Year < 1950 || (parameters.DateOfBirth.Year >= DateTime.Now.Year
                && parameters.DateOfBirth.Month >= DateTime.Now.Month && parameters.DateOfBirth.Day > DateTime.Now.Day))
            {
                throw new ArgumentException("Minimum date of birth 01-Jan-1950 maximum current date");
            }

            if (parameters.Salary < 100 || parameters.Salary > 10000)
            {
                throw new ArgumentException($"Invalid value, the {nameof(parameters.Salary)} value must be between 100 and 10000");
            }

            if (parameters.WorkRate < 0.25m || parameters.WorkRate > 1.5m)
            {
                throw new ArgumentException($"Invalid value, the {nameof(parameters.WorkRate)} value must be between 0.25 and 1.5");
            }

            if (parameters.Gender != 'M' && parameters.Gender != 'F')
            {
                throw new ArgumentException($"Invalid value, the value of the {nameof(parameters.Gender)} variable must be M or F");
            }
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
