using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short salary, decimal workRate, char gender)
        {
            Validation(firstName, lastName, dateOfBirth, salary, workRate, gender);

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Salary = salary,
                WorkRate = workRate,
                Gender = gender,
            };

            this.list.Add(record);
            this.AddToFirstNameDictionary(firstName, record);
            this.AddToLastNameDictionary(lastName, record);

            return record.Id;
        }

        public FileCabinetRecord[] GetRecords()
        {
            FileCabinetRecord[] arrayRecords = this.list.ToArray();

            return arrayRecords;
        }

        public int GetStat()
        {
            return this.list.Count;
        }

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short salary, decimal workRate, char gender)
        {
            if (this.list.Exists(x => x.Id == id))
            {
                Validation(firstName, lastName, dateOfBirth, salary, workRate, gender);

                FileCabinetRecord record = this.list.Find(x => x.Id == id);
                string initialFirstName = record.FirstName;
                string initialLastName = record.LastName;

                record.FirstName = firstName;
                record.LastName = lastName;
                record.DateOfBirth = dateOfBirth;
                record.Salary = salary;
                record.WorkRate = workRate;
                record.Gender = gender;

                this.UpdateFirstNameDictionary(id, initialFirstName, record);
                this.UpdateLastNameDictionary(id, initialLastName, record);
            }
            else
            {
                throw new ArgumentException("There is no record with this id");
            }
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            if (firstName is null)
            {
                throw new ArgumentNullException($"{nameof(firstName)} can't be null");
            }

            firstName = firstName.ToLower(CultureInfo.CurrentCulture);
            List<FileCabinetRecord> firstNameList = this.firstNameDictionary[firstName];

            return firstNameList.ToArray();
        }

        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            if (lastName is null)
            {
                throw new ArgumentNullException($"{nameof(lastName)} can't be null");
            }

            lastName = lastName.ToLower(CultureInfo.CurrentCulture);
            List<FileCabinetRecord> firstNameList = this.lastNameDictionary[lastName];

            return firstNameList.ToArray();
        }

        public FileCabinetRecord[] FindByDateOfBirth(DateTime dateOfBirth)
        {
            List<FileCabinetRecord> listElements = new List<FileCabinetRecord>();

            for (int i = 0; i < this.list.Count; i++)
            {
                if (this.list[i].DateOfBirth.Equals(dateOfBirth))
                {
                    listElements.Add(this.list[i]);
                }
            }

            return listElements.ToArray();
        }

        private static void Validation(string firstName, string lastName, DateTime dateOfBirth, short salary, decimal workRate, char gender)
        {
            if (firstName == null)
            {
                throw new ArgumentNullException($"The {nameof(firstName)} parameter must not be null.");
            }

            if (lastName == null)
            {
                throw new ArgumentNullException($"The {nameof(firstName)} parameter must not be null.");
            }

            if (firstName.Length < 2 || firstName.Length > 60)
            {
                throw new ArgumentException($"The length of {nameof(firstName)} must be between 2 and 60 characters.");
            }

            if (lastName.Length < 2 || lastName.Length > 60)
            {
                throw new ArgumentException($"The length of {nameof(lastName)} must be between 2 and 60 characters.");
            }

            if (dateOfBirth.Year < 1950 || (dateOfBirth.Year >= DateTime.Now.Year
                && dateOfBirth.Month >= DateTime.Now.Month && dateOfBirth.Day > DateTime.Now.Day))
            {
                throw new ArgumentException("Minimum date of birth 01-Jan-1950 maximum current date.");
            }

            if (salary < 100 || salary > 10000)
            {
                throw new ArgumentException($"Invalid value, the {nameof(salary)} value must be between 100 and 10000.");
            }

            if (workRate < 0.25m || workRate > 1.5m)
            {
                throw new ArgumentException($"Invalid value, the {nameof(workRate)} value must be between 0.25 and 1.5.");
            }

            if (gender != 'M' && gender != 'F')
            {
                throw new ArgumentException($"Invalid value, the value of the {nameof(gender)} variable must be M or F.");
            }
        }

        private void AddToFirstNameDictionary(string firstName, FileCabinetRecord record)
        {
            firstName = firstName.ToLower(CultureInfo.CurrentCulture);

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
            List<FileCabinetRecord> list = this.firstNameDictionary[firstName.ToLower(CultureInfo.CurrentCulture)];
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
            lastName = lastName.ToLower(CultureInfo.CurrentCulture);

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
            List<FileCabinetRecord> list = this.lastNameDictionary[lastName.ToLower(CultureInfo.CurrentCulture)];
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
    }
}
