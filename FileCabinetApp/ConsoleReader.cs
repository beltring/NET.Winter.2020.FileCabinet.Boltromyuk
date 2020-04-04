using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp
{
    /// <summary>Сlass for reading data from the console.</summary>
    internal static class ConsoleReader
    {
        /// <summary>Consoles the read.</summary>
        /// <param name="validator">The validator.</param>
        /// <returns>Record arguments.</returns>
        public static RecordArgs ConsoleRead(IInputValidator validator)
        {
            Func<string, Tuple<bool, string, string>> stringConverter = Converter.StringConverter;
            Func<string, Tuple<bool, string>> firstNameValidator = validator.FirstNameValidator;

            Func<string, Tuple<bool, string>> lastNameValidator = validator.LastNameValidator;

            Func<string, Tuple<bool, string, DateTime>> dateConverter = Converter.DateConverter;
            Func<DateTime, Tuple<bool, string>> dateOfBirthValidator = validator.DateOfBirthValidator;

            Func<string, Tuple<bool, string, short>> salaryConverter = Converter.SalaryConverter;
            Func<short, Tuple<bool, string>> salaryValidator = validator.SalaryValidator;

            Func<string, Tuple<bool, string, decimal>> workRateConverter = Converter.WorkRateConverter;
            Func<decimal, Tuple<bool, string>> workRateValidator = validator.WorkRateValidator;

            Func<string, Tuple<bool, string, char>> genderConverter = Converter.GenderConverter;
            Func<char, Tuple<bool, string>> genderValidator = validator.GenderValidator;

            Console.Write("First name: ");
            string firstName = ReadInput(stringConverter, firstNameValidator);

            Console.Write("Last name: ");
            var lastName = ReadInput(stringConverter, lastNameValidator);

            Console.Write("Date of birth: ");
            var dob = ReadInput(dateConverter, dateOfBirthValidator);

            Console.Write("Salary: ");
            short salary = ReadInput(salaryConverter, salaryValidator);

            Console.Write("Work rate: ");
            decimal workRate = ReadInput(workRateConverter, workRateValidator);

            Console.Write("Gender: ");
            char gender = ReadInput(genderConverter, genderValidator);

            RecordArgs eventArgs = new RecordArgs()
            {
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dob,
                Salary = salary,
                WorkRate = workRate,
                Gender = gender,
            };

            return eventArgs;
        }

        private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }
    }
}
