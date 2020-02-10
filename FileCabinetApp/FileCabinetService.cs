using System;
using System.Collections.Generic;
using System.Globalization;
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

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short salary, decimal workRate, char gender)
        {
            if (this.list.Exists(x => x.Id == id))
            {
                Validation(firstName, lastName, dateOfBirth, salary, workRate, gender);

                foreach (var record in this.list)
                {
                    if (record.Id == id)
                    {
                        record.FirstName = firstName;
                        record.LastName = lastName;
                        record.DateOfBirth = dateOfBirth;
                        record.Salary = salary;
                        record.WorkRate = workRate;
                        record.Gender = gender;
                        return;
                    }
                }
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
            List<FileCabinetRecord> listElements = new List<FileCabinetRecord>();

            for (int i = 0; i < this.list.Count; i++)
            {
                string firstName1 = this.list[i].FirstName.ToLower(CultureInfo.CurrentCulture);

                if (firstName == firstName1)
                {
                    listElements.Add(this.list[i]);
                }
            }

            return listElements.ToArray();
        }

        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            if (lastName is null)
            {
                throw new ArgumentNullException($"fdf");
            }

            lastName = lastName.ToLower(CultureInfo.CurrentCulture);
            List<FileCabinetRecord> listElements = new List<FileCabinetRecord>();

            for (int i = 0; i < this.list.Count; i++)
            {
                string lastName1 = this.list[i].LastName.ToLower(CultureInfo.CurrentCulture);

                if (lastName == lastName1)
                {
                    listElements.Add(this.list[i]);
                }
            }

            return listElements.ToArray();
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
    }
}
