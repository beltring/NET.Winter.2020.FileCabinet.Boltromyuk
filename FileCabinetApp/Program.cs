using System;
using System.Globalization;

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
        private static FileCabinetService fileCabinetService;
        private static CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("en-US");

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
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
            bool isRunning = true;

            while (isRunning)
            {
                try
                {
                    RecordsParameters recordsParameters = new RecordsParameters();
                    recordsParameters.ReadParameters();

                    int id = fileCabinetService.CreateRecord(recordsParameters);
                    Console.WriteLine($"Record #{id} is created.");
                    isRunning = false;
                }
                catch (ArgumentNullException ex)
                {
                    Console.WriteLine($"{ex.Message}, try again.");
                    continue;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"{ex.Message}, try again.");
                    continue;
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
                catch (OverflowException ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
            }
        }

        private static void List(string parameters)
        {
            FileCabinetRecord[] records = fileCabinetService.GetRecords();

            Representation(records);
        }

        private static void Edit(string parameters)
        {
            int id = 0;
            cultureInfo.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";

            try
            {
                id = int.Parse(parameters, cultureInfo);
                if (id <= fileCabinetService.GetStat())
                {
                    RecordsParameters recordsParameters = new RecordsParameters();
                    recordsParameters.ReadParameters();

                    fileCabinetService.EditRecord(id, recordsParameters);
                    Console.WriteLine($"Record #{id} is updated.");
                }
                else
                {
                    Console.WriteLine($"#{id} record is not found.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"{ex.Message}, try again.");
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (OverflowException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void Find(string parameters)
        {
            string[] param = parameters.Split(' ');
            FileCabinetRecord[] records = null;
            cultureInfo.DateTimeFormat.ShortDatePattern = "yyyy-MMM-dd";

            string search = param[1].Trim('"');
            string searchParam = param[0].ToLower(cultureInfo);

            switch (searchParam)
            {
                case "firstname":
                    records = fileCabinetService.FindByFirstName(search);
                    break;
                case "lastname":
                    records = fileCabinetService.FindByLastName(search);
                    break;
                case "dateofbirth":
                    DateTime dateofbirth = DateTime.ParseExact(search, "d", cultureInfo);
                    records = fileCabinetService.FindByDateOfBirth(dateofbirth);
                    break;
            }

            Representation(records);
        }

        private static void Representation(FileCabinetRecord[] records)
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
                fileCabinetService = new FileCabinetCustomService();
                result = "Using custom validation rules.";
            }

            fileCabinetService = new FileCabinetDefaultService();

            return result;
        }
    }
}