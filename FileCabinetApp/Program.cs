using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.Enums;
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

        /// <summary>The is running.</summary>
        public static bool isRunning = true;

        /// <summary>The file cabinet service.</summary>
        public static IFileCabinetService fileCabinetService;

        /// <summary>The validator type.</summary>
        public static ValidatorType validatorType;

        /// <summary>The service type.</summary>
        public static ServiceType serviceType;

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

            ParseArguments(args);
            var commandHandler = CreateCommandHandlers();

            Console.WriteLine($"File Cabinet Application, developed by {DeveloperName}");
            Console.WriteLine($"Using {validatorType} validation rules.");
            Console.WriteLine($"The {serviceType.ToString()} service.");
            Console.WriteLine(HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                const int parametersIndex = 1;
                var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                commandHandler.Handle(new AppCommandRequest(command, parameters));
            }
            while (isRunning);
        }

        private static void ParseArguments(string[] args)
        {
            bool isCustom = args.Any(x => x.Contains(ValidatorType.Custom.ToString(), StringComparison.OrdinalIgnoreCase));
            bool isFile = args.Any(x => x.Contains(ServiceType.File.ToString(), StringComparison.OrdinalIgnoreCase));

            validatorType = isCustom ? ValidatorType.Custom : ValidatorType.Default;
            serviceType = isFile ? ServiceType.File : ServiceType.Memory;
        }

        private static CommandHandler CreateCommandHandlers()
        {
            return new CommandHandler();
        }
    }
}