using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
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
        private const string DefaultBinaryFileName = "cabinet-records.db";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private static readonly CultureInfo CultureInfo = CultureInfo.CreateSpecificCulture("en-US");
        private static bool isRunning = true;
        private static IFileCabinetService fileCabinetService;
        private static IRecordValidator validator = new DefaultValidator();
        private static FileStream fileStream;

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
            new Tuple<string, Action<string>>("import", Import),
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
            new string[] { "export", "export records", "The 'export' command export records to csv or xml file." },
            new string[] { "import", "import records", "The 'import' command import records to csv or xml file." },
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
            RecordArgs eventArgs = ConsoleRead();

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
            CultureInfo.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";

            id = int.Parse(parameters, CultureInfo);
            if (id <= fileCabinetService.GetStat())
            {
                RecordArgs eventArgs = ConsoleRead();

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

            if (param.Length > 1)
            {
                string search = param[1].Trim('"');
                string searchParam = param[0].ToLower(CultureInfo);

                switch (searchParam)
                {
                    case "firstname":
                        records = fileCabinetService.FindByFirstName(search);
                        break;
                    case "lastname":
                        records = fileCabinetService.FindByLastName(search);
                        break;
                    case "dateofbirth":
                        DateTime dateofbirth = DateTime.ParseExact(search, "yyyy-MMM-dd", CultureInfo);
                        records = fileCabinetService.FindByDateOfBirth(dateofbirth);
                        break;
                }

                Representation(records);
            }
            else
            {
                Console.WriteLine("The command was entered incorrectly. Input format: \"find category item\".");
            }
        }

        private static void Export(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                string[] param = parameters.Split(' ');
                string path = param[1];
                if (param[0] == "csv")
                {
                    ExportToCsv(path);
                }

                if (param[0] == "xml")
                {
                    ExportToXml(path);
                }
            }
            else
            {
                throw new ArgumentException($"{nameof(parameters)} can't be empty.");
            }
        }

        private static void Import(string parameters)
        {
            string[] commands = parameters.Split(' ');
            string fileFormat = commands[0];
            string path = commands[1];

            if (commands.Length > 1)
            {
                if (!File.Exists(path))
                {
                    Console.WriteLine($"Import error: file {nameof(path)} is not exist.");
                    return;
                }

                switch (fileFormat)
                {
                    case "csv":
                        ImportFromCsv(path);
                        break;
                    case "xml":
                        ImportFromXML(path);
                        break;
                    default:
                        Console.WriteLine($"Incorrect {nameof(fileFormat)}.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("The command was entered incorrectly. Input format: \"import type path\".");
            }
        }

        private static void Representation(ReadOnlyCollection<FileCabinetRecord> records)
        {
            if (records is null)
            {
                Console.WriteLine("The record is not found");
            }
            else if (records.Count < 1)
            {
                Console.WriteLine("The record is not found");
            }
            else
            {
                foreach (FileCabinetRecord record in records)
                {
                    Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo)}, " +
                        $"{record.Salary}, {record.WorkRate}, {record.Gender}");
                }
            }
        }

        private static string CheckCommandLine(string[] args)
        {
            string result = "Using default validation rules.";

            bool isCustom = args.Any(x => x.Contains("custom", StringComparison.OrdinalIgnoreCase));
            bool isFile = args.Any(x => x.Contains("file", StringComparison.OrdinalIgnoreCase));

            if (isCustom)
            {
                validator = new CustomValidator();
                result = "Using custom validation rules.";
            }

            if (isFile)
            {
                fileStream = new FileStream(DefaultBinaryFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                fileCabinetService = new FileCabinetFilesystemService(validator, fileStream);
                return result;
            }

            fileCabinetService = new FileCabinetMemoryService(validator);

            return result;
        }

        private static RecordArgs ConsoleRead()
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
            else
            {
                Console.WriteLine($"File {path} didn't found.");
            }
        }

        private static void ExportToXml(string path)
        {
            if (IsExists(path))
            {
                try
                {
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.WriteEndDocumentOnClose = true;

                    using XmlWriter xmlWriter = XmlWriter.Create(path, settings);
                    var snapshot = fileCabinetService.MakeSnapshot();
                    snapshot.SaveToXML(xmlWriter);
                    Console.WriteLine($"All records are exported to file {path}.");
                }
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine($"Export failed: can't open file {path}.");
                }
            }
            else
            {
                Console.WriteLine($"File {path} didn't found.");
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

        private static void ImportFromCsv(string path)
        {
            FileCabinetServiceSnapshot snapshot = new FileCabinetServiceSnapshot();
            using StreamReader reader = new StreamReader(path);

            snapshot.LoadFromCSV(reader, out int countRecords);
            fileCabinetService.Restore(snapshot);

            if (countRecords > 0)
            {
                Console.WriteLine($"{countRecords} records were imported from {path}.");
            }
            else
            {
                Console.WriteLine("Records were not imported, and the file is empty.");
            }
        }

        private static void ImportFromXML(string path)
        {
            throw new NotImplementedException();
        }
    }
}