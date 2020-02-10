using System;
using System.Globalization;

namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Pavel Boltromyuk";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private const string Format = "yyyy-MMM-dd";
        private static bool isRunning = true;
        private static FileCabinetService fileCabinetService = new FileCabinetService();

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

        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
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
            while (true)
            {
                try
                {
                    Console.Write("First name:");
                    string firstName = Console.ReadLine();
                    Console.Write("Last name:");
                    string lastName = Console.ReadLine();
                    Console.Write("Date of birth:");
                    string date = Console.ReadLine();
                    DateTime dateOfBirth = DateTime.ParseExact(date, "M/dd/yyyy", null);
                    Console.Write("Salary:");
                    short salary = short.Parse(Console.ReadLine(), CultureInfo.CurrentCulture);
                    Console.Write("Work rate:");
                    decimal workRate = decimal.Parse(Console.ReadLine(), CultureInfo.CurrentCulture);
                    Console.Write("Gender(M/F):");
                    char gender = char.Parse(Console.ReadLine());

                    int id = fileCabinetService.CreateRecord(firstName, lastName, dateOfBirth, salary, workRate, gender);
                    Console.WriteLine($"Record #{id} is created.");
                    break;
                }
                catch (ArgumentNullException)
                {
                    Console.WriteLine("Invalid data, try again.");
                    continue;
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Invalid data, try again.");
                    continue;
                }
            }
        }

        private static void List(string parameters)
        {
            FileCabinetRecord[] records = fileCabinetService.GetRecords();

            for (int i = 0; i < records.Length; i++)
            {
                int id = records[i].Id;
                string firstName = records[i].FirstName;
                string lastName = records[i].LastName;
                string dateOfBirth = records[i].DateOfBirth.ToString(Format, CultureInfo.CurrentCulture);
                short salary = records[i].Salary;
                decimal workRate = records[i].WorkRate;
                char gender = records[i].Gender;

                Console.WriteLine($"#{id}, {firstName}, {lastName}, {dateOfBirth}, {salary}, {workRate}, {gender}");
            }
        }

        private static void Edit(string parameters)
        {
            int id = 0;

            try
            {
                id = int.Parse(parameters, CultureInfo.CurrentCulture);
                if (id <= fileCabinetService.GetStat())
                {
                    Console.Write("First name:");
                    string firstName = Console.ReadLine();
                    Console.Write("Last name:");
                    string lastName = Console.ReadLine();
                    Console.Write("Date of birth:");
                    string date = Console.ReadLine();
                    DateTime dateOfBirth = DateTime.ParseExact(date, "M/d/yyyy", null);
                    Console.Write("Salary:");
                    short salary = short.Parse(Console.ReadLine(), CultureInfo.CurrentCulture);
                    Console.Write("Work rate:");
                    decimal workRate = decimal.Parse(Console.ReadLine(), CultureInfo.CurrentCulture);
                    Console.Write("Gender(M/F):");
                    char gender = char.Parse(Console.ReadLine());

                    fileCabinetService.EditRecord(id, firstName, lastName, dateOfBirth, salary, workRate, gender);
                    Console.WriteLine($"Record #{id} is updated.");
                }
                else
                {
                    Console.WriteLine($"#{id} record is not found.");
                }
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"#{id} record is not found.");
            }
        }

        private static void Find(string parameters)
        {
            string[] param = parameters.Split(' ');
            FileCabinetRecord[] records = null;

            string search = param[1].Trim('"');
            string searchParam = param[0].ToLower(CultureInfo.CurrentCulture);

            switch (searchParam)
            {
                case "firstname":
                    records = fileCabinetService.FindByFirstName(search);
                    break;
            }

            Representation(records);
        }

        private static void Representation(FileCabinetRecord[] records)
        {
            foreach (FileCabinetRecord record in records)
            {
                Console.WriteLine($"#{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth.Year}-{record.DateOfBirth.Month}-{record.DateOfBirth.Day}, " +
                    $"{record.Salary}, {record.WorkRate}, {record.Gender}");
            }
        }
    }
}