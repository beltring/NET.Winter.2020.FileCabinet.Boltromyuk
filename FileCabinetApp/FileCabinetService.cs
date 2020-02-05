using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

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
    }
}
