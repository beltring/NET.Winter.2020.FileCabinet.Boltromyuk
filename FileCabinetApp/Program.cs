using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp
{
    /// <summary>
    /// Class Program.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Pavel Boltromyuk";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private static bool isRunning = true;
        private static IFileCabinetService fileCabinetService;
        private static CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
        private static IRecordValidator validator;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("export", Export),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "displays statistics for the entries", "The 'stat' command displays statistics for the entries." },
            new string[] { "create", "creates a record", "The 'create' command creates a record." },
            new string[] { "list", "returns a list of records added to the service", "The 'list' command returns a list of records added to the service" },
            new string[] { "edit", "editing record", "The 'edit' command editing record" },
            new string[] { "find", "find records", "The 'find' command find records" },
            new string[] { "find", "export records", "The 'export' command export records to csv or xml file." },
        };

        /// <summary>
        /// The method that starts the program execution.
        /// </summary>
        /// <param name="args">Input parameters.</param>
        public static void Main(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException($"{nameof(args)} can't be null.");
            }

            string rule = CheckCommandLine(args);

            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(rule);
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            RecordEventArgs eventArgs = ConsoleRead();

            int id = fileCabinetService.CreateRecord(eventArgs);
            Console.WriteLine($"Record #{id} is created.");
        }

        private static void List(string parameters)
        {
            ReadOnlyCollection<FileCabinetRecord> records = fileCabinetService.GetRecords();

            Representation(records);
        }

        private static void Edit(string parameters)
        {
            int id = 0;
            cultureInfo.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";

            id = int.Parse(parameters, cultureInfo);
            if (id <= fileCabinetService.GetStat())
            {
                RecordEventArgs eventArgs = ConsoleRead();

                fileCabinetService.EditRecord(id, eventArgs);
                Console.WriteLine($"Record #{id} is updated.");
            }
            else
            {
                Console.WriteLine($"#{id} record is not found.");
            }
        }

        private static void Find(string parameters)
        {
            string[] param = parameters.Split(' ');
            ReadOnlyCollection<FileCabinetRecord> records = null;
            cultureInfo.DateTimeFormat.ShortDatePattern = "yyyy-MMM-dd";

            string search = param[1].Trim('"');
            string searchParam = param[0].ToLower(cultureInfo);

            switch (searchParam)
            {
                case "firstname":
                    records = new ReadOnlyCollection<FileCabinetRecord>(fileCabinetService.FindByFirstName(search));
                    break;
                case "lastname":
                    records = new ReadOnlyCollection<FileCabinetRecord>(fileCabinetService.FindByLastName(search));
                    break;
                case "dateofbirth":
                    DateTime dateofbirth = DateTime.ParseExact(search, "d", cultureInfo);
                    records = new ReadOnlyCollection<FileCabinetRecord>(fileCabinetService.FindByDateOfBirth(dateofbirth));
                    break;
            }

            Representation(records);
        }

        private static void Export(string parameters)
        {
            string[] param = parameters.Split(' ');
            string path = param[1];
            if (param[0] == "csv")
            {
                ExportToCsv(path);
            }
        }

        private static void Representation(ReadOnlyCollection<FileCabinetRecord> records)
        {
            cultureInfo.DateTimeFormat.ShortDatePattern = "yyyy-MMM-dd";

            foreach (FileCabinetRecord record in records)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth.ToString("d", cultureInfo)}, " +
                    $"{record.Salary}, {record.WorkRate}, {record.Gender}");
            }
        }

        private static string CheckCommandLine(string[] args)
        {
            string command = string.Empty;
            string result = "Using default validation rules.";

            if (args.Length == 1)
            {
                command = args[0];
            }

            if (args.Length == 2)
            {
                command = args[1];
            }

            if (command.Contains("custom", StringComparison.OrdinalIgnoreCase))
            {
                CustomValidator customValidator = new CustomValidator();
                fileCabinetService = new FileCabinetService(customValidator);
                validator = customValidator;
                result = "Using custom validation rules.";

                return result;
            }

            DefaultValidator defaultValidator = new DefaultValidator();
            fileCabinetService = new FileCabinetService(defaultValidator);
            validator = defaultValidator;

            return result;
        }

        private static RecordEventArgs ConsoleRead()
        {
            Func<string, Tuple<bool, string, string>> stringConverter = Converter.StringConverter;
            Func<string, Tuple<bool, string>> stringValidator = validator.StringValidator;

            Func<string, Tuple<bool, string, DateTime>> dateConverter = Converter.DateConverter;
            Func<DateTime, Tuple<bool, string>> dateOfBirthValidator = validator.DateOfBirthValidator;

            Func<string, Tuple<bool, string, short>> salaryConverter = Converter.SalaryConverter;
            Func<short, Tuple<bool, string>> salaryValidator = validator.SalaryValidator;

            Func<string, Tuple<bool, string, decimal>> workRateConverter = Converter.WorkRateConverter;
            Func<decimal, Tuple<bool, string>> workRateValidator = validator.WorkRateValidator;

            Func<string, Tuple<bool, string, char>> genderConverter = Converter.GenderConverter;
            Func<char, Tuple<bool, string>> genderValidator = validator.GenderValidator;

            Console.Write("First name: ");
            string firstName = ReadInput(stringConverter, stringValidator);

            Console.Write("Last name: ");
            var lastName = ReadInput(stringConverter, stringValidator);

            Console.Write("Date of birth: ");
            var dob = ReadInput(dateConverter, dateOfBirthValidator);

            Console.Write("Salary: ");
            short salary = ReadInput(salaryConverter, salaryValidator);

            Console.Write("Work rate: ");
            decimal workRate = ReadInput(workRateConverter, workRateValidator);

            Console.Write("Gender: ");
            char gender = ReadInput(genderConverter, genderValidator);

            RecordEventArgs eventArgs = new RecordEventArgs()
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

        private static void ExportToCsv(string path)
        {
            if (IsExists(path))
            {
                try
                {
                    using StreamWriter sw = new StreamWriter(path);
                    var snapshot = fileCabinetService.MakeSnapshot();
                    snapshot.SaveToCsv(sw);
                    Console.WriteLine($"All records are exported to file {path}.");
                }
                catch (IOException)
                {
                    Console.WriteLine($"Export failed: can't open file {path}.");
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Add at least one record.");
                }
            }
        }

        private static bool IsExists(string path)
        {
            if (File.Exists(path))
            {
                Console.WriteLine($"File is exist - rewrite {path}?[Y / n]");
                string result = Console.ReadLine();
                return result == "Y" ? true : false;
            }

            return true;
        }
    }
}